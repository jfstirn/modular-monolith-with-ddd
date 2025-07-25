using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.GetRoles.Directory;

public class GetRolesQuery : QueryBase<Result<IEnumerable<RoleDto>>>
{
}