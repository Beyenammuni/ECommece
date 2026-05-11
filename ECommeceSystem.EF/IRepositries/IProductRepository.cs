using ECommeceSystem.EF.BaseRepositry;
using ECommeceSystem.EF.Filters;
using ECommeceSystem.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommeceSystem.EF.IRepositries
{
    public interface IProductRepository : IGenericRepository<ProductModel>
    {
        Task SoftDeleteAsync(ProductModel product);
        void UpdateStock(ProductModel product);
        Task<List<ProductModel>> GetProductsAsync(ProductFilterDto filter);
        Task<ProductModel> GetByIdToCustomerAsync(int id);
    }
}
