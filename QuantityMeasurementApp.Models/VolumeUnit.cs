using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Units of volume measurement.
    /// </summary>
    public enum VolumeUnit
    {
        Litre,
        Millilitre,
        Gallon
    }

    /// <summary>
    /// Provides conversion logic for Volume units relative to the base unit (Litre).
    /// </summary>
    public static class VolumeUnitExtensions
    {
        /// <summary>
        /// Returns the conversion factor to convert this unit to the base unit (Litre).
        /// </summary>
        public static double GetConversionFactor(this VolumeUnit unit)
        {
            return unit switch
            {
                VolumeUnit.Litre => 1.0,         // Base unit
                VolumeUnit.Millilitre => 0.001,  // 1000 ml = 1 litre
                VolumeUnit.Gallon => 3.78541,    // 1 US gallon = 3.78541 litres
                _ => throw new ArgumentException("Invalid Volume Unit")
            };
        }

        /// <summary>
        /// Converts a value from this unit to the base unit (Litre).
        /// </summary>
        public static double ConvertToBaseUnit(this VolumeUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        /// <summary>
        /// Converts a value from the base unit (Litre) to this unit.
        /// </summary>
        public static double ConvertFromBaseUnit(this VolumeUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }

        /// <summary>
        /// Returns a uppercase string representation of the unit name.
        /// </summary>
        public static string GetUnitName(this VolumeUnit unit)
        {
            return unit.ToString().ToUpper();
        }
    }
}
