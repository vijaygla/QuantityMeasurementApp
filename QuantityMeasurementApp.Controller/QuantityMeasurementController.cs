using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Service;

namespace QuantityMeasurementApp.Controller
{
    /// <summary>
    /// Controller for the Quantity Measurement Application.
    /// Orchestrates interaction between the user interface and the service layer.
    /// </summary>
    public class QuantityMeasurementController
    {
        private readonly IQuantityMeasurementService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuantityMeasurementController"/> class.
        /// </summary>
        /// <param name="service">The measurement service.</param>
        public QuantityMeasurementController(IQuantityMeasurementService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Performs a comparison between two quantities and prints the result.
        /// </summary>
        public void PerformComparison(QuantityDTO q1, QuantityDTO q2)
        {
            try
            {
                var result = service.Compare(q1, q2);
                Console.WriteLine($"[RESULT] Comparison: {q1.Value} {q1.Unit} == {q2.Value} {q2.Unit} -> {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Comparison failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Performs a conversion of a quantity to a target unit and prints the result.
        /// </summary>
        public void PerformConversion(QuantityDTO q, string targetUnit)
        {
            try
            {
                var result = service.Convert(q, targetUnit);
                Console.WriteLine($"[RESULT] Conversion: {q.Value} {q.Unit} -> {result.Value} {result.Unit}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Conversion failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Performs addition of two quantities and prints the result.
        /// </summary>
        public void PerformAddition(QuantityDTO q1, QuantityDTO q2)
        {
            try
            {
                var result = service.Add(q1, q2);
                Console.WriteLine($"[RESULT] Addition: ({q1.Value} {q1.Unit}) + ({q2.Value} {q2.Unit}) = {result.Value} {result.Unit}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Addition failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Performs subtraction of two quantities and prints the result.
        /// </summary>
        public void PerformSubtraction(QuantityDTO q1, QuantityDTO q2)
        {
            try
            {
                var result = service.Subtract(q1, q2);
                Console.WriteLine($"[RESULT] Subtraction: ({q1.Value} {q1.Unit}) - ({q2.Value} {q2.Unit}) = {result.Value} {result.Unit}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Subtraction failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Performs division of two quantities and prints the resulting ratio.
        /// </summary>
        public void PerformDivision(QuantityDTO q1, QuantityDTO q2)
        {
            try
            {
                var result = service.Divide(q1, q2);
                Console.WriteLine($"[RESULT] Division: ({q1.Value} {q1.Unit}) / ({q2.Value} {q2.Unit}) = {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Division failed: {ex.Message}");
            }
        }
    }
}
