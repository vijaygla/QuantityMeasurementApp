using System;
using QuantityMeasurementApp.Controller;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Repository;
using QuantityMeasurementApp.Service;

/// <summary>
/// Entry point for the Quantity Measurement Application.
/// Why: To provide a user-friendly console interface for interacting with the measurement system.
/// How: Uses a simple loop-based UI, manual Dependency Injection (DI) to assemble the layers, and calls the Controller for all operations.
/// </summary>
class Program
{
    // The main controller that orchestrates UI interactions.
    // Why: Keeping it at class level (static) simplifies access within this simple console entry point.
    static QuantityMeasurementController controller = null!;

    /// <summary>
    /// The application's entry method.
    /// How: 
    /// 1. Initializes the repository (SQL persistence), service (business logic), and controller (orchestration).
    /// 2. Sets up a main application loop.
    /// 3. Navigates based on user menu selection.
    /// </summary>
    static void Main()
    {
        // First, ensure the database and table are correctly set up.
        // This handles cases where the user hasn't manually run the schema.sql file.
        QuantityMeasurementApp.Utilities.DatabaseInitializer.Initialize();

        // Setup Dependency Injection (DI) manually.
        // Why: For a small console app, manual DI is simpler than setting up a full container (like Microsoft.Extensions.DependencyInjection).
        // Database Repository is used here to enable persistent storage as requested (UC-16).
        var repository = new QuantityMeasurementDatabaseRepository();
        var service = new QuantityMeasurementServiceImpl(repository);
        controller = new QuantityMeasurementController(service);

        Console.Title = "Quantity Measurement App";

        while (true)
        {
            Console.Clear();
            Console.WriteLine("==================================================");
            Console.WriteLine("          QUANTITY MEASUREMENT SYSTEM             ");
            Console.WriteLine("          (Data Persistence Enabled)              ");
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
                // Navigate to sub-menus based on the choice.
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
                // Final safety net for errors not caught deeper in the stack.
                ShowErrorMessage($"An unexpected error occurred: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }
    }

    /// <summary>
    /// Displays the comparison menu.
    /// Why: To collect two inputs and check for logical equality (e.g., 1 foot vs 12 inches).
    /// </summary>
    private static void CompareMenu()
    {
        Console.WriteLine("\n--- [COMPARE QUANTITIES] ---");
        var q1 = ReadQuantity("first");
        var q2 = ReadQuantity("second");
        controller.PerformComparison(q1, q2);
    }

    /// <summary>
    /// Displays the conversion menu.
    /// Why: To transform a source value (e.g., Celsius) into a target unit (e.g., Fahrenheit).
    /// </summary>
    private static void ConvertMenu()
    {
        Console.WriteLine("\n--- [CONVERT QUANTITY] ---");
        var q1 = ReadQuantity("source");
        
        Console.WriteLine("\nSelect Target Unit:");
        string target = SelectUnit();

        controller.PerformConversion(q1, target);
    }

    /// <summary>
    /// Displays the arithmetic operations menu.
    /// Why: To support adding or subtracting different units within the same category (UC-10, UC-12, etc.).
    /// </summary>
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

    /// <summary>
    /// Helper to read a quantity from the console.
    /// Why: Standardizes how values and units are collected from the user.
    /// </summary>
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

    /// <summary>
    /// Helper to select a unit category and then a specific unit.
    /// How: Uses a two-step selection process (Category -> Unit).
    /// </summary>
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

    /// <summary>
    /// Helper to display and select a value from a specific Enum type.
    /// Why: Avoids hardcoding unit names and uses reflection to build the menu dynamically.
    /// </summary>
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

    /// <summary>
    /// Helper to display error messages in a distinct color.
    /// Why: Visual cues help users identify when they've provided invalid input.
    /// </summary>
    private static void ShowErrorMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nERROR: {message}");
        Console.ResetColor();
    }
}
