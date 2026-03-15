using System;
using System.Diagnostics.CodeAnalysis;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Represents a persistent entity for storing measurement operation details.
    /// </summary>
    [Serializable]
    public class QuantityMeasurementEntity
    {
        public required QuantityDTO Operand1 { get; set; }
        public QuantityDTO? Operand2 { get; set; } // Operand 2 is optional for CONVERT
        public required string Operation { get; set; }
        public QuantityDTO? Result { get; set; }
        public string? ErrorMessage { get; set; }

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// Initializes a new instance of the <see cref="QuantityMeasurementEntity"/> class.
        /// </summary>
        public QuantityMeasurementEntity() { }

        /// <summary>
        /// Initializes a new instance with required fields.
        /// </summary>
        [SetsRequiredMembers]
        public QuantityMeasurementEntity(QuantityDTO operand1, string operation)
        {
            Operand1 = operand1;
            Operation = operation;
        }
    }
}
