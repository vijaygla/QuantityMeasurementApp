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
            var q1 = new Quantity<LengthUnit>(v1, u1);
            var q2 = new Quantity<LengthUnit>(v2, u2);

            return q1.Equals(q2);
        }

        // ---------------------------
        // UC5 : Conversion
        // ---------------------------
        public static double Convert(
            double value, LengthUnit source, LengthUnit target)
        {
            return new Quantity<LengthUnit>(value, source)
                .ConvertTo(target)
                .Value;
        }

        // ---------------------------
        // UC6 : Addition (Implicit Target)
        // ---------------------------
        public static Quantity<LengthUnit> Add(
            double v1, LengthUnit u1,
            double v2, LengthUnit u2)
        {
            var q1 = new Quantity<LengthUnit>(v1, u1);
            var q2 = new Quantity<LengthUnit>(v2, u2);

            return q1.Add(q2);
        }

        // ---------------------------
        // UC7 : Addition (Explicit Target)
        // ---------------------------
        public static Quantity<LengthUnit> Add(
            double v1, LengthUnit u1,
            double v2, LengthUnit u2,
            LengthUnit targetUnit)
        {
            var q1 = new Quantity<LengthUnit>(v1, u1);
            var q2 = new Quantity<LengthUnit>(v2, u2);

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
            var q1 = new Quantity<WeightUnit>(v1, u1);
            var q2 = new Quantity<WeightUnit>(v2, u2);

            return q1.Equals(q2);
        }

        // ---------------------------
        // UC9 : Weight Conversion
        // ---------------------------
        public static double Convert(
            double value, WeightUnit source, WeightUnit target)
        {
            return new Quantity<WeightUnit>(value, source)
                .ConvertTo(target)
                .Value;
        }

        // ---------------------------
        // UC9 : Weight Addition (Implicit)
        // ---------------------------
        public static Quantity<WeightUnit> Add(
            double v1, WeightUnit u1,
            double v2, WeightUnit u2)
        {
            var q1 = new Quantity<WeightUnit>(v1, u1);
            var q2 = new Quantity<WeightUnit>(v2, u2);

            return q1.Add(q2);
        }

        // ---------------------------
        // UC9 : Weight Addition (Explicit)
        // ---------------------------
        public static Quantity<WeightUnit> Add(
            double v1, WeightUnit u1,
            double v2, WeightUnit u2,
            WeightUnit targetUnit)
        {
            var q1 = new Quantity<WeightUnit>(v1, u1);
            var q2 = new Quantity<WeightUnit>(v2, u2);

            return q1.Add(q2, targetUnit);
        }


        // ==================================================
        // ================= UC10 : GENERIC =================
        // ==================================================

        // Fully Generic Equality (Works for Any Unit Category)
        public static bool AreEqual<U>(
            double v1, U u1,
            double v2, U u2) where U : System.Enum
        {
            var q1 = new Quantity<U>(v1, u1);
            var q2 = new Quantity<U>(v2, u2);

            return q1.Equals(q2);
        }

        // Fully Generic Conversion
        public static double Convert<U>(
            double value, U source, U target) where U : System.Enum
        {
            return new Quantity<U>(value, source)
                .ConvertTo(target)
                .Value;
        }

        // Fully Generic Addition (Implicit)
        public static Quantity<U> Add<U>(
            double v1, U u1,
            double v2, U u2) where U : System.Enum
        {
            var q1 = new Quantity<U>(v1, u1);
            var q2 = new Quantity<U>(v2, u2);

            return q1.Add(q2);
        }

        // Fully Generic Addition (Explicit)
        public static Quantity<U> Add<U>(
            double v1, U u1,
            double v2, U u2,
            U targetUnit) where U : System.Enum
        {
            var q1 = new Quantity<U>(v1, u1);
            var q2 = new Quantity<U>(v2, u2);

            return q1.Add(q2, targetUnit);
        }
    }
}

