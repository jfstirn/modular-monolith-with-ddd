using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Authentication.RequestResetPasswordToken;

public class RequestResetPasswordTokenCommand : CommandBase<Result>
{
    public RequestResetPasswordTokenCommand(string emailAddress)
    {
        EmailAddress = emailAddress;
    }

    public string EmailAddress { get; }
}