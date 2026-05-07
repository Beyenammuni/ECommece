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
    public class CategoryRepository : ICategoryRepsitory
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(CategoryModel category)
        {
             await _context.AddAsync(category);
        }

        public  void Delete(CategoryModel category)
        {
        _context.Remove(category);
        }

        public async Task<List<CategoryModel>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<CategoryModel> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public void Update(CategoryModel category)
        {
            _context.Categories.Update(category);
        }
    }
}
