using CompanyName.MyMeetings.BuildingBlocks.Application;
using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Authorization;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Authorization.GetPermissions;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace CompanyName.MyMeetings.Modules.UsersMI.Infrastructure.Configuration.Identity;

internal class HasPermissionAuthorizationHandler : AttributeAuthorizationHandler<HasPermissionAuthorizationRequirement, HasPermissionAttribute>
{
    private readonly IUserAccessModule _userManagementModule;
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public HasPermissionAuthorizationHandler(
        IUserAccessModule userManagementModule,
        IExecutionContextAccessor executionContextAccessor)
    {
        _userManagementModule = userManagementModule;
        _executionContextAccessor = executionContextAccessor;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        HasPermissionAuthorizationRequirement requirement,
        HasPermissionAttribute attribute)
    {
        if (!_executionContextAccessor.IsAvailable)
        {
            context.Fail();
            return;
        }

        var userId = _executionContextAccessor.UserId;
        var response = await _userManagementModule.ExecuteQueryAsync(new GetPermissionsQuery(userId));
        if (!response.IsSuccess)
        {
            context.Fail();
            return;
        }

        var permissions = response.Value ?? Enumerable.Empty<PermissionDto>();

        // Short circuit if the user owns the administrator privilege.
        if (permissions.Any(x => x.Code.Equals(ApplicationPermissions.Administrator)))
        {
            context.Succeed(requirement);
            return;
        }

        // Check if the user owns the necessary rights.
        if (!IsAuthorized(attribute.Name, permissions))
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }

    private bool IsAuthorized(string permission, IEnumerable<PermissionDto> permissions)
    {
        return permissions.Any(x => x.Code == permission);
    }
}