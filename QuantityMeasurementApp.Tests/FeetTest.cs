using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    public class FeetTests
    {
        // Test 1 – Same Value
        [Test]
        public void TestEquality_SameValue()
        {
            // Arrange
            Feet feet1 = new Feet(1.0);
            Feet feet2 = new Feet(1.0);

            // Act
            bool result = feet1.Equals(feet2);

            // Assert
            Assert.IsTrue(result, "Expected 1.0 ft to be equal to 1.0 ft");
        }

        // Test 2 – Different Value
        [Test]
        public void TestEquality_DifferentValue()
        {
            Feet feet1 = new Feet(1.0);
            Feet feet2 = new Feet(2.0);

            bool result = feet1.Equals(feet2);

            Assert.IsFalse(result, "Expected 1.0 ft to NOT equal 2.0 ft");
        }

        // Test 3 – Null Comparison
        [Test]
        public void TestEquality_NullComparison()
        {
            Feet feet = new Feet(1.0);

            bool result = feet.Equals(null);

            Assert.IsFalse(result, "Expected comparison with null to return false");
        }

        // Test 4 – Same Reference (Reflexive)
        [Test]
        public void TestEquality_SameReference()
        {
            Feet feet = new Feet(1.0);

            bool result = feet.Equals(feet);

            Assert.IsTrue(result, "Object should be equal to itself");
        }

        // Test 5 – Type Safety
        [Test]
        public void TestEquality_DifferentType()
        {
            Feet feet = new Feet(1.0);
            object obj = 1.0;

            bool result = feet.Equals(obj);

            Assert.IsFalse(result, "Feet object should not equal a different type");
        }
    }
}
