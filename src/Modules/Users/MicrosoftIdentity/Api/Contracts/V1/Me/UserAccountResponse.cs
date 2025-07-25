namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Me;

public class UserAccountResponse
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string? Name { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? EmailAddress { get; set; }
}