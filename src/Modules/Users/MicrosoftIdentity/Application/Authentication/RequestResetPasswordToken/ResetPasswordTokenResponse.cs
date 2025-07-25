using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Authentication.RequestResetPasswordToken;

public class ResetPasswordTokenResponse : Result
{
    public string? Token { get; set; }
}