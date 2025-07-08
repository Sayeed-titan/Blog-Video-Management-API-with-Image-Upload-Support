using System.Linq.Expressions;

namespace MasterDetails.API.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task AddAsync(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Update(T entity);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> filter);
    }
}
