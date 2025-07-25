using CompanyName.MyMeetings.Modules.UsersMI.Application;
using CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Me;
using FluentValidation;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi.Endpoints.Me.Validators;

internal class RequestChangeEmailAddressTokenRequestValidator : AbstractValidator<RequestChangeEmailAddressTokenRequest>
{
    public RequestChangeEmailAddressTokenRequestValidator()
    {
        RuleFor(x => x.NewEmailAddress).CustomEmailAddress();
    }
}