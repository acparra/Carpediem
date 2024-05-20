using Carpediem.Controllers;
using Carpediem.Controllers.Users;
using FluentValidation;

namespace Carpediem.Utils
{
    public static class ValidationsServiceExtensions
    {
        public static IServiceCollection AddValidationsService(this IServiceCollection services)
        {
            return services.AddScoped<IValidator<AddUserRequest>, AddUserRequestValidator>()
                .AddScoped<IValidator<AddPersonRequest>, AddPersonRequestValidator>();
        }
    }
}