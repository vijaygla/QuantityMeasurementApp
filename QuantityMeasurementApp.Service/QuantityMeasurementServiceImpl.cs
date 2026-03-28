using System;
using System.Text.Json;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Repository;
using QuantityMeasurementApp.Exceptions;
using QuantityMeasurementApp.Utilities;

namespace QuantityMeasurementApp.Service
{
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository repository;
        private readonly RedisCacheService _cache;
        private readonly RabbitMQProducer _producer; // 🔥 RabbitMQ added

        // 🔥 Updated constructor
        public QuantityMeasurementServiceImpl(
            IQuantityMeasurementRepository repo,
            RedisCacheService cache,
            RabbitMQProducer producer)
        {
            repository = repo;
            _cache = cache;
            _producer = producer;
        }

        // 🔥 Convert with Redis + RabbitMQ
        public QuantityDTO Convert(QuantityDTO source, string targetUnit)
        {
            var entity = CreateEntity(source, null, "CONVERT");

            // Unique cache key
            string cacheKey = $"convert_{source.Value}_{source.Unit}_{targetUnit}";

            try
            {
                // 🔍 1. Check Redis cache
                var cachedResult = _cache.GetAsync<QuantityDTO>(cacheKey).Result;
                if (cachedResult != null)
                {
                    return cachedResult; // ⚡ return cached result
                }

                // ❌ Cache miss → perform calculation
                QuantityDTO result = ExecuteSingleOperandOperation(source, (q) =>
                {
                    if (TryParseUnit(source.Unit, out Enum? sourceEnumUnit, out Type? enumType)
                        && sourceEnumUnit != null && enumType != null)
                    {
                        try
                        {
                            var targetEnum = (Enum)Enum.Parse(enumType, targetUnit, true);

                            var quantityType = typeof(Quantity<>).MakeGenericType(enumType);
                            var quantityInstance = Activator.CreateInstance(
                                quantityType,
                                source.Value,
                                (Enum)Enum.Parse(enumType, source.Unit, true)
                            );

                            var convertMethod = quantityType.GetMethod("ConvertTo");
                            var resultQuantity = convertMethod!.Invoke(
                                quantityInstance,
                                new object[] { targetEnum });

                            double val = (double)(resultQuantity!
                                .GetType().GetProperty("Value")!
                                .GetValue(resultQuantity) ?? 0.0);

                            return new QuantityDTO(val, targetUnit);
                        }
                        catch (ArgumentException)
                        {
                            throw new QuantityMeasurementException(
                                $"Target unit '{targetUnit}' is not valid.");
                        }
                    }

                    throw new QuantityMeasurementException(
                        $"Source unit '{source.Unit}' is not recognized.");
                });

                // 💾 2. Store in Redis
                _cache.SetAsync(cacheKey, result).Wait();

                entity.Result = result;

                // 📤 3. Send to RabbitMQ instead of direct DB save
                var message = JsonSerializer.Serialize(entity);
                _producer.SendMessage(message);

                return result;
            }
            catch (Exception ex)
            {
                entity.ErrorMessage = ex.Message;

                // Send error log to RabbitMQ
                var message = JsonSerializer.Serialize(entity);
                _producer.SendMessage(message);

                if (ex is QuantityMeasurementException) throw;
                throw new QuantityMeasurementException($"Conversion failed: {ex.Message}");
            }
        }

        // ---------------- HELPER METHODS ----------------

        private T ExecuteDoubleOperandOperation<T>(QuantityDTO q1, QuantityDTO q2, Func<object, object, T> operation)
        {
            if (TryParseUnit(q1.Unit, out Enum? unit1, out Type? type1) &&
                TryParseUnit(q2.Unit, out Enum? unit2, out Type? type2))
            {
                if (type1 != type2)
                    throw new QuantityMeasurementException($"Cannot operate on {q1.Unit} and {q2.Unit} as they are different measurement types.");

                var quantityType = typeof(Quantity<>).MakeGenericType(type1!);
                var instance1 = Activator.CreateInstance(quantityType, q1.Value, unit1);
                var instance2 = Activator.CreateInstance(quantityType, q2.Value, unit2);

                return operation(instance1!, instance2!);
            }
            throw new QuantityMeasurementException($"Invalid units: {q1.Unit} or {q2.Unit}");
        }

        public bool Compare(QuantityDTO q1, QuantityDTO q2)
        {
            return ExecuteDoubleOperandOperation(q1, q2, (inst1, inst2) =>
            {
                var equalsMethod = inst1.GetType().GetMethod("Equals");
                return (bool)equalsMethod!.Invoke(inst1, new[] { inst2 })!;
            });
        }

        public QuantityDTO Add(QuantityDTO q1, QuantityDTO q2)
        {
            return ExecuteDoubleOperandOperation(q1, q2, (inst1, inst2) =>
            {
                var addMethod = inst1.GetType().GetMethod("Add", new[] { inst1.GetType() });
                var resultQuantity = addMethod!.Invoke(inst1, new[] { inst2 });
                double val = (double)resultQuantity!.GetType().GetProperty("Value")!.GetValue(resultQuantity)!;
                string unit = resultQuantity.GetType().GetProperty("Unit")!.GetValue(resultQuantity)!.ToString()!;
                return new QuantityDTO(val, unit);
            });
        }

        public QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2)
        {
            return ExecuteDoubleOperandOperation(q1, q2, (inst1, inst2) =>
            {
                var subMethod = inst1.GetType().GetMethod("Subtract", new[] { inst1.GetType() });
                var resultQuantity = subMethod!.Invoke(inst1, new[] { inst2 });
                double val = (double)resultQuantity!.GetType().GetProperty("Value")!.GetValue(resultQuantity)!;
                string unit = resultQuantity.GetType().GetProperty("Unit")!.GetValue(resultQuantity)!.ToString()!;
                return new QuantityDTO(val, unit);
            });
        }

        public double Divide(QuantityDTO q1, QuantityDTO q2)
        {
            return ExecuteDoubleOperandOperation(q1, q2, (inst1, inst2) =>
            {
                var divMethod = inst1.GetType().GetMethod("Divide", new[] { inst1.GetType() });
                return (double)divMethod!.Invoke(inst1, new[] { inst2 })!;
            });
        }

        private QuantityMeasurementEntity CreateEntity(QuantityDTO q1, QuantityDTO? q2, string op)
        {
            return new QuantityMeasurementEntity(q1, op)
            {
                Operand2 = q2
            };
        }

        private T ExecuteSingleOperandOperation<T>(QuantityDTO q, Func<QuantityDTO, T> operation)
        {
            return operation(q);
        }

        private bool TryParseUnit(string unitStr, out Enum? unit, out Type? enumType)
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

            unit = null;
            enumType = null;
            return false;
        }
    }
}
