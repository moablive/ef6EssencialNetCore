using Microsoft.EntityFrameworkCore;
using ef6EssencialNetCore.Context; 

namespace ef6EssencialNetCore.Repository; 

    public static class MySqlConfig
    {
        public static void ConfigureDatabase(
            IServiceCollection services, 
            IConfiguration configuration
        )
        {
            string dockerConnection = configuration.GetConnectionString("DockerConnection");
            string defaultConnection = configuration.GetConnectionString("DefaultConnection");

            try
            {
                // Try to use the Docker connection
                services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(dockerConnection, ServerVersion.AutoDetect(dockerConnection)));
            }
            catch (Exception)
            {
                // If the Docker connection fails, use the default connection
                services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(defaultConnection, ServerVersion.AutoDetect(defaultConnection)));
            }
        }
    }