using ECommeceSystem.EF.Data;
using ECommeceSystem.EF.Filters;
using ECommeceSystem.EF.IRepositries;
using ECommeceSystem.EF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommeceSystem.EF.Repository
{
    public class OrderRepositry:IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepositry(AppDbContext context)
        {
            _context = context;

        }

        public async Task AddAsync(OrderModel order)
        {
            await _context.Orders.AddAsync(order);
        }

        public void Delete(OrderModel order)
        {
            _context.Remove(order);
        }

        public async Task<List<OrderModel>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<OrderModel> GetByIdAsync(int id)
        {
           return await _context.Orders.FindAsync(id);
        }

        public async Task<List<OrderModel>> GetFilteredOrdersAsync(OrderFilterDto filter)
        {

            var query = _context.Orders.AsQueryable();
            // Filter By Status
            if (filter.Status.HasValue)
            {
                query = query.Where(x =>
                    x.Status == filter.Status.Value);
            }
            // Filter By Customer
            if (filter.CustomerId.HasValue)
            {
                query = query.Where(x =>
                    x.CustomerId == filter.CustomerId.Value);
            }
            // Filter From Date
            if (filter.FromDate.HasValue)
            {
                query = query.Where(x =>
                    x.OrderDate >= filter.FromDate.Value);
            }

            // Filter To Date
            if (filter.ToDate.HasValue)
            {
                query = query.Where(x =>
                    x.OrderDate <= filter.ToDate.Value);
            }
            return await query.ToListAsync();
        }

        public  void Update(OrderModel order)
        {
             _context.Orders.Update(order);
        }
    }
}
