using Microsoft.OpenApi;

namespace ClinicManagement.Helper
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ClinicManagement",
                    Description = "An ASP.NET Core Web API for managing clinic appointments",
                    Contact = new OpenApiContact
                    {
                        Name = "Bayan Ammuni",
                        Email = "bayanammuny@gmail.com"
                    }
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by your token. Example: Bearer abc123xyz"
                });

                options.AddSecurityRequirement(document =>
                {
                    var schemeReference = new OpenApiSecuritySchemeReference("Bearer", document);
                    return new OpenApiSecurityRequirement
                    {
                        [schemeReference] = []
                    };
                });

            });

            services.AddSwaggerGen();
            return services;
        }

    }
}
