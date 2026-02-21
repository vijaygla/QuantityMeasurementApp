using Xunit;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    public class FeetTests
    {
        [Fact]
        public void testEquality_SameValue()
        {
            Feet f1 = new Feet(1.0);
            Feet f2 = new Feet(1.0);

            Assert.True(f1.Equals(f2));
        }

        [Fact]
        public void testEquality_DifferentValue()
        {
            Feet f1 = new Feet(1.0);
            Feet f2 = new Feet(2.0);

            Assert.False(f1.Equals(f2));
        }

        [Fact]
        public void testEquality_NullComparison()
        {
            Feet f1 = new Feet(1.0);

            Assert.False(f1.Equals(null));
        }

        [Fact]
        public void testEquality_SameReference()
        {
            Feet f1 = new Feet(1.0);

            Assert.True(f1.Equals(f1));
        }

        [Fact]
        public void testEquality_NonNumericInput()
        {
            Feet f1 = new Feet(1.0);

            Assert.False(f1.Equals("Not a Feet Object"));
        }
    }
}