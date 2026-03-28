using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // 🔐 Added for authentication
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Service;
using QuantityMeasurementApp.API.DTO;

namespace QuantityMeasurementApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    [Authorize] // Only logged-in users can access all APIs in this controller
    public class QuantityController : ControllerBase
    {
        private readonly IQuantityMeasurementService service;

        public QuantityController(IQuantityMeasurementService service)
        {
            this.service = service;
        }

        [HttpPost("compare")]
        public IActionResult Compare([FromBody] QuantityDTO[] quantities)
        {
            if (quantities == null || quantities.Length != 2)
                return BadRequest("Exactly two quantities must be provided.");

            var result = service.Compare(quantities[0], quantities[1]);
            return Ok(result);
        }

        [HttpPost("convert/{target}")]
        public IActionResult ConvertWithUrlParam([FromBody] QuantityDTO source, string target)
        {
            if (source == null) return BadRequest("Source quantity required.");

            var result = service.Convert(source, target);
            return Ok(result);
        }

        [HttpPost("convert")]
        public IActionResult Convert([FromBody] ConvertRequest request)
        {
            if (request == null) return BadRequest("Invalid request.");

            var source = new QuantityDTO { Value = request.Value, Unit = request.Unit };
            var result = service.Convert(source, request.TargetUnit);

            return Ok(result);
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] QuantityDTO[] quantities)
        {
            if (quantities == null || quantities.Length != 2)
                return BadRequest("Addition requires two quantities.");

            var result = service.Add(quantities[0], quantities[1]);
            return Ok(result);
        }

        [HttpPost("subtract")]
        public IActionResult Subtract([FromBody] QuantityDTO[] quantities)
        {
            if (quantities == null || quantities.Length != 2)
                return BadRequest("Subtraction requires two quantities.");

            var result = service.Subtract(quantities[0], quantities[1]);
            return Ok(result);
        }

        [HttpPost("divide")]
        public IActionResult Divide([FromBody] QuantityDTO[] quantities)
        {
            if (quantities == null || quantities.Length != 2)
                return BadRequest("Division requires two quantities.");

            var result = service.Divide(quantities[0], quantities[1]);
            return Ok(result);
        }
    }
}
