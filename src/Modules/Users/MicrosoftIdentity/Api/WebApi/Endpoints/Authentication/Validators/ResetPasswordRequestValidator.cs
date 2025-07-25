using CompanyName.MyMeetings.Modules.UsersMI.Application;
using CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Authentication;
using FluentValidation;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi.Endpoints.Authentication.Validators;

internal class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Token).CustomNotEmpty();
        RuleFor(x => x.EmailAddress).CustomEmailAddress();
        RuleFor(x => x.Password).CustomNotEmpty();
        RuleFor(x => x.ConfirmPassword).CustomNotEmpty();

        RuleFor(x => x.ConfirmPassword).CustomEqual(x => x.Password);
    }
}