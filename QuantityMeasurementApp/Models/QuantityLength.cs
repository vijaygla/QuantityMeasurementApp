using System;

namespace QuantityMeasurementApp.Models
{
    public class QuantityLength
    {
        private readonly double value;
        private readonly LengthUnit unit;

        private const double CmToInch = 0.393701;   // 1 cm = 0.393701 inches

        public QuantityLength(double value, LengthUnit unit)
        {
            this.value = value;
            this.unit = unit;
        }

        public double Value => value;
        public LengthUnit Unit => unit;

        /// <summary>
        /// Converts all units to base unit (Inches)
        /// </summary>
        private double ConvertToInches()
        {
            return unit switch
            {
                LengthUnit.Inch => value,
                LengthUnit.Feet => value * 12,
                LengthUnit.Yards => value * 36,
                LengthUnit.Centimeters => value * CmToInch,
                _ => throw new ArgumentException("Unsupported unit type")
            };
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (obj == null)
                return false;

            if (this.GetType() != obj.GetType())
                return false;

            QuantityLength other = (QuantityLength)obj;

            double thisInches = this.ConvertToInches();
            double otherInches = other.ConvertToInches();

            return thisInches.CompareTo(otherInches) == 0;
        }

        public override int GetHashCode()
        {
            return ConvertToInches().GetHashCode();
        }

        public override string ToString()
        {
            return $"Quantity({value}, {unit})";
        }
    }
}
