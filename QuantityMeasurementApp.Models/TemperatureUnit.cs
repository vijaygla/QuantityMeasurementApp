using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Units of temperature measurement.
    /// </summary>
    public enum TemperatureUnit
    {
        Celsius,
        Fahrenheit,
        Kelvin
    }

    /// <summary>
    /// Provides conversion logic for Temperature units relative to the base unit (Celsius).
    /// </summary>
    public static class TemperatureUnitExtensions
    {
        /// <summary>
        /// Converts a value from this unit to the base unit (Celsius).
        /// </summary>
        public static double ConvertToBaseUnit(this TemperatureUnit unit, double value)
        {
            return unit switch
            {
                TemperatureUnit.Celsius => value,
                TemperatureUnit.Fahrenheit => (value - 32) * 5.0 / 9.0,
                TemperatureUnit.Kelvin => value - 273.15,
                _ => throw new ArgumentException("Unsupported temperature unit")
            };
        }

        /// <summary>
        /// Converts a value from the base unit (Celsius) to this unit.
        /// </summary>
        public static double ConvertFromBaseUnit(this TemperatureUnit unit, double baseValue)
        {
            return unit switch
            {
                TemperatureUnit.Celsius => baseValue,
                TemperatureUnit.Fahrenheit => (baseValue * 9.0 / 5.0) + 32,
                TemperatureUnit.Kelvin => baseValue + 273.15,
                _ => throw new ArgumentException("Unsupported temperature unit")
            };
        }

        /// <summary>
        /// Validates whether a specific arithmetic operation is supported for Temperature.
        /// Temperature generally does not support direct addition or subtraction of absolute values.
        /// </summary>
        public static void ValidateOperationSupport(this TemperatureUnit unit, string operation)
        {
            if (operation.Equals("ADD", StringComparison.OrdinalIgnoreCase) || 
                operation.Equals("SUBTRACT", StringComparison.OrdinalIgnoreCase))
            {
                throw new NotSupportedException(
                    $"Temperature unit ({unit}) does not support {operation} operations because absolute temperature addition/subtraction is not physically meaningful.");
            }
        }
    }
}
