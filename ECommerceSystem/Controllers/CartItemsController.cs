using ECommerceSystem.Domain.DTOs.CartItemDtos.Request;
using ECommerceSystem.Domain.DTOs.OrderDtos.Request;
using ECommerceSystem.Domain.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartItemsController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpPost("CreateCart{customerId}")]
        public async Task<IActionResult> CreateCartItemAsync(int CustomerId, [FromBody] CreateCartItem dto)
        {
            var result = await _cartItemService.CreateCartItemAsync(CustomerId, dto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }
        [HttpDelete("DeleteCartItem{id}")]
        public async Task<IActionResult> DeleteCartItemAsync(int id)
        {
            var result = await _cartItemService.DeleteCartItemAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }
        [HttpGet("GetCustomerCart{customerId}")]
        public async Task<IActionResult> GetCustomerCartItemAsync(int CustomerId)
        {
            var result = await _cartItemService.GetCstomerCartItemAsync(CustomerId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Value);
        }
            [HttpPut("UpdateCartItem{id}")]
        public async Task<IActionResult> UpdateCartItemAsync(int id, [FromBody] UpdateCartItem dto)
        {
            var result = await _cartItemService.UpdateCartItemAsync(id, dto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }
    }
}
