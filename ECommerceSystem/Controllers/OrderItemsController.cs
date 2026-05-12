using ECommerceSystem.Domain.DTOs.OrderDtos.Request;
using ECommerceSystem.Domain.DTOs.OrderItemDtos.Request;
using ECommerceSystem.Domain.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemsController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }
         [HttpPost("CreateOrderItemForCustomer/{customerId}")]
         public async Task<IActionResult> CreateOrderItemForCustomer(int customerId, [FromBody] CreateOrderItemSto dto)
            {
                var result = await _orderItemService.CreateOrderForCustomerAsync(customerId, dto);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result);
        }
            [HttpGet("GetAllOrderItems")]
            public async Task<IActionResult> GetAllOrderItems()
            {
                var result = await _orderItemService.GetAllOrderItemsAsync();
                if (!result.IsSuccess)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result);
            }
        [HttpDelete("DeleteOrderItem/{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var result = await _orderItemService.DeleteOrderItemAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }
            return Ok(result);
        }
        [HttpGet("GetOrderItemById/{id}")]
        public async Task<IActionResult> GetOrderItemById(int id)
        {
            var result = await _orderItemService.GetOrderItemByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }
            return Ok(result);
        }
        [HttpPut("UpdateOrderItem/{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, [FromBody] UpdateOrderItem dto)
        {
            var result = await _orderItemService.UpdateOrderItemAsync(id, dto);
            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }
            return Ok(result);
        }
    }
}
