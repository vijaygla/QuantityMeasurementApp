using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    public class QuantityWeightTests
    {
        private const double EPSILON = 1e-6;

        // =====================================================
        // ================== EQUALITY TESTS ===================
        // =====================================================

        [Test]
        public void UC9_KilogramToKilogram_SameValue_ShouldBeEqual()
        {
            var w1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var w2 = new QuantityWeight(1.0, WeightUnit.Kilogram);

            Assert.IsTrue(w1.Equals(w2));
        }

        [Test]
        public void UC9_KilogramToGram_Equivalent_ShouldBeEqual()
        {
            var kg = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var g = new QuantityWeight(1000.0, WeightUnit.Gram);

            Assert.IsTrue(kg.Equals(g));
        }

        [Test]
        public void UC9_KilogramToPound_Equivalent_ShouldBeEqual()
        {
            var kg = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var lb = new QuantityWeight(2.20462, WeightUnit.Pound);

            Assert.IsTrue(kg.Equals(lb));
        }

        [Test]
        public void UC9_WeightVsLength_ShouldNotBeEqual()
        {
            var weight = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var length = new QuantityLength(1.0, LengthUnit.Feet);

            Assert.IsFalse(weight.Equals(length));
        }

        // =====================================================
        // ================== CONVERSION TESTS =================
        // =====================================================

        [Test]
        public void UC9_Convert_KilogramToGram()
        {
            var kg = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var grams = kg.ConvertTo(WeightUnit.Gram);

            Assert.AreEqual(1000.0, grams.Value, EPSILON);
        }

        [Test]
        public void UC9_Convert_PoundToKilogram()
        {
            var lb = new QuantityWeight(2.20462, WeightUnit.Pound);
            var kg = lb.ConvertTo(WeightUnit.Kilogram);

            Assert.AreEqual(1.0, kg.Value, 1e-4);
        }

        [Test]
        public void UC9_RoundTripConversion_ShouldPreserveValue()
        {
            var kg = new QuantityWeight(5.0, WeightUnit.Kilogram);

            var grams = kg.ConvertTo(WeightUnit.Gram);
            var back = grams.ConvertTo(WeightUnit.Kilogram);

            Assert.AreEqual(5.0, back.Value, EPSILON);
        }

        [Test]
        public void UC9_Convert_ZeroValue()
        {
            var kg = new QuantityWeight(0.0, WeightUnit.Kilogram);
            var grams = kg.ConvertTo(WeightUnit.Gram);

            Assert.AreEqual(0.0, grams.Value, EPSILON);
        }

        [Test]
        public void UC9_Convert_NegativeValue()
        {
            var kg = new QuantityWeight(-1.0, WeightUnit.Kilogram);
            var grams = kg.ConvertTo(WeightUnit.Gram);

            Assert.AreEqual(-1000.0, grams.Value, EPSILON);
        }

        // =====================================================
        // ================== ADDITION TESTS ===================
        // =====================================================

        [Test]
        public void UC9_Add_SameUnit_KgPlusKg()
        {
            var w1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var w2 = new QuantityWeight(2.0, WeightUnit.Kilogram);

            var result = w1.Add(w2);

            Assert.AreEqual(3.0, result.Value, EPSILON);
            Assert.AreEqual(WeightUnit.Kilogram, result.Unit);
        }

        [Test]
        public void UC9_Add_CrossUnit_KgPlusGram()
        {
            var kg = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var g = new QuantityWeight(1000.0, WeightUnit.Gram);

            var result = kg.Add(g);

            Assert.AreEqual(2.0, result.Value, EPSILON);
        }

        [Test]
        public void UC9_Add_CrossUnit_PoundPlusKilogram()
        {
            var lb = new QuantityWeight(2.20462, WeightUnit.Pound);
            var kg = new QuantityWeight(1.0, WeightUnit.Kilogram);

            var result = lb.Add(kg);

            Assert.AreEqual(4.40924, result.Value, 1e-4);
        }

        [Test]
        public void UC9_Add_WithExplicitTarget()
        {
            var kg = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var g = new QuantityWeight(1000.0, WeightUnit.Gram);

            var result = kg.Add(g, WeightUnit.Gram);

            Assert.AreEqual(2000.0, result.Value, EPSILON);
            Assert.AreEqual(WeightUnit.Gram, result.Unit);
        }

        [Test]
        public void UC9_Add_Commutativity()
        {
            var w1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var w2 = new QuantityWeight(1000.0, WeightUnit.Gram);

            var r1 = w1.Add(w2).ConvertTo(WeightUnit.Kilogram);
            var r2 = w2.Add(w1).ConvertTo(WeightUnit.Kilogram);

            Assert.AreEqual(r1.Value, r2.Value, EPSILON);
        }

        [Test]
        public void UC9_Add_WithZero()
        {
            var w1 = new QuantityWeight(5.0, WeightUnit.Kilogram);
            var w2 = new QuantityWeight(0.0, WeightUnit.Gram);

            var result = w1.Add(w2);

            Assert.AreEqual(5.0, result.Value, EPSILON);
        }

        [Test]
        public void UC9_Add_NegativeValue()
        {
            var w1 = new QuantityWeight(5.0, WeightUnit.Kilogram);
            var w2 = new QuantityWeight(-2000.0, WeightUnit.Gram);

            var result = w1.Add(w2);

            Assert.AreEqual(3.0, result.Value, EPSILON);
        }

        [Test]
        public void UC9_Add_LargeValues()
        {
            var w1 = new QuantityWeight(1e6, WeightUnit.Kilogram);
            var w2 = new QuantityWeight(1e6, WeightUnit.Kilogram);

            var result = w1.Add(w2);

            Assert.AreEqual(2e6, result.Value, EPSILON);
        }

        // =====================================================
        // ================== VALIDATION TESTS =================
        // =====================================================

        [Test]
        public void UC9_NullOperand_ShouldThrow()
        {
            var w1 = new QuantityWeight(1.0, WeightUnit.Kilogram);

            Assert.Throws<System.ArgumentException>(() => w1.Add(null));
        }

        [Test]
        public void UC9_InvalidValue_ShouldThrow()
        {
            Assert.Throws<System.ArgumentException>(() =>
                new QuantityWeight(double.NaN, WeightUnit.Kilogram));
        }
    }
}
