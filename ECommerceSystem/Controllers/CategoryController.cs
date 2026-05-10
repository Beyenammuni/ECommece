using ECommerceSystem.Domain.DTOs.CategoryDtos.Request;
using ECommerceSystem.Domain.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        //[Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            var result = await _categoryService.CreateCategoryAsync(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _categoryService.GetAllCategoriesAsync();
            return Ok(result);
        }
        //[Authorize(Roles = "Admin")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto)
        {
            var result = await _categoryService.UpdateCategoryAsync(id, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
