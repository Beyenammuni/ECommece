using ECommeceSystem.EF.IRepositries;
using ECommeceSystem.EF.Repository;
using ECommeceSystem.EF.UnitOfWork;
using ECommerceSystem.App.IServices;
using ECommerceSystem.App.Service;
using ECommerceSystem.Domain.IServices;
using ECommerceSystem.Domain.Service;


namespace ECommerceSystem.Extensions
{
    public static class ServiesCollection
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>(); 
            services.AddScoped<IProductRepository, ProductRepostory>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepositry>();
            services.AddScoped<ICartItemService, CartItemService>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<IOrderItemService, OrderItemService>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IUserRepository,UserRepository>();
        }
    }
}
