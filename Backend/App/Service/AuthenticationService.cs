using Carpediem.Repository;
using Carpediem.Repository.MySQL;
using Carpediem.Service.Rol;
using Carpediem.Utils;

namespace Carpediem.Service
{
    public class AuthenticationService
    {
        private readonly IUserRepository UserRepository;
        private readonly RolService RolService;
        private readonly TokenService TokenService;

        public AuthenticationService(IUserRepository userRepository, RolService rolService, TokenService tokenService)
        {
            UserRepository = userRepository;
            RolService = rolService;
            TokenService = tokenService;
        }

        public async Task<AuthenticateOutDto> Authenticate(AuthenticateInDto data)
        {
            var user = await UserRepository.GetByUsername(data.Username);
            if (user == null)
            {
                return new AuthenticateOutDto
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (!PasswordService.VerifyPassword(data.Password, user.Password))
            {
                return new AuthenticateOutDto
                {
                    Success = false,
                    Message = "Invalid password"
                };
            }

            var rol = await RolService.GetById(user.RolID);
            if (rol == null)
            {
                return new AuthenticateOutDto
                {
                    Success = false,
                    Message = "Rol not found"
                };
            }

            var token = TokenService.GenerateToken(user.ID, rol.Name, user.Username);

            return new AuthenticateOutDto
            {
                Success = true,
                Message = "Authenticated",
                Token = token
            };
        }

    }

    public class AuthenticateInDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticateOutDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}