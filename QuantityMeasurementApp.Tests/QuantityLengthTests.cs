using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    public class QuantityConversionTests
    {
        private const double EPSILON = 1e-6;

        [Test]
        public void TestConversion_FeetToInches()
        {
            Assert.AreEqual(12.0,
                QuantityLength.Convert(1.0, LengthUnit.Feet, LengthUnit.Inch),
                EPSILON);
        }

        [Test]
        public void TestConversion_InchesToFeet()
        {
            Assert.AreEqual(2.0,
                QuantityLength.Convert(24.0, LengthUnit.Inch, LengthUnit.Feet),
                EPSILON);
        }

        [Test]
        public void TestConversion_YardsToInches()
        {
            Assert.AreEqual(36.0,
                QuantityLength.Convert(1.0, LengthUnit.Yards, LengthUnit.Inch),
                EPSILON);
        }

        [Test]
        public void TestConversion_CentimetersToInches()
        {
            Assert.AreEqual(1.0,
                QuantityLength.Convert(2.54, LengthUnit.Centimeters, LengthUnit.Inch),
                1e-4);
        }

        [Test]
        public void TestConversion_RoundTrip_PreservesValue()
        {
            double value = 5.0;

            double converted = QuantityLength.Convert(value, LengthUnit.Feet, LengthUnit.Inch);
            double back = QuantityLength.Convert(converted, LengthUnit.Inch, LengthUnit.Feet);

            Assert.AreEqual(value, back, EPSILON);
        }

        [Test]
        public void TestConversion_ZeroValue()
        {
            Assert.AreEqual(0.0,
                QuantityLength.Convert(0.0, LengthUnit.Feet, LengthUnit.Inch),
                EPSILON);
        }

        [Test]
        public void TestConversion_NegativeValue()
        {
            Assert.AreEqual(-12.0,
                QuantityLength.Convert(-1.0, LengthUnit.Feet, LengthUnit.Inch),
                EPSILON);
        }

        [Test]
        public void TestConversion_InvalidValue_Throws()
        {
            Assert.Throws<System.ArgumentException>(() =>
                QuantityLength.Convert(double.NaN, LengthUnit.Feet, LengthUnit.Inch));
        }
    }
}
