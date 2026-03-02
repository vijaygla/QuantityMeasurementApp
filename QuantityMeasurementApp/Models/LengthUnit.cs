using System;

namespace QuantityMeasurementApp.Models
{
    public enum LengthUnit
    {
        Feet,
        Inch,
        Yards,
        Centimeters
    }

    public static class LengthUnitExtensions
    {
        // Conversion factors relative to base unit (Feet)
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

        // Convert value to base unit (Feet)
        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        // Convert value from base unit (Feet) to this unit
        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }

        public static string GetUnitName(this LengthUnit unit)
        {
            return unit.ToString().ToUpper();
        }
    }
}

