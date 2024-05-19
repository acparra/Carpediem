using System.Net;
using System.Text.Json.Serialization;
using Carpediem.Controllers.Utils;
using Carpediem.Service.Users;
using Microsoft.AspNetCore.Mvc;

namespace Carpediem.Controllers.Users
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly UserService UsersService;

        public UserController(UserService usersService)
        {
            UsersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await UsersService.Get();
            var response = users.Select(u => new UserResponse
            {
                ID = u.ID,
                Username = u.Username,
                Password = u.Password,
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
                Password = user.Password,
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
                Password = user.Password,
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
            var user = new AddUserDto
            {
                Username = data.Username,
                Password = data.Password,
                RolID = data.RolID
            };
            var result = await UsersService.Add(user);
            if (result == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ControllerResponse
                {
                    Message = "User not added",
                    Data = null
                });
            }
            var response = new UserResponse
            {
                ID = result.ID,
                Username = result.Username,
                Password = result.Password,
                RolID = result.RolID
            };

            var controllerResponse = new ControllerResponse
            {
                Message = response != null ? "User added" : "User not added",
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
                return StatusCode((int)HttpStatusCode.InternalServerError, new ControllerResponse
                {
                    Message = "User not updated",
                    Data = null
                });
            }
            var response = new UserResponse
            {
                ID = result.ID,
                Username = result.Username,
                Password = result.Password,
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
        [JsonPropertyName("password")]
        public string Password { get; set; }
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
}