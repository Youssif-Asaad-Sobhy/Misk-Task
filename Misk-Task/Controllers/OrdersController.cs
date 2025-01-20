using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misk_Task.Extensions;
using Misk_Task.Models;
using Misk_Task.Models.DTOs;

namespace Misk_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private static readonly List<Orders> _orders = new();

        [HttpPost]
        public ActionResult<OrderDto> CreateOrder(CreateOrderDto createOrderDto)
        {
            var order = createOrderDto.ToEntity();
            order.Id = Guid.NewGuid();
            order.OrderDate = DateTime.UtcNow;
            order.Status = OrderStatus.Pending;
            order.TotalAmount = order.CalculateTotalAmount();

            _orders.Add(order);

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order.ToDto());
        }

        [HttpGet]
        public ActionResult<IEnumerable<OrderDto>> GetOrders()
        {
            var query = _orders.AsQueryable();

            return Ok(query.Select(o => o.ToDto()));
        }

        [HttpGet("{id}")]
        public ActionResult<OrderDto> GetOrder(Guid id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order.ToDto());
        }

        [HttpPut("{id}/status")]
        public ActionResult<OrderDto> UpdateOrderStatus(Guid id, [FromBody] OrderStatus status)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            return Ok(order.ToDto());
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(Guid id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            _orders.Remove(order);
            return NoContent();
        }
    }
}
