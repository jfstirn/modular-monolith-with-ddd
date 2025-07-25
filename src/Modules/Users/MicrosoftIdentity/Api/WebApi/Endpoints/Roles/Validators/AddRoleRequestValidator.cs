using CompanyName.MyMeetings.Modules.UsersMI.Application;
using CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Roles;
using FluentValidation;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi.Endpoints.Roles.Validators;

internal class AddRoleRequestValidator : AbstractValidator<AddRoleRequest>
{
    public AddRoleRequestValidator()
    {
        RuleFor(x => x.Name).CustomNotEmpty();
        RuleFor(x => x.Permissions).CustomNotEmpty();
    }
}
