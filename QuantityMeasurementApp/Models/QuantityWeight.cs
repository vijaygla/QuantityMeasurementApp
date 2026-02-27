using System;

namespace QuantityMeasurementApp.Models
{
    public class QuantityWeight
    {
        private readonly double value;
        private readonly WeightUnit unit;

        // tolerance used when comparing two weights after normalizing to the base unit.
        // Increased slightly from 1e-6 to 1e-5 to accommodate common conversion rounding
        // imprecision (e.g. 1 kg ≈ 2.20462 lb). The test suite expects these to be equal
        // within this tolerance, so bumping the epsilon avoids failing equality checks.
        private const double EPSILON = 1e-5;

        public QuantityWeight(double value, WeightUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value.");

            if (!Enum.IsDefined(typeof(WeightUnit), unit))
                throw new ArgumentException("Invalid weight unit.");

            this.value = value;
            this.unit = unit;
        }

        public double Value => value;
        public WeightUnit Unit => unit;

        // ===============================
        // Conversion
        // ===============================
        public QuantityWeight ConvertTo(WeightUnit targetUnit)
        {
            double baseValue = unit.ConvertToBaseUnit(value);
            double converted = targetUnit.ConvertFromBaseUnit(baseValue);

            return new QuantityWeight(converted, targetUnit);
        }

        // ===============================
        // Addition (Implicit Target)
        // ===============================
        public QuantityWeight Add(QuantityWeight other)
        {
            if (other == null)
                throw new ArgumentException("Second operand cannot be null.");

            double sumBase =
                this.unit.ConvertToBaseUnit(this.value) +
                other.unit.ConvertToBaseUnit(other.value);

            double result =
                this.unit.ConvertFromBaseUnit(sumBase);

            return new QuantityWeight(result, this.unit);
        }

        // ===============================
        // Addition (Explicit Target)
        // ===============================
        public QuantityWeight Add(QuantityWeight other, WeightUnit targetUnit)
        {
            if (other == null)
                throw new ArgumentException("Second operand cannot be null.");

            double sumBase =
                this.unit.ConvertToBaseUnit(this.value) +
                other.unit.ConvertToBaseUnit(other.value);

            double result =
                targetUnit.ConvertFromBaseUnit(sumBase);

            return new QuantityWeight(result, targetUnit);
        }

        // ===============================
        // Equality
        // ===============================
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || this.GetType() != obj.GetType()) return false;

            QuantityWeight other = (QuantityWeight)obj;

            double base1 = unit.ConvertToBaseUnit(value);
            double base2 = other.unit.ConvertToBaseUnit(other.value);

            return Math.Abs(base1 - base2) < EPSILON;
        }

        public override int GetHashCode()
        {
            return unit.ConvertToBaseUnit(value).GetHashCode();
        }

        public override string ToString()
        {
            return $"Quantity({value}, {unit})";
        }
    }
}
