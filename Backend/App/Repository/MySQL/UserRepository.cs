using Carpediem.Repository.Entities;
using Carpediem.Utils;
using Dapper;
using MySqlConnector;

namespace Carpediem.Repository.MySQL
{
    public class UserRepository : IUserRepository
    {

        private readonly string connection;

        public UserRepository(IConfiguration configuration)
        {
            connection = configuration.GetConnectionString("MySQL");
            if (string.IsNullOrEmpty(connection))
            {
                throw new Exception("database connection is empty");
            }
        }

        public async Task<IEnumerable<UserEntity>> Get()
        {
            var query = "SELECT id, username, password, rol_id as RolID FROM users";
            MySqlConnection con = new(connection);

            return await con.QueryAsync<UserEntity>(query);
        }

        public async Task<UserEntity> GetById(int id)
        {
            var query = "SELECT id, username, password, rol_id as RolID FROM users WHERE id = @id";
            MySqlConnection con = new(connection);

            return await con.QuerySingleOrDefaultAsync<UserEntity>(query, new { id });
        }

        public async Task<UserEntity> GetByUsername(string username)
        {
            var query = "SELECT id, username, password, rol_id as RolID FROM users WHERE username = @username";
            MySqlConnection con = new(connection);

            return await con.QuerySingleOrDefaultAsync<UserEntity>(query, new { username });
        }

        public async Task<UserEntity> Add(UserEntity entity)
        {
            var query = @"INSERT INTO users (username, password, rol_id) VALUES (@username, @password, @rolid);
            SELECT LAST_INSERT_ID();";

            try
            {
                MySqlConnection con = new(connection);

                var result = await con.QuerySingleOrDefaultAsync<int>(query, entity);
                entity.ID = result;

                return entity;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1062:
                        throw new RepositoryException("Username already exists", ex);
                    case 1452:
                        throw new RepositoryException("Role not found", ex);
                    default:
                        throw new RepositoryException("User could not be created", ex);
                }
            }
        }

        public async Task<UserEntity> Update(UserEntity entity)
        {
            var query = "UPDATE users SET username = @username, password = @password, rol_id = @rolid WHERE id = @id";

            try
            {
                MySqlConnection con = new(connection);
                var result = await con.ExecuteAsync(query, entity);
                if (result == 0)
                {
                    return null;
                }

                return entity;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1062:
                        throw new RepositoryException("Username already exists", ex);
                    case 1452:
                        throw new RepositoryException("Role not found", ex);
                    default:
                        throw new RepositoryException("User could not be updated", ex);
                }
            }
        }

        public async Task<bool> Delete(int id)
        {
            var query = "DELETE FROM users WHERE id = @id";

            try
            {
                MySqlConnection con = new(connection);

                return await con.ExecuteAsync(query, new { id }) > 0;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1451:
                        throw new RepositoryException("User has dependencies", ex);
                    default:
                        throw new RepositoryException("User could not be deleted", ex);
                }
            }
        }

    }

}