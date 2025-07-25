using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Authorization.GetPermissions;

public class GetPermissionsQuery : QueryBase<Result<IEnumerable<PermissionDto>>>
{
    public GetPermissionsQuery(Guid? userId)
    {
        UserId = userId;
    }

    public Guid? UserId { get; }
}