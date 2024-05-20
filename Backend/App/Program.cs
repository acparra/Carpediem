using Carpediem.Middlewares;
using Carpediem.Repository;
using Carpediem.Repository.MySQL;
using Carpediem.Service;
using Carpediem.Service.Persons;
using Carpediem.Service.Rol;
using Carpediem.Service.Users;
using Carpediem.Utils;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddValidationsService();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddToken(builder.Configuration);

builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IRolRepository, RolRepository>();
builder.Services.AddSingleton<IPersonRepository, PersonRepository>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<RolService>();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddSingleton<AuthenticationService>();
builder.Services.AddSingleton<PersonService>();

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
