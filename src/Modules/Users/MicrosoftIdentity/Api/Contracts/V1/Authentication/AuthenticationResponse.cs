namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Authentication;

public class AuthenticationResponse
{
    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }

    public string? UserName { get; set; }

    public bool IsLockedOut { get; set; } = false;

    public bool IsNotAllowed { get; set; } = false;

    public bool RequiresTwoFactor { get; set; } = false;
}