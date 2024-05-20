using Carpediem.Repository.Entities;
using Carpediem.Utils;
using Dapper;
using MySqlConnector;

namespace Carpediem.Repository.MySQL
{
    public class PersonRepository : IPersonRepository
    {
        private readonly string connection;

        public PersonRepository(IConfiguration configuration)
        {
            connection = configuration.GetConnectionString("MySQL");
            if (string.IsNullOrEmpty(connection))
            {
                throw new Exception("database connection is empty");
            }
        }

        public async Task<IEnumerable<PersonEntity>> Get()
        {
            var query = "SELECT id, name, last_name AS lastname, sex, eps, email, phone, document_number AS documentnumber, document_type_id AS documenttypeid, user_id AS userid FROM persons";
            MySqlConnection con = new(connection);

            return await con.QueryAsync<PersonEntity>(query);
        }

        public async Task<PersonEntity> GetById(int id)
        {
            var query = "SELECT id, name, last_name AS lastname, sex, eps, email, phone, document_number AS documentnumber, document_type_id AS documenttypeid, user_id AS userid FROM persons WHERE id = @id";
            MySqlConnection con = new(connection);

            return await con.QuerySingleOrDefaultAsync<PersonEntity>(query, new { id });
        }

        public async Task<PersonEntity> GetByName(string name)
        {
            var query = "SELECT id, name, last_name AS lastname, sex, eps, email, phone, document_number AS documentnumber, document_type_id AS documenttypeid, user_id AS userid FROM persons WHERE name = @name";
            MySqlConnection con = new(connection);

            return await con.QuerySingleOrDefaultAsync<PersonEntity>(query, new { name });
        }

        public async Task<PersonEntity> Add(PersonEntity entity)
        {
            var query = @"INSERT INTO persons (name, last_name, sex, eps, email, phone, document_number, document_type_id, user_id) 
            VALUES (@name, @lastname, @sex, @eps, @email, @phone, @documentnumber, @documenttypeid, @userid);
            SELECT LAST_INSERT_ID();";

            try
            {
                MySqlConnection con = new(connection);

                var result = await con.QuerySingleOrDefaultAsync<int>(query, entity);
                entity.Id = result;

                return entity;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 1062:
                        throw new RepositoryException("Person already exists", ex);
                    default:
                        throw new RepositoryException("Person could not be created", ex);
                }
            }
        }

        public async Task<PersonEntity> Update(PersonEntity entity)
        {
            var query = "UPDATE persons SET name = @name, last_name = @lastname, sex = @sex, eps=@eps, email=@email, phone=@phone, document_number=@documentnumber, " +
                "document_type_id=@documenttypeid  WHERE id = @Id";
            MySqlConnection con = new(connection);

            var result = await con.ExecuteAsync(query, entity);
            if (result == 0)
            {
                return null;
            }

            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            var query = "DELETE FROM persons WHERE id = @id";
            MySqlConnection con = new(connection);

            return await con.ExecuteAsync(query, new { id }) > 0;
        }
    }
}