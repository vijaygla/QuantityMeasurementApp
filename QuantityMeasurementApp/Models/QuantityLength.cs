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

            this.value = value;
            this.unit = unit;
        }

        public double Value => value;
        public LengthUnit Unit => unit;

        // -------------------------
        // Convert to Base Unit (Feet)
        // -------------------------
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

        // -------------------------
        // UC5 - Conversion
        // -------------------------
        public QuantityLength ConvertTo(LengthUnit targetUnit)
        {
            double feetValue = ConvertToFeet();

            double convertedValue = targetUnit switch
            {
                LengthUnit.Feet => feetValue,
                LengthUnit.Inch => feetValue * InchesPerFoot,
                LengthUnit.Yards => feetValue / FeetPerYard,
                LengthUnit.Centimeters => feetValue * CmPerFoot,
                _ => throw new ArgumentException("Unsupported unit.")
            };

            return new QuantityLength(convertedValue, targetUnit);
        }

        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            var q = new QuantityLength(value, source);
            return q.ConvertTo(target).Value;
        }

        // -------------------------
        // UC6 - Addition
        // -------------------------
        public QuantityLength Add(QuantityLength other)
        {
            if (other == null)
                throw new ArgumentException("Second operand cannot be null.");

            double sumInFeet = this.ConvertToFeet() + other.ConvertToFeet();

            double resultValue = this.unit switch
            {
                LengthUnit.Feet => sumInFeet,
                LengthUnit.Inch => sumInFeet * InchesPerFoot,
                LengthUnit.Yards => sumInFeet / FeetPerYard,
                LengthUnit.Centimeters => sumInFeet * CmPerFoot,
                _ => throw new ArgumentException("Unsupported unit.")
            };

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

        // -------------------------
        // UC1–UC4 Equality
        // -------------------------
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
