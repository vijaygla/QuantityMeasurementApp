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
        private const double GramToKg = 0.001;
        private const double PoundToKg = 0.453592;

        // Convert to base unit (Kilogram)
        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            return unit switch
            {
                WeightUnit.Kilogram => value,
                WeightUnit.Gram => value * GramToKg,
                WeightUnit.Pound => value * PoundToKg,
                _ => throw new ArgumentException("Unsupported weight unit.")
            };
        }

        // Convert from base unit (Kilogram)
        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            return unit switch
            {
                WeightUnit.Kilogram => baseValue,
                WeightUnit.Gram => baseValue / GramToKg,
                WeightUnit.Pound => baseValue / PoundToKg,
                _ => throw new ArgumentException("Unsupported weight unit.")
            };
        }
    }
}
