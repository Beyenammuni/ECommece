using ECommeceSystem.EF.Models;
using ECommerceSystem.Domain.DTOs.OrderDtos.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommeceSystem.EF.IRepositries
{
    public interface IOrderRepository
    {
        Task<List<OrderModel>> GetAllAsync();
        Task<OrderModel> GetByIdAsync(int id);
        Task AddAsync(OrderModel order);
        Task<List<OrderModel>> GetFilteredOrdersAsync(
        OrderFilterDto filter);
        void Update(OrderModel order);
        void Delete(OrderModel order);
    }
}
