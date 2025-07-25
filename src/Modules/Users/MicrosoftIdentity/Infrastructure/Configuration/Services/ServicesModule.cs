using Autofac;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Infrastructure.Services.IdentityTokenService;

namespace CompanyName.MyMeetings.Modules.UsersMI.Infrastructure.Configuration.Services;

internal class ServicesModule : Module
{
    private readonly UserAccessConfiguration _userAccessConfiguration;

    public ServicesModule(UserAccessConfiguration userAccessConfiguration)
    {
        _userAccessConfiguration = userAccessConfiguration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<IdentityTokenClaimService>()
            .As<ITokenClaimsService>()
            .InstancePerLifetimeScope();

        builder.Register(x => _userAccessConfiguration)
            .As<UserAccessConfiguration>();
    }
}