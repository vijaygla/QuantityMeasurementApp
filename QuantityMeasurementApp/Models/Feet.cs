using System;

namespace QuantityMeasurementApp.Models
{
    public class Feet
    {
        // Step 3 – Encapsulation with immutability
        private readonly double value;

        // Step 4 – Constructor
        public Feet(double value)
        {
            this.value = value;
        }

        // Property to read value (optional but good practice)
        public double Value
        {
            get { return value; }
        }

        // Step 5 – Override Equals()
        public override bool Equals(object obj)
        {
            // Reflexive check
            if (this == obj)
                return true;

            // Null check
            if (obj == null)
                return false;

            // Type check
            if (this.GetType() != obj.GetType())
                return false;

            Feet other = (Feet)obj;

            // Floating-point comparison
            return this.value.CompareTo(other.value) == 0;
        }

        // Best practice: override GetHashCode when overriding Equals
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}
