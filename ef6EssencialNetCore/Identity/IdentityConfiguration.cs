using ef6EssencialNetCore.Context;
using Microsoft.AspNetCore.Identity;

namespace ef6EssencialNetCore.Identity;

    public static class IdentityConfiguration
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        }
    }
