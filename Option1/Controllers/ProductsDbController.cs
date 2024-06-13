using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Option1.Models;

namespace Option1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsDbController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductsDbController(AppDbContext context)
        {
            _context = context;
        }

        // Получить отсортированный список существующих товаров
        [HttpGet]
        [Route("GetAllProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            try
            {
                var products = await _context.products
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .Select(p => new ProductDTO
                {
                    Name = p.Name,
                    Price = p.Price,
                }).ToListAsync();
                return StatusCode(200, products);
            }
            catch
            {
                return StatusCode(500, "Server side error");
            }
        }

        // Получить товар по id
        [HttpGet("GetProductById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            try
            {
                var product = await _context.products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    return StatusCode(404, "NotFound (");
                }

                var productDto = new ProductDTO
                {
                    Name = product.Name,
                    Price = product.Price
                };

                return StatusCode(200, productDto);
            }
            catch
            {
                return StatusCode(500, "Server side error");
            }
        }

        // Добавить товар
        [HttpPost]
        [Route("AddProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Orders>> PostProduct(Products product)
        {
            try
            {
                _context.products.Add(product);
                await _context.SaveChangesAsync();

                return StatusCode(200, CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product));
            }
            catch
            {
                return StatusCode(500, "Server side error");
            }
        }

        // Удаление существующего товара
        [HttpDelete("DeleteProduct/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.products.FindAsync(id);
                if (product == null)
                {
                    return StatusCode(404, "Not found (");
                }

                _context.products.Remove(product);
                await _context.SaveChangesAsync();

                return StatusCode(200, NoContent());
            }
            catch
            {
                return StatusCode(500, "Server side error");
            }
        }
    }
}
