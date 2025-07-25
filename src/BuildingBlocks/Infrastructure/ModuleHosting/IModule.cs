using Autofac;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#nullable enable

namespace CompanyName.MyMeetings.BuildingBlocks.Infrastructure.ModuleHosting;

public interface IModule
{
    /// <summary>
    /// Gets the search pattern used to locate Web API assembly within the module.
    /// </summary>
    string WebApiAssemblySearchPattern { get; }

    /// <summary>
    /// Gets the host application's configuration.
    /// </summary>
    IConfiguration HostConfiguration { get; }

    /// <summary>
    /// Registers the module within the host application's dependency injection container.
    /// </summary>
    /// <param name="containerBuilder">The host's DI container builder.</param>
    void RegisterModule(ContainerBuilder containerBuilder);

    /// <summary>
    /// Adds the services provided by the module to the host application's service collection.
    /// </summary>
    /// <param name="services">The host's service collection.</param>
    /// <param name="applicationPartManager">The application part manager to register MVC components.</param>
    void AddHostServices(IServiceCollection services, ApplicationPartManager applicationPartManager);

    /// <summary>
    /// Initializes the module using the specified host services.
    /// </summary>
    /// <param name="hostServices">The services initialized by the host that can be used for module initialization.</param>
    void InitializeModule(HostServices hostServices);
}