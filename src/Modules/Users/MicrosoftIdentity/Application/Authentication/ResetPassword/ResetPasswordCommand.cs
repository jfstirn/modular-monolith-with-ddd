using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Authentication.ResetPassword;

public class ResetPasswordCommand : CommandBase<Result>
{
    public ResetPasswordCommand(string token, string emailAddress, string password)
    {
        Token = token;
        EmailAddress = emailAddress;
        Password = password;
    }

    public string Token { get; }

    public string EmailAddress { get; }

    public string Password { get; }
}