using AutoMapper;

namespace ef6EssencialNetCore.DTO.Map;

    public static class MappingService
    {
        public static IMapper ConfigureMappings()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapProfile());
                // Adicione outros perfis de mapeamento, se necessário
            });

            return mappingConfig.CreateMapper();
        }

        public static void AddMapping(this IServiceCollection services)
        {
            IMapper mapper = ConfigureMappings();
            services.AddSingleton(mapper);
        }
    }
