using Microsoft.AspNetCore.Mvc;
using ProductMicroservice.API.DTOs;
using ProductMicroservice.API.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ProductMicroservice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll()
        {
            var products = await _service.GetAllAsync().ConfigureAwait(false);
            return Ok(products);
        }

        // GET: api/products/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductResponseDto>> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id).ConfigureAwait(false);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> Create([FromBody] ProductRequestDto dto)
        {
            var created = await _service.CreateAsync(dto).ConfigureAwait(false);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/products/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductRequestDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto).ConfigureAwait(false);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id).ConfigureAwait(false);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        // GET: api/products/{id}/validate
        [HttpGet("{id:int}/validate")]
        public async Task<ActionResult> Validate(int id)
        {
            var exists = await _service.ValidateProductAsync(id).ConfigureAwait(false);
            if (!exists)
                return NotFound(new { Message = $"Product with ID {id} does not exist." });

            return Ok(new { Message = "Product is valid." });
        }

        // GET: api/products/{id}/stock/{qty}
        [HttpGet("{id:int}/stock/{qty:int}")]
        public async Task<ActionResult> CheckStock(int id, int qty)
        {
            var ok = await _service.CheckStockAsync(id, qty).ConfigureAwait(false);
            if (!ok)
                return BadRequest(new { Message = "Insufficient stock or product not found." });

            return Ok(new { Message = "Sufficient stock available." });
        }
    }
}
