using CompanyName.MyMeetings.Modules.UsersMI.Application;
using CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Roles;
using FluentValidation;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi.Endpoints.Roles.Validators;

internal class RenameRoleRequestValidator : AbstractValidator<RenameRoleRequest>
{
    public RenameRoleRequestValidator()
    {
        RuleFor(x => x.Name).CustomNotEmpty();
    }
}