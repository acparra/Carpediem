using Carpediem.Repository.Entities;

namespace Carpediem.Repository
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        Task<UserEntity> GetByUsername(string user);
    }
}