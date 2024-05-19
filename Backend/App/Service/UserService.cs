using Carpediem.Repository;
using Carpediem.Repository.Entities;
using Carpediem.Utils;

namespace Carpediem.Service.Users
{
    public class UserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository urepository)
        {
            userRepository = urepository;
        }

        public async Task<IEnumerable<UserDto>> Get()
        {
            var users = await userRepository.Get();
            var result = users.Select(u => new UserDto
            {
                ID = u.ID,
                Username = u.Username,
                RolID = u.RolID
            });

            return result;
        }

        public async Task<UserDto> GetById(int id)
        {
            var user = await userRepository.GetById(id);
            if (user == null)
            {
                return null;
            }
            var result = new UserDto
            {
                ID = user.ID,
                Username = user.Username,
                RolID = user.RolID
            };

            return result;
        }

        public async Task<UserDto> GetByUsername(string username)
        {
            var user = await userRepository.GetByUsername(username);
            if (user == null)
            {
                return null;
            }
            var result = new UserDto
            {
                ID = user.ID,
                Username = user.Username,
                RolID = user.RolID
            };

            return result;
        }

        public async Task<UserDto> Add(AddUserDto data)
        {
            var password = PasswordService.HashPassword(data.Password);
            var user = new UserEntity
            {
                Username = data.Username,
                Password = password,
                RolID = data.RolID
            };
            var result = await userRepository.Add(user);
            var response = new UserDto
            {
                ID = result.ID,
                Username = result.Username,
                RolID = result.RolID
            };

            return response;
        }

        public async Task<UserDto> Update(UpdateUserDto data)
        {
            var password = PasswordService.HashPassword(data.Password);
            var user = new UserEntity
            {
                ID = data.ID,
                Username = data.Username,
                Password = password,
                RolID = data.RolID
            };
            var result = await userRepository.Update(user);
            if (result == null)
            {
                return null;
            }
            var response = new UserDto
            {
                ID = result.ID,
                Username = result.Username,
                RolID = result.RolID
            };

            return response;
        }

        public async Task<bool> Delete(int id)
        {
            return await userRepository.Delete(id);
        }
    }

    // DTOs

    public class UserDto
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public int RolID { get; set; }
    }

    public class AddUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int RolID { get; set; }
    }

    public class UpdateUserDto
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RolID { get; set; }
    }

}