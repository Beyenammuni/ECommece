using ECommeceSystem.EF.Filters;
using ECommerceSystem.Domain.DTOs.OrderDtos.Response;
using ECommerceSystem.Domain.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderService.GetAllOrderAsync();
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result);
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet("filter")]
        public async Task<IActionResult> GetOrders([FromQuery] OrderFilterDto filter)
        {
            var result = await _orderService.GetOrdersAsync(filter);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result);
        }
        //[Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id,[FromBody] UpdateOrderStatusDto dto)
        {
            var result = await _orderService.UpdateStatusAsync(id, dto);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
