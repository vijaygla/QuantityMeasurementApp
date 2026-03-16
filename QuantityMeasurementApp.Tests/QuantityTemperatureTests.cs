using NUnit.Framework;
using QuantityMeasurementApp.Models;
using System;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// Test suite for temperature-specific measurement use cases.
    /// Why: Temperature involves non-linear conversions and specific arithmetic restrictions.
    /// How: Verifies Celsius/Fahrenheit/Kelvin equality and ensures invalid operations throw exceptions.
    /// </summary>
    public class QuantityTemperatureTests
    {
        /// <summary>
        /// Verifies that freezing point in Celsius equals freezing point in Fahrenheit.
        /// </summary>
        [Test]
        public void CelsiusToFahrenheit_ShouldBeEqual()
        {
            var c = new Quantity<TemperatureUnit>(0, TemperatureUnit.Celsius);
            var f = new Quantity<TemperatureUnit>(32, TemperatureUnit.Fahrenheit);

            Assert.IsTrue(c.Equals(f));
        }

        /// <summary>
        /// Verifies equality between Celsius and Kelvin (offset by 273.15).
        /// </summary>
        [Test]
        public void CelsiusToKelvin_ShouldBeEqual()
        {
            var c = new Quantity<TemperatureUnit>(0, TemperatureUnit.Celsius);
            var k = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.Kelvin);

            Assert.IsTrue(c.Equals(k));
        }

        /// <summary>
        /// Verifies that adding two temperatures is not allowed and throws NotSupportedException.
        /// </summary>
        [Test]
        public void TemperatureAddition_ShouldThrowException()
        {
            var t1 = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);
            var t2 = new Quantity<TemperatureUnit>(50, TemperatureUnit.Celsius);

            // Temperatures represent intensity/states, so adding them is physically invalid.
            Assert.Throws<NotSupportedException>(() => t1.Add(t2));
        }
    }
}

