using Microsoft.AspNetCore.Mvc;
using OrderMicroservice.API.DTOs;
using OrderMicroservice.API.Services;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

using System.Threading.Tasks;
namespace OrderMicroservice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService service, ILogger<OrdersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAll()
        {
            var orders = await _service.GetAllAsync().ConfigureAwait(false);
            return Ok(orders);
        }

        // GET: api/orders/{reference}
        [HttpGet("{reference}")]
        public async Task<ActionResult<OrderResponseDto>> GetByReference(string reference)
        {
            var order = await _service.GetByReferenceAsync(reference).ConfigureAwait(false);
            if (order == null)
                return NotFound(new { Message = $"Order {reference} not found." });

            return Ok(order);
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<OrderResponseDto>> Create([FromBody] CreateOrderRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid order creation request received.");
                return BadRequest(ModelState);
            }

            var (success, error, order) = await _service.CreateAsync(dto).ConfigureAwait(false);

            if (!success)
            {
                _logger.LogWarning("Order creation failed: {Error}", error);
                return BadRequest(new { Message = error });
            }

            _logger.LogInformation("Order created successfully with reference {Reference}", order!.Reference);
            return CreatedAtAction(nameof(GetByReference), new { reference = order.Reference }, order);
        }

        // PUT: api/orders/{reference}
        [HttpPut("{reference}")]
        public async Task<IActionResult> Update(string reference, [FromBody] CreateOrderRequestDto dto)
        {
            var (success, error) = await _service.UpdateAsync(reference, dto).ConfigureAwait(false);

            if (!success)
            {
                _logger.LogWarning("Order update failed for reference {Reference}: {Error}", reference, error);
                return BadRequest(new { Message = error });
            }

            _logger.LogInformation("Order updated successfully for reference {Reference}", reference);
            return NoContent();
        }

        // DELETE: api/orders/{reference}
        [HttpDelete("{reference}")]
        public async Task<IActionResult> Delete(string reference)
        {
            var deleted = await _service.DeleteAsync(reference).ConfigureAwait(false);

            if (!deleted)
            {
                _logger.LogWarning("Attempt to delete non-existing order {Reference}", reference);
                return NotFound(new { Message = $"Order {reference} not found." });
            }

            _logger.LogInformation("Order deleted successfully for reference {Reference}", reference);
            return NoContent();
        }
    }
}
