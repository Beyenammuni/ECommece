using ECommeceSystem.EF.BaseRepository;
using ECommeceSystem.EF.BaseRepositry;
using ECommeceSystem.EF.Data;
using ECommeceSystem.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommeceSystem.EF.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IGenericRepository<ProductModel> Products {get; private set;}
        public IGenericRepository<CategoryModel> Categories {get; private set;}
        public IGenericRepository<UserModel> Users {get; private set; }
        public IGenericRepository<OrderModel> Orders {get; private set; }
        public IGenericRepository<CartItemModel> CartItems {get; private set; }
        public IGenericRepository<OrderItemModel> OrderItems {get; private set; }
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
                Products = new GenericRepository<ProductModel>(_context);
                Categories = new GenericRepository<CategoryModel>(_context);
                Users = new GenericRepository<UserModel>(_context);
                Orders = new GenericRepository<OrderModel>(_context);
                CartItems = new GenericRepository<CartItemModel>(_context);
                OrderItems = new GenericRepository<OrderItemModel>(_context);
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public async void Dispose()
        {
           await _context.DisposeAsync();
        }
    }
}
