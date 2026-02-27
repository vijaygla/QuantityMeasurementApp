using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

class Program
{
    static void Main()
    {
        // ================= UC1–UC4 : Length Equality =================
        Console.WriteLine("=== Length Equality ===");
        Console.WriteLine(QuantityMeasurement.AreEqual(1, LengthUnit.Feet, 12, LengthUnit.Inch));
        Console.WriteLine(QuantityMeasurement.AreEqual(1, LengthUnit.Yards, 3, LengthUnit.Feet));
        Console.WriteLine(QuantityMeasurement.AreEqual(1, LengthUnit.Centimeters, 0.393701, LengthUnit.Inch));


        // ================= UC5 : Length Conversion =================
        Console.WriteLine("\n=== Length Conversion ===");
        Console.WriteLine(QuantityMeasurement.Convert(1, LengthUnit.Feet, LengthUnit.Inch));
        Console.WriteLine(QuantityMeasurement.Convert(36, LengthUnit.Inch, LengthUnit.Yards));
        Console.WriteLine(QuantityMeasurement.Convert(2.54, LengthUnit.Centimeters, LengthUnit.Inch));


        // ================= UC6 : Length Addition (Implicit) =================
        Console.WriteLine("\n=== Length Addition (Implicit) ===");
        Console.WriteLine(QuantityMeasurement.Add(1, LengthUnit.Feet, 12, LengthUnit.Inch));
        Console.WriteLine(QuantityMeasurement.Add(5, LengthUnit.Feet, -2, LengthUnit.Feet));


        // ================= UC7 : Length Addition (Explicit) =================
        Console.WriteLine("\n=== Length Addition (Explicit) ===");
        Console.WriteLine(QuantityMeasurement.Add(1, LengthUnit.Feet, 12, LengthUnit.Inch, LengthUnit.Yards));
        Console.WriteLine(QuantityMeasurement.Add(36, LengthUnit.Inch, 1, LengthUnit.Yards, LengthUnit.Feet));


        // ================= UC8 : Enum Responsibility =================
        Console.WriteLine("\n=== LengthUnit Direct Conversion ===");
        Console.WriteLine(LengthUnit.Inch.ConvertToBaseUnit(12));      // 1 foot
        Console.WriteLine(LengthUnit.Yards.ConvertFromBaseUnit(3));   // 1 yard


        // ================= UC9 : Weight =================
        Console.WriteLine("\n=== Weight Equality ===");
        Console.WriteLine(QuantityMeasurement.AreEqual(1, WeightUnit.Kilogram, 1000, WeightUnit.Gram));
        Console.WriteLine(QuantityMeasurement.AreEqual(1, WeightUnit.Kilogram, 2.20462, WeightUnit.Pound));

        Console.WriteLine("\n=== Weight Conversion ===");
        Console.WriteLine(QuantityMeasurement.Convert(1, WeightUnit.Kilogram, WeightUnit.Pound));
        Console.WriteLine(QuantityMeasurement.Convert(500, WeightUnit.Gram, WeightUnit.Kilogram));

        Console.WriteLine("\n=== Weight Addition ===");
        Console.WriteLine(QuantityMeasurement.Add(1, WeightUnit.Kilogram, 1000, WeightUnit.Gram));
        Console.WriteLine(QuantityMeasurement.Add(1, WeightUnit.Kilogram, 1000, WeightUnit.Gram, WeightUnit.Gram));
    }
}
