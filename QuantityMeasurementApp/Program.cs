using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

class Program
{
    static void Main()
    {
        Console.WriteLine("========= UC3 & UC4 : Equality =========\n");

        Console.WriteLine($"1 Foot == 12 Inches -> " +
            $"{QuantityMeasurement.AreEqual(1.0, LengthUnit.Feet, 12.0, LengthUnit.Inch)}");

        Console.WriteLine($"1 Yard == 3 Feet -> " +
            $"{QuantityMeasurement.AreEqual(1.0, LengthUnit.Yards, 3.0, LengthUnit.Feet)}");

        Console.WriteLine($"1 CM == 0.393701 Inches -> " +
            $"{QuantityMeasurement.AreEqual(1.0, LengthUnit.Centimeters, 0.393701, LengthUnit.Inch)}");


        Console.WriteLine("\n========= UC5 : Conversion =========\n");

        Console.WriteLine($"1 Foot -> Inches = " +
            $"{QuantityMeasurement.Convert(1.0, LengthUnit.Feet, LengthUnit.Inch)}");

        Console.WriteLine($"3 Yards -> Feet = " +
            $"{QuantityMeasurement.Convert(3.0, LengthUnit.Yards, LengthUnit.Feet)}");

        Console.WriteLine($"36 Inches -> Yards = " +
            $"{QuantityMeasurement.Convert(36.0, LengthUnit.Inch, LengthUnit.Yards)}");

        Console.WriteLine($"2.54 CM -> Inches = " +
            $"{QuantityMeasurement.Convert(2.54, LengthUnit.Centimeters, LengthUnit.Inch)}");


        Console.WriteLine("\n========= UC6 : Addition (Implicit Target Unit) =========\n");

        var add1 = QuantityMeasurement.Add(
            1.0, LengthUnit.Feet,
            12.0, LengthUnit.Inch);

        Console.WriteLine($"1 Foot + 12 Inches = {add1}");

        var add2 = QuantityMeasurement.Add(
            5.0, LengthUnit.Feet,
            -2.0, LengthUnit.Feet);

        Console.WriteLine($"5 Feet + (-2 Feet) = {add2}");


        Console.WriteLine("\n========= UC7 : Addition (Explicit Target Unit) =========\n");

        var explicit1 = QuantityMeasurement.Add(
            1.0, LengthUnit.Feet,
            12.0, LengthUnit.Inch,
            LengthUnit.Feet);

        Console.WriteLine($"1 Foot + 12 Inches (in Feet) = {explicit1}");

        var explicit2 = QuantityMeasurement.Add(
            1.0, LengthUnit.Feet,
            12.0, LengthUnit.Inch,
            LengthUnit.Inch);

        Console.WriteLine($"1 Foot + 12 Inches (in Inches) = {explicit2}");

        var explicit3 = QuantityMeasurement.Add(
            1.0, LengthUnit.Feet,
            12.0, LengthUnit.Inch,
            LengthUnit.Yards);

        Console.WriteLine($"1 Foot + 12 Inches (in Yards) = {explicit3}");

        var explicit4 = QuantityMeasurement.Add(
            36.0, LengthUnit.Inch,
            1.0, LengthUnit.Yards,
            LengthUnit.Feet);

        Console.WriteLine($"36 Inches + 1 Yard (in Feet) = {explicit4}");

        var explicit5 = QuantityMeasurement.Add(
            2.54, LengthUnit.Centimeters,
            1.0, LengthUnit.Inch,
            LengthUnit.Centimeters);

        Console.WriteLine($"2.54 CM + 1 Inch (in CM) = {explicit5}");
    }
}
