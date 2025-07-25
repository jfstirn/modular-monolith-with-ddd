namespace CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.GetUserAccounts;

public class UserAccountDto
{
    /// <summary>
    /// Gets or sets the primary key for this user.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    public string? FirstName { get; init; }

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    public string? LastName { get; init; }

    /// <summary>
    /// Gets or sets the date and time, in UTC, when any user lockout ends.
    /// A value in the past means the user is not locked out.
    /// </summary>
    public DateTimeOffset? LockoutEnd { get; init; }

    /// <summary>
    /// Gets or sets a flag indicating if two factor authentication is enabled for this user.
    /// True if 2fa is enabled, otherwise false.
    /// </summary>
    public bool TwoFactorEnabled { get; init; }

    /// <summary>
    /// Gets or sets a flag indicating if a user has confirmed their telephone address.
    /// True if the telephone number has been confirmed, otherwise false.
    /// </summary>
    public bool PhoneNumberConfirmed { get; init; }

    /// <summary>
    /// Gets or sets a telephone number for the user.
    /// </summary>
    public string? PhoneNumber { get; init; }

    /// <summary>
    /// Gets or sets a flag indicating if a user has confirmed their email address.
    /// True if the email address has been confirmed, otherwise false.
    /// </summary>
    public bool EmailConfirmed { get; init; }

    /// <summary>
    /// Gets or sets the normalized email address for this user.
    /// </summary>
    public string? NormalizedEmail { get; init; }

    /// <summary>
    /// Gets or sets the email address for this user.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Gets or sets the normalized user name for this user.
    /// </summary>
    public required string NormalizedUserName { get; init; }

    /// <summary>
    /// Gets or sets the login for this user.
    /// </summary>
    public required string UserName { get; init; }

    /// <summary>
    /// True if the user could be locked out, otherwise false.
    /// </summary>
    public bool LockoutEnabled { get; init; }

    /// <summary>
    /// Gets or sets the number of failed login attempts for the current user.
    /// </summary>
    public int AccessFailedCount { get; init; }
}