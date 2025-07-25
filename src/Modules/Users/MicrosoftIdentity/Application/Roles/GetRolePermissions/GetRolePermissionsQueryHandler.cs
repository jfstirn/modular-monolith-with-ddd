using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Queries;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using CompanyName.MyMeetings.Modules.UsersMI.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.GetRolePermissions;

internal class GetRolePermissionsQueryHandler : IQueryHandler<GetRolePermissionsQuery, Result<IEnumerable<PermissionDto>>>
{
    private readonly RoleManager<Role> _roleManager;
    private readonly IReadOnlyPermissionRepository _permissionRepository;

    public GetRolePermissionsQueryHandler(
        RoleManager<Role> roleManager,
        IReadOnlyPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
        _roleManager = roleManager;
    }

    public async Task<Result<IEnumerable<PermissionDto>>> Handle(GetRolePermissionsQuery request, CancellationToken cancellationToken)
    {
        var role = _roleManager.Roles
            .Where(x => x.Id == request.RoleId)
            .Select(x => x)
            .FirstOrDefault();

        if (role is null)
        {
            return Errors.General.NotFound(request.RoleId, "Role");
        }

        var roleClaims = await _roleManager.GetClaimsAsync(role);
        var claims = roleClaims
            .Where(x => x.Type == CustomClaimTypes.Permission)
            .Select(x => x.Value)
            .ToList();

        // Short circuit
        if (!claims.Any())
        {
            return Result.Ok(Enumerable.Empty<PermissionDto>());
        }

        var options = request
             .MapToOptions()
             .WithCodes(claims);

        var permissions = await _permissionRepository.GetPermissionsAsync(options, cancellationToken);
        var result = permissions.Select(p => new PermissionDto(p.Code, p.Name, p.Description)).ToList();
        return result;
    }
}