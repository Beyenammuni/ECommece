using ECommeceSystem.EF.Models;
using ECommeceSystem.EF.UnitOfWork;
using ECommerceSystem.App.DTOs.CategoryDtos.Response;
using ECommerceSystem.App.DTOs.ProductDtos.Response;
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
        private readonly IUnitOfWork _unit;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unit = unitOfWork;
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
            await _unit.Categories.AddAsync(category);
            await _unit.Complete();
            return Result<CategoryModel>.Success(category, "Category Created Successfully");
        }

        public async Task<Result<List<CategoryDto>>> GetAllCategoriesAsync()
        {
            var categories = await _unit.Categories.GetAllAsync();

            var result = categories.Select(p => new CategoryDto
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();

            return Result<List<CategoryDto>>.Success(result);
        }

        public async Task<Result<bool>> UpdateCategoryAsync(int id, UpdateCategoryDto dto)
        {
            var category = await _unit.Categories.GetByIdAsync(id);
            if (category == null)
                return Result<bool>.Failure("Category not found");

            if(string.IsNullOrWhiteSpace(dto.Name))
                return Result<bool>.Failure("Category name is required");

            category.Name = dto.Name;
            await _unit.Categories.UpdateAsync(category);
            await _unit.Complete();
            return Result<bool>.Success(true, "Category updated successfully");
        }
    }
}
