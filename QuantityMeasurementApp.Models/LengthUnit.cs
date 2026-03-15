using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Units of length measurement.
    /// </summary>
    public enum LengthUnit
    {
        Feet,
        Inch,
        Yards,
        Centimeters
    }

    /// <summary>
    /// Provides conversion logic for Length units relative to the base unit (Feet).
    /// </summary>
    public static class LengthUnitExtensions
    {
        /// <summary>
        /// Returns the conversion factor to convert this unit to the base unit (Feet).
        /// </summary>
        public static double GetConversionFactor(this LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.Feet => 1.0,              // Base
                LengthUnit.Inch => 1.0 / 12.0,       // 12 inches = 1 foot
                LengthUnit.Yards => 3.0,             // 1 yard = 3 feet
                LengthUnit.Centimeters => 1.0 / 30.48, // 30.48 cm = 1 foot
                _ => throw new ArgumentException("Unsupported Length Unit")
            };
        }

        /// <summary>
        /// Converts a value from this unit to the base unit (Feet).
        /// </summary>
        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        /// <summary>
        /// Converts a value from the base unit (Feet) to this unit.
        /// </summary>
        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }

        /// <summary>
        /// Returns a uppercase string representation of the unit name.
        /// </summary>
        public static string GetUnitName(this LengthUnit unit)
        {
            return unit.ToString().ToUpper();
        }
    }
}

