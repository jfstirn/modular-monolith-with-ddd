using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CompanyName.MyMeetings.Modules.UsersMI.Infrastructure.Configuration;

internal static class ConfigurationExtensions
{
    public static UserAccessConfiguration GetUserAccessConfiguration(this IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        var userManagementSection = configuration.GetSection("Modules:UserAccess");
        return userManagementSection.Get<UserAccessConfiguration>() ?? throw new InvalidOperationException("UserAccess configuration section is missing.");
    }

    public static string? GetValidAudience(this UserAccessConfiguration configuration)
        => configuration.Security?.JwtAudience;

    public static bool ShouldValidateAudience(this UserAccessConfiguration configuration)
        => !string.IsNullOrEmpty(configuration.GetValidAudience());

    public static string? GetValidIssuer(this UserAccessConfiguration configuration)
        => configuration.Security?.JwtIssuer;

    public static bool ShouldValidateIssuer(this UserAccessConfiguration configuration)
        => !string.IsNullOrEmpty(configuration.GetValidIssuer());

    public static byte[] GetJwtSecretKeyEncrypted(this UserAccessConfiguration configuration)
        => Encoding.ASCII.GetBytes(configuration.Security?.JwtSecretKey ?? string.Empty);

    public static SymmetricSecurityKey GetIssuerSigningKey(this UserAccessConfiguration configuration)
        => new SymmetricSecurityKey(configuration.GetJwtSecretKeyEncrypted());
}