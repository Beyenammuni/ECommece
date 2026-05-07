using ECommeceSystem.EF.Models;
using ECommeceSystem.EF.UnitOfWork;
using ECommerceSystem.Core.Result;
using ECommerceSystem.Domain.DTOs.CategoryDtos.Request;
using ECommerceSystem.Domain.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Domain.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CategoryModel>> CreateCategoryAsync(CreateCategoryDto dto)
        {
            if(string.IsNullOrEmpty(dto.Name))
            {
                return Result<CategoryModel>.Failure("Category name is required");
            }
            var category = new CategoryModel
            {
                Name = dto.Name
            };
            return Result<CategoryModel>.Success(category, "Category Created Successfully");
        }

        public Task<string> GetCategoryName(string categoryName)
        {
            throw new NotImplementedException();
        }
    }
}
