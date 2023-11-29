//System
using System.Text.Json.Serialization;

//Project
using ef6EssencialNetCore.DTO.Map;
using ef6EssencialNetCore.Extensions;
using ef6EssencialNetCore.Repository;
using ef6EssencialNetCore.Identity;
using ef6EssencialNetCore.Log;


var builder = WebApplication.CreateBuilder(args);

//json - Desserialização | using System.Text.Json.Serialization;
builder.Services.AddControllers().AddJsonOptions(
    options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// IOC => MySQL
MySqlConfig.ConfigureDatabase(builder.Services, builder.Configuration);

//Repository/UnitOfWork  || VALIDAR O USO DE IOC
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// IOC IdentityConfiguration (USER | JWT)
IdentityConfiguration.ConfigureIdentity(builder.Services);

// IOC MappingService (DTO)
builder.Services.AddMapping();

// IOC  LogConfiguration
LogConfiguration.ConfigureLogging(builder.Services); 

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

app.MapControllers();
app.Run();
