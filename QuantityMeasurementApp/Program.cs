using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            bool result1 = QuantityMeasurement.AreEqual(
                1.0, LengthUnit.Feet,
                12.0, LengthUnit.Inch);

            bool result2 = QuantityMeasurement.AreEqual(
                1.0, LengthUnit.Inch,
                1.0, LengthUnit.Inch);

            Console.WriteLine($"1 ft and 12 inches -> Equal ({result1})");
            Console.WriteLine($"1 inch and 1 inch -> Equal ({result2})");
        }
    }
}
