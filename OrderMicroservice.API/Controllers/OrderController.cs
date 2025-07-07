using Microsoft.AspNetCore.Mvc;
using OrderMicroservice.API.DTOs;
using OrderMicroservice.API.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace OrderMicroservice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service)
        {
            _service = service;
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
            var (success, error, order) = await _service.CreateAsync(dto).ConfigureAwait(false);
            if (!success)
                return BadRequest(new { Message = error });

            return CreatedAtAction(nameof(GetByReference), new { reference = order!.Reference }, order);
        }

        // PUT: api/orders/{reference}
        [HttpPut("{reference}")]
        public async Task<IActionResult> Update(string reference, [FromBody] CreateOrderRequestDto dto)
        {
            var (success, error) = await _service.UpdateAsync(reference, dto).ConfigureAwait(false);
            if (!success)
                return BadRequest(new { Message = error });

            return NoContent();
        }

        // DELETE: api/orders/{reference}
        [HttpDelete("{reference}")]
        public async Task<IActionResult> Delete(string reference)
        {
            var deleted = await _service.DeleteAsync(reference).ConfigureAwait(false);
            if (!deleted)
                return NotFound(new { Message = $"Order {reference} not found." });

            return NoContent();
        }
    }
}
