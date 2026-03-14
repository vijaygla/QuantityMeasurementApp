using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    public class QuantityTemperatureTests
    {
        [Test]
        public void CelsiusToFahrenheit_ShouldBeEqual()
        {
            var c = new Quantity<TemperatureUnit>(0, TemperatureUnit.Celsius);
            var f = new Quantity<TemperatureUnit>(32, TemperatureUnit.Fahrenheit);

            Assert.IsTrue(c.Equals(f));
        }

        [Test]
        public void CelsiusToKelvin_ShouldBeEqual()
        {
            var c = new Quantity<TemperatureUnit>(0, TemperatureUnit.Celsius);
            var k = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.Kelvin);

            Assert.IsTrue(c.Equals(k));
        }

        [Test]
        public void TemperatureAddition_ShouldThrowException()
        {
            var t1 = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);
            var t2 = new Quantity<TemperatureUnit>(50, TemperatureUnit.Celsius);

            Assert.Throws<NotSupportedException>(() => t1.Add(t2));
        }
    }
}

