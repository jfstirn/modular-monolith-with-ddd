using System.Reflection;
using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.ModuleHosting;
using Microsoft.OpenApi.Models;

namespace CompanyName.MyMeetings.API.Configuration.Extensions
{
    internal static class SwaggerExtensions
    {
        internal static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, ModuleLoader moduleLoader)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MyMeetings API",
                    Version = "v1",
                    Description = "MyMeetings API for modular monolith .NET application."
                });
                options.CustomSchemaIds(t => t.ToString());

                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var commentsFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var commentsFile = Path.Combine(baseDirectory, commentsFileName);
                options.IncludeXmlComments(commentsFile);

                moduleLoader.ConfigureSwaggerModules(options);
            });

            return services;
        }

        internal static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyMeetings API"); });

            return app;
        }
    }
}