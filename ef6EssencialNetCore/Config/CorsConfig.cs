namespace ef6EssencialNetCore.Config;

    public static class CorsConfig
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(opt => 
            {
                opt.AddPolicy("PermitirApiRequest", 
                    builder => builder.WithOrigins("https://gorest.co.in/")
                    .WithMethods("GET")
                );
            });
        }
    }


