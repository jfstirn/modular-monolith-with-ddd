using CompanyName.MyMeetings.BuildingBlocks.Application.Emails;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Commands;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;

internal class CreateUserAccountCommandHandler : ICommandHandler<CreateUserAccountCommand, Result<Guid>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;

    public CreateUserAccountCommandHandler(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
    {
        _emailSender = emailSender;
        _userManager = userManager;
    }

    public async Task<Result<Guid>> Handle(CreateUserAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Login);

        // If user wasn't found, go ahead and create the new user.
        // But if user exists don't tell anyone. -> Protection against account enumeration
        if (user is null)
        {
            Email? email = null;
            if (!string.IsNullOrEmpty(request.EmailAddress))
            {
                if (!Email.IsValid(request.EmailAddress, out Error? error))
                {
                    return error!;
                }

                email = Email.Parse(request.EmailAddress);
            }

            var userResult = ApplicationUser.CreateUser(new UserId(request.UserId), request.Login, email, request.FirstName, request.LastName, request.Name);
            if (userResult.IsFailure)
            {
                return userResult.Error;
            }

            user = userResult.Value;
            var identityResult = await _userManager.CreateAsync(user);
            if (!identityResult.Succeeded)
            {
                return identityResult.Errors.Map().Combine();
            }

            if (!string.IsNullOrEmpty(request.Password))
            {
                var passwordResult = await _userManager.AddPasswordAsync(user, request.Password);
                if (!passwordResult.Succeeded)
                {
                    return passwordResult.Errors.Map().Combine();
                }
            }

            return Result.Created(user.Id);
        }

        // Send email to user informing them that he already have an account.
        // This way they can pro actively go out and use the forgot password functionality and reclaim there account.
        if (!string.IsNullOrEmpty(user.Email))
        {
            var emailMessage = new EmailMessage(
                user.Email,
                "MyMeetings - Create user account",
                $"An account with the provided email address {user.Email} already exists. Please use the forgot password functionality to reclaim your account.");
            await _emailSender.SendEmail(emailMessage);
        }

        return Result.Ok(Guid.NewGuid());
    }
}