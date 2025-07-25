using System.Reflection;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;

namespace CompanyName.MyMeetings.Modules.UsersMI.Infrastructure.Configuration;

/// <summary>
/// Static helper class referring to the module assemblies.
/// </summary>
internal static class Assemblies
{
    /// <summary>
    /// Get the application assembly.
    /// </summary>
    public static readonly Assembly Application = typeof(IUserAccessModule).Assembly;

    /// <summary>
    /// Get the infrastructure assembly.
    /// </summary>
    public static readonly Assembly Infrastructure = typeof(UserAccessStartup).Assembly;
}