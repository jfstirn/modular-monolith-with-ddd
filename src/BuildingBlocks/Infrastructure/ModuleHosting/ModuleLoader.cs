using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CompanyName.MyMeetings.BuildingBlocks.Infrastructure.ModuleHosting;

public class ModuleLoader
{
    private readonly List<IModule> _modules = new();

    public IReadOnlyCollection<IModule> Modules => _modules.AsReadOnly();

    public ModuleLoader AddModule(IModule module)
    {
        AddModules([module]);
        return this;
    }

    public ModuleLoader AddModules(IEnumerable<IModule> modules)
    {
        _modules.AddRange(modules);
        return this;
    }

    public ModuleLoader AddModules(params IModule[] modules)
    {
        _modules.AddRange(modules);
        return this;
    }

    public ModuleLoader AddModules(IConfiguration config, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var moduleTypes = assembly.GetTypes()
                .Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
            foreach (var moduleType in moduleTypes)
            {
                if (Activator.CreateInstance(moduleType, config) is IModule moduleInstance)
                {
                    _modules.Add(moduleInstance);
                }
            }
        }

        return this;
    }

    public void RegisterModules(ContainerBuilder containerBuilder)
    {
        foreach (var module in _modules)
        {
            module.RegisterModule(containerBuilder);
        }
    }

    public void AddHostServices(IServiceCollection services, ApplicationPartManager applicationPartManager)
    {
        foreach (var module in _modules)
        {
            module.AddHostServices(services, applicationPartManager);
        }
    }

    public void InitializeModules(HostServices hostServices)
    {
        foreach (var module in _modules)
        {
            module.InitializeModule(hostServices);
        }
    }

    public void ConfigureSwaggerModules(SwaggerGenOptions options)
    {
        foreach (var module in _modules)
        {
            module.ConfigureSwagger(options);
        }
    }
}