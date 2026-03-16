using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Service;
using QuantityMeasurementApp.API.DTO;

namespace QuantityMeasurementApp.API.Controllers
{
    /// <summary>
    /// API Controller for Quantity Measurement operations.
    /// Why: Exposes the core measurement logic (Equality, Conversion, Addition) over HTTP.
    /// What: Handles RESTful requests and coordinates with the service layer for processing.
    /// How: Uses [HttpPost] endpoints to receive QuantityDTOs or ConvertRequests and returns JSON results.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class QuantityController : ControllerBase
    {
        private readonly IQuantityMeasurementService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuantityController"/> class with injected dependencies.
        /// Why: To enable decoupling between the HTTP interface and the business logic (DI).
        /// </summary>
        /// <param name="service">The measurement service injected by the ASP.NET Core DI container.</param>
        public QuantityController(IQuantityMeasurementService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Compares two quantities for equality.
        /// Why: Allows clients to verify if 1 foot is equal to 12 inches via a simple HTTP call.
        /// How: Normalizes both quantities to a base unit and checks for equality with epsilon tolerance.
        /// </summary>
        /// <param name="quantities">An array containing exactly two QuantityDTOs (value, unit).</param>
        /// <returns>HTTP 200 with a boolean result or HTTP 400 if the input is malformed.</returns>
        [HttpPost("compare")]
        public IActionResult Compare([FromBody] QuantityDTO[] quantities)
        {
            if (quantities == null || quantities.Length != 2)
                return BadRequest("Exactly two quantities (Operand 1 and Operand 2) must be provided in the request body.");

            // Delegate logic to the service layer.
            var result = service.Compare(quantities[0], quantities[1]);
            return Ok(result);
        }

        /// <summary>
        /// Converts a quantity to a target unit specified in the URL.
        /// Why: To support alternative RESTful patterns and maintain compatibility with existing tests.
        /// </summary>
        /// <param name="source">The source QuantityDTO from the body.</param>
        /// <param name="target">The target unit from the URL.</param>
        [HttpPost("convert/{target}")]
        public IActionResult ConvertWithUrlParam([FromBody] QuantityDTO source, string target)
        {
            if (source == null) return BadRequest("Source quantity must be provided.");
            var result = service.Convert(source, target);
            return Ok(result);
        }

        /// <summary>
        /// Converts a quantity from one unit to another within the same category.
        /// Why: Essential for transforming measurements (e.g., Celsius to Fahrenheit).
        /// How: Accepts a ConvertRequest object specifying the value, source unit, and target unit.
        /// </summary>
        /// <param name="request">DTO containing 'Value', 'Unit', and 'TargetUnit'.</param>
        /// <returns>HTTP 200 with a new QuantityDTO representing the converted value.</returns>
        [HttpPost("convert")]
        public IActionResult Convert([FromBody] ConvertRequest request)
        {
            if (request == null) return BadRequest("The conversion request body is missing or malformed.");

            // Create source DTO from request and call service.
            var source = new QuantityDTO { Value = request.Value, Unit = request.Unit };
            var result = service.Convert(source, request.TargetUnit);
            
            return Ok(result);
        }

        /// <summary>
        /// Adds two quantities and returns the result in the unit of the first quantity.
        /// Why: Supports composite measurement calculations (e.g., 2 ft + 5 in).
        /// How: Converts both to base unit, sums them, and converts back to the first unit's type.
        /// </summary>
        /// <param name="quantities">An array of exactly two QuantityDTOs.</param>
        /// <returns>HTTP 200 with the resulting sum as a QuantityDTO.</returns>
        [HttpPost("add")]
        public IActionResult Add([FromBody] QuantityDTO[] quantities)
        {
            if (quantities == null || quantities.Length != 2)
                return BadRequest("Addition requires exactly two quantities.");

            var result = service.Add(quantities[0], quantities[1]);
            return Ok(result);
        }

        /// <summary>
        /// Subtracts the second quantity from the first.
        /// Why: Calculates the difference between two measurements.
        /// </summary>
        /// <param name="quantities">An array of exactly two QuantityDTOs.</param>
        /// <returns>HTTP 200 with the resulting difference as a QuantityDTO.</returns>
        [HttpPost("subtract")]
        public IActionResult Subtract([FromBody] QuantityDTO[] quantities)
        {
            if (quantities == null || quantities.Length != 2)
                return BadRequest("Subtraction requires exactly two quantities.");

            var result = service.Subtract(quantities[0], quantities[1]);
            return Ok(result);
        }

        /// <summary>
        /// Divides the first quantity by the second, resulting in a dimensionless ratio.
        /// Why: Useful for calculating relative magnitudes (e.g., how many inches are in a foot).
        /// </summary>
        /// <param name="quantities">An array of exactly two QuantityDTOs.</param>
        /// <returns>HTTP 200 with the resulting double value.</returns>
        [HttpPost("divide")]
        public IActionResult Divide([FromBody] QuantityDTO[] quantities)
        {
            if (quantities == null || quantities.Length != 2)
                return BadRequest("Division requires exactly two quantities.");

            var result = service.Divide(quantities[0], quantities[1]);
            return Ok(result);
        }
    }
}
