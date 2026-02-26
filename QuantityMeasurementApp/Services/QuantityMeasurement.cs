using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    // Service Layer
    // This class reduces dependency on Program.cs
    // All equality logic is handled here
    public static class QuantityMeasurement
    {
        /// <summary>
        /// Compares two feet values for equality.
        /// </summary>
        /// <param name="value1">First feet value</param>
        /// <param name="value2">Second feet value</param>
        /// <returns>True if equal, otherwise false</returns>
        public static bool AreFeetEqual(double value1, double value2)
        {
            // Create two Feet objects
            Feet feet1 = new Feet(value1);
            Feet feet2 = new Feet(value2);

            // Call overridden Equals method
            return feet1.Equals(feet2);
        }

        /// <summary>
        /// Compares two inch values for equality.
        /// </summary>
        /// <param name="value1">First inch value</param>
        /// <param name="value2">Second inch value</param>
        /// <returns>True if equal, otherwise false</returns>
        public static bool AreInchesEqual(double value1, double value2)
        {
            // Create two Inches objects
            Inches inch1 = new Inches(value1);
            Inches inch2 = new Inches(value2);

            return inch1.Equals(inch2);
        }

        /// <summary>
        /// Validates numeric input safely (optional helper method)
        /// </summary>
        public static bool TryParseInput(string input, out double value)
        {
            return double.TryParse(input, out value);
        }
    }
}
