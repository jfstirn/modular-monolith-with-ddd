using Autofac;
using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Authorization;
using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.ModuleHosting;
using CompanyName.MyMeetings.Modules.UserAccess.Application.Contracts;
using CompanyName.MyMeetings.Modules.UserAccess.Infrastructure.Configuration.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.MyMeetings.Modules.UserAccess.Infrastructure.Configuration.ModuleHosting;

public class UserAccessModule(IConfiguration hostConfiguration) : ModuleBase(hostConfiguration)
{
    public override string WebApiAssemblySearchPattern => "*.Modules.UserAccess.WebApi.dll";

    public override void InitializeModule(HostServices hostServices)
    {
        UserAccessStartup.Initialize(
                hostServices.ConnectionString,
                hostServices.ExecutionContextAccessor,
                hostServices.Logger,
                hostServices.EmailsConfiguration,
                hostServices.TextEncryptionKey,
                null,
                null);

        hostServices.ApplicationBuilder.AddIdentityService();
    }

    public override void RegisterModule(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterType<Infrastructure.UserAccessModule>()
            .As<IUserAccessModule>()
            .InstancePerLifetimeScope();
    }

    protected override void AddHostServices(IServiceCollection services)
    {
        services.ConfigureIdentityService()
            .AddAuthorization(options =>
            {
                options.AddPolicy(HasPermissionAttribute.HasPermissionPolicyName, policyBuilder =>
                {
                    policyBuilder.Requirements.Add(new HasPermissionAuthorizationRequirement());
                    policyBuilder.AddAuthenticationSchemes("Bearer");
                });
            })
            .AddScoped<IAuthorizationHandler, HasPermissionAuthorizationHandler>();
    }
}