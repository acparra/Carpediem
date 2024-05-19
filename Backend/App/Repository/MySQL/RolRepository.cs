using Carpediem.Repository.Entities;
using Carpediem.Utils;
using Dapper;
using MySqlConnector;

namespace Carpediem.Repository.MySQL
{
    public class RolRepository : IRolRepository
    {

        private readonly string connection;

        public RolRepository(IConfiguration configuration)
        {
            connection = configuration.GetConnectionString("MySQL");
            if (string.IsNullOrEmpty(connection))
            {
                throw new Exception("database connection is empty");
            }
        }

        public async Task<IEnumerable<RolEntity>> Get()
        {
            throw new NotImplementedException();
        }

        public async Task<RolEntity> GetById(int id)
        {
            var query = "SELECT id, name FROM roles WHERE id = @id";
            MySqlConnection con = new(connection);

            return await con.QuerySingleOrDefaultAsync<RolEntity>(query, new { id });
        }

        public async Task<RolEntity> Add(RolEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<RolEntity> Update(RolEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

    }

}