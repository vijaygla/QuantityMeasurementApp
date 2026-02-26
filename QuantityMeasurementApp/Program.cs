using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter first value in feet:");
            string input1 = Console.ReadLine();

            Console.WriteLine("Enter second value in feet:");
            string input2 = Console.ReadLine();

            // Validation – numeric check
            if (double.TryParse(input1, out double value1) &&
                double.TryParse(input2, out double value2))
            {
                Feet feet1 = new Feet(value1);
                Feet feet2 = new Feet(value2);

                bool result = feet1.Equals(feet2);

                Console.WriteLine($"Equal ({result})");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter numeric values only.");
            }
        }
    }
}

