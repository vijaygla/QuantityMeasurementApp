using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    public class QuantityLengthTests
    {
        // Same unit equality
        [Test]
        public void TestEquality_FeetToFeet_SameValue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.Feet);
            var q2 = new QuantityLength(1.0, LengthUnit.Feet);

            Assert.IsTrue(q1.Equals(q2));
        }

        [Test]
        public void TestEquality_InchToInch_SameValue()
        {
            var q1 = new QuantityLength(5.0, LengthUnit.Inch);
            var q2 = new QuantityLength(5.0, LengthUnit.Inch);

            Assert.IsTrue(q1.Equals(q2));
        }

        // Cross unit equality
        [Test]
        public void TestEquality_FeetToInch_EquivalentValue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.Feet);
            var q2 = new QuantityLength(12.0, LengthUnit.Inch);

            Assert.IsTrue(q1.Equals(q2));
        }

        [Test]
        public void TestEquality_InchToFeet_EquivalentValue()
        {
            var q1 = new QuantityLength(12.0, LengthUnit.Inch);
            var q2 = new QuantityLength(1.0, LengthUnit.Feet);

            Assert.IsTrue(q1.Equals(q2));
        }

        // Different values
        [Test]
        public void TestEquality_FeetToFeet_DifferentValue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.Feet);
            var q2 = new QuantityLength(2.0, LengthUnit.Feet);

            Assert.IsFalse(q1.Equals(q2));
        }

        [Test]
        public void TestEquality_InchToInch_DifferentValue()
        {
            var q1 = new QuantityLength(10.0, LengthUnit.Inch);
            var q2 = new QuantityLength(5.0, LengthUnit.Inch);

            Assert.IsFalse(q1.Equals(q2));
        }

        // Null handling
        [Test]
        public void TestEquality_NullComparison()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.Feet);

            Assert.IsFalse(q1.Equals(null));
        }

        // Same reference
        [Test]
        public void TestEquality_SameReference()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.Feet);

            Assert.IsTrue(q1.Equals(q1));
        }

        // Symmetry test
        [Test]
        public void TestEquality_SymmetricProperty()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.Feet);
            var q2 = new QuantityLength(12.0, LengthUnit.Inch);

            Assert.IsTrue(q1.Equals(q2));
            Assert.IsTrue(q2.Equals(q1));
        }

        // Transitive test
        [Test]
        public void TestEquality_TransitiveProperty()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.Feet);
            var q2 = new QuantityLength(12.0, LengthUnit.Inch);
            var q3 = new QuantityLength(1.0, LengthUnit.Feet);

            Assert.IsTrue(q1.Equals(q2));
            Assert.IsTrue(q2.Equals(q3));
            Assert.IsTrue(q1.Equals(q3));
        }
    }
}
