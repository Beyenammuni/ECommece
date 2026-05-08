using Microsoft.OpenApi;

namespace ECommerceSystem.Extensions
{
    public static class SwaggerExtension
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ECommerceSystem API",
                    Description = "API documentation for ECommerceSystem",
                    Contact = new OpenApiContact
                    {
                        Name = "ECommerceSystem Developer",
                        Email = "Bayanammuny@gmail.com"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
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
        }
    }
}
