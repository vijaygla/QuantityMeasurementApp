using System;

namespace QuantityMeasurementApp.Models
{
    public class Quantity<U> where U : Enum
    {
        private readonly double value;
        private readonly U unit;

        private const double EPSILON = 1e-6;

        public Quantity(double value, U unit)
        {
            if (unit == null)
                throw new ArgumentException("Unit cannot be null");

            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            this.value = value;
            this.unit = unit;
        }

        public double Value => value;
        public U Unit => unit;

        // -------------------------------------------------
        // ENUM FOR ARITHMETIC OPERATIONS
        // -------------------------------------------------

        private enum ArithmeticOperation
        {
            ADD,
            SUBTRACT,
            DIVIDE
        }

        // -------------------------------------------------
        // INTERNAL BASE CONVERSION (Reflection based)
        // -------------------------------------------------

        private double ConvertToBase()
        {
            var unitType = unit.GetType();
            var extensionType = unitType.Assembly.GetType($"{unitType.Namespace}.{unitType.Name}Extensions");

            if (extensionType == null)
                throw new NotSupportedException($"No extension class for {unitType.Name}");

            var method = extensionType.GetMethod(
                "ConvertToBaseUnit",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            if (method == null)
                throw new NotSupportedException("ConvertToBaseUnit method missing");

            return (double)method.Invoke(null, new object[] { unit, value });
        }

        private double ConvertFromBase(double baseValue, U targetUnit)
        {
            var unitType = targetUnit.GetType();
            var extensionType = unitType.Assembly.GetType($"{unitType.Namespace}.{unitType.Name}Extensions");

            if (extensionType == null)
                throw new NotSupportedException($"No extension class for {unitType.Name}");

            var method = extensionType.GetMethod(
                "ConvertFromBaseUnit",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            if (method == null)
                throw new NotSupportedException("ConvertFromBaseUnit method missing");

            return (double)method.Invoke(null, new object[] { targetUnit, baseValue });
        }

        // -------------------------------------------------
        // VALIDATION
        // -------------------------------------------------

        private void ValidateArithmeticOperands(Quantity<U> other)
        {
            if (other == null)
                throw new ArgumentException("Other quantity cannot be null");

            if (!unit.GetType().Equals(other.unit.GetType()))
                throw new ArgumentException("Measurement category mismatch");
        }

        // -------------------------------------------------
        // CENTRALIZED ARITHMETIC HELPER (UC13)
        // -------------------------------------------------

        private double PerformBaseArithmetic(Quantity<U> other, ArithmeticOperation operation)
        {
            ValidateArithmeticOperands(other);

            double base1 = this.ConvertToBase();
            double base2 = other.ConvertToBase();

            return operation switch
            {
                ArithmeticOperation.ADD => base1 + base2,

                ArithmeticOperation.SUBTRACT => base1 - base2,

                ArithmeticOperation.DIVIDE =>
                    Math.Abs(base2) < EPSILON
                        ? throw new ArithmeticException("Division by zero")
                        : base1 / base2,

                _ => throw new NotSupportedException("Unsupported operation")
            };
        }

        // -------------------------------------------------
        // UC5 / UC10 : CONVERSION
        // -------------------------------------------------

        public Quantity<U> ConvertTo(U targetUnit)
        {
            double baseValue = ConvertToBase();
            double converted = ConvertFromBase(baseValue, targetUnit);

            return new Quantity<U>(Math.Round(converted, 6), targetUnit);
        }

        // -------------------------------------------------
        // UC6 / UC7 : ADDITION
        // -------------------------------------------------

        public Quantity<U> Add(Quantity<U> other)
        {
            return Add(other, this.unit);
        }

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            double baseResult = PerformBaseArithmetic(other, ArithmeticOperation.ADD);

            double result = ConvertFromBase(baseResult, targetUnit);

            return new Quantity<U>(Math.Round(result, 6), targetUnit);
        }

        // -------------------------------------------------
        // UC12 : SUBTRACTION
        // -------------------------------------------------

        public Quantity<U> Subtract(Quantity<U> other)
        {
            return Subtract(other, this.unit);
        }

        public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
        {
            double baseResult = PerformBaseArithmetic(other, ArithmeticOperation.SUBTRACT);

            double result = ConvertFromBase(baseResult, targetUnit);

            return new Quantity<U>(Math.Round(result, 6), targetUnit);
        }

        // -------------------------------------------------
        // UC12 : DIVISION (Dimensionless)
        // -------------------------------------------------

        public double Divide(Quantity<U> other)
        {
            return PerformBaseArithmetic(other, ArithmeticOperation.DIVIDE);
        }

        // -------------------------------------------------
        // UC1–UC4 / UC11 : EQUALITY
        // -------------------------------------------------

        public override bool Equals(object obj)
        {
            if (this == obj) return true;

            if (obj == null || obj.GetType() != this.GetType())
                return false;

            var other = (Quantity<U>)obj;

            if (!unit.GetType().Equals(other.unit.GetType()))
                return false;

            return Math.Abs(this.ConvertToBase() - other.ConvertToBase()) < EPSILON;
        }

        public override int GetHashCode()
        {
            return ConvertToBase().GetHashCode();
        }

        public override string ToString()
        {
            return $"Quantity({value}, {unit})";
        }
    }
}

