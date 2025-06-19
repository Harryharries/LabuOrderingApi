using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;

namespace OrderApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController(IOrder orderInterface, IOrderService orderService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await orderInterface.GetAllAsync();
            if (!orders.Any())
            {
                return NotFound("No orders found");
            }
            var (_, list) = OrderConversion.FromEntity(null!, orders);
            return list!.Any() ? Ok(list) : NotFound("No orders found");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await orderInterface.FindByIdAsync(id);
            if (order is null)
            {
                return NotFound("Order not found");
            }
            var (singleOrder, _) = OrderConversion.FromEntity(order, null!);
            return singleOrder is not null ? Ok(singleOrder) : NotFound("Order not found");
        }

        [HttpGet("client/{clientId:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrdersByClientId(int clientId)
        {
            if (clientId <= 0) return BadRequest("Invalid client id");
            var orders = await orderInterface.GetAllOrdersAsync(p => p.ClientId == clientId);
            return orders!.Any() ? Ok(orders) : NotFound("No orders found");
        }

        [HttpGet("detail/{orderId:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrdersByOrderId(int orderId)
        {
            if (orderId <= 0) return BadRequest("Invalid order id");
            var orderDetails = await orderService.GetOrderDetailsAsync(orderId);
            return orderDetails.OrderId > 0 ? Ok(orderDetails) : NotFound("No orders found");
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> CreateOrder(OrderDTO order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newOrder = await orderInterface.CreateAsync(OrderConversion.ToEntity(order));
            if (newOrder is null)
            {
                return NotFound("Order not found");
            }
            return newOrder is not null ? Ok(newOrder) : BadRequest(newOrder);
        }

        [HttpPut()]
        public async Task<ActionResult<OrderDTO>> UpdateOrder(OrderDTO order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedOrder = await orderInterface.UpdateAsync(OrderConversion.ToEntity(order));
            if (updatedOrder is null)
            {
                return NotFound("Order not found");
            }
            return updatedOrder is not null ? Ok(updatedOrder) : BadRequest(updatedOrder);
        }

        [HttpDelete()]
        public async Task<ActionResult<OrderDTO>> DeleteOrder(OrderDTO order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var deletedOrder = await orderInterface.DeleteAsync(OrderConversion.ToEntity(order));
            if (deletedOrder is null)
            {
                return NotFound("Order not found");
            }
            return deletedOrder is not null ? Ok(deletedOrder) : BadRequest(deletedOrder);
        }
    }
}
