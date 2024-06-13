using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Option1.Models;

namespace Option1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersDbController : ControllerBase
    {
        // Создаание заказа на товар
        // Удаление заказа

        private readonly AppDbContext _context;
        public OrdersDbController(AppDbContext context)
        {
            _context = context;
        }

        // Получить список существующих заказов
        [HttpGet]
        [Route("GetAllOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            try
            {
                var orders = await _context.orders
                    .AsNoTracking()
                    .Select(o => new OrderDTO
                    {
                        ProductId = o.ProductId,
                        Quantity = o.Quantity
                    }).ToListAsync();
                return StatusCode(200, orders);
            }
            catch
            {
                return StatusCode(500, "Server side error");
            }
        }

        // Получить заказ по id
        [HttpGet("GetOrderById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            try
            {
                var order = await _context.orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.ProductId == id);

                if (order == null)
                {
                    return StatusCode(404, "Not found (");
                }

                var OrderDto = new OrderDTO
                {
                    ProductId= order.ProductId,
                    Quantity = order.Quantity
                };

                return StatusCode(200, OrderDto);
            }
            catch
            {
                return StatusCode(500, "Server side error");
            }
        }

        // Добавить заказ
        [HttpPost]
        [Route("AddOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Orders>> PostProduct(Orders order)
        {
            try
            {
                _context.orders.Add(order);
                await _context.SaveChangesAsync();

                return StatusCode(200, CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order));
            }
            catch
            {
                return StatusCode(500, "Server side error");
            }
        }

        // Удаление заказа
        [HttpDelete("DeleteOrder/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var order = await _context.orders.FindAsync(id);
                if (order == null)
                {
                    return StatusCode(404, "Not found (");
                }

                _context.orders.Remove(order);
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
