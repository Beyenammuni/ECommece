using ECommeceSystem.EF.Data;
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
    public class CartItemRepository : ICartItemRepository
    {
        private readonly AppDbContext _context;

        public CartItemRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(CartItemModel CartItem)
        {
            await _context.AddAsync(CartItem);
        }

        public void Delete(CartItemModel CartItem)
        {
            _context.Remove(CartItem);
        }

        public async Task<List<CartItemModel>> GetAllAsync()
        {
            return await _context.CardItems.ToListAsync();
        }

        public async Task<List<CartItemModel>> GetCustomerCartAsync(int customerId)
        {
            return await _context.CardItems.Where(c => c.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<CartItemModel> GetByIdAsync(int id)
        {
            return await _context.CardItems.FindAsync(id);
        }

        public void Update(CartItemModel CartItem)
        {
            _context.CardItems.Update(CartItem);
        }
    }
}
