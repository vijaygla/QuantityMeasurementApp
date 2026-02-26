using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public static class QuantityMeasurement
    {
        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            return QuantityLength.Convert(value, source, target);
        }

        public static bool AreEqual(double v1, LengthUnit u1,
                                    double v2, LengthUnit u2)
        {
            var q1 = new QuantityLength(v1, u1);
            var q2 = new QuantityLength(v2, u2);

            return q1.Equals(q2);
        }
    }
}
