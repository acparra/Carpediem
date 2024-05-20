using Carpediem.Repository;
using Carpediem.Repository.MySQL;

namespace Carpediem.Service.Document
{
    public class DocumentService
    {
        private readonly IDocumentRepository DocumentRepository;

        public DocumentService(IDocumentRepository documentRepository)
        {
            DocumentRepository = documentRepository;
        }

        public async Task<IEnumerable<DocumentDto>> Get()
        {
            var documents = await DocumentRepository.Get();
            var result = documents.Select(r => new DocumentDto
            {
                ID = r.ID,
                Name = r.Name,
            });

            return result;
        }

        public async Task<DocumentDto> GetById(int id)
        {
            var rol = await DocumentRepository.GetById(id);
            if (rol == null)
            {
                return null;
            }
            var result = new DocumentDto
            {
                ID = rol.ID,
                Name = rol.Name,
            };

            return result;
        }


        public async Task<DocumentDto> Add(DocumentDto data)
        {
            throw new NotImplementedException();
        }

        public async Task<DocumentDto> Update(DocumentDto data)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        // DTOs
    }

    public class DocumentDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

}