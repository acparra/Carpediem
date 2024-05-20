using Carpediem.Repository.Entities;
using Carpediem.Utils;
using Dapper;
using MySqlConnector;

namespace Carpediem.Repository.MySQL
{
    public class DocumentRepository : IDocumentRepository
    {

        private readonly string connection;

        public DocumentRepository(IConfiguration configuration)
        {
            connection = configuration.GetConnectionString("MySQL");
            if (string.IsNullOrEmpty(connection))
            {
                throw new Exception("database connection is empty");
            }
        }

        public async Task<IEnumerable<DocumentEntity>> Get()
        {
            var query = "SELECT id, name FROM document_type";
            MySqlConnection con = new(connection);

            return await con.QueryAsync<DocumentEntity>(query);
        }

        public async Task<DocumentEntity> GetById(int id)
        {
            var query = "SELECT id, name FROM document_type WHERE id = @id";
            MySqlConnection con = new(connection);

            return await con.QuerySingleOrDefaultAsync<DocumentEntity>(query, new { id });
        }

        public async Task<DocumentEntity> Add(DocumentEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<DocumentEntity> Update(DocumentEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

    }

}