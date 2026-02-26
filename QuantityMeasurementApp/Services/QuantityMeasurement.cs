using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public static class QuantityMeasurement
    {
        // ==================================================
        // UC3 / UC4 - Equality
        // ==================================================
        public static bool AreEqual(
            double v1, LengthUnit u1,
            double v2, LengthUnit u2)
        {
            var q1 = new QuantityLength(v1, u1);
            var q2 = new QuantityLength(v2, u2);

            return q1.Equals(q2);
        }

        // ==================================================
        // UC5 - Conversion
        // (Delegates conversion to LengthUnit via QuantityLength)
        // ==================================================
        public static double Convert(
            double value, LengthUnit source, LengthUnit target)
        {
            // Direct enum-based conversion (UC8 pattern)
            double baseValue = source.ConvertToBaseUnit(value);
            return target.ConvertFromBaseUnit(baseValue);
        }

        // ==================================================
        // UC6 - Addition (Implicit Target Unit)
        // ==================================================
        public static QuantityLength Add(
            double v1, LengthUnit u1,
            double v2, LengthUnit u2)
        {
            var q1 = new QuantityLength(v1, u1);
            var q2 = new QuantityLength(v2, u2);

            return q1.Add(q2);
        }

        // ==================================================
        // UC7 - Addition (Explicit Target Unit)
        // ==================================================
        public static QuantityLength Add(
            double v1, LengthUnit u1,
            double v2, LengthUnit u2,
            LengthUnit targetUnit)
        {
            var q1 = new QuantityLength(v1, u1);
            var q2 = new QuantityLength(v2, u2);

            return q1.Add(q2, targetUnit);
        }
    }
}

