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
                case 0: return;
                default: Console.WriteLine("Invalid choice"); break;
            }
        }
    }

    // ---------------- LENGTH UNIT MENU ----------------

    static LengthUnit GetLengthUnit()
    {
        Console.WriteLine("\nSelect Length Unit:");
        Console.WriteLine("1. Feet");
        Console.WriteLine("2. Inch");
        Console.WriteLine("3. Yards");
        Console.WriteLine("4. Centimeters");

        Console.Write("Enter choice: ");
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

    // ---------------- WEIGHT UNIT MENU ----------------

    static WeightUnit GetWeightUnit()
    {
        Console.WriteLine("\nSelect Weight Unit:");
        Console.WriteLine("1. Kilogram");
        Console.WriteLine("2. Gram");
        Console.WriteLine("3. Pound");

        Console.Write("Enter choice: ");
        int choice = Convert.ToInt32(Console.ReadLine());

        return choice switch
        {
            1 => WeightUnit.Kilogram,
            2 => WeightUnit.Gram,
            3 => WeightUnit.Pound,
            _ => throw new Exception("Invalid Weight Unit")
        };
    }

    // ================= UC1–UC4 =================

    static void LengthEquality()
    {
        Console.Write("\nEnter first value: ");
        double v1 = Convert.ToDouble(Console.ReadLine());
        LengthUnit u1 = GetLengthUnit();

        Console.Write("\nEnter second value: ");
        double v2 = Convert.ToDouble(Console.ReadLine());
        LengthUnit u2 = GetLengthUnit();

        var q1 = new Quantity<LengthUnit>(v1, u1);
        var q2 = new Quantity<LengthUnit>(v2, u2);

        Console.WriteLine($"Equality Result: {q1.Equals(q2)}");
    }

    // ================= UC5 =================

    static void LengthConversion()
    {
        Console.Write("\nEnter value: ");
        double value = Convert.ToDouble(Console.ReadLine());

        LengthUnit source = GetLengthUnit();
        LengthUnit target = GetLengthUnit();

        var q = new Quantity<LengthUnit>(value, source);
        var result = q.ConvertTo(target);

        Console.WriteLine($"Converted Result: {result}");
    }

    // ================= UC6 =================

    static void LengthAdditionImplicit()
    {
        Console.Write("\nEnter first value: ");
        double v1 = Convert.ToDouble(Console.ReadLine());
        LengthUnit u1 = GetLengthUnit();

        Console.Write("\nEnter second value: ");
        double v2 = Convert.ToDouble(Console.ReadLine());
        LengthUnit u2 = GetLengthUnit();

        var q1 = new Quantity<LengthUnit>(v1, u1);
        var q2 = new Quantity<LengthUnit>(v2, u2);

        var result = q1.Add(q2);

        Console.WriteLine($"Addition Result: {result}");
    }

    // ================= UC7 =================

    static void LengthAdditionExplicit()
    {
        Console.Write("\nEnter first value: ");
        double v1 = Convert.ToDouble(Console.ReadLine());
        LengthUnit u1 = GetLengthUnit();

        Console.Write("\nEnter second value: ");
        double v2 = Convert.ToDouble(Console.ReadLine());
        LengthUnit u2 = GetLengthUnit();

        Console.WriteLine("\nSelect Result Unit:");
        LengthUnit target = GetLengthUnit();

        var q1 = new Quantity<LengthUnit>(v1, u1);
        var q2 = new Quantity<LengthUnit>(v2, u2);

        var result = q1.Add(q2, target);

        Console.WriteLine($"Addition Result: {result}");
    }

    // ================= UC8 =================

    static void LengthUnitDirect()
    {
        Console.Write("\nEnter inch value: ");
        double value = Convert.ToDouble(Console.ReadLine());

        double feet = LengthUnit.Inch.ConvertToBaseUnit(value);

        Console.WriteLine($"Converted to Feet: {feet}");
    }

    // ================= UC9 =================

    static void WeightEquality()
    {
        Console.Write("\nEnter first value: ");
        double v1 = Convert.ToDouble(Console.ReadLine());
        WeightUnit u1 = GetWeightUnit();

        Console.Write("\nEnter second value: ");
        double v2 = Convert.ToDouble(Console.ReadLine());
        WeightUnit u2 = GetWeightUnit();

        var w1 = new Quantity<WeightUnit>(v1, u1);
        var w2 = new Quantity<WeightUnit>(v2, u2);

        Console.WriteLine($"Equality Result: {w1.Equals(w2)}");
    }

    static void WeightConversion()
    {
        Console.Write("\nEnter value: ");
        double value = Convert.ToDouble(Console.ReadLine());

        WeightUnit source = GetWeightUnit();
        WeightUnit target = GetWeightUnit();

        var q = new Quantity<WeightUnit>(value, source);
        var result = q.ConvertTo(target);

        Console.WriteLine($"Converted Result: {result}");
    }

    static void WeightAddition()
    {
        Console.Write("\nEnter first value: ");
        double v1 = Convert.ToDouble(Console.ReadLine());
        WeightUnit u1 = GetWeightUnit();

        Console.Write("\nEnter second value: ");
        double v2 = Convert.ToDouble(Console.ReadLine());
        WeightUnit u2 = GetWeightUnit();

        var w1 = new Quantity<WeightUnit>(v1, u1);
        var w2 = new Quantity<WeightUnit>(v2, u2);

        var result = w1.Add(w2);

        Console.WriteLine($"Addition Result: {result}");
    }

    // ================= UC10 =================

    static void GenericDemo()
    {
        Console.WriteLine("\nGeneric Quantity Demo");

        var length = new Quantity<LengthUnit>(1, LengthUnit.Feet);
        var inch = new Quantity<LengthUnit>(12, LengthUnit.Inch);

        Console.WriteLine($"Length Add: {length.Add(inch, LengthUnit.Feet)}");

        var weight = new Quantity<WeightUnit>(1, WeightUnit.Kilogram);
        var gram = new Quantity<WeightUnit>(1000, WeightUnit.Gram);

        Console.WriteLine($"Weight Add: {weight.Add(gram, WeightUnit.Kilogram)}");
    }
}
