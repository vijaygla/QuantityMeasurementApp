using System;

namespace QuantityMeasurementApp.Models
{
    public class Inches
    {
        // Immutable storage of inch value
        private readonly double value;

        public Inches(double value)
        {
            this.value = value;
        }

        public double Value => value;

        public override bool Equals(object obj)
        {
            // Reflexive property
            if (this == obj)
                return true;

            // Null check
            if (obj == null)
                return false;

            // Ensure same type comparison
            if (this.GetType() != obj.GetType())
                return false;

            Inches other = (Inches)obj;

            // Safe floating point comparison
            return this.value.CompareTo(other.value) == 0;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}
