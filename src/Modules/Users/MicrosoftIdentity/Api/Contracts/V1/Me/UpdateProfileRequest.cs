namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Me;

public class UpdateProfileRequest
{
    public string Login { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}