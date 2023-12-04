//External
using Microsoft.AspNetCore.Authentication.JwtBearer;

//System
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;

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

//Repository/UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// IOC IdentityConfiguration (USER | JWT)
IdentityConfiguration.ConfigureIdentity(builder.Services);

//Valida o Token JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["TokenConfigurations:Issuer"],
            ValidAudience = builder.Configuration["TokenConfigurations:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });

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
