using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public static class QuantityMeasurement
    {
        // ==================================================
        // ================= LENGTH (UC1–UC8) ===============
        // ==================================================

        // ---------------------------
        // UC1–UC4 : Equality
        // ---------------------------
        public static bool AreEqual(
            double v1, LengthUnit u1,
            double v2, LengthUnit u2)
        {
            var q1 = new QuantityLength(v1, u1);
            var q2 = new QuantityLength(v2, u2);

            return q1.Equals(q2);
        }

        // ---------------------------
        // UC5 : Conversion
        // ---------------------------
        public static double Convert(
            double value, LengthUnit source, LengthUnit target)
        {
            double baseValue = source.ConvertToBaseUnit(value);
            return target.ConvertFromBaseUnit(baseValue);
        }

        // ---------------------------
        // UC6 : Addition (Implicit Target)
        // ---------------------------
        public static QuantityLength Add(
            double v1, LengthUnit u1,
            double v2, LengthUnit u2)
        {
            var q1 = new QuantityLength(v1, u1);
            var q2 = new QuantityLength(v2, u2);

            return q1.Add(q2);
        }

        // ---------------------------
        // UC7 : Addition (Explicit Target)
        // ---------------------------
        public static QuantityLength Add(
            double v1, LengthUnit u1,
            double v2, LengthUnit u2,
            LengthUnit targetUnit)
        {
            var q1 = new QuantityLength(v1, u1);
            var q2 = new QuantityLength(v2, u2);

            return q1.Add(q2, targetUnit);
        }


        // ==================================================
        // ================= WEIGHT (UC9) ===================
        // ==================================================

        // ---------------------------
        // UC9 : Weight Equality
        // ---------------------------
        public static bool AreEqual(
            double v1, WeightUnit u1,
            double v2, WeightUnit u2)
        {
            var q1 = new QuantityWeight(v1, u1);
            var q2 = new QuantityWeight(v2, u2);

            return q1.Equals(q2);
        }

        // ---------------------------
        // UC9 : Weight Conversion
        // ---------------------------
        public static double Convert(
            double value, WeightUnit source, WeightUnit target)
        {
            double baseValue = source.ConvertToBaseUnit(value);
            return target.ConvertFromBaseUnit(baseValue);
        }

        // ---------------------------
        // UC9 : Weight Addition (Implicit)
        // ---------------------------
        public static QuantityWeight Add(
            double v1, WeightUnit u1,
            double v2, WeightUnit u2)
        {
            var q1 = new QuantityWeight(v1, u1);
            var q2 = new QuantityWeight(v2, u2);

            return q1.Add(q2);
        }

        // ---------------------------
        // UC9 : Weight Addition (Explicit)
        // ---------------------------
        public static QuantityWeight Add(
            double v1, WeightUnit u1,
            double v2, WeightUnit u2,
            WeightUnit targetUnit)
        {
            var q1 = new QuantityWeight(v1, u1);
            var q2 = new QuantityWeight(v2, u2);

            return q1.Add(q2, targetUnit);
        }
    }
}
