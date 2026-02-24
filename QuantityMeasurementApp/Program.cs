using QuantityMeasurementApp.Models;

// Do alag objects banaye same value ke
Quantity q1 = new Quantity(0.0);
Quantity q2 = new Quantity(0.0);

// Check if they are equal
bool result = q1.Equals(q2);

Console.WriteLine("--- UC 1: Feet Equality Test ---");
Console.WriteLine($"Are 0.0 and 0.0 equal? {result}");
