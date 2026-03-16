using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// Test suite for weight (mass) measurement use cases (UC9, UC10, UC12).
    /// Why: To ensure the correctness of weight conversions (Kg, Gram, Pound) and safety checks.
    /// </summary>
    public class QuantityWeightTests
    {
        private const double EPSILON = 1e-6;

        // ================== EQUALITY ==================

        /// <summary>
        /// UC9: Verifies that identical weights in Kilograms are equal.
        /// </summary>
        [Test]
        public void UC9_KgToKg_ShouldBeEqual()
        {
            var w1 = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var w2 = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);

            Assert.IsTrue(w1.Equals(w2));
        }

        /// <summary>
        /// UC9: Verifies equality between Kilograms and Grams.
        /// </summary>
        [Test]
        public void UC9_KgToGram_ShouldBeEqual()
        {
            var kg = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var g = new Quantity<WeightUnit>(1000.0, WeightUnit.Gram);

            Assert.IsTrue(kg.Equals(g));
        }

        /// <summary>
        /// UC9: Verifies equality between Kilograms and Pounds.
        /// </summary>
        [Test]
        public void UC9_KgToPound_ShouldBeEqual()
        {
            var kg = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var lb = new Quantity<WeightUnit>(2.20462262, WeightUnit.Pound);

            Assert.IsTrue(kg.Equals(lb));
        }

        // ================== CONVERSION ==================

        /// <summary>
        /// UC9: Verifies conversion from Kilograms to Grams.
        /// </summary>
        [Test]
        public void UC9_Convert_KgToGram()
        {
            var result = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram)
                .ConvertTo(WeightUnit.Gram);

            Assert.AreEqual(1000.0, result.Value, EPSILON);
        }

        /// <summary>
        /// UC9: Verifies conversion from Pounds to Kilograms.
        /// </summary>
        [Test]
        public void UC9_Convert_PoundToKg()
        {
            var result = new Quantity<WeightUnit>(2.20462, WeightUnit.Pound)
                .ConvertTo(WeightUnit.Kilogram);

            Assert.AreEqual(1.0, result.Value, 1e-4);
        }

        // ================== ADDITION ==================

        /// <summary>
        /// UC9: Verifies addition of Kg and Grams with implicit target (Kg).
        /// </summary>
        [Test]
        public void UC9_Add_Implicit()
        {
            var result = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram)
                .Add(new Quantity<WeightUnit>(1000.0, WeightUnit.Gram));

            Assert.AreEqual(2.0, result.Value, EPSILON);
        }

        /// <summary>
        /// UC9: Verifies addition of Kg and Grams with explicit target (Grams).
        /// </summary>
        [Test]
        public void UC9_Add_Explicit()
        {
            var result = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram)
                .Add(new Quantity<WeightUnit>(1000.0, WeightUnit.Gram), WeightUnit.Gram);

            Assert.AreEqual(2000.0, result.Value, EPSILON);
            Assert.AreEqual(WeightUnit.Gram, result.Unit);
        }

        // ================== VALIDATION ==================

        /// <summary>
        /// UC9: Verifies that creating a quantity with NaN throws ArgumentException.
        /// </summary>
        [Test]
        public void UC9_InvalidValue_ShouldThrow()
        {
            Assert.Throws<System.ArgumentException>(() =>
                new Quantity<WeightUnit>(double.NaN, WeightUnit.Kilogram));
        }

        /// <summary>
        /// UC10: Verifies that quantities from different categories (Weight vs Length) are not equal.
        /// </summary>
        [Test]
        public void UC10_CrossCategory_ShouldBeFalse()
        {
            var weight = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var length = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

            // They cannot be equal even if the numeric base value might coincide.
            Assert.IsFalse(weight.Equals(length));
        }

        // UC12

        /// <summary>
        /// UC12: Verifies subtraction logic.
        /// </summary>
        [Test]
        public void UC12_Subtraction_FeetMinusInches()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(6, LengthUnit.Inch);

            var result = q1.Subtract(q2);

            Assert.AreEqual(9.5, result.Value, 0.01);
        }

        /// <summary>
        /// UC12: Verifies division logic.
        /// </summary>
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
                