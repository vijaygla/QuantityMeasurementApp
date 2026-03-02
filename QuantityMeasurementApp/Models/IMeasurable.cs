namespace QuantityMeasurementApp.Models
{
    public interface IMeasurable
    {
        double GetConversionFactor();              // Relative to base unit
        double ConvertToBaseUnit(double value);    // Convert to base
        double ConvertFromBaseUnit(double baseValue); // Convert from base
        string GetUnitName();                      // Readable name
    }
}
