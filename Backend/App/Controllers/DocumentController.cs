using System.Net;
using System.Text.Json.Serialization;
using Carpediem.Controllers.Utils;
using Carpediem.Repository.MySQL;
using Carpediem.Service.Document;
using Carpediem.Service.Users;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carpediem.Controllers.Document
{
    [ApiController]
    [Route("documents")]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly DocumentService DocumentService;
        private readonly IValidator<AddUserRequest> AddUserRequestValidator;

        public DocumentController(DocumentService documentService)
        {
            DocumentService = documentService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var documents = await DocumentService.Get();
            var response = documents.Select(d => new DocumentResponse
            {
                ID = d.ID,
                Name = d.Name
            });

            var controllerResponse = new ControllerResponse
            {
                Message = "Documents retrieved sucessfully",
                Data = response.ToArray()
            };

            return Ok(controllerResponse);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByID([FromRoute] int id)
        {
            var document = await DocumentService.GetById(id);
            if (document == null)
            {
                return NotFound(new ControllerResponse
                {
                    Message = "Document not found",
                    Data = null
                });
            }

            var response = new DocumentResponse
            {
                ID = document.ID,
                Name = document.Name
            };

            var controllerResponse = new ControllerResponse
            {
                Message = "Document found",
                Data = new object[] { response }
            };

            return Ok(controllerResponse);
        }

    }

    // DTOs

    public class DocumentResponse
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class AddUserRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("rol_id")]
        public int RolID { get; set; }
    }

    // Validators

    public class AddUserRequestValidator : AbstractValidator<AddUserRequest>
    {
        public AddUserRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("El nombre de usuario es requerido");
            RuleFor(x => x.Password).NotEmpty()
            .MinimumLength(10)
            .WithMessage("La clave debe tener al menos 10 caracteres")
            .Matches("[a-z]")
            .WithMessage("La clave debe tener al menos una letra minúscula")
            .Matches("[A-Z]")
            .WithMessage("La clave debe tener al menos una letra mayúscula")
            .Matches("[0-9]")
            .WithMessage("La clave debe tener al menos un número")
            .Matches("[^a-zA-Z0-9]")
            .WithMessage("La clave debe tener al menos un caracter especial");
            RuleFor(x => x.RolID).NotEmpty().WithMessage("El rol no puede ser 0");
        }
    }
}