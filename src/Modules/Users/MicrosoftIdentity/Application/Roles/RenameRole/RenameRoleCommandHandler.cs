using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Commands;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.RenameRole;

internal class RenameRoleCommandHandler : ICommandHandler<RenameRoleCommand, Result>
{
    private readonly RoleManager<Role> _roleManager;

    public RenameRoleCommandHandler(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<Result> Handle(RenameRoleCommand request, CancellationToken cancellationToken)
    {
        var role = (from r in _roleManager.Roles
                    where r.Id == request.RoleId
                    select r).SingleOrDefault();
        if (role == null)
        {
            return Errors.General.NotFound(request.RoleId, "User role");
        }

        // Validate role name
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Errors.General.ValueIsRequired("Role name");
        }

        // Check for duplicate role name
        var existingRole = (from r in _roleManager.Roles
                            where r.Name == request.Name
                            select r).SingleOrDefault();
        if (existingRole != null && existingRole.Id != role.Id)
        {
            return Errors.General.ValueMustBeUnique("Role name");
        }

        var result = await _roleManager.SetRoleNameAsync(role, request.Name);
        if (!result.Succeeded)
        {
            return result.Errors.Map().Combine();
        }

        return Result.Ok();
    }
}