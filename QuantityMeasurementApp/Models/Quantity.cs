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

        private double ConvertToBase()
        {
            var unitType = unit.GetType();
            var extensionTypeName = $"{unitType.Name}Extensions";
            var extensionType = unitType.Assembly.GetType($"{unitType.Namespace}.{extensionTypeName}");

            if (extensionType == null)
                throw new NotSupportedException($"No extension class found for {unitType.Name}");

            var method = extensionType.GetMethod("ConvertToBaseUnit",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public,
                null,
                new[] { unitType, typeof(double) },
                null);

            if (method == null)
                throw new NotSupportedException($"No ConvertToBaseUnit method found for {unitType.Name}");

            return (double)method.Invoke(null, new object[] { unit, value });
        }

        private double ConvertFromBase(double baseValue, U targetUnit)
        {
            var unitType = targetUnit.GetType();
            var extensionTypeName = $"{unitType.Name}Extensions";
            var extensionType = unitType.Assembly.GetType($"{unitType.Namespace}.{extensionTypeName}");

            if (extensionType == null)
                throw new NotSupportedException($"No extension class found for {unitType.Name}");

            var method = extensionType.GetMethod("ConvertFromBaseUnit",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public,
                null,
                new[] { unitType, typeof(double) },
                null);

            if (method == null)
                throw new NotSupportedException($"No ConvertFromBaseUnit method found for {unitType.Name}");

            return (double)method.Invoke(null, new object[] { targetUnit, baseValue });
        }

        public Quantity<U> ConvertTo(U targetUnit)
        {
            double baseValue = ConvertToBase();
            double converted = ConvertFromBase(baseValue, targetUnit);
            return new Quantity<U>(Math.Round(converted, 6), targetUnit);
        }

        public Quantity<U> Add(Quantity<U> other)
        {
            return Add(other, this.unit);
        }

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            if (other == null)
                throw new ArgumentException("Other quantity cannot be null");

            if (!unit.GetType().Equals(other.unit.GetType()))
                throw new ArgumentException("Category mismatch");

            double sumBase = this.ConvertToBase() + other.ConvertToBase();
            double result = ConvertFromBase(sumBase, targetUnit);

            return new Quantity<U>(Math.Round(result, 6), targetUnit);
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || obj.GetType() != this.GetType()) return false;

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
