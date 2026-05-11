using ECommeceSystem.EF.BaseRepositry;
using ECommeceSystem.EF.Filters;
using ECommeceSystem.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommeceSystem.EF.IRepositries
{
    public interface IOrderRepository : IGenericRepository<OrderModel>
    {
        Task<List<OrderModel>> GetFilteredOrdersAsync(
        OrderFilterDto filter);
    }
}
