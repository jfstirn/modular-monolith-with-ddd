using Autofac;
using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Authorization;
using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.ModuleHosting;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Infrastructure.Configuration.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.MyMeetings.Modules.UsersMI.Infrastructure.Configuration.ModuleHosting;

public class UserAccessModule(IConfiguration hostConfiguration) : ModuleBase(hostConfiguration)
{
    private readonly UserAccessConfiguration _userAccessConfiguration = hostConfiguration.GetUserAccessConfiguration();

    public override string WebApiAssemblySearchPattern => "*.Modules.UsersMI.WebApi.dll";

    public override void InitializeModule(HostServices hostServices)
    {
        UserAccessStartup.Initialize(
            hostServices.ConnectionString,
            hostServices.ExecutionContextAccessor,
            hostServices.Logger,
            hostServices.EmailsConfiguration,
            hostServices.TextEncryptionKey,
            null,
            null,
            _userAccessConfiguration);
    }

    public override void RegisterModule(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterType<Infrastructure.UserAccessModule>()
            .As<IUserAccessModule>()
            .InstancePerLifetimeScope();
    }

    protected override void AddHostServices(IServiceCollection services)
    {
        services.ConfigureIdentityService(_userAccessConfiguration)
            .AddAuthorization(options =>
            {
                // Update the default policy
                // Since the requests won't be authenticated automatically anymore, putting [Authorize] attributes
                // on some actions will result in the requests being rejected and an HTTP 401 will be issued.

                // Since that's not what we want because we want to give the authentication handlers a chance to
                // authenticate the request, we change the default policy of the authorization system by indicating
                // that the Bearer authentication scheme should be tried to authenticate the request.

                // That doesn't prevent you from being more restrictive on some actions; the [Authorize] attribute
                // has an AuthenticationSchemes property that allows you to override which authentication schemes are valid.

                // If you have more complex scenarios, you can make use of policy - based authorization.
                // The official documentation is great. https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .Build();

                options.AddPolicy(HasPermissionAttribute.HasPermissionPolicyName, policyBuilder =>
                {
                    policyBuilder.Requirements.Add(new HasPermissionAuthorizationRequirement());
                    policyBuilder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                });
            })
            .AddScoped<IAuthorizationHandler, HasPermissionAuthorizationHandler>();
    }
}