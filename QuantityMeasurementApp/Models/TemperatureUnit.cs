using System;

namespace QuantityMeasurementApp.Models
{
    public enum TemperatureUnit
    {
        Celsius,
        Fahrenheit,
        Kelvin
    }

    public static class TemperatureUnitExtensions
    {
        public static double ConvertToBaseUnit(this TemperatureUnit unit, double value)
        {
            return unit switch
            {
                TemperatureUnit.Celsius => value,
                TemperatureUnit.Fahrenheit => (value - 32) * 5 / 9,
                TemperatureUnit.Kelvin => value - 273.15,
                _ => throw new ArgumentException("Unsupported temperature unit")
            };
        }

        public static double ConvertFromBaseUnit(this TemperatureUnit unit, double baseValue)
        {
            return unit switch
            {
                TemperatureUnit.Celsius => baseValue,
                TemperatureUnit.Fahrenheit => (baseValue * 9 / 5) + 32,
                TemperatureUnit.Kelvin => baseValue + 273.15,
                _ => throw new ArgumentException("Unsupported temperature unit")
            };
        }

        // Temperature does NOT support arithmetic
        public static void ValidateOperationSupport(this TemperatureUnit unit, string operation)
        {
            throw new NotSupportedException(
                $"Temperature does not support {operation} operation.");
        }
    }
}
