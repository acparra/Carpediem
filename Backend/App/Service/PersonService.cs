using Carpediem.Repository;
using Carpediem.Repository.Entities;
using Carpediem.Repository.MySQL;
using Carpediem.Service.Users;
using Carpediem.Utils;

namespace Carpediem.Service.Persons
{
    public class PersonService
    {
        private readonly IPersonRepository personRepository;

        public PersonService(IPersonRepository prepository)
        {
            personRepository = prepository;
        }

        public async Task<IEnumerable<PersonDto>> Get()
        {
            var persons = await personRepository.Get();
            var result = persons.Select(p => new PersonDto
            {
                Id = p.Id,
                Name = p.Name,
                LastName = p.LastName,
                Sex = p.Sex,
                Eps = p.Eps,
                Email = p.Email,
                Phone = p.Phone,
                DocumentNumber = p.DocumentNumber,
                DocumentTypeId = p.DocumentTypeId,
                UserId = p.UserId
            });

            return result;
        }

        public async Task<PersonDto> GetById(int id)
        {
            var person = await personRepository.GetById(id);
            if (person == null)
            {
                return null;
            }
            var result = new PersonDto
            {
                Id = person.Id,
                Name = person.Name,
                LastName = person.LastName,
                Sex = person.Sex,
                Eps = person.Eps,
                Email = person.Email,
                Phone = person.Phone,
                DocumentNumber = person.DocumentNumber,
                DocumentTypeId = person.DocumentTypeId,
                UserId = person.UserId
            };

            return result;
        }

        public async Task<PersonDto> GetByName(string name)
        {
            var person = await personRepository.GetByName(name);
            if (person == null)
            {
                return null;
            }
            var result = new PersonDto
            {
                Id = person.Id,
                Name = person.Name,
                LastName = person.LastName,
                Sex = person.Sex,
                Eps = person.Eps,
                Email = person.Email,
                Phone = person.Phone,
                DocumentNumber = person.DocumentNumber,
                DocumentTypeId = person.DocumentTypeId,
                UserId = person.UserId
            };

            return result;
        }

        public async Task<PersonDto> Add(AddPersonDto data)
        {
            var person = new PersonEntity
            {
                Name = data.Name,
                LastName = data.LastName,
                Sex = data.Sex,
                Eps = data.Eps,
                Email = data.Email,
                Phone = data.Phone,
                DocumentNumber = data.DocumentNumber,
                DocumentTypeId = data.DocumentTypeId,
                UserId = data.UserId
            };
            var result = await personRepository.Add(person);
            var response = new PersonDto
            {
                Id = result.Id,
                Name = result.Name,
                LastName = result.LastName,
                Sex = result.Sex,
                Eps = result.Eps,
                Email = result.Email,
                Phone = result.Phone,
                DocumentNumber = result.DocumentNumber,
                DocumentTypeId = result.DocumentTypeId,
                UserId = result.UserId
            };

            return response;
        }

        public async Task<PersonDto> Update(UpdatePersonDto data)
        {
            var person = new PersonEntity
            {
                Id = data.Id,
                Name = data.Name,
                LastName = data.LastName,
                Sex = data.Sex,
                Eps = data.Eps,
                Email = data.Email,
                Phone = data.Phone,
                DocumentNumber = data.DocumentNumber,
                DocumentTypeId = data.DocumentTypeId,
                UserId = data.UserId
            };
            var result = await personRepository.Update(person);
            if (result == null)
            {
                return null;
            }
            var response = new PersonDto
            {
                Id = result.Id,
                Name = result.Name,
                LastName = result.LastName,
                Sex = result.Sex,
                Eps = result.Eps,
                Email = result.Email,
                Phone = result.Phone,
                DocumentNumber = result.DocumentNumber,
                DocumentTypeId = result.DocumentTypeId,
                UserId = result.UserId
            };

            return response;
        }

        public async Task<bool> Delete(int id)
        {
            return await personRepository.Delete(id);
        }



    }

    public class PersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool Sex { get; set; }
        public string Eps { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public int DocumentNumber { get; set; }
        public int DocumentTypeId { get; set; }
        public int UserId { get; set; }
    }

    public class AddPersonDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool Sex { get; set; }
        public string Eps { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public int DocumentNumber { get; set; }
        public int DocumentTypeId { get; set; }
        public int UserId { get; set; }

    }
    public class UpdatePersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool Sex { get; set; }
        public string Eps { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public int DocumentNumber { get; set; }
        public int DocumentTypeId { get; set; }
        public int UserId { get; set; }

    }


}