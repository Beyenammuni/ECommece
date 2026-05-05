using ECommeceSystem.EF.IRepositries;
using ECommeceSystem.EF.Models;
using ECommeceSystem.EF.UnitOfWork;
using ECommerceSystem.App.DTOs.ProductDtos.Request;
using ECommerceSystem.App.DTOs.ProductDtos.Response;
using ECommerceSystem.App.IServices;
using ECommerceSystem.Core.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.App.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unit;
        public ProductService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<Result<ProductDto>> CreateProductAsync(ProductCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return Result<ProductDto>.Failure("Product name is required");

            if (dto.Price <= 0)
                return Result<ProductDto>.Failure("Price must be greater than 0");

            var product = new ProductModel
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId,
                IsActive = true
            };

            await _unit.Products.AddAsync(product);
            await _unit.Complete();

            var result = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };

            return Result<ProductDto>.Success(result);
        }

        // 🔹 GET ALL
        public async Task<Result<List<ProductDto>>> GetAllProductsAsync()
        {
            var products = await _unit.Products.GetAllAsync();

            var result = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }).ToList();

            return Result<List<ProductDto>>.Success(result);
        }

        public async Task<Result<ProductDto>> GetProductByIdAsync(int productId)
        {
            var product = await _unit.Products.GetByIdAsync(productId);

            if (product == null)
                return Result<ProductDto>.Failure("Product not found");

            var result = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };

            return Result<ProductDto>.Success(result);
        }

        public async Task<Result<ProductDto>> UpdateProductAsync(int productId, ProductUpdateDto dto)
        {
            var product = await _unit.Products.GetByIdAsync(productId);

            if (product == null)
                return Result<ProductDto>.Failure("Product not found");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;

            await _unit.Complete();

            var result = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };

            return Result<ProductDto>.Success(result);
        }

        public async Task<Result<bool>> DeleteProductAsync(int productId)
        {
            var product = await _unit.Products.GetByIdAsync(productId);

            if (product == null)
                return Result<bool>.Failure("Product not found");

            await _unit.Products.DeleteAsync(product);
            await _unit.Complete();

            return Result<bool>.Success(true);
        }
    }
}