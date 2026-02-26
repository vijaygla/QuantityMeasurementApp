using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public static class QuantityMeasurement
    {
        public static bool AreEqual(double value1, LengthUnit unit1,
                                    double value2, LengthUnit unit2)
        {
            QuantityLength q1 = new QuantityLength(value1, unit1);
            QuantityLength q2 = new QuantityLength(value2, unit2);

            return q1.Equals(q2);
        }
    }
}
