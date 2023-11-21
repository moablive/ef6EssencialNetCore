//Nuget
using AutoMapper;

//System
using System.Text.Json.Serialization;

//Project
using ef6EssencialNetCore.DTO.Map;
using ef6EssencialNetCore.Extensions;
using ef6EssencialNetCore.Filters;
using ef6EssencialNetCore.Log;
using ef6EssencialNetCore.Repository;


var builder = WebApplication.CreateBuilder(args);

//json - Desserialização | using System.Text.Json.Serialization;
builder.Services.AddControllers().AddJsonOptions(
    options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// IOC => MySQL
MySqlConfig.ConfigureDatabase(builder.Services, builder.Configuration);

//Filters/ApiLogginFilter
builder.Services.AddScoped<ApiLogginFilter>();

//Repository/UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

#region DTO
    var mappingConfig = new MapperConfiguration(mc =>
    {
        mc.AddProfile(new MapProfile());
    });

    IMapper mapper = mappingConfig.CreateMapper();
    builder.Services.AddSingleton(mapper);
#endregion


// Configuração do LoggerFactory e do provedor de log personalizado
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration()));
LogLevel logLevel = LogLevel.Information;

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Extensions/ApiExceptionMiddlewareExtensions
app.ConfigureExceptionHandler();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
