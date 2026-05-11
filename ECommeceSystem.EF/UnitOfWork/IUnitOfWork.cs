using ECommeceSystem.EF.BaseRepositry;
using ECommeceSystem.EF.IRepositries;
using ECommeceSystem.EF.Models;
using System;
using System.Threading.Tasks;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }

    ICartItemRepository CartItems { get; }

    IOrderRepository Orders { get; }

    IGenericRepository<CategoryModel> Categories { get; }

    IGenericRepository<UserModel> Users { get; }

    IGenericRepository<OrderItemModel> OrderItems { get; }

    Task<int> Complete();
}