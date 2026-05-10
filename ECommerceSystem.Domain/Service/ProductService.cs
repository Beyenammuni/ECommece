using ECommeceSystem.EF.Filters;
using ECommeceSystem.EF.IRepositries;
using ECommeceSystem.EF.Models;
using ECommeceSystem.EF.UnitOfWork;
using ECommerceSystem.App.DTOs.ProductDtos.Request;
using ECommerceSystem.App.DTOs.ProductDtos.Response;
using ECommerceSystem.App.IServices;
using ECommerceSystem.Core.Result;
using ECommerceSystem.Domain.DTOs.ProductDtos.Request;
using ECommerceSystem.Domain.DTOs.ProductDtos.Response;
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

             _unit.Products.Delete(product);
            await _unit.Complete();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> SoftDeleteAsync(int productId)
        {
            var product = _unit.Products.GetByIdAsync(productId);
            if (product == null)
                return Result<bool>.Failure("Product not found");
            product.Result.IsActive = false;
                  await _unit.Complete();
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> UpdateStockAsync(int productId, UpdateStockDto stockDto)
        {
            var stock = _unit.Products.GetByIdAsync(productId);
            if (stock == null)
                return Result<bool>.Failure("Product not found");
            if(stockDto.StockQuantity < 0)
                return Result<bool>.Failure("Stock quantity cannot be negative");
            stock.Result.StockQuantity = stockDto.StockQuantity;
            await _unit.Complete();
            return Result<bool>.Success(true);
        }


        public async Task<Result<List<ProductResponseDto>>> GetByIdToCustomerAsync(int id)
        {
            var product = await _unit.Products.GetByIdAsync(id);
            if (product == null)
                return Result<List<ProductResponseDto>>.Failure("Product not found");

            var response = new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name
            };

            return Result<List<ProductResponseDto>>.Success(new List<ProductResponseDto> { response });
        }

        public async Task<Result<ProductResponseDto>> GetProductAsync(ProductFilterDto filter)
        {
            var products = await _unit.Products.GetAllAsync();
            var query = products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
                query = query.Where(p => p.Name.Contains(filter.Search, StringComparison.OrdinalIgnoreCase));

            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);

            if (filter.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == filter.CategoryId.Value);

            var pagedProducts = query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            var product = pagedProducts.FirstOrDefault();
            if (product == null)
                return Result<ProductResponseDto>.Failure("Product not found");

            var response = new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name
            };

            return Result<ProductResponseDto>.Success(response);
        }
    }
}