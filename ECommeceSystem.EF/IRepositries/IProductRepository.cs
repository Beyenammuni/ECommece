using ECommeceSystem.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommeceSystem.EF.IRepositries
{
    public interface IProductRepository
    {

        Task<List<ProductModel>> GetAllAsync();
        Task<ProductModel> GetByIdAsync(int id);
        Task AddAsync(ProductModel product);
        void Update(ProductModel product);
        void Delete(ProductModel product);
    }
}
