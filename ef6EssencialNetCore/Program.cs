//System
using System.Text.Json.Serialization;

//Project
using ef6EssencialNetCore.DTO.Map;
using ef6EssencialNetCore.Extensions;
using ef6EssencialNetCore.Repository;
using ef6EssencialNetCore.Identity;
using ef6EssencialNetCore.Filters;
using ef6EssencialNetCore.Swagger;
using ef6EssencialNetCore.JWT;
using ef6EssencialNetCore.Config;

var builder = WebApplication.CreateBuilder(args);

//json - Desserialização | using System.Text.Json.Serialization;
builder.Services.AddControllers().AddJsonOptions(
    options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

builder.Services.AddEndpointsApiExplorer();

//IOC => AddSwaggerGen + JWT UI
builder.Services.ConfigureSwagger();

// IOC => MySQL DefaultConnection || DockerConnection
string databaseConnection = builder.Configuration.GetConnectionString("DefaultConnection");
MySqlConfig.ConfigureDatabase(builder.Services, builder.Configuration, databaseConnection);

//Repository/UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// IOC IdentityConfiguration (USER | JWT)
IdentityConfiguration.ConfigureIdentity(builder.Services);

// IOC => MappingService (DTO)
builder.Services.AddMapping();

//IOC => Valida o Token JWT
builder.Services.ConfigureJwt(builder.Configuration);

// IOC => LogConfiguration
builder.Services.AddScoped<ApiLogginFilter>();

// IOC => CORS Restritivo URL
builder.Services.ConfigureCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Extensions/ApiExceptionMiddlewareExtensions
app.ConfigureExceptionHandler();

app.UseHttpsRedirection();

//Middleware | UseAuthentication => UseAuthorization 
app.UseAuthentication();
app.UseAuthorization();

// CORS Restritivo URL
app.UseCors(
    opt => opt.WithOrigins("https://gorest.co.in/")
    .WithMethods("GET")
);

app.MapControllers();
app.Run();
