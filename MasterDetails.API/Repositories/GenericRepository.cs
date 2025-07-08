using MasterDetails.API.Data;
using MasterDetails.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace MasterDetails.API.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly BlogDbContext _db;
        internal DbSet<T> _dbSet;

        public GenericRepository(BlogDbContext db)
        {
            _db = db;
            _dbSet = db.Set<T>();
        }

        public GenericRepository(BlogDbContext db, DbSet<T> dbSet)
        {
            this._db = db;
            this._dbSet = dbSet;
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.AnyAsync(filter);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query = tracked ? _dbSet : _dbSet.AsNoTracking();
            query = query.Where(filter);

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
