using System;

namespace QuantityMeasurementApp.Models
{
    public class QuantityLength
    {
        private readonly double value;
        private readonly LengthUnit unit;

        private const double EPSILON = 1e-6;

        public QuantityLength(double value, LengthUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value.");

            if (!Enum.IsDefined(typeof(LengthUnit), unit))
                throw new ArgumentException("Invalid unit.");

            this.value = value;
            this.unit = unit;
        }

        public double Value => value;
        public LengthUnit Unit => unit;

        // =========================================================
        // BASE CONVERSION (Delegated to LengthUnit)
        // =========================================================
        private double ConvertToFeet()
        {
            return unit.ConvertToBaseUnit(value);
        }

        // =========================================================
        // UC5 - CONVERSION
        // =========================================================
        public QuantityLength ConvertTo(LengthUnit targetUnit)
        {
            double baseValue = ConvertToFeet();
            double convertedValue = targetUnit.ConvertFromBaseUnit(baseValue);

            return new QuantityLength(convertedValue, targetUnit);
        }

        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            double baseValue = source.ConvertToBaseUnit(value);
            return target.ConvertFromBaseUnit(baseValue);
        }

        // =========================================================
        // UC6 - ADDITION (Implicit Target Unit)
        // =========================================================
        public QuantityLength Add(QuantityLength other)
        {
            if (other == null)
                throw new ArgumentException("Second operand cannot be null.");

            double sumInFeet =
                this.unit.ConvertToBaseUnit(this.value) +
                other.unit.ConvertToBaseUnit(other.value);

            double resultValue = this.unit.ConvertFromBaseUnit(sumInFeet);

            return new QuantityLength(resultValue, this.unit);
        }

        public static QuantityLength Add(
            double v1, LengthUnit u1,
            double v2, LengthUnit u2)
        {
            var q1 = new QuantityLength(v1, u1);
            var q2 = new QuantityLength(v2, u2);

            return q1.Add(q2);
        }

        // =========================================================
        // UC7 - ADDITION (Explicit Target Unit)
        // =========================================================
        public QuantityLength Add(QuantityLength other, LengthUnit targetUnit)
        {
            if (other == null)
                throw new ArgumentException("Second operand cannot be null.");

            if (!Enum.IsDefined(typeof(LengthUnit), targetUnit))
                throw new ArgumentException("Invalid target unit.");

            double sumInFeet =
                this.unit.ConvertToBaseUnit(this.value) +
                other.unit.ConvertToBaseUnit(other.value);

            double resultValue = targetUnit.ConvertFromBaseUnit(sumInFeet);

            return new QuantityLength(resultValue, targetUnit);
        }

        public static QuantityLength Add(
            double v1, LengthUnit u1,
            double v2, LengthUnit u2,
            LengthUnit targetUnit)
        {
            var q1 = new QuantityLength(v1, u1);
            var q2 = new QuantityLength(v2, u2);

            return q1.Add(q2, targetUnit);
        }

        // =========================================================
        // UC1–UC4 - EQUALITY
        // =========================================================
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || this.GetType() != obj.GetType()) return false;

            QuantityLength other = (QuantityLength)obj;

            double thisFeet = this.unit.ConvertToBaseUnit(this.value);
            double otherFeet = other.unit.ConvertToBaseUnit(other.value);

            return Math.Abs(thisFeet - otherFeet) < EPSILON;
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
