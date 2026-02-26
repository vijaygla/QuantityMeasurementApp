using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

class Program
{
    static void Main()
    {
        Console.WriteLine($"1 Foot -> Inches: {QuantityMeasurement.Convert(1.0, LengthUnit.Feet, LengthUnit.Inch)}");

        Console.WriteLine($"3 Yards -> Feet: {QuantityMeasurement.Convert(3.0, LengthUnit.Yards, LengthUnit.Feet)}");

        Console.WriteLine($"36 Inches -> Yards: {QuantityMeasurement.Convert(36.0, LengthUnit.Inch, LengthUnit.Yards)}");

        Console.WriteLine($"1 CM -> Inches: {QuantityMeasurement.Convert(1.0, LengthUnit.Centimeters, LengthUnit.Inch)}");

        Console.WriteLine($"0 Feet -> Inches: {QuantityMeasurement.Convert(0.0, LengthUnit.Feet, LengthUnit.Inch)}");
    }
}
