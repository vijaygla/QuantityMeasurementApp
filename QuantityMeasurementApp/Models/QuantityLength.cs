using System;

namespace QuantityMeasurementApp.Models
{
    public class QuantityLength
    {
        private readonly double value;
        private readonly LengthUnit unit;

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
        // BASE CONVERSION
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
        // INSTANCE METHOD CONVERSION
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

        // -------------------------
        // STATIC API METHOD
        // -------------------------
        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value.");

            QuantityLength q = new QuantityLength(value, source);
            return q.ConvertTo(target).Value;
        }

        // -------------------------
        // EQUALITY OVERRIDE
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
