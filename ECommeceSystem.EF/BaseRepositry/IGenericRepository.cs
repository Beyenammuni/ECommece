using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommeceSystem.EF.BaseRepositry
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
