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
            product.IsActive = false;
            _context.Products.Update(product);
        }
        public void UpdateStock(ProductModel product)
        {
           _context.Products.Update(product);
        }

 

        public async Task<ProductModel> GetByIdToCustomerAsync(int id)
        {
           return await _context.Products.Where(x => x.Id == id && x.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ProductModel>> GetProductsAsync(ProductFilterDto filter)
        {
            var query = _context.Products.Where(x => x.IsActive).AsQueryable();

            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(x => x.Name.Contains(filter.Search) || x.Description.Contains(filter.Search));
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == filter.CategoryId.Value);
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(x => x.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(x => x.Price <= filter.MaxPrice.Value);
            }

            return await query.Skip((filter.PageNumber - 1) * filter.PageSize)
                        .Take(filter.PageSize).ToListAsync();
        }
    }
}