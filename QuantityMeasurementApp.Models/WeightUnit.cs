using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Units of weight (mass) measurement.
    /// </summary>
    public enum WeightUnit
    {
        Kilogram,
        Gram,
        Pound
    }

    /// <summary>
    /// Provides conversion logic for Weight units relative to the base unit (Kilogram).
    /// </summary>
    public static class WeightUnitExtensions
    {
        /// <summary>
        /// Returns the conversion factor to convert this unit to the base unit (Kilogram).
        /// </summary>
        public static double GetConversionFactor(this WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.Kilogram => 1.0,      // Base unit
                WeightUnit.Gram => 0.001,        // 1 g = 0.001 kg
                WeightUnit.Pound => 0.453592,    // 1 lb = 0.453592 kg
                _ => throw new ArgumentException("Unsupported Weight Unit")
            };
        }

        /// <summary>
        /// Converts a value from this unit to the base unit (Kilogram).
        /// </summary>
        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        /// <summary>
        /// Converts a value from the base unit (Kilogram) to this unit.
        /// </summary>
        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }

        /// <summary>
        /// Returns a uppercase string representation of the unit name.
        /// </summary>
        public static string GetUnitName(this WeightUnit unit)
        {
            return unit.ToString().ToUpper();
        }
    }
}
