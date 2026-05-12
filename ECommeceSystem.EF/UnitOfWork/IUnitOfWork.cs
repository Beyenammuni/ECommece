using ECommeceSystem.EF.BaseRepositry;
using ECommeceSystem.EF.IRepositries;
using ECommeceSystem.EF.Models;
using ECommeceSystem.EF.Repository;
using System;
using System.Threading.Tasks;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }

    ICartItemRepository CartItems { get; }

    IOrderRepository Orders { get; }

    ICategoryRepository Categories { get; }

    IUserRepository Users { get; }

    IOrderItemRepository OrderItems { get; }

    Task<int> Complete();
}