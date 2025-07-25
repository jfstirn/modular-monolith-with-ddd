namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Authentication;

public class TokenRequest
{
    public string AccessToken { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;
}