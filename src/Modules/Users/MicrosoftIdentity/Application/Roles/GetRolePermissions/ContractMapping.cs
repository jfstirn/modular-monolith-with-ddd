using CompanyName.MyMeetings.Modules.UsersMI.Domain.Repositories;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.GetRolePermissions;

internal static class ContractMapping
{
    public static GetPermissionsOptions MapToOptions(this GetRolePermissionsQuery query)
        => new GetPermissionsOptions(null);

    public static GetPermissionsOptions WithCodes(this GetPermissionsOptions permissionsOptions, IEnumerable<string> codes)
        => new GetPermissionsOptions(codes);
}