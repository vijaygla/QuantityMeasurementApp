using System;

namespace QuantityMeasurementApp.Models
{
    public enum WeightUnit
    {
        Kilogram,
        Gram,
        Pound
    }

    public static class WeightUnitExtensions
    {
        // Conversion factors relative to base unit (Kilogram)
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

        // Convert value to base unit (Kilogram)
        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        // Convert value from base unit (Kilogram) to this unit
        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }

        public static string GetUnitName(this WeightUnit unit)
        {
            return unit.ToString().ToUpper();
        }
    }
}
