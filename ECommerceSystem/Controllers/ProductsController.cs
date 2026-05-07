using ECommerceSystem.App.DTOs.ProductDtos.Request;
using ECommerceSystem.App.IServices;
using ECommerceSystem.Core.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _productService.GetAllProductsAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ProductCreateDto dto)
        {
            var result = await _productService.CreateProductAsync(dto);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(result.Value);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetPrductById{id}")]
        public async Task<IActionResult> GetByIdAsyc(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Update{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ProductUpdateDto dto)
        {
            var result = await _productService.UpdateProductAsync(id, dto);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(result.Value);
        }

        [Authorize(Roles = "Admin")]//the user must be an admin to delete a product
        [HttpDelete("SoftDelete{id}")]
        public async Task<IActionResult> SoftDeleteAsync(int id)
        {
            var result = await _productService.SoftDeleteAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
