using ECommeceSystem.EF.Models;
using ECommerceSystem.Core.Result;
using ECommerceSystem.Domain.DTOs.CategoryDtos.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Domain.IServices
{
    public interface ICategoryService
    {
        Task<string> GetCategoryName(string categoryName);
        Task<Result<CategoryModel>>  CreateCategoryAsync(CreateCategoryDto dto);
    }
}
