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
        private const double InchesPerFoot = 12.0;
        private const double FeetPerYard = 3.0;
        private const double CmPerFoot = 30.48;

        // Convert value from this unit to base unit (Feet)
        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            return unit switch
            {
                LengthUnit.Feet => value,
                LengthUnit.Inch => value / InchesPerFoot,
                LengthUnit.Yards => value * FeetPerYard,
                LengthUnit.Centimeters => value / CmPerFoot,
                _ => throw new ArgumentException("Unsupported unit.")
            };
        }

        // Convert value from base unit (Feet) to this unit
        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            return unit switch
            {
                LengthUnit.Feet => baseValue,
                LengthUnit.Inch => baseValue * InchesPerFoot,
                LengthUnit.Yards => baseValue / FeetPerYard,
                LengthUnit.Centimeters => baseValue * CmPerFoot,
                _ => throw new ArgumentException("Unsupported unit.")
            };
        }
    }
}
