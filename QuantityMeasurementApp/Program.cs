using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Is 1 Yard equal to 3 Feet? -> " + $"{QuantityMeasurement.AreEqual(1.0, LengthUnit.Yards, 3.0, LengthUnit.Feet)}");

            Console.WriteLine(
                $"Is 1 Yard equal to 36 Inches? -> " +
                $"{QuantityMeasurement.AreEqual(1.0, LengthUnit.Yards,
                                                36.0, LengthUnit.Inch)}");

            Console.WriteLine(
                $"Is 1 Centimeter equal to 0.393701 Inches? -> " +
                $"{QuantityMeasurement.AreEqual(1.0, LengthUnit.Centimeters,
                                                0.393701, LengthUnit.Inch)}");
        }
    }
}
