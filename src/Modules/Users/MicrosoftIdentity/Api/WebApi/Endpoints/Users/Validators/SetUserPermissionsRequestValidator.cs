using CompanyName.MyMeetings.Modules.UsersMI.Application;
using CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Users;
using FluentValidation;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi.Endpoints.Users.Validators;

internal class SetUserPermissionsRequestValidator : AbstractValidator<SetUserPermissionsRequest>
{
    public SetUserPermissionsRequestValidator()
    {
        RuleFor(x => x.Permissions).CustomNotNull();
    }
}