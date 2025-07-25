namespace CompanyName.MyMeetings.Modules.UsersMI.Infrastructure.Configuration;

public class UserAccessConfiguration
{
    public Security? Security { get; set; }
}

public class Security
{
    public string? JwtSecretKey { get; set; }

    public string? JwtIssuer { get; set; }

    public string? JwtAudience { get; set; }

    public int JwtTokenLifetimeInMinutes { get; set; }
}