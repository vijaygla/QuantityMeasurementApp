namespace QuantityMeasurementApp.Models
{
    // Each unit stores its conversion factor to base unit (Inches)
    public enum LengthUnit
    {
        Feet = 12,            // 1 foot = 12 inches
        Inch = 1,             // Base unit
        Yards = 36,           // 1 yard = 36 inches
        Centimeters = 0       // Special handling (see below)
    }
}
