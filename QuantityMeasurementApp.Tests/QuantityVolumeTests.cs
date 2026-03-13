using NUnit.Framework;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    public class QuantityVolumeTests
    {
        private const double EPSILON = 1e-6;

        [Test]
        public void UC11_LitreToMillilitre_ShouldBeEqual()
        {
            var litre = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var ml = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

            Assert.IsTrue(litre.Equals(ml));
        }

        [Test]
        public void UC11_GallonToLitre_ShouldBeEqual()
        {
            var gallon = new Quantity<VolumeUnit>(1.0, VolumeUnit.Gallon);
            var litre = new Quantity<VolumeUnit>(3.78541, VolumeUnit.Litre);

            Assert.IsTrue(gallon.Equals(litre));
        }

        [Test]
        public void UC11_Convert_LitreToMillilitre()
        {
            var litre = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

            var result = litre.ConvertTo(VolumeUnit.Millilitre);

            Assert.AreEqual(1000.0, result.Value, EPSILON);
        }

        [Test]
        public void UC11_Convert_GallonToLitre()
        {
            var gallon = new Quantity<VolumeUnit>(1.0, VolumeUnit.Gallon);

            var result = gallon.ConvertTo(VolumeUnit.Litre);

            Assert.AreEqual(3.78541, result.Value, 1e-4);
        }

        [Test]
        public void UC11_Add_LitreAndMillilitre()
        {
            var litre = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var ml = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

            var result = litre.Add(ml);

            Assert.AreEqual(2.0, result.Value, EPSILON);
        }

        [Test]
        public void UC11_Add_WithExplicitTarget()
        {
            var litre = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var ml = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

            var result = litre.Add(ml, VolumeUnit.Millilitre);

            Assert.AreEqual(2000.0, result.Value, EPSILON);
        }
    }
}
