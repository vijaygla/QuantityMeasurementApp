using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    public class QuantityWeightTests
    {
        private const double EPSILON = 1e-6;

        // ================== EQUALITY ==================

        [Test]
        public void UC9_KgToKg_ShouldBeEqual()
        {
            var w1 = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var w2 = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);

            Assert.IsTrue(w1.Equals(w2));
        }

        [Test]
        public void UC9_KgToGram_ShouldBeEqual()
        {
            var kg = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var g = new Quantity<WeightUnit>(1000.0, WeightUnit.Gram);

            Assert.IsTrue(kg.Equals(g));
        }

        [Test]
        public void UC9_KgToPound_ShouldBeEqual()
        {
            var kg = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var lb = new Quantity<WeightUnit>(2.20462262, WeightUnit.Pound);

            Assert.IsTrue(kg.Equals(lb));
        }

        // ================== CONVERSION ==================

        [Test]
        public void UC9_Convert_KgToGram()
        {
            var result = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram)
                .ConvertTo(WeightUnit.Gram);

            Assert.AreEqual(1000.0, result.Value, EPSILON);
        }

        [Test]
        public void UC9_Convert_PoundToKg()
        {
            var result = new Quantity<WeightUnit>(2.20462, WeightUnit.Pound)
                .ConvertTo(WeightUnit.Kilogram);

            Assert.AreEqual(1.0, result.Value, 1e-4);
        }

        // ================== ADDITION ==================

        [Test]
        public void UC9_Add_Implicit()
        {
            var result = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram)
                .Add(new Quantity<WeightUnit>(1000.0, WeightUnit.Gram));

            Assert.AreEqual(2.0, result.Value, EPSILON);
        }

        [Test]
        public void UC9_Add_Explicit()
        {
            var result = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram)
                .Add(new Quantity<WeightUnit>(1000.0, WeightUnit.Gram), WeightUnit.Gram);

            Assert.AreEqual(2000.0, result.Value, EPSILON);
            Assert.AreEqual(WeightUnit.Gram, result.Unit);
        }

        // ================== VALIDATION ==================

        [Test]
        public void UC9_InvalidValue_ShouldThrow()
        {
            Assert.Throws<System.ArgumentException>(() =>
                new Quantity<WeightUnit>(double.NaN, WeightUnit.Kilogram));
        }

        [Test]
        public void UC10_CrossCategory_ShouldBeFalse()
        {
            var weight = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var length = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

            Assert.IsFalse(weight.Equals(length));
        }

        // UC12
        [Test]
        public void UC12_Subtraction_FeetMinusInches()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(6, LengthUnit.Inch);

            var result = q1.Subtract(q2);

            Assert.AreEqual(9.5, result.Value, 0.01);
        }

        [Test]
        public void UC12_Division_FeetByFeet()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(2, LengthUnit.Feet);

            double result = q1.Divide(q2);

            Assert.AreEqual(5.0, result);
        }
    }
}
                