using NUnit.Framework;

using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    public class QuantityAllUseCasesTests
    {
        private const double EPSILON = 1e-6;

        //  UC1–UC4 : Length Equality 

        [Test]
        public void UC1_SameUnit_ShouldBeEqual()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

            Assert.IsTrue(q1.Equals(q2));
        }

        [Test]
        public void UC3_CrossUnit_ShouldBeEqual()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);

            Assert.IsTrue(q1.Equals(q2));
        }

        [Test]
        public void UC4_YardToFeet_ShouldBeEqual()
        {
            var yard = new Quantity<LengthUnit>(1.0, LengthUnit.Yards);
            var feet = new Quantity<LengthUnit>(3.0, LengthUnit.Feet);

            Assert.IsTrue(yard.Equals(feet));
        }

        //  UC5 : Conversion 

        [Test]
        public void UC5_Convert_FeetToInches()
        {
            var result = new Quantity<LengthUnit>(1.0, LengthUnit.Feet)
                .ConvertTo(LengthUnit.Inch);

            Assert.AreEqual(12.0, result.Value, EPSILON);
        }

        [Test]
        public void UC5_RoundTrip_ShouldPreserveValue()
        {
            var q = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);

            var inches = q.ConvertTo(LengthUnit.Inch);
            var back = inches.ConvertTo(LengthUnit.Feet);

            Assert.AreEqual(10.0, back.Value, EPSILON);
        }

        //  UC6 : Addition (Implicit) 

        [Test]
        public void UC6_Add_SameUnit()
        {
            var result = new Quantity<LengthUnit>(1.0, LengthUnit.Feet)
                .Add(new Quantity<LengthUnit>(2.0, LengthUnit.Feet));

            Assert.AreEqual(3.0, result.Value, EPSILON);
        }

        [Test]
        public void UC6_Add_CrossUnit()
        {
            var result = new Quantity<LengthUnit>(1.0, LengthUnit.Feet)
                .Add(new Quantity<LengthUnit>(12.0, LengthUnit.Inch));

            Assert.AreEqual(2.0, result.Value, EPSILON);
        }

        //  UC7 : Addition (Explicit) 

        [Test]
        public void UC7_Add_Explicit_Yards()
        {
            var result = new Quantity<LengthUnit>(1.0, LengthUnit.Feet)
                .Add(new Quantity<LengthUnit>(12.0, LengthUnit.Inch), LengthUnit.Yards);

            Assert.AreEqual(0.666666, result.Value, 1e-4);
            Assert.AreEqual(LengthUnit.Yards, result.Unit);
        }

        //  UC8 : Enum Responsibility 

        [Test]
        public void UC8_LengthUnit_BaseConversion()
        {
            double feet = LengthUnit.Inch.ConvertToBaseUnit(12.0);
            Assert.AreEqual(1.0, feet, EPSILON);
        }

        //  UC10 : Generic Commutativity 

        [Test]
        public void UC10_Addition_Commutativity()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);

            var r1 = q1.Add(q2);
            var r2 = q2.Add(q1);

            Assert.AreEqual(
                r1.ConvertTo(LengthUnit.Feet).Value,
                r2.ConvertTo(LengthUnit.Feet).Value,
                EPSILON);
        }
    }
}
