using System;
using QuantityMeasurementApp.Models;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n========== QUANTITY MEASUREMENT APP ==========");
            Console.WriteLine("1. UC1-UC4 Length Equality");
            Console.WriteLine("2. UC5 Length Conversion");
            Console.WriteLine("3. UC6 Length Addition (Implicit Unit)");
            Console.WriteLine("4. UC7 Length Addition (Explicit Unit)");
            Console.WriteLine("5. UC8 LengthUnit Direct Conversion");
            Console.WriteLine("6. UC9 Weight Equality");
            Console.WriteLine("7. UC9 Weight Conversion");
            Console.WriteLine("8. UC9 Weight Addition");
            Console.WriteLine("9. UC10 Generic Quantity Demo");
            Console.WriteLine("10. UC11 Volume Equality");
            Console.WriteLine("11. UC11 Volume Conversion");
            Console.WriteLine("12. UC11 Volume Addition");
            Console.WriteLine("13. UC12 Subtraction");
            Console.WriteLine("14. UC12 Division");
            Console.WriteLine("15. UC14 Temperature Equality");
            Console.WriteLine("16. UC14 Temperature Conversion");
            Console.WriteLine("0. Exit");

            Console.Write("\nChoose option: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1: LengthEquality(); break;
                case 2: LengthConversion(); break;
                case 3: LengthAdditionImplicit(); break;
                case 4: LengthAdditionExplicit(); break;
                case 5: LengthUnitDirect(); break;
                case 6: WeightEquality(); break;
                case 7: WeightConversion(); break;
                case 8: WeightAddition(); break;
                case 9: GenericDemo(); break;
                case 10: VolumeEquality(); break;
                case 11: VolumeConversion(); break;
                case 12: VolumeAddition(); break;
                case 13: QuantitySubtraction(); break;
                case 14: QuantityDivision(); break;
                case 15: TemperatureEquality(); break;
                case 16: TemperatureConversion(); break;
                case 0: return;
                default: Console.WriteLine("Invalid choice"); break;
            }
        }
    }

    // =================================================
    // CATEGORY SELECTION
    // =================================================

    static int GetCategory()
    {
        Console.WriteLine("\nSelect Category:");
        Console.WriteLine("1. Length");
        Console.WriteLine("2. Weight");
        Console.WriteLine("3. Volume");

        return Convert.ToInt32(Console.ReadLine());
    }

    // =================================================
    // LENGTH UNIT MENU
    // =================================================

    static LengthUnit GetLengthUnit()
    {
        Console.WriteLine("\nSelect Length Unit:");
        Console.WriteLine("1. Feet");
        Console.WriteLine("2. Inch");
        Console.WriteLine("3. Yards");
        Console.WriteLine("4. Centimeters");

        int choice = Convert.ToInt32(Console.ReadLine());

        return choice switch
        {
            1 => LengthUnit.Feet,
            2 => LengthUnit.Inch,
            3 => LengthUnit.Yards,
            4 => LengthUnit.Centimeters,
            _ => throw new Exception("Invalid Length Unit")
        };
    }

    // =================================================
    // WEIGHT UNIT MENU
    // =================================================

    static WeightUnit GetWeightUnit()
    {
        Console.WriteLine("\nSelect Weight Unit:");
        Console.WriteLine("1. Kilogram");
        Console.WriteLine("2. Gram");
        Console.WriteLine("3. Pound");

        int choice = Convert.ToInt32(Console.ReadLine());

        return choice switch
        {
            1 => WeightUnit.Kilogram,
            2 => WeightUnit.Gram,
            3 => WeightUnit.Pound,
            _ => throw new Exception("Invalid Weight Unit")
        };
    }

    // =================================================
    // VOLUME UNIT MENU
    // =================================================

    static VolumeUnit GetVolumeUnit()
    {
        Console.WriteLine("\nSelect Volume Unit:");
        Console.WriteLine("1. Litre");
        Console.WriteLine("2. Millilitre");
        Console.WriteLine("3. Gallon");

        int choice = Convert.ToInt32(Console.ReadLine());

        return choice switch
        {
            1 => VolumeUnit.Litre,
            2 => VolumeUnit.Millilitre,
            3 => VolumeUnit.Gallon,
            _ => throw new Exception("Invalid Volume Unit")
        };
    }

    // =================================================
    // TEMPERATURE UNIT MENU (UC14)
    // =================================================

    static TemperatureUnit GetTemperatureUnit()
    {
        Console.WriteLine("\nSelect Temperature Unit:");
        Console.WriteLine("1. Celsius");
        Console.WriteLine("2. Fahrenheit");
        Console.WriteLine("3. Kelvin");

        int choice = Convert.ToInt32(Console.ReadLine());

        return choice switch
        {
            1 => TemperatureUnit.Celsius,
            2 => TemperatureUnit.Fahrenheit,
            3 => TemperatureUnit.Kelvin,
            _ => throw new Exception("Invalid Temperature Unit")
        };
    }

    // =================================================
    // UC1–UC4 LENGTH EQUALITY
    // =================================================

    static void LengthEquality()
    {
        double v1 = ReadDouble("Enter first value: ");
        LengthUnit u1 = GetLengthUnit();

        double v2 = ReadDouble("Enter second value: ");
        LengthUnit u2 = GetLengthUnit();

        var q1 = new Quantity<LengthUnit>(v1, u1);
        var q2 = new Quantity<LengthUnit>(v2, u2);

        Console.WriteLine($"Equality Result: {q1.Equals(q2)}");
    }

    // =================================================
    // UC5 LENGTH CONVERSION
    // =================================================

    static void LengthConversion()
    {
        double value = ReadDouble("Enter value: ");

        LengthUnit source = GetLengthUnit();
        LengthUnit target = GetLengthUnit();

        var q = new Quantity<LengthUnit>(value, source);

        Console.WriteLine($"Converted Result: {q.ConvertTo(target)}");
    }

    // =================================================
    // UC6 LENGTH ADDITION
    // =================================================

    static void LengthAdditionImplicit()
    {
        double v1 = ReadDouble("Enter first value: ");
        LengthUnit u1 = GetLengthUnit();

        double v2 = ReadDouble("Enter second value: ");
        LengthUnit u2 = GetLengthUnit();

        var q1 = new Quantity<LengthUnit>(v1, u1);
        var q2 = new Quantity<LengthUnit>(v2, u2);

        Console.WriteLine($"Addition Result: {q1.Add(q2)}");
    }

    // =================================================
    // UC7 LENGTH ADDITION EXPLICIT
    // =================================================

    static void LengthAdditionExplicit()
    {
        double v1 = ReadDouble("Enter first value: ");
        LengthUnit u1 = GetLengthUnit();

        double v2 = ReadDouble("Enter second value: ");
        LengthUnit u2 = GetLengthUnit();

        Console.WriteLine("Select Result Unit:");
        LengthUnit target = GetLengthUnit();

        var q1 = new Quantity<LengthUnit>(v1, u1);
        var q2 = new Quantity<LengthUnit>(v2, u2);

        Console.WriteLine($"Addition Result: {q1.Add(q2, target)}");
    }

    // =================================================
    // UC8 DIRECT CONVERSION
    // =================================================

    static void LengthUnitDirect()
    {
        double value = ReadDouble("Enter inch value: ");

        double feet = LengthUnit.Inch.ConvertToBaseUnit(value);

        Console.WriteLine($"Converted to Feet: {feet}");
    }

    // =================================================
    // UC9 WEIGHT
    // =================================================

    static void WeightEquality()
    {
        double v1 = ReadDouble("Enter first value: ");
        WeightUnit u1 = GetWeightUnit();

        double v2 = ReadDouble("Enter second value: ");
        WeightUnit u2 = GetWeightUnit();

        var w1 = new Quantity<WeightUnit>(v1, u1);
        var w2 = new Quantity<WeightUnit>(v2, u2);

        Console.WriteLine($"Equality Result: {w1.Equals(w2)}");
    }

    static void WeightConversion()
    {
        double value = ReadDouble("Enter value:");

        WeightUnit source = GetWeightUnit();
        WeightUnit target = GetWeightUnit();

        var q = new Quantity<WeightUnit>(value, source);

        Console.WriteLine($"Converted Result: {q.ConvertTo(target)}");
    }

    static void WeightAddition()
    {
        double v1 = ReadDouble("Enter first value:");
        WeightUnit u1 = GetWeightUnit();

        double v2 = ReadDouble("Enter second value:");
        WeightUnit u2 = GetWeightUnit();

        var w1 = new Quantity<WeightUnit>(v1, u1);
        var w2 = new Quantity<WeightUnit>(v2, u2);

        Console.WriteLine($"Addition Result: {w1.Add(w2)}");
    }

    // =====================================================
    // UC10 GENERIC DEMO (User Input)
    // =====================================================

    static void GenericDemo()
    {
        Console.WriteLine("\n===== GENERIC QUANTITY DEMO =====");

        Console.WriteLine("Select Category:");
        Console.WriteLine("1. Length");
        Console.WriteLine("2. Weight");
        Console.WriteLine("3. Volume");

        int category = Convert.ToInt32(Console.ReadLine());

        if (category == 1)
        {
            double v1 = ReadDouble("Enter first length value: ");
            LengthUnit u1 = GetLengthUnit();

            double v2 = ReadDouble("Enter second length value: ");
            LengthUnit u2 = GetLengthUnit();

            Console.WriteLine("Select result unit:");
            LengthUnit target = GetLengthUnit();

            var q1 = new Quantity<LengthUnit>(v1, u1);
            var q2 = new Quantity<LengthUnit>(v2, u2);

            Console.WriteLine($"Result: {q1.Add(q2, target)}");
        }

        else if (category == 2)
        {
            double v1 = ReadDouble("Enter first weight value: ");
            WeightUnit u1 = GetWeightUnit();

            double v2 = ReadDouble("Enter second weight value: ");
            WeightUnit u2 = GetWeightUnit();

            Console.WriteLine("Select result unit:");
            WeightUnit target = GetWeightUnit();

            var q1 = new Quantity<WeightUnit>(v1, u1);
            var q2 = new Quantity<WeightUnit>(v2, u2);

            Console.WriteLine($"Result: {q1.Add(q2, target)}");
        }

        else if (category == 3)
        {
            double v1 = ReadDouble("Enter first volume value: ");
            VolumeUnit u1 = GetVolumeUnit();

            double v2 = ReadDouble("Enter second volume value: ");
            VolumeUnit u2 = GetVolumeUnit();

            Console.WriteLine("Select result unit:");
            VolumeUnit target = GetVolumeUnit();

            var q1 = new Quantity<VolumeUnit>(v1, u1);
            var q2 = new Quantity<VolumeUnit>(v2, u2);

            Console.WriteLine($"Result: {q1.Add(q2, target)}");
        }

        else
        {
            Console.WriteLine("Invalid category.");
        }
    }
    // =================================================
    // UC11 VOLUME
    // =================================================

    static void VolumeEquality()
    {
        double v1 = ReadDouble("Enter first value:");
        VolumeUnit u1 = GetVolumeUnit();

        double v2 = ReadDouble("Enter second value:");
        VolumeUnit u2 = GetVolumeUnit();

        var q1 = new Quantity<VolumeUnit>(v1, u1);
        var q2 = new Quantity<VolumeUnit>(v2, u2);

        Console.WriteLine($"Equality Result: {q1.Equals(q2)}");
    }

    static void VolumeConversion()
    {
        double value = ReadDouble("Enter value:");

        VolumeUnit source = GetVolumeUnit();
        VolumeUnit target = GetVolumeUnit();

        var q = new Quantity<VolumeUnit>(value, source);

        Console.WriteLine($"Converted Result: {q.ConvertTo(target)}");
    }

    static void VolumeAddition()
    {
        double v1 = ReadDouble("Enter first value:");
        VolumeUnit u1 = GetVolumeUnit();

        double v2 = ReadDouble("Enter second value:");
        VolumeUnit u2 = GetVolumeUnit();

        VolumeUnit target = GetVolumeUnit();

        var q1 = new Quantity<VolumeUnit>(v1, u1);
        var q2 = new Quantity<VolumeUnit>(v2, u2);

        Console.WriteLine($"Addition Result: {q1.Add(q2, target)}");
    }

    // =================================================
    // UC14 TEMPERATURE
    // =================================================

    static void TemperatureEquality()
    {
        double v1 = ReadDouble("Enter first temperature: ");
        TemperatureUnit u1 = GetTemperatureUnit();

        double v2 = ReadDouble("Enter second temperature: ");
        TemperatureUnit u2 = GetTemperatureUnit();

        var t1 = new Quantity<TemperatureUnit>(v1, u1);
        var t2 = new Quantity<TemperatureUnit>(v2, u2);

        Console.WriteLine($"Equality Result: {t1.Equals(t2)}");
    }

    static void TemperatureConversion()
    {
        double value = ReadDouble("Enter temperature:");

        TemperatureUnit source = GetTemperatureUnit();
        TemperatureUnit target = GetTemperatureUnit();

        var temp = new Quantity<TemperatureUnit>(value, source);

        Console.WriteLine($"Converted Result: {temp.ConvertTo(target)}");
    }

    // =================================================
    // UC12 GENERIC OPERATIONS
    // =================================================

    static void QuantitySubtraction()
    {
        int category = GetCategory();

        if (category == 1)
            ExecuteSubtraction<LengthUnit>(GetLengthUnit);
        else if (category == 2)
            ExecuteSubtraction<WeightUnit>(GetWeightUnit);
        else if (category == 3)
            ExecuteSubtraction<VolumeUnit>(GetVolumeUnit);
    }

    static void QuantityDivision()
    {
        int category = GetCategory();

        if (category == 1)
            ExecuteDivision<LengthUnit>(GetLengthUnit);
        else if (category == 2)
            ExecuteDivision<WeightUnit>(GetWeightUnit);
        else if (category == 3)
            ExecuteDivision<VolumeUnit>(GetVolumeUnit);
    }

    static void ExecuteSubtraction<U>(Func<U> unitSelector) where U : Enum
    {
        double v1 = ReadDouble("Enter first value:");
        U u1 = unitSelector();

        double v2 = ReadDouble("Enter second value:");
        U u2 = unitSelector();

        var q1 = new Quantity<U>(v1, u1);
        var q2 = new Quantity<U>(v2, u2);

        Console.WriteLine($"Subtraction Result: {q1.Subtract(q2)}");
    }

    static void ExecuteDivision<U>(Func<U> unitSelector) where U : Enum
    {
        double v1 = ReadDouble("Enter first value:");
        U u1 = unitSelector();

        double v2 = ReadDouble("Enter second value:");
        U u2 = unitSelector();

        var q1 = new Quantity<U>(v1, u1);
        var q2 = new Quantity<U>(v2, u2);

        Console.WriteLine($"Division Result: {q1.Divide(q2)}");
    }

    // =================================================
    // COMMON INPUT METHOD
    // =================================================

    static double ReadDouble(string message)
    {
        Console.Write(message);
        return Convert.ToDouble(Console.ReadLine());
    }
}
