using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Feet f1 = new Feet(1.0);
            Feet f2 = new Feet(1.0);

            bool result = f1.Equals(f2);

            Console.WriteLine("Input: 1.0 ft and 1.0 ft");
            Console.WriteLine($"Output: Equal ({result})");
        }
    }
}
