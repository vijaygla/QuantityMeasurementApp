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
            Console.WriteLine("0. Exit");
            Console.Write("Choose option: ");

            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    LengthEquality();
                    break;

                case 2:
                    LengthConversion();
                    break;

                case 3:
                    LengthAdditionImplicit();
                    break;

                case 4:
                    LengthAdditionExplicit();
                    break;

                case 5:
                    LengthUnitDirect();
                    break;

                case 6:
                    WeightEquality();
                    break;

                case 7:
                    WeightConversion();
                    break;

                case 8:
                    WeightAddition();
                    break;

                case 9:
                    GenericDemo();
                    break;

                case 0:
                    return;

                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }
    }

    // ================= UC1–UC4 =================
    static void LengthEquality()
    {
        Console.Write("Enter first value: ");
        double v1 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter unit (Feet/Inch/Yards/Centimeters): ");
        LengthUnit u1 = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

        Console.Write("Enter second value: ");
        double v2 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter unit (Feet/Inch/Yards/Centimeters): ");
        LengthUnit u2 = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

        var q1 = new Quantity<LengthUnit>(v1, u1);
        var q2 = new Quantity<LengthUnit>(v2, u2);

        Console.WriteLine($"Result: {q1.Equals(q2)}");
    }

    // ================= UC5 =================
    static void LengthConversion()
    {
        Console.Write("Enter value: ");
        double value = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter source unit: ");
        LengthUnit source = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

        Console.Write("Enter target unit: ");
        LengthUnit target = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

        var q = new Quantity<LengthUnit>(value, source);
        var result = q.ConvertTo(target);

        Console.WriteLine($"Converted: {result}");
    }

    // ================= UC6 =================
    static void LengthAdditionImplicit()
    {
        Console.Write("Enter first value: ");
        double v1 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter first unit: ");
        LengthUnit u1 = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

        Console.Write("Enter second value: ");
        double v2 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter second unit: ");
        LengthUnit u2 = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

        var q1 = new Quantity<LengthUnit>(v1, u1);
        var q2 = new Quantity<LengthUnit>(v2, u2);

        var result = q1.Add(q2);

        Console.WriteLine($"Result: {result}");
    }

    // ================= UC7 =================
    static void LengthAdditionExplicit()
    {
        Console.Write("Enter first value: ");
        double v1 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter first unit: ");
        LengthUnit u1 = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

        Console.Write("Enter second value: ");
        double v2 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter second unit: ");
        LengthUnit u2 = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

        Console.Write("Enter result unit: ");
        LengthUnit target = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

        var q1 = new Quantity<LengthUnit>(v1, u1);
        var q2 = new Quantity<LengthUnit>(v2, u2);

        var result = q1.Add(q2, target);

        Console.WriteLine($"Result: {result}");
    }

    // ================= UC8 =================
    static void LengthUnitDirect()
    {
        Console.Write("Enter inches value: ");
        double value = Convert.ToDouble(Console.ReadLine());

        double feet = LengthUnit.Inch.ConvertToBaseUnit(value);

        Console.WriteLine($"Converted to Feet: {feet}");
    }

    // ================= UC9 =================
    static void WeightEquality()
    {
        Console.Write("Enter first value: ");
        double v1 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter unit (Kilogram/Gram/Pound): ");
        WeightUnit u1 = Enum.Parse<WeightUnit>(Console.ReadLine(), true);

        Console.Write("Enter second value: ");
        double v2 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter unit (Kilogram/Gram/Pound): ");
        WeightUnit u2 = Enum.Parse<WeightUnit>(Console.ReadLine(), true);

        var w1 = new Quantity<WeightUnit>(v1, u1);
        var w2 = new Quantity<WeightUnit>(v2, u2);

        Console.WriteLine($"Result: {w1.Equals(w2)}");
    }

    static void WeightConversion()
    {
        Console.Write("Enter value: ");
        double value = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter source unit: ");
        WeightUnit source = Enum.Parse<WeightUnit>(Console.ReadLine(), true);

        Console.Write("Enter target unit: ");
        WeightUnit target = Enum.Parse<WeightUnit>(Console.ReadLine(), true);

        var q = new Quantity<WeightUnit>(value, source);
        var result = q.ConvertTo(target);

        Console.WriteLine($"Converted: {result}");
    }

    static void WeightAddition()
    {
        Console.Write("Enter first value: ");
        double v1 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter first unit: ");
        WeightUnit u1 = Enum.Parse<WeightUnit>(Console.ReadLine(), true);

        Console.Write("Enter second value: ");
        double v2 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter second unit: ");
        WeightUnit u2 = Enum.Parse<WeightUnit>(Console.ReadLine(), true);

        var w1 = new Quantity<WeightUnit>(v1, u1);
        var w2 = new Quantity<WeightUnit>(v2, u2);

        var result = w1.Add(w2);

        Console.WriteLine($"Result: {result}");
    }

    // ================= UC10 =================
    static void GenericDemo()
    {
        var length = new Quantity<LengthUnit>(1, LengthUnit.Feet);
        var inch = new Quantity<LengthUnit>(12, LengthUnit.Inch);

        Console.WriteLine(length.Add(inch, LengthUnit.Feet));

        var weight = new Quantity<WeightUnit>(1, WeightUnit.Kilogram);
        var gram = new Quantity<WeightUnit>(1000, WeightUnit.Gram);

        Console.WriteLine(weight.Add(gram, WeightUnit.Kilogram));
    }
}
