using System.Net;
using System.Text.Json.Serialization;
using Carpediem.Controllers.Utils;
using Carpediem.Service.Users;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carpediem.Controllers.Users
{
    [ApiController]
    [Route("users")]
    [Authorize(Roles = "Administrador")]
    public class UserController : ControllerBase
    {
        private readonly UserService UsersService;
        private readonly IValidator<AddUserRequest> AddUserRequestValidator;

        public UserController(UserService usersService, IValidator<AddUserRequest> addUserRequestValidator)
        {
            UsersService = usersService;
            AddUserRequestValidator = addUserRequestValidator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await UsersService.Get();
            var response = users.Select(u => new UserResponse
            {
                ID = u.ID,
                Username = u.Username,
                RolID = u.RolID
            });

            var controllerResponse = new ControllerResponse
            {
                Message = "Users retrieved sucessfully",
                Data = response.ToArray()
            };

            return Ok(controllerResponse);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByID([FromRoute] int id)
        {
            var user = await UsersService.GetById(id);
            if (user == null)
            {
                return NotFound(new ControllerResponse
                {
                    Message = "User not found",
                    Data = null
                });
            }

            var response = new UserResponse
            {
                ID = user.ID,
                Username = user.Username,
                RolID = user.RolID
            };

            var controllerResponse = new ControllerResponse
            {
                Message = "User found",
                Data = new object[] { response }
            };

            return Ok(controllerResponse);
        }

        [HttpGet]
        [Route("username/{username}")]
        public async Task<IActionResult> GetByUsername([FromRoute] string username)
        {
            var user = await UsersService.GetByUsername(username);
            if (user == null)
            {
                return NotFound(new ControllerResponse
                {
                    Message = "User not found",
                    Data = null
                });
            }

            var response = new UserResponse
            {
                ID = user.ID,
                Username = user.Username,
                RolID = user.RolID
            };

            var controllerResponse = new ControllerResponse
            {
                Message = "User found",
                Data = new object[] { response }
            };

            return Ok(controllerResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUserRequest data)
        {
            var validationResult = AddUserRequestValidator.Validate(data);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ControllerResponse
                {
                    Message = "Validation error",
                    Data = validationResult.Errors.Select(e => e.ErrorMessage).ToArray()
                });
            }

            var user = new AddUserDto
            {
                Username = data.Username,
                Password = data.Password,
                RolID = data.RolID
            };
            var result = await UsersService.Add(user);
            var response = new UserResponse
            {
                ID = result.ID,
                Username = result.Username,
                RolID = result.RolID
            };

            var controllerResponse = new ControllerResponse
            {
                Message = "User added",
                Data = new object[] { response }
            };

            return Ok(controllerResponse);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, AddUserRequest data)
        {
            var user = new UpdateUserDto
            {
                ID = id,
                Username = data.Username,
                Password = data.Password,
                RolID = data.RolID
            };
            var result = await UsersService.Update(user);
            if (result == null)
            {
                return NotFound(new ControllerResponse
                {
                    Message = "User not found",
                    Data = null
                });
            }

            var response = new UserResponse
            {
                ID = result.ID,
                Username = result.Username,
                RolID = result.RolID
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
            var result = await UsersService.Delete(id);
            if (!result)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ControllerResponse
                {
                    Message = "User not deleted",
                    Data = null
                });
            }

            var controllerResponse = new ControllerResponse
            {
                Message = "User deleted",
                Data = null
            };

            return Ok(controllerResponse);
        }
    }

    // DTOs

    public class UserResponse
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("rol_id")]
        public int RolID { get; set; }
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