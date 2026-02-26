using System;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            bool feetResult = QuantityMeasurement.AreFeetEqual(1.0, 1.0);
            bool inchResult = QuantityMeasurement.AreInchesEqual(1.0, 1.0);

            Console.WriteLine($"1.0 ft and 1.0 ft -> Equal ({feetResult})");
            Console.WriteLine($"1.0 inch and 1.0 inch -> Equal ({inchResult})");
        }
    }
}

