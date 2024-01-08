using Microsoft.EntityFrameworkCore;
using ef6EssencialNetCore.Context; 

namespace ef6EssencialNetCore.Repository; 

public static class MySqlConfig
{
    public static void ConfigureDatabase(
        IServiceCollection services, 
        IConfiguration configuration,
        string databaseConnection
    )
    {
        try
        {
            // Try to use the passed database connection
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(databaseConnection, ServerVersion.AutoDetect(databaseConnection)));
        }
        catch (Exception)
        {
            // If the passed database connection fails, use the default connection
            string defaultConnection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(defaultConnection, ServerVersion.AutoDetect(defaultConnection)));
        }
    }
}