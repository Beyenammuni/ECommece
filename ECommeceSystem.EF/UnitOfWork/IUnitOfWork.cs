using ECommeceSystem.EF.BaseRepositry;
using ECommeceSystem.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommeceSystem.EF.UnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
        IGenericRepository<ProductModel> Products { get; }
        IGenericRepository<CategoryModel> Categories { get; }
        IGenericRepository<UserModel> Users { get; }
        IGenericRepository<OrderModel> Orders { get; }
        IGenericRepository<CartItemModel> CartItems { get; }
        IGenericRepository<OrderItemModel> OrderItems { get; }
        Task<int> Complete();
    }
}
