using System.Reflection;
using Autofac;
using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
#nullable enable

namespace CompanyName.MyMeetings.BuildingBlocks.Infrastructure.ModuleHosting;

public abstract class ModuleBase(IConfiguration hostConfiguration) : IModule
{
    public abstract string WebApiAssemblySearchPattern { get; }

    public Assembly? WebApiAssembly
    {
        get
        {
            var file = Directory
                .GetFiles(AppContext.BaseDirectory, WebApiAssemblySearchPattern)
                .SingleOrDefault();

            return file is null ? null : Assembly.LoadFrom(file);
        }
    }

    public IConfiguration HostConfiguration { get; } = hostConfiguration;

    public abstract void RegisterModule(ContainerBuilder containerBuilder);

    public abstract void InitializeModule(HostServices hostServices);

    public void AddHostServices(IServiceCollection services, ApplicationPartManager applicationPartManager)
    {
        RegisterModuleParts(applicationPartManager);
        AddHostServices(services);
    }

    public virtual void ConfigureSwagger(SwaggerGenOptions options)
    {
        if (WebApiAssembly is null)
        {
            return;
        }

        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var commentsFileName = $"{WebApiAssembly.GetName().Name}.xml";
        var commentsFile = Path.Combine(baseDirectory, commentsFileName);
        options.IncludeXmlComments(commentsFile);
    }

    /// <summary>
    /// Adds application-specific services to the provided service collection for host configuration.
    /// </summary>
    /// <remarks>Implementations should register any services required for the host's operation. This method
    /// is typically called during application startup to configure dependency injection.</remarks>
    /// <param name="services">The service collection to which host services will be added.</param>
    protected abstract void AddHostServices(IServiceCollection services);

    /// <summary>
    /// Registers the Web API assembly as an application part with the specified <see cref="ApplicationPartManager"/>
    /// instance, enabling discovery of controllers and other MVC features from that assembly.
    /// </summary>
    /// <remarks>This method searches for the Web API assembly in the application's base directory using a
    /// predefined search pattern. If the assembly is found, it is loaded and added to the <paramref
    /// name="applicationPartManager"/>. This allows ASP.NET Core to discover MVC controllers and related features
    /// defined in the Web API assembly.</remarks>
    /// <param name="applicationPartManager">The <see cref="ApplicationPartManager"/> to which the Web API assembly will be added as an application part.</param>
    private void RegisterModuleParts(ApplicationPartManager applicationPartManager)
    {
        if (WebApiAssembly is not null)
        {
            AuthorizationChecker.CheckAllEndpoints(WebApiAssembly);
            applicationPartManager.ApplicationParts.Add(new AssemblyPart(WebApiAssembly));
        }
    }
}