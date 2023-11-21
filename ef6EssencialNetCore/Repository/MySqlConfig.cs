using Microsoft.EntityFrameworkCore;
using ef6EssencialNetCore.Context; 

namespace ef6EssencialNetCore.Repository; 

    public static class MySqlConfig
    {
        public static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                string mysqlConnection = configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(mysqlConnection, ServerVersion.AutoDetect(mysqlConnection)));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao configurar o banco de dados MySQL: {ex}");
                throw;
            }
        }
    }