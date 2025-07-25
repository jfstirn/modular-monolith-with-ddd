using CompanyName.MyMeetings.Modules.UsersMI.Application;
using CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Roles;
using FluentValidation;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi.Endpoints.Roles.Validators;

internal class SetRolePermissionsRequestValidator : AbstractValidator<SetRolePermissionsRequest>
{
    public SetRolePermissionsRequestValidator()
    {
        RuleFor(x => x.Permissions).CustomNotEmpty();
    }
}