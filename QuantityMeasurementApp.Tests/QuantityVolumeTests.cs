using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// Test suite for volume measurement use cases (UC11, UC12).
    /// Why: To ensure the correctness of volume conversions (Litre, Gallon, Millilitre) and arithmetic.
    /// </summary>
    public class QuantityVolumeTests
    {
        private const double EPSILON = 1e-6;

        /// <summary>
        /// UC11: Verifies equality between Litres and Millilitres.
        /// </summary>
        [Test]
        public void UC11_LitreToMillilitre_ShouldBeEqual()
        {
            var litre = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var ml = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

            Assert.IsTrue(litre.Equals(ml));
        }

        /// <summary>
        /// UC11: Verifies equality between Gallons and Litres.
        /// </summary>
        [Test]
        public void UC11_GallonToLitre_ShouldBeEqual()
        {
            var gallon = new Quantity<VolumeUnit>(1.0, VolumeUnit.Gallon);
            var litre = new Quantity<VolumeUnit>(3.78541, VolumeUnit.Litre);

            Assert.IsTrue(gallon.Equals(litre));
        }

        /// <summary>
        /// UC11: Verifies explicit conversion from Litre to Millilitre.
        /// </summary>
        [Test]
        public void UC11_Convert_LitreToMillilitre()
        {
            var litre = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

            var result = litre.ConvertTo(VolumeUnit.Millilitre);

            Assert.AreEqual(1000.0, result.Value, EPSILON);
        }

        /// <summary>
        /// UC11: Verifies explicit conversion from Gallon to Litre.
        /// </summary>
        [Test]
        public void UC11_Convert_GallonToLitre()
        {
            var gallon = new Quantity<VolumeUnit>(1.0, VolumeUnit.Gallon);

            var result = gallon.ConvertTo(VolumeUnit.Litre);

            Assert.AreEqual(3.78541, result.Value, 1e-4);
        }

        /// <summary>
        /// UC11: Verifies addition of Litre and Millilitre.
        /// </summary>
        [Test]
        public void UC11_Add_LitreAndMillilitre()
        {
            var litre = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var ml = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

            // 1.0 L + 1000.0 ml = 2.0 L
            var result = litre.Add(ml);

            Assert.AreEqual(2.0, result.Value, EPSILON);
        }

        /// <summary>
        /// UC11: Verifies addition with an explicit target unit.
        /// </summary>
        [Test]
        public void UC11_Add_WithExplicitTarget()
        {
            var litre = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var ml = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

            // 1.0 L + 1000.0 ml = 2000.0 ml
            var result = litre.Add(ml, VolumeUnit.Millilitre);

            Assert.AreEqual(2000.0, result.Value, EPSILON);
        }

        // UC12 - Cross checking logic with length units for variety

        /// <summary>
        /// UC12: Verifies subtraction logic using Length units within Volume tests.
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
        /// UC12: Verifies division logic resulting in a ratio.
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
