using System.Diagnostics.CodeAnalysis;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Data Transfer Object for representing a quantity in a serializable format.
    /// </summary>
    public class QuantityDTO
    {
        /// <summary>
        /// Gets or sets the numeric value of the quantity.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the string representation of the unit.
        /// </summary>
        public required string Unit { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuantityDTO"/> class.
        /// </summary>
        public QuantityDTO() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuantityDTO"/> class with value and unit.
        /// </summary>
        /// <param name="value">The numeric value.</param>
        /// <param name="unit">The unit name.</param>
        [SetsRequiredMembers]
        public QuantityDTO(double value, string unit)
        {
            Value = value;
            Unit = unit;
        }
    }
}
