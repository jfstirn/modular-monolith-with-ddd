using CompanyName.MyMeetings.BuildingBlocks.Application.Security;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Infrastructure.Configuration.Identity;

public class CustomPasswordHasher : IPasswordHasher<ApplicationUser>
{
    /// <summary>
    /// As the passwords are already hashed by the PasswordManager upon user registration, we do not need to hash them again here.
    /// </summary>
    /// <param name="user">The application user.</param>
    /// <param name="password">The already hashed password.</param>
    /// <returns>Hashed password.</returns>
    public string HashPassword(ApplicationUser user, string password) => password;

    public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string stored, string provided)
        => PasswordManager.VerifyHashedPassword(stored, provided)
            ? PasswordVerificationResult.Success
            : PasswordVerificationResult.Failed;
}