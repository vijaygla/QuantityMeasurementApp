namespace QuantityMeasurementApp.Models
{
    public interface IMeasurable
    {
        // -------------------------------
        // Conversion Methods (Mandatory)
        // -------------------------------

        double GetConversionFactor();                  // Relative to base unit
        double ConvertToBaseUnit(double value);        // Convert to base unit
        double ConvertFromBaseUnit(double baseValue);  // Convert from base unit
        string GetUnitName();                          // Readable unit name

        // ----------------------------------------
        // Arithmetic Capability (Default Support)
        // ----------------------------------------

        // Functional interface equivalent using delegate
        public delegate bool SupportsArithmetic();

        // Default lambda: arithmetic supported
        SupportsArithmetic supportsArithmetic => () => true;

        // Check whether arithmetic operations are allowed
        public virtual bool SupportsArithmeticOperations()
        {
            return supportsArithmetic();
        }

        // ------------------------------------------------
        // Validate Arithmetic Operation (Optional Override)
        // ------------------------------------------------

        public virtual void ValidateOperationSupport(string operation)
        {
            // Default behavior: allow operation
            // Units like Temperature will override this
        }
    }
}
