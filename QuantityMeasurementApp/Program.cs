using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

class Program
{
    static void Main()
    {
        Console.WriteLine("========= Equality =========\n");

        Console.WriteLine($"1 Foot == 12 Inches -> " +
            $"{QuantityMeasurement.AreEqual(1.0, LengthUnit.Feet, 12.0, LengthUnit.Inch)}");

        Console.WriteLine($"1 Yard == 3 Feet -> " +
            $"{QuantityMeasurement.AreEqual(1.0, LengthUnit.Yards, 3.0, LengthUnit.Feet)}");

        Console.WriteLine($"1 CM == 0.393701 Inches -> " +
            $"{QuantityMeasurement.AreEqual(1.0, LengthUnit.Centimeters, 0.393701, LengthUnit.Inch)}");


        Console.WriteLine("\n========= Conversion =========\n");

        Console.WriteLine($"1 Foot -> Inches = " +
            $"{QuantityMeasurement.Convert(1.0, LengthUnit.Feet, LengthUnit.Inch)}");

        Console.WriteLine($"3 Yards -> Feet = " +
            $"{QuantityMeasurement.Convert(3.0, LengthUnit.Yards, LengthUnit.Feet)}");


        Console.WriteLine("\n========= Addition =========\n");

        var result1 = QuantityMeasurement.Add(
            1.0, LengthUnit.Feet,
            12.0, LengthUnit.Inch);

        Console.WriteLine($"1 Foot + 12 Inches = {result1}");

        var result2 = QuantityMeasurement.Add(
            5.0, LengthUnit.Feet,
            -2.0, LengthUnit.Feet);

        Console.WriteLine($"5 Feet + (-2 Feet) = {result2}");
    }
}
