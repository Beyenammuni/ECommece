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
    public class ProductRepostory : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepostory(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ProductModel>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddAsync(ProductModel product)
        {
            await _context.Products.AddAsync(product);
        }

        public void Update(ProductModel product)
        {
            _context.Products.Update(product);
        }

        public void Delete(ProductModel product)
        {
            _context.Products.Remove(product);
        }
        public async Task SoftDeleteAsync(ProductModel product)
        {
            product.IsActive = true;
            _context.Products.Update(product);
        }
        public void UpdateStock(ProductModel product)
        {
           _context.Products.Update(product);
        }
    }
}