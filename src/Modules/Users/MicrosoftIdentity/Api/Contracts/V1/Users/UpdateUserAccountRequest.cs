namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Users;

public class UpdateUserAccountRequest
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    public string? FirstName { get; init; }

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    public string? LastName { get; init; }
}
