using MasterDetails.API.Data;
using MasterDetails.API.Entities;
using MasterDetails.API.Interfaces;
using MasterDetails.API.Repositories;

namespace MasterDetails.API.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlogDbContext _db;

        public IGenericRepository<Role> Roles { get; private set; }
        public IGenericRepository<User> Users { get; private set; }

        public UnitOfWork(BlogDbContext db)
        {
            _db = db;

            Roles = new GenericRepository<Role>(_db);
            Users = new GenericRepository<User>(_db);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
