using ECommeceSystem.EF.Models;
using ECommerceSystem.Core.Result;
using ECommerceSystem.Domain.DTOs.CartItemDtos.Request;
using ECommerceSystem.Domain.DTOs.OrderDtos.Request;
using ECommerceSystem.Domain.DTOs.OrderDtos.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Domain.IServices
{
    public interface ICartItemService
    {
        Task<Result<CartItemModel>> CreateCartItemAsync(int CustomerId, CreateCartItem dto);
        Task<Result<List<CartItemDto>>> GetCstomerCartItemAsync(int customerId);
        Task<Result<bool>> UpdateCartItemAsync(int id, UpdateCartItem dto);
        Task<Result<bool>> DeleteCartItemAsync(int id);
    }
}
