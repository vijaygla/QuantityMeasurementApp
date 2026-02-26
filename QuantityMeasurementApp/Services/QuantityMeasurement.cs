using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public static class QuantityMeasurement
    {
        // ===============================
        // UC3/UC4 - Equality
        // ===============================
        public static bool AreEqual(
            double v1, LengthUnit u1,
            double v2, LengthUnit u2)
        {
            var q1 = new QuantityLength(v1, u1);
            var q2 = new QuantityLength(v2, u2);

            return q1.Equals(q2);
        }

        // ===============================
        // UC5 - Conversion
        // ===============================
        public static double Convert(
            double value, LengthUnit source, LengthUnit target)
        {
            return QuantityLength.Convert(value, source, target);
        }

        // ===============================
        // UC6 - Addition (Implicit Target)
        // ===============================
        public static QuantityLength Add(
            double v1, LengthUnit u1,
            double v2, LengthUnit u2)
        {
            return QuantityLength.Add(v1, u1, v2, u2);
        }

        // ===============================
        // UC7 - Addition (Explicit Target)
        // ===============================
        public static QuantityLength Add(
            double v1, LengthUnit u1,
            double v2, LengthUnit u2,
            LengthUnit targetUnit)
        {
            return QuantityLength.Add(v1, u1, v2, u2, targetUnit);
        }
    }
}
