using CompanyName.MyMeetings.Modules.UsersMI.Application;
using CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Me;
using FluentValidation;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi.Endpoints.Me.Validators;

internal class ChangeEmailAddressRequestValidator : AbstractValidator<ChangeEmailAddressRequest>
{
    public ChangeEmailAddressRequestValidator()
    {
        RuleFor(x => x.Token).CustomNotEmpty();
        RuleFor(x => x.NewEmailAddress).CustomEmailAddress();
    }
}