using ECommeceSystem.EF.Data;
using ECommeceSystem.EF.IRepositries;
using ECommeceSystem.EF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommeceSystem.EF.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext _context;

        public OrderItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OrderItemModel entity)
        {
             await _context.OrderItems.AddAsync(entity);
        }

        public async Task<OrderItemModel> CreateOrderForCustomer(int CustomerId)
        {
             return await _context.OrderItems.FirstOrDefaultAsync(o => o.Order.CustomerId == CustomerId);


        }

        public void Delete(OrderItemModel entity)
        {
            _context.OrderItems.Remove(entity);
        }

        public async Task<List<OrderItemModel>> GetAllAsync()
        {
           return await _context.OrderItems.ToListAsync();
        }

        public async Task<OrderItemModel> GetByIdAsync(int id)
        {
            return await _context.OrderItems.FindAsync(id);
        }

        public void Update(OrderItemModel entity)
        {
            _context.OrderItems.Update(entity);
        }
    }
}
