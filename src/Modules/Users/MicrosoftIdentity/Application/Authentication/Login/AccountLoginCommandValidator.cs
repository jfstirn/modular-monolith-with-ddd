using FluentValidation;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Authentication.Login;

internal class AccountLoginCommandValidator : AbstractValidator<AccountLoginCommand>
{
    public AccountLoginCommandValidator()
    {
        RuleFor(x => x.Login).CustomNotEmpty();
        RuleFor(x => x.Password).CustomNotEmpty();
    }
}