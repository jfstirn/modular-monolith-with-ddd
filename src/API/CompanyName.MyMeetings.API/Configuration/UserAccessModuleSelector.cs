using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.ModuleHosting;
using IdentityServerUserAccess = CompanyName.MyMeetings.Modules.UserAccess.Infrastructure.Configuration.ModuleHosting;
using MicrosoftIdentityUserAccess = CompanyName.MyMeetings.Modules.UsersMI.Infrastructure.Configuration.ModuleHosting;

namespace CompanyName.MyMeetings.API.Configuration;

internal static class UserAccessModuleSelector
{
    private const string IdentityServerModuleType = "IdentityServer";
    private const string MicrosoftIdentityModuleType = "MicrosoftIdentity";

    private static readonly string[] AcceptedModuleTypes = [IdentityServerModuleType, MicrosoftIdentityModuleType];

    public static void AddUserAccessModule(ModuleLoader moduleLoader, IConfiguration configuration)
    {
        var userModule = configuration["Modules:UserModule"];
        if (string.IsNullOrWhiteSpace(userModule) || !AcceptedModuleTypes.Contains(userModule))
        {
            throw new InvalidOperationException($"Invalid user module configuration. Accepted values are: {string.Join(", ", AcceptedModuleTypes)}");
        }

        if (userModule == IdentityServerModuleType)
        {
            moduleLoader.AddModule(new IdentityServerUserAccess.UserAccessModule(configuration));
            return;
        }

        if (userModule == MicrosoftIdentityModuleType)
        {
            moduleLoader.AddModule(new MicrosoftIdentityUserAccess.UserAccessModule(configuration));
            return;
        }
    }
}