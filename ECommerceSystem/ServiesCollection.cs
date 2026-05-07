using ECommeceSystem.EF.IRepositries;
using ECommeceSystem.EF.Repository;
using ECommeceSystem.EF.UnitOfWork;
using ECommerceSystem.App.IServices;
using ECommerceSystem.App.Service;
using ECommerceSystem.Domain.IServices;
using ECommerceSystem.Domain.Service;


namespace ECommerceSystem
{
    public static class ServiesCollection
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>(); 
            services.AddScoped<IProductRepository, ProductRepostory>();
            services.AddScoped<ICategoryRepsitory, CategoryRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            //services.AddScoped<IOrderService, OrderService>();
            //services.AddScoped<ICartItemService, CartItemService>();
            //services.AddScoped<IOrderItemService, OrderItemService>();
        }
    }
}
