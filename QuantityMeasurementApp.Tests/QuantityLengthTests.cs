using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    public class QuantityAllUseCasesTests
    {
        private const double EPSILON = 1e-6;

        // ============================================================
        // ======================= UC1 TESTS ==========================
        // ============================================================

        [Test]
        public void UC1_Feet_SameValue_ShouldBeEqual()
        {
            var f1 = new QuantityLength(1.0, LengthUnit.Feet);
            var f2 = new QuantityLength(1.0, LengthUnit.Feet);

            Assert.IsTrue(f1.Equals(f2));
        }

        [Test]
        public void UC1_Feet_DifferentValue_ShouldNotBeEqual()
        {
            var f1 = new QuantityLength(1.0, LengthUnit.Feet);
            var f2 = new QuantityLength(2.0, LengthUnit.Feet);

            Assert.IsFalse(f1.Equals(f2));
        }

        // ============================================================
        // ======================= UC2 TESTS ==========================
        // ============================================================

        [Test]
        public void UC2_Inches_SameValue_ShouldBeEqual()
        {
            var i1 = new QuantityLength(5.0, LengthUnit.Inch);
            var i2 = new QuantityLength(5.0, LengthUnit.Inch);

            Assert.IsTrue(i1.Equals(i2));
        }

        // ============================================================
        // ======================= UC3 TESTS ==========================
        // ============================================================

        [Test]
        public void UC3_FeetToInches_ShouldBeEqual()
        {
            var feet = new QuantityLength(1.0, LengthUnit.Feet);
            var inches = new QuantityLength(12.0, LengthUnit.Inch);

            Assert.IsTrue(feet.Equals(inches));
        }

        // ============================================================
        // ======================= UC4 TESTS ==========================
        // ============================================================

        [Test]
        public void UC4_YardToFeet_ShouldBeEqual()
        {
            var yard = new QuantityLength(1.0, LengthUnit.Yards);
            var feet = new QuantityLength(3.0, LengthUnit.Feet);

            Assert.IsTrue(yard.Equals(feet));
        }

        [Test]
        public void UC4_CentimeterToInch_ShouldBeEqual()
        {
            var cm = new QuantityLength(2.54, LengthUnit.Centimeters);
            var inch = new QuantityLength(1.0, LengthUnit.Inch);

            Assert.IsTrue(cm.Equals(inch));
        }

        // ============================================================
        // ======================= UC5 TESTS ==========================
        // ============================================================

        [Test]
        public void UC5_Convert_FeetToInches()
        {
            Assert.AreEqual(12.0,
                QuantityLength.Convert(1.0, LengthUnit.Feet, LengthUnit.Inch),
                EPSILON);
        }

        [Test]
        public void UC5_RoundTrip_ShouldPreserveValue()
        {
            double value = 10.0;

            double inches = QuantityLength.Convert(value, LengthUnit.Feet, LengthUnit.Inch);
            double back = QuantityLength.Convert(inches, LengthUnit.Inch, LengthUnit.Feet);

            Assert.AreEqual(value, back, EPSILON);
        }

        // ============================================================
        // ======================= UC6 TESTS ==========================
        // ============================================================

        [Test]
        public void UC6_Add_SameUnit()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.Feet);
            var q2 = new QuantityLength(2.0, LengthUnit.Feet);

            var result = q1.Add(q2);

            Assert.AreEqual(3.0, result.Value, EPSILON);
        }

        [Test]
        public void UC6_Add_CrossUnit()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.Feet);
            var q2 = new QuantityLength(12.0, LengthUnit.Inch);

            var result = q1.Add(q2);

            Assert.AreEqual(2.0, result.Value, EPSILON);
        }

        // ============================================================
        // ======================= UC7 TESTS ==========================
        // ============================================================

        [Test]
        public void UC7_Add_WithExplicitTarget_Yards()
        {
            var result = QuantityLength.Add(
                1.0, LengthUnit.Feet,
                12.0, LengthUnit.Inch,
                LengthUnit.Yards);

            Assert.AreEqual(0.6666667, result.Value, 1e-4);
            Assert.AreEqual(LengthUnit.Yards, result.Unit);
        }

        [Test]
        public void UC7_Commutativity_WithExplicitTarget()
        {
            var r1 = QuantityLength.Add(
                1.0, LengthUnit.Feet,
                12.0, LengthUnit.Inch,
                LengthUnit.Yards);

            var r2 = QuantityLength.Add(
                12.0, LengthUnit.Inch,
                1.0, LengthUnit.Feet,
                LengthUnit.Yards);

            Assert.AreEqual(r1.Value, r2.Value, EPSILON);
        }

        // ============================================================
        // ======================= UC8 TESTS ==========================
        // Standalone LengthUnit Conversion Responsibility
        // ============================================================

        [Test]
        public void UC8_LengthUnit_ConvertToBase_InchesToFeet()
        {
            double feet = LengthUnit.Inch.ConvertToBaseUnit(12.0);

            Assert.AreEqual(1.0, feet, EPSILON);
        }

        [Test]
        public void UC8_LengthUnit_ConvertFromBase_FeetToInches()
        {
            double inches = LengthUnit.Inch.ConvertFromBaseUnit(1.0);

            Assert.AreEqual(12.0, inches, EPSILON);
        }

        [Test]
        public void UC8_QuantityLength_DelegatesConversion()
        {
            var q = new QuantityLength(1.0, LengthUnit.Feet);
            var converted = q.ConvertTo(LengthUnit.Inch);

            Assert.AreEqual(12.0, converted.Value, EPSILON);
        }

        [Test]
        public void UC8_BackwardCompatibility_UC1ToUC7StillWorks()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.Feet);
            var q2 = new QuantityLength(12.0, LengthUnit.Inch);

            Assert.IsTrue(q1.Equals(q2));
        }

        [Test]
        public void UC8_InvalidValue_ShouldThrow()
        {
            Assert.Throws<System.ArgumentException>(() =>
                new QuantityLength(double.NaN, LengthUnit.Feet));
        }
    }
}
