using Carpediem.Repository;
using Carpediem.Repository.MySQL;

namespace Carpediem.Service.Rol
{
    public class RolService : IRolService
    {
        private readonly IRolRepository RolRepository;

        public RolService(IRolRepository rolRepository)
        {
            RolRepository = rolRepository;
        }

        public async Task<IEnumerable<RolDto>> Get()
        {
            throw new NotImplementedException();
        }

        public async Task<RolDto> GetById(int id)
        {
            var rol = await RolRepository.GetById(id);
            if (rol == null)
            {
                return null;
            }
            var result = new RolDto
            {
                ID = rol.ID,
                Name = rol.Name,
            };

            return result;
        }


        public async Task<RolDto> Add(RolDto data)
        {
            throw new NotImplementedException();
        }

        public async Task<RolDto> Update(RolDto data)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        // DTOs
    }

    public class RolDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public interface IRolService
    {
        public Task<RolDto> GetById(int id);
    }
}