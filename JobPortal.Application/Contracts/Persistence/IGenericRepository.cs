using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Contracts.Persistence
{
    public interface IGenericRepository<T>
     where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<int> CountAsync();
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
        Task<IReadOnlyList<T>> GetPagedAsync(
    int pageNumber,
    int pageSize);
    }
}
