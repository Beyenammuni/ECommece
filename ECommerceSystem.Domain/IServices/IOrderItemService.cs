using ECommeceSystem.EF.Models;
using ECommerceSystem.Core.Result;
using ECommerceSystem.Domain.DTOs.OrderDtos.Request;
using ECommerceSystem.Domain.DTOs.OrderDtos.Response;
using ECommerceSystem.Domain.DTOs.OrderItemDtos.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Domain.IServices
{
    public interface IOrderItemService
    {
        Task<Result<List<OrderItemDto>>> GetAllOrderItemsAsync();
        Task<Result<OrderItemDto>> GetOrderItemByIdAsync(int id);
        Task<Result<OrderItemModel>> CreateOrderForCustomerAsync(int CustomerId, CreateOrderItemSto dto);
        Task<Result<OrderItemModel>> UpdateOrderItemAsync(int id, UpdateOrderItem dto);
            Task<Result<bool>> DeleteOrderItemAsync(int id);
    }
}
