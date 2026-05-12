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

        public ICategoryRepository Categories { get; }

        public IUserRepository Users { get; }

        public IOrderItemRepository OrderItems { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Products = new ProductRepostory(_context);

            CartItems = new CartItemRepository(_context);

            Orders = new OrderRepositry(_context);

            Categories =new CategoryRepository(_context);

            Users = new UserRepository(_context);

            OrderItems = new OrderItemRepository(_context);
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