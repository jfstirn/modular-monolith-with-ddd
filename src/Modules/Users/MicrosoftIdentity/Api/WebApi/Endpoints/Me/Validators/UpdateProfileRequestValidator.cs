using CompanyName.MyMeetings.Modules.UsersMI.Application;
using CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Me;
using FluentValidation;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi.Endpoints.Me.Validators;

internal class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.Login).CustomNotEmpty();
        RuleFor(x => x.Name).CustomNotEmpty();
    }
}