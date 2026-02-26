using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public static class QuantityMeasurement
    {
        // Equality (UC3/UC4)
        public static bool AreEqual(
            double v1, LengthUnit u1,
            double v2, LengthUnit u2)
        {
            var q1 = new QuantityLength(v1, u1);
            var q2 = new QuantityLength(v2, u2);
            return q1.Equals(q2);
        }

        // Conversion (UC5)
        public static double Convert(
            double value, LengthUnit source, LengthUnit target)
        {
            return QuantityLength.Convert(value, source, target);
        }

        // Addition (UC6)
        public static QuantityLength Add(
            double v1, LengthUnit u1,
            double v2, LengthUnit u2)
        {
            return QuantityLength.Add(v1, u1, v2, u2);
        }
    }
}
