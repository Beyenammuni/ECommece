using ECommeceSystem.EF.Filters;
using ECommeceSystem.EF.Models;
using ECommerceSystem.Core.Result;
using ECommerceSystem.Domain.DTOs.OrderDtos.Request;
using ECommerceSystem.Domain.DTOs.OrderDtos.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Domain.IServices
{
    public interface IOrderService
    {
        Task<Result<List<OrderDto>>> GetAllOrderAsync();
        Task<Result<bool>> Update(int id, UpdateOrder dto);

        Task<Result<List<OrderModel>>> GetOrdersAsync(OrderFilterDto filter);

        Task<Result<bool>> UpdateStatusAsync(int orderId, UpdateOrderStatusDto dto);
    }
}
