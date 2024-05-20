using Carpediem.Repository.Entities;

namespace Carpediem.Repository
{
    public interface IPersonRepository : IRepository<PersonEntity>
    {
        Task<PersonEntity> GetByName(string name);
    }
}