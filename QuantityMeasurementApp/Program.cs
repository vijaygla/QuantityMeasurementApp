using System;
using QuantityMeasurementApp.Controller;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Repository;
using QuantityMeasurementApp.Service;

class Program
{
    static QuantityMeasurementController controller = null!;
    static AuthService authService = null!;

    static bool isLoggedIn = false;
    static string loggedInUser = "";

    static void Main()
    {
        QuantityMeasurementApp.Utilities.DatabaseInitializer.Initialize();

        // 🟢 Redis & RabbitMQ Setup
        var redis = StackExchange.Redis.ConnectionMultiplexer.Connect("localhost");
        var cacheService = new QuantityMeasurementApp.Utilities.RedisCacheService(redis);
        var rabbitProducer = new QuantityMeasurementApp.Service.RabbitMQProducer("localhost", "QuantityQueue");

        // Existing setup
        var repository = new QuantityMeasurementDatabaseRepository();
        var service = new QuantityMeasurementServiceImpl(repository, cacheService, rabbitProducer);
        controller = new QuantityMeasurementController(service);

        // 🔐 Auth setup
        var userRepo = new UserRepository();
        var jwtService = new QuantityMeasurementApp.Utilities.JwtService("THIS_IS_A_SECURE_32_CHARACTER_KEY_!!!");
        authService = new AuthService(userRepo, jwtService);

        Console.Title = "Quantity Measurement App";

        // 🔐 First screen (Login/Register)
        while (!isLoggedIn)
        {
            ShowAuthMenu();
        }

        // ✅ Main App (only after login)
        RunMainApp();
    }

    // 🔐 AUTH MENU
    private static void ShowAuthMenu()
    {
        Console.Clear();
        Console.WriteLine("==================================");
        Console.WriteLine("        AUTHENTICATION MENU        ");
        Console.WriteLine("==================================");
        Console.WriteLine(" 1. Register");
        Console.WriteLine(" 2. Login");
        Console.WriteLine(" 0. Exit");
        Console.WriteLine("==================================");

        Console.Write("\nSelect option: ");
        int choice = int.Parse(Console.ReadLine() ?? "0");

        try
        {
            switch (choice)
            {
                case 1:
                    RegisterUser();
                    break;
                case 2:
                    LoginUser();
                    break;
                case 0:
                    Environment.Exit(0);
                    break;
                default:
                    ShowErrorMessage("Invalid choice");
                    break;
            }
        }
        catch (Exception ex)
        {
            ShowErrorMessage(ex.Message);
        }

        Console.WriteLine("\nPress any key...");
        Console.ReadKey();
    }

    // 🔐 REGISTER
    private static void RegisterUser()
    {
        Console.Write("\nEnter Name: ");
        string name = Console.ReadLine()!;

        Console.Write("Enter Email: ");
        string email = Console.ReadLine()!;

        Console.Write("Enter Password: ");
        string password = Console.ReadLine()!;

        var result = authService.Register(name, email, password);
        Console.WriteLine(result);
    }

    // 🔐 LOGIN
    private static void LoginUser()
    {
        Console.Write("\nEnter Email: ");
        string email = Console.ReadLine()!;

        Console.Write("Enter Password: ");
        string password = Console.ReadLine()!;

        var token = authService.Login(email, password);

        if (!string.IsNullOrEmpty(token))
        {
            isLoggedIn = true;
            loggedInUser = email;
            Console.WriteLine("\nLogin Successful ✅");
        }
    }

    // ✅ MAIN APP (UNCHANGED LOGIC)
    private static void RunMainApp()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("==================================================");
            Console.WriteLine($"   Welcome {loggedInUser}");
            Console.WriteLine("==================================================");
            Console.WriteLine(" 1. Compare Two Quantities");
            Console.WriteLine(" 2. Convert a Quantity");
            Console.WriteLine(" 3. Arithmetic Operations");
            Console.WriteLine(" 4. Logout");
            Console.WriteLine(" 0. Exit");
            Console.WriteLine("==================================================");

            Console.Write("\nSelect an option: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                ShowErrorMessage("Invalid input");
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
                    case 4:
                        isLoggedIn = false;
                        Main(); // 🔁 back to login
                        return;
                    case 0:
                        return;
                    default:
                        ShowErrorMessage("Invalid choice");
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }

            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }
    }

    // ----------- EXISTING METHODS (NO CHANGE) -----------

    private static void CompareMenu()
    {
        Console.WriteLine("\n--- COMPARE ---");
        var q1 = ReadQuantity("first");
        var q2 = ReadQuantity("second");
        controller.PerformComparison(q1, q2);
    }

    private static void ConvertMenu()
    {
        Console.WriteLine("\n--- CONVERT ---");
        var q1 = ReadQuantity("source");
        string target = SelectUnit();
        controller.PerformConversion(q1, target);
    }

    private static void ArithmeticMenu()
    {
        Console.WriteLine("\n--- ARITHMETIC ---");
        Console.WriteLine("1.Add  2.Subtract  3.Divide");

        int op = int.Parse(Console.ReadLine() ?? "0");

        var q1 = ReadQuantity("first");
        var q2 = ReadQuantity("second");

        if (op == 1) controller.PerformAddition(q1, q2);
        else if (op == 2) controller.PerformSubtraction(q1, q2);
        else if (op == 3) controller.PerformDivision(q1, q2);
    }

    private static QuantityDTO ReadQuantity(string label)
    {
        Console.Write($"Enter {label} value: ");
        double value = double.Parse(Console.ReadLine()!);
        string unit = SelectUnit();
        return new QuantityDTO(value, unit);
    }

    private static string SelectUnit()
    {
        Console.WriteLine("\n1.Length 2.Weight 3.Volume 4.Temperature");
        int cat = int.Parse(Console.ReadLine()!);

        return cat switch
        {
            1 => SelectFromEnum<LengthUnit>(),
            2 => SelectFromEnum<WeightUnit>(),
            3 => SelectFromEnum<VolumeUnit>(),
            4 => SelectFromEnum<TemperatureUnit>(),
            _ => throw new Exception("Invalid category")
        };
    }

    private static string SelectFromEnum<T>() where T : Enum
    {
        var names = Enum.GetNames(typeof(T));
        for (int i = 0; i < names.Length; i++)
            Console.WriteLine($"{i + 1}. {names[i]}");

        int choice = int.Parse(Console.ReadLine()!);
        return names[choice - 1];
    }

    private static void ShowErrorMessage(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
}
