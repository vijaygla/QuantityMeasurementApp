using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Repository;
using QuantityMeasurementApp.Exceptions;

namespace QuantityMeasurementApp.Service
{
    /// <summary>
    /// Implementation of the Quantity Measurement Service.
    /// Handles the orchestration of measurement operations by leveraging the domain models.
    /// </summary>
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuantityMeasurementServiceImpl"/> class.
        /// </summary>
        /// <param name="repo">The repository for storing measurement entities.</param>
        public QuantityMeasurementServiceImpl(IQuantityMeasurementRepository repo)
        {
            repository = repo;
        }

        /// <summary>
        /// Compares two quantities for equality after normalizing them to their base units.
        /// </summary>
        /// <param name="q1">The first quantity.</param>
        /// <param name="q2">The second quantity.</param>
        /// <returns>True if the quantities are equal within a small tolerance; otherwise, false.</returns>
        /// <exception cref="QuantityMeasurementException">Thrown if units are incompatible or invalid.</exception>
        public bool Compare(QuantityDTO q1, QuantityDTO q2)
        {
            var entity = CreateEntity(q1, q2, "COMPARE");
            try
            {
                bool result = ExecuteOperation(q1, q2, (a, b) => a.Equals(b));
                entity.Result = new QuantityDTO(result ? 1 : 0, "BOOLEAN");
                return result;
            }
            catch (Exception ex)
            {
                entity.ErrorMessage = ex.Message;
                // Re-throw as QuantityMeasurementException if it's not already one
                if (ex is QuantityMeasurementException) throw;
                throw new QuantityMeasurementException($"Comparison failed: {ex.Message}");
            }
            finally
            {
                repository.Save(entity);
            }
        }

        /// <summary>
        /// Converts a quantity from one unit to another within the same category.
        /// </summary>
        /// <param name="source">The source quantity.</param>
        /// <param name="targetUnit">The target unit name.</param>
        /// <returns>A new <see cref="QuantityDTO"/> containing the converted value and unit.</returns>
        /// <exception cref="QuantityMeasurementException">Thrown if unit conversion is not supported.</exception>
        public QuantityDTO Convert(QuantityDTO source, string targetUnit)
        {
            var entity = CreateEntity(source, null, "CONVERT");
            try
            {
                QuantityDTO result = ExecuteSingleOperandOperation(source, (q) =>
                {
                    if (TryParseUnit(source.Unit, out Enum _, out Type enumType))
                    {
                        try
                        {
                            var targetEnum = (Enum)Enum.Parse(enumType, targetUnit, true);
                            
                            // Use reflection to call ConvertTo on Quantity<U>
                            var quantityType = typeof(Quantity<>).MakeGenericType(enumType);
                            var quantityInstance = Activator.CreateInstance(quantityType, source.Value, (Enum)Enum.Parse(enumType, source.Unit, true));
                            var convertMethod = quantityType.GetMethod("ConvertTo");
                            var resultQuantity = convertMethod.Invoke(quantityInstance, new object[] { targetEnum });

                            double val = (double)resultQuantity.GetType().GetProperty("Value").GetValue(resultQuantity);
                            return new QuantityDTO(val, targetUnit);
                        }
                        catch (ArgumentException)
                        {
                            throw new QuantityMeasurementException($"Target unit '{targetUnit}' is not valid for the category of '{source.Unit}'.");
                        }
                    }
                    throw new QuantityMeasurementException($"Source unit '{source.Unit}' is not recognized.");
                });

                entity.Result = result;
                return result;
            }
            catch (Exception ex)
            {
                entity.ErrorMessage = ex.Message;
                if (ex is QuantityMeasurementException) throw;
                throw new QuantityMeasurementException($"Conversion failed: {ex.Message}");
            }
            finally
            {
                repository.Save(entity);
            }
        }

        /// <summary>
        /// Adds two quantities and returns the result in the unit of the first operand.
        /// </summary>
        /// <param name="q1">The first quantity.</param>
        /// <param name="q2">The second quantity.</param>
        /// <returns>A new <see cref="QuantityDTO"/> representing the sum.</returns>
        public QuantityDTO Add(QuantityDTO q1, QuantityDTO q2)
        {
            var entity = CreateEntity(q1, q2, "ADD");
            try
            {
                var result = ExecuteOperation(q1, q2, (a, b) => {
                    var addMethod = a.GetType().GetMethod("Add", new[] { a.GetType() });
                    var resultObj = addMethod.Invoke(a, new object[] { b });
                    
                    double val = (double)resultObj.GetType().GetProperty("Value").GetValue(resultObj);
                    string unit = resultObj.GetType().GetProperty("Unit").GetValue(resultObj).ToString();
                    return new QuantityDTO(val, unit);
                });

                entity.Result = result;
                return result;
            }
            catch (Exception ex)
            {
                entity.ErrorMessage = ex.Message;
                if (ex is QuantityMeasurementException) throw;
                throw new QuantityMeasurementException($"Addition failed: {ex.Message}");
            }
            finally
            {
                repository.Save(entity);
            }
        }

        /// <summary>
        /// Subtracts the second quantity from the first and returns the result in the unit of the first operand.
        /// </summary>
        /// <param name="q1">The first quantity.</param>
        /// <param name="q2">The second quantity.</param>
        /// <returns>A new <see cref="QuantityDTO"/> representing the difference.</returns>
        public QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2)
        {
            var entity = CreateEntity(q1, q2, "SUBTRACT");
            try
            {
                var result = ExecuteOperation(q1, q2, (a, b) => {
                    var subMethod = a.GetType().GetMethod("Subtract", new[] { a.GetType() });
                    var resultObj = subMethod.Invoke(a, new object[] { b });
                    
                    double val = (double)resultObj.GetType().GetProperty("Value").GetValue(resultObj);
                    string unit = resultObj.GetType().GetProperty("Unit").GetValue(resultObj).ToString();
                    return new QuantityDTO(val, unit);
                });

                entity.Result = result;
                return result;
            }
            catch (Exception ex)
            {
                entity.ErrorMessage = ex.Message;
                if (ex is QuantityMeasurementException) throw;
                throw new QuantityMeasurementException($"Subtraction failed: {ex.Message}");
            }
            finally
            {
                repository.Save(entity);
            }
        }

        /// <summary>
        /// Divides the first quantity by the second, resulting in a dimensionless ratio.
        /// </summary>
        /// <param name="q1">The first quantity.</param>
        /// <param name="q2">The second quantity.</param>
        /// <returns>The ratio of the two quantities.</returns>
        public double Divide(QuantityDTO q1, QuantityDTO q2)
        {
            var entity = CreateEntity(q1, q2, "DIVIDE");
            try
            {
                double result = ExecuteOperation(q1, q2, (a, b) => {
                    var divMethod = a.GetType().GetMethod("Divide", new[] { a.GetType() });
                    return (double)divMethod.Invoke(a, new object[] { b });
                });

                entity.Result = new QuantityDTO(result, "RATIO");
                return result;
            }
            catch (Exception ex)
            {
                entity.ErrorMessage = ex.Message;
                if (ex is QuantityMeasurementException) throw;
                throw new QuantityMeasurementException($"Division failed: {ex.Message}");
            }
            finally
            {
                repository.Save(entity);
            }
        }

        // -----------------------------------------------------------------------------------------
        // HELPER METHODS
        // -----------------------------------------------------------------------------------------

        private QuantityMeasurementEntity CreateEntity(QuantityDTO q1, QuantityDTO? q2, string op)
        {
            return new QuantityMeasurementEntity(q1, op)
            {
                Operand2 = q2
            };
        }

        private T ExecuteOperation<T>(QuantityDTO q1, QuantityDTO q2, Func<object, object, T> operation)
        {
            if (!TryParseUnit(q1.Unit, out Enum u1, out Type type1) ||
                !TryParseUnit(q2.Unit, out Enum u2, out Type type2))
            {
                throw new QuantityMeasurementException("Invalid or unsupported unit provided.");
            }

            if (type1 != type2)
            {
                throw new QuantityMeasurementException($"Category mismatch: Cannot operate on {type1.Name} and {type2.Name}.");
            }

            try
            {
                var quantityType = typeof(Quantity<>).MakeGenericType(type1);
                var inst1 = Activator.CreateInstance(quantityType, q1.Value, u1);
                var inst2 = Activator.CreateInstance(quantityType, q2.Value, u2);

                return operation(inst1, inst2);
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                throw ex.InnerException ?? ex;
            }
        }

        private T ExecuteSingleOperandOperation<T>(QuantityDTO q, Func<QuantityDTO, T> operation)
        {
            return operation(q);
        }

        private bool TryParseUnit(string unitStr, out Enum unit, out Type enumType)
        {
            if (Enum.TryParse<LengthUnit>(unitStr, true, out var l))
            {
                unit = l;
                enumType = typeof(LengthUnit);
                return true;
            }
            if (Enum.TryParse<WeightUnit>(unitStr, true, out var w))
            {
                unit = w;
                enumType = typeof(WeightUnit);
                return true;
            }
            if (Enum.TryParse<VolumeUnit>(unitStr, true, out var v))
            {
                unit = v;
                enumType = typeof(VolumeUnit);
                return true;
            }
            if (Enum.TryParse<TemperatureUnit>(unitStr, true, out var t))
            {
                unit = t;
                enumType = typeof(TemperatureUnit);
                return true;
            }

            unit = default;
            enumType = default;
            return false;
        }
    }
}

