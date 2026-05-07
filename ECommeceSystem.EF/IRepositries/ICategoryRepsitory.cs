using ECommeceSystem.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommeceSystem.EF.IRepositries
{
    public interface ICategoryRepsitory
    {
        Task<List<CategoryModel>> GetAllAsync();
        Task<CategoryModel> GetByIdAsync(int id);
        Task AddAsync(CategoryModel product);
        void Update(CategoryModel product);
        void Delete(CategoryModel product);
    }
}
