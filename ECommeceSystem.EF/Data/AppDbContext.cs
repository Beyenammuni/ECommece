using ECommeceSystem.EF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace ECommeceSystem.EF.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderItemModel> OrderItems { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<CartItemModel> CardItems { get; set; }
    }
}
