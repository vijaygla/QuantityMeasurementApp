using System;
using QuantityMeasurementApp.Controller;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Repository;
using QuantityMeasurementApp.Service;

/// <summary>
/// Entry point for the Quantity Measurement Application.
/// Provides a console-based user interface for performing measurement operations.
/// </summary>
class Program
{
    static QuantityMeasurementController controller;

    static void Main()
    {
        // Setup Dependency Injection manually for this simple console application
        var repository = new QuantityMeasurementCacheRepository();
        var service = new QuantityMeasurementServiceImpl(repository);
        controller = new QuantityMeasurementController(service);

        Console.Title = "Quantity Measurement App";

        while (true)
        {
            Console.Clear();
            Console.WriteLine("==================================================");
            Console.WriteLine("          QUANTITY MEASUREMENT SYSTEM             ");
            Console.WriteLine("==================================================");
            Console.WriteLine(" 1. Compare Two Quantities");
            Console.WriteLine(" 2. Convert a Quantity to Another Unit");
            Console.WriteLine(" 3. Perform Arithmetic Operations (+, -, /)");
            Console.WriteLine(" 0. Exit Application");
            Console.WriteLine("==================================================");

            Console.Write("\nSelect an option: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                ShowErrorMessage("Invalid input. Please enter a number.");
                continue;
            }

            try
            {
                switch (choice)
                {
                    case 1:
                        CompareMenu();
                        break;
                    case 2:
                        ConvertMenu();
                        break;
                    case 3:
                        ArithmeticMenu();
                        break;
                    case 0:
                        Console.WriteLine("Exiting application. Goodbye!");
                        return;
                    default:
                        ShowErrorMessage("Invalid choice. Please select from the menu.");
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"An unexpected error occurred: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }
    }

    private static void CompareMenu()
    {
        Console.WriteLine("\n--- [COMPARE QUANTITIES] ---");
        var q1 = ReadQuantity("first");
        var q2 = ReadQuantity("second");
        controller.PerformComparison(q1, q2);
    }

    private static void ConvertMenu()
    {
        Console.WriteLine("\n--- [CONVERT QUANTITY] ---");
        var q1 = ReadQuantity("source");
        
        Console.WriteLine("\nSelect Target Unit:");
        string target = SelectUnit();

        controller.PerformConversion(q1, target);
    }

    private static void ArithmeticMenu()
    {
        Console.WriteLine("\n--- [ARITHMETIC OPERATIONS] ---");
        Console.WriteLine(" 1. Addition (+)");
        Console.WriteLine(" 2. Subtraction (-)");
        Console.WriteLine(" 3. Division (/)");
        Console.WriteLine(" 0. Back");

        Console.Write("\nSelect operation: ");
        if (!int.TryParse(Console.ReadLine(), out int op) || op == 0) return;

        var q1 = ReadQuantity("first");
        var q2 = ReadQuantity("second");

        switch (op)
        {
            case 1:
                controller.PerformAddition(q1, q2);
                break;
            case 2:
                controller.PerformSubtraction(q1, q2);
                break;
            case 3:
                controller.PerformDivision(q1, q2);
                break;
            default:
                ShowErrorMessage("Invalid operation selected.");
                break;
        }
    }

    private static QuantityDTO ReadQuantity(string label)
    {
        Console.Write($"Enter {label} numeric value: ");
        if (!double.TryParse(Console.ReadLine(), out double value))
        {
            throw new Exception("Invalid numeric value entered.");
        }

        string unit = SelectUnit();
        return new QuantityDTO(value, unit);
    }

    private static string SelectUnit()
    {
        Console.WriteLine("\nSelect Measurement Category:");
        Console.WriteLine(" 1. Length (Feet, Inch, Yards, Centimeters)");
        Console.WriteLine(" 2. Weight (Kilogram, Gram, Pound)");
        Console.WriteLine(" 3. Volume (Litre, Millilitre, Gallon)");
        Console.WriteLine(" 4. Temperature (Celsius, Fahrenheit, Kelvin)");

        Console.Write("\nEnter category: ");
        if (!int.TryParse(Console.ReadLine(), out int category)) throw new Exception("Invalid category selection.");

        return category switch
        {
            1 => SelectFromEnum<LengthUnit>(),
            2 => SelectFromEnum<WeightUnit>(),
            3 => SelectFromEnum<VolumeUnit>(),
            4 => SelectFromEnum<TemperatureUnit>(),
            _ => throw new Exception("Unsupported category.")
        };
    }

    private static string SelectFromEnum<T>() where T : Enum
    {
        Console.WriteLine($"\nSelect {typeof(T).Name}:");
        var names = Enum.GetNames(typeof(T));
        for (int i = 0; i < names.Length; i++)
        {
            Console.WriteLine($" {i + 1}. {names[i]}");
        }

        Console.Write("\nEnter choice: ");
        if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > names.Length)
        {
            throw new Exception($"Invalid {typeof(T).Name} selection.");
        }

        return names[choice - 1];
    }

    private static void ShowErrorMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nERROR: {message}");
        Console.ResetColor();
    }
}
