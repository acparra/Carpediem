using System.Net;
using System.Text.Json.Serialization;
using Carpediem.Controllers.Users;
using Carpediem.Controllers.Utils;
using Carpediem.Service.Persons;
using Carpediem.Service.Users;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Superpower.Model;

namespace Carpediem.Controllers
{
    [ApiController]
    [Route("persons")]
    //[Authorize(Roles = "Administrador, Psicologo")]
    public class PersonController : ControllerBase
    {
        private readonly PersonService PersonsService;
        private readonly IValidator<AddPersonRequest> AddPersonRequestValidator;
        public PersonController(PersonService personsService, IValidator<AddPersonRequest> addPersonRequestValidator)
        {
            PersonsService = personsService;
            AddPersonRequestValidator = addPersonRequestValidator;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var person = await PersonsService.Get();
            var response = person.Select(p => new PersonResponse
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

            var controllerResponse = new ControllerResponse
            {
                Message = "Person retrieved sucessfully",
                Data = response.ToArray()
            };

            return Ok(controllerResponse);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByID([FromRoute] int id)
        {
            var person = await PersonsService.GetById(id);
            if (person == null)
            {
                return NotFound(new ControllerResponse
                {
                    Message = "Person not found",
                    Data = null
                });
            }

            var response = new PersonResponse
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

            var controllerResponse = new ControllerResponse
            {
                Message = "Person found",
                Data = new object[] { response }
            };

            return Ok(controllerResponse);
        }

        [HttpGet]
        [Route("name/{name}")]
        public async Task<IActionResult> GetByUsername([FromRoute] string name)
        {
            var person = await PersonsService.GetByName(name);
            if (person == null)
            {
                return NotFound(new ControllerResponse
                {
                    Message = "Person not found",
                    Data = null
                });
            }

            var response = new PersonResponse
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

            var controllerResponse = new ControllerResponse
            {
                Message = "Person found",
                Data = new object[] { response }
            };

            return Ok(controllerResponse);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddPersonRequest data)
        {
            var validationResult = AddPersonRequestValidator.Validate(data);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ControllerResponse
                {
                    Message = "Validation error",
                    Data = validationResult.Errors.Select(e => e.ErrorMessage).ToArray()
                });
            }

            var person = new AddPersonDto
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
            var result = await PersonsService.Add(person);
            var response = new PersonResponse
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

            var controllerResponse = new ControllerResponse
            {
                Message = "Person added",
                Data = new object[] { response }
            };

            return Ok(controllerResponse);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, AddPersonRequest data)
        {
            var validationResult = AddPersonRequestValidator.Validate(data);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ControllerResponse
                {
                    Message = "Validation error",
                    Data = validationResult.Errors.Select(e => e.ErrorMessage).ToArray()
                });
            }

            var person = new UpdatePersonDto
            {
                Id = id,
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
            var result = await PersonsService.Update(person);
            if (result == null)
            {
                return NotFound(new ControllerResponse
                {
                    Message = "Person not found",
                    Data = null
                });
            }

            var response = new PersonResponse
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

            var controllerResponse = new ControllerResponse
            {
                Message = "User updated",
                Data = new object[] { response }
            };

            return Ok(controllerResponse);
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await PersonsService.Delete(id);
            if (!result)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ControllerResponse
                {
                    Message = "Person not deleted",
                    Data = null
                });
            }

            var controllerResponse = new ControllerResponse
            {
                Message = "Person deleted",
                Data = null
            };

            return Ok(controllerResponse);
        }
    }

    public class PersonResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }
        [JsonPropertyName("sex")]
        public bool Sex { get; set; }
        [JsonPropertyName("eps")]
        public string Eps { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("phone")]
        public int Phone { get; set; }
        [JsonPropertyName("document_number")]
        public int DocumentNumber { get; set; }
        [JsonPropertyName("document_type_id")]
        public int DocumentTypeId { get; set; }
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
    }
    public class AddPersonRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }
        [JsonPropertyName("sex")]
        public bool Sex { get; set; }
        [JsonPropertyName("eps")]
        public string Eps { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("phone")]
        public int Phone { get; set; }
        [JsonPropertyName("document_number")]
        public int DocumentNumber { get; set; }
        [JsonPropertyName("document_type_id")]
        public int DocumentTypeId { get; set; }
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
    }

    public class AddPersonRequestValidator : AbstractValidator<AddPersonRequest>
    {
        public AddPersonRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre de la persona es requerido");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("El apellido de la persona es requerido");
            RuleFor(x => x.Sex).NotEmpty().WithMessage("El sexo de la persona es requerido");
            RuleFor(x => x.Eps).NotEmpty().WithMessage("La EPS de la persona es requerido");
            RuleFor(x => x.Email).NotEmpty().WithMessage("El correo de la persona es requerido");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("El telefono de la persona es requerido");
            RuleFor(x => x.DocumentNumber).NotEmpty().WithMessage("El numero de documento de la persona es requerido");
            RuleFor(x => x.DocumentTypeId).NotEmpty().WithMessage("El tipo de documento de la persona es requerido");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("El id del usuario de la persona es requerido");
        }
    }
}
