using System;

namespace QuantityMeasurementApp.Models
{
    public class QuantityLength
    {
        private readonly double value;
        private readonly LengthUnit unit;

        // Conversion constant
        private const double InchesInOneFoot = 12.0;

        public QuantityLength(double value, LengthUnit unit)
        {
            this.value = value;
            this.unit = unit;
        }

        public double Value => value;
        public LengthUnit Unit => unit;

        /// <summary>
        /// Converts any unit into base unit (Feet)
        /// Centralized conversion logic (DRY principle)
        /// </summary>
        private double ConvertToFeet()
        {
            return unit switch
            {
                LengthUnit.Feet => value,
                LengthUnit.Inch => value / InchesInOneFoot,
                _ => throw new ArgumentException("Unsupported unit type")
            };
        }

        public override bool Equals(object obj)
        {
            // Reflexive property
            if (this == obj)
                return true;

            // Null safety
            if (obj == null)
                return false;

            // Type safety
            if (this.GetType() != obj.GetType())
                return false;

            QuantityLength other = (QuantityLength)obj;

            // Convert both values to base unit before comparison
            double thisInFeet = this.ConvertToFeet();
            double otherInFeet = other.ConvertToFeet();

            // Floating point safe comparison
            return thisInFeet.CompareTo(otherInFeet) == 0;
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
