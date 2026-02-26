using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    public class FeetTests
    {
        [Test]
        public void TestEquality_SameValue()
        {
            Feet f1 = new Feet(1.0);
            Feet f2 = new Feet(1.0);

            Assert.IsTrue(f1.Equals(f2));
        }

        [Test]
        public void TestEquality_DifferentValue()
        {
            Feet f1 = new Feet(1.0);
            Feet f2 = new Feet(2.0);

            Assert.IsFalse(f1.Equals(f2));
        }

        [Test]
        public void TestEquality_NullComparison()
        {
            Feet f1 = new Feet(1.0);

            Assert.IsFalse(f1.Equals(null));
        }

        [Test]
        public void TestEquality_SameReference()
        {
            Feet f1 = new Feet(1.0);

            Assert.IsTrue(f1.Equals(f1));
        }

        [Test]
        public void TestEquality_DifferentType()
        {
            Feet f1 = new Feet(1.0);
            object obj = new object();

            Assert.IsFalse(f1.Equals(obj));
        }
    }
}
