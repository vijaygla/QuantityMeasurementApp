using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    public class InchesTests
    {
        [Test]
        public void TestEquality_SameValue()
        {
            Inches i1 = new Inches(1.0);
            Inches i2 = new Inches(1.0);

            Assert.IsTrue(i1.Equals(i2));
        }

        [Test]
        public void TestEquality_DifferentValue()
        {
            Inches i1 = new Inches(1.0);
            Inches i2 = new Inches(5.0);

            Assert.IsFalse(i1.Equals(i2));
        }

        [Test]
        public void TestEquality_NullComparison()
        {
            Inches i1 = new Inches(1.0);

            Assert.IsFalse(i1.Equals(null));
        }

        [Test]
        public void TestEquality_SameReference()
        {
            Inches i1 = new Inches(1.0);

            Assert.IsTrue(i1.Equals(i1));
        }

        [Test]
        public void TestEquality_DifferentType()
        {
            Inches i1 = new Inches(1.0);
            object obj = 5;

            Assert.IsFalse(i1.Equals(obj));
        }
    }
}
