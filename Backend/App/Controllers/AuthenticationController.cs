using System.Text.Json.Serialization;
using Carpediem.Controllers.Utils;
using Carpediem.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carpediem.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService AuthenticationService;

        public AuthenticationController(AuthenticationService authenticationService)
        {
            AuthenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest data)
        {

            var authenticate = new AuthenticateInDto
            {
                Username = data.Username,
                Password = data.Password
            };
            var response = await AuthenticationService.Authenticate(authenticate);
            if (!response.Success)
            {
                return Unauthorized(new ControllerResponse
                {
                    Message = response.Message,
                    Data = null
                });
            }

            return Ok(new ControllerResponse
            {
                Message = response.Message,
                Data = new
                {
                    Token = response.Token
                }
            });
        }
    }

    public class AuthenticateRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}