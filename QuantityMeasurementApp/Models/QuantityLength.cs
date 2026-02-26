using System;

namespace QuantityMeasurementApp.Models
{
    public class QuantityLength
    {
        private readonly double value;
        private readonly LengthUnit unit;

        // Conversion constants
        private const double InchesPerFoot = 12.0;
        private const double FeetPerYard = 3.0;
        private const double CmPerFoot = 30.48;

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
        // BASE CONVERSION
        // =========================================================
        private double ConvertToFeet()
        {
            return unit switch
            {
                LengthUnit.Feet => value,
                LengthUnit.Inch => value / InchesPerFoot,
                LengthUnit.Yards => value * FeetPerYard,
                LengthUnit.Centimeters => value / CmPerFoot,
                _ => throw new ArgumentException("Unsupported unit.")
            };
        }

        private QuantityLength ConvertFromFeet(double feetValue, LengthUnit targetUnit)
        {
            double resultValue = targetUnit switch
            {
                LengthUnit.Feet => feetValue,
                LengthUnit.Inch => feetValue * InchesPerFoot,
                LengthUnit.Yards => feetValue / FeetPerYard,
                LengthUnit.Centimeters => feetValue * CmPerFoot,
                _ => throw new ArgumentException("Unsupported unit.")
            };

            return new QuantityLength(resultValue, targetUnit);
        }

        // =========================================================
        // UC5 - CONVERSION
        // =========================================================
        public QuantityLength ConvertTo(LengthUnit targetUnit)
        {
            double feetValue = ConvertToFeet();
            return ConvertFromFeet(feetValue, targetUnit);
        }

        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            var q = new QuantityLength(value, source);
            return q.ConvertTo(target).Value;
        }

        // =========================================================
        // UC6 - ADDITION (Result in First Operand Unit)
        // =========================================================
        public QuantityLength Add(QuantityLength other)
        {
            if (other == null)
                throw new ArgumentException("Second operand cannot be null.");

            double sumInFeet = AddInFeet(other);

            return ConvertFromFeet(sumInFeet, this.unit);
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
        // UC7 - ADDITION WITH EXPLICIT TARGET UNIT
        // =========================================================
        public QuantityLength Add(QuantityLength other, LengthUnit targetUnit)
        {
            if (other == null)
                throw new ArgumentException("Second operand cannot be null.");

            if (!Enum.IsDefined(typeof(LengthUnit), targetUnit))
                throw new ArgumentException("Invalid target unit.");

            double sumInFeet = AddInFeet(other);

            return ConvertFromFeet(sumInFeet, targetUnit);
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
        // PRIVATE UTILITY FOR ADDITION
        // =========================================================
        private double AddInFeet(QuantityLength other)
        {
            return this.ConvertToFeet() + other.ConvertToFeet();
        }

        // =========================================================
        // UC1–UC4 - EQUALITY
        // =========================================================
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || this.GetType() != obj.GetType()) return false;

            QuantityLength other = (QuantityLength)obj;

            return Math.Abs(this.ConvertToFeet() - other.ConvertToFeet()) < EPSILON;
        }

        public override int GetHashCode()
        {
            return ConvertToFeet().GetHashCode();
        }

        public override string ToString()
        {
            return $"Quantity({value}, {unit})";
        }
    }
}
