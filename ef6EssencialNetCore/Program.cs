using System.Text.Json.Serialization;
using ef6EssencialNetCore.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//json - Desserialização | using System.Text.Json.Serialization;
builder.Services.AddControllers().AddJsonOptions(
    options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//String DB {appsettings.json}
string mysqlConnectio = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseMySql(mysqlConnectio,ServerVersion.AutoDetect(mysqlConnectio)));
//String DB END

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
