using ECommeceSystem.EF.Filters;
using ECommeceSystem.EF.Models;
using ECommerceSystem.App.DTOs.ProductDtos.Request;
using ECommerceSystem.App.DTOs.ProductDtos.Response;
using ECommerceSystem.Core.Result;
using ECommerceSystem.Domain.DTOs.ProductDtos.Request;
using ECommerceSystem.Domain.DTOs.ProductDtos.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.App.IServices
{
    public interface IProductService
    {
        Task<Result<ProductDto>> CreateProductAsync(ProductCreateDto productCreateDto);
        Task<Result<ProductDto>> GetProductByIdAsync(int productId);
        Task<Result<List<ProductDto>>> GetAllProductsAsync();
        Task<Result<ProductDto>> UpdateProductAsync(int productId, ProductUpdateDto productUpdateDto);
        Task<Result<bool>> DeleteProductAsync(int productId);
        Task<Result<bool>> SoftDeleteAsync(int productId);
        Task<Result<bool>> UpdateStockAsync(int productId, UpdateStockDto stockDto);
        Task<Result<ProductResponseDto>> GetProductAsync(ProductFilterDto filter);
        Task<Result<List<ProductResponseDto>>> GetByIdToCustomerAsync(int id);
    }
}
