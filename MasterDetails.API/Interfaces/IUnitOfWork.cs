using MasterDetails.API.Data;
using MasterDetails.API.Entities;

namespace MasterDetails.API.Interfaces
{
    public interface IUnitOfWork 
    {

        IGenericRepository<User> Users { get; }
        IGenericRepository<Role> Roles { get; }

        Task SaveAsync();
    }
}
