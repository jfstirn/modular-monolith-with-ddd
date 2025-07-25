using CompanyName.MyMeetings.Modules.UsersMI.Application;
using CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Users;
using FluentValidation;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi.Endpoints.Users.Validators;

internal class SetUserRolesRequestValidator : AbstractValidator<SetUserRolesRequest>
{
    public SetUserRolesRequestValidator()
    {
        RuleFor(x => x.RoleIds).CustomNotNull();
    }
}