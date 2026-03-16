using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// Test suite for length measurement use cases (UC1 to UC8, UC10, UC12).
    /// Why: To ensure the correctness of equality, conversion, and arithmetic for length units.
    /// How: Uses NUnit framework to assert expected outcomes for various unit combinations.
    /// </summary>
    public class QuantityAllUseCasesTests
    {
        // Delta for floating point comparisons.
        private const double EPSILON = 1e-6;

        //  UC1–UC4 : Length Equality 

        /// <summary>
        /// UC1: Verifies that two identical quantities of the same unit are equal.
        /// </summary>
        [Test]
        public void UC1_SameUnit_ShouldBeEqual()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

            Assert.IsTrue(q1.Equals(q2));
        }

        /// <summary>
        /// UC3: Verifies equality between different but equivalent units (Feet and Inches).
        /// </summary>
        [Test]
        public void UC3_CrossUnit_ShouldBeEqual()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);

            Assert.IsTrue(q1.Equals(q2));
        }

        /// <summary>
        /// UC4: Verifies equality between Yards and Feet.
        /// </summary>
        [Test]
        public void UC4_YardToFeet_ShouldBeEqual()
        {
            var yard = new Quantity<LengthUnit>(1.0, LengthUnit.Yards);
            var feet = new Quantity<LengthUnit>(3.0, LengthUnit.Feet);

            Assert.IsTrue(yard.Equals(feet));
        }

        //  UC5 : Conversion 

        /// <summary>
        /// UC5: Verifies that ConvertTo correctly calculates the value in a new unit.
        /// </summary>
        [Test]
        public void UC5_Convert_FeetToInches()
        {
            var result = new Quantity<LengthUnit>(1.0, LengthUnit.Feet)
                .ConvertTo(LengthUnit.Inch);

            Assert.AreEqual(12.0, result.Value, EPSILON);
        }

        /// <summary>
        /// UC5: Verifies that converting back and forth preserves the original value.
        /// </summary>
        [Test]
        public void UC5_RoundTrip_ShouldPreserveValue()
        {
            var q = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);

            var inches = q.ConvertTo(LengthUnit.Inch);
            var back = inches.ConvertTo(LengthUnit.Feet);

            Assert.AreEqual(10.0, back.Value, EPSILON);
        }

        //  UC6 : Addition (Implicit) 

        /// <summary>
        /// UC6: Verifies addition within the same unit.
        /// </summary>
        [Test]
        public void UC6_Add_SameUnit()
        {
            var result = new Quantity<LengthUnit>(1.0, LengthUnit.Feet)
                .Add(new Quantity<LengthUnit>(2.0, LengthUnit.Feet));

            Assert.AreEqual(3.0, result.Value, EPSILON);
        }

        /// <summary>
        /// UC6: Verifies addition across different units (Feet + Inches).
        /// </summary>
        [Test]
        public void UC6_Add_CrossUnit()
        {
            var result = new Quantity<LengthUnit>(1.0, LengthUnit.Feet)
                .Add(new Quantity<LengthUnit>(12.0, LengthUnit.Inch));

            // 1 ft + 12 in = 2 ft
            Assert.AreEqual(2.0, result.Value, EPSILON);
        }

        //  UC7 : Addition (Explicit) 

        /// <summary>
        /// UC7: Verifies addition with an explicit target unit (Feet + Inches -> Yards).
        /// </summary>
        [Test]
        public void UC7_Add_Explicit_Yards()
        {
            var result = new Quantity<LengthUnit>(1.0, LengthUnit.Feet)
                .Add(new Quantity<LengthUnit>(12.0, LengthUnit.Inch), LengthUnit.Yards);

            // 1 ft + 1 ft = 2 ft. 2 ft = 0.666... yards.
            Assert.AreEqual(0.666666, result.Value, 1e-4);
            Assert.AreEqual(LengthUnit.Yards, result.Unit);
        }

        //  UC8 : Enum Responsibility 

        /// <summary>
        /// UC8: Verifies that the extension methods on the unit enum work correctly.
        /// </summary>
        [Test]
        public void UC8_LengthUnit_BaseConversion()
        {
            double feet = LengthUnit.Inch.ConvertToBaseUnit(12.0);
            Assert.AreEqual(1.0, feet, EPSILON);
        }

        //  UC10 : Generic Commutativity 

        /// <summary>
        /// UC10: Verifies that addition is commutative (a+b = b+a).
        /// </summary>
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

        // UC12

        /// <summary>
        /// UC12: Verifies subtraction between different length units.
        /// </summary>
        [Test]
        public void UC12_Subtraction_FeetMinusInches()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(6, LengthUnit.Inch);

            var result = q1.Subtract(q2);

            // 10 ft - 0.5 ft = 9.5 ft
            Assert.AreEqual(9.5, result.Value, 0.01);
        }

        /// <summary>
        /// UC12: Verifies division between quantities, resulting in a scalar ratio.
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
