using ECommeceSystem.EF.Models;
using ECommerceSystem.App.DTOs.CategoryDtos.Response;
using ECommerceSystem.Core.Result;
using ECommerceSystem.Domain.DTOs.CategoryDtos.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Domain.IServices
{
    public interface ICategoryService
    {
        Task<Result<CategoryModel>>  CreateCategoryAsync(CreateCategoryDto dto);
        Task<Result<List<CategoryDto>>> GetAllCategoriesAsync();
        Task<Result<bool>> UpdateCategoryAsync(int id, UpdateCategoryDto dto);
    }
}
