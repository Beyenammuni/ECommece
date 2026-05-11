using ECommeceSystem.EF.BaseRepository;
using ECommeceSystem.EF.BaseRepositry;
using ECommeceSystem.EF.Data;
using ECommeceSystem.EF.IRepositries;
using ECommeceSystem.EF.Models;
using ECommeceSystem.EF.Repository;
using System.Threading.Tasks;

namespace ECommeceSystem.EF.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IProductRepository Products { get; }

        public ICartItemRepository CartItems { get; }

        public IOrderRepository Orders { get; }

        public IGenericRepository<CategoryModel> Categories { get; }

        public IGenericRepository<UserModel> Users { get; }

        public IGenericRepository<OrderItemModel> OrderItems { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Products = new ProductRepostory(_context);

            CartItems = new CartItemRepository(_context);

            Orders = new OrderRepositry(_context);

            Categories =
                new GenericRepository<CategoryModel>(_context);

            Users =
                new GenericRepository<UserModel>(_context);

            OrderItems =
                new GenericRepository<OrderItemModel>(_context);
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}