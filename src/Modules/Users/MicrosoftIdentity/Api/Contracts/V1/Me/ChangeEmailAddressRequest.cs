namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Me;

public class ChangeEmailAddressRequest
{
    public string Token { get; set; } = null!;

    public string NewEmailAddress { get; set; } = null!;
}