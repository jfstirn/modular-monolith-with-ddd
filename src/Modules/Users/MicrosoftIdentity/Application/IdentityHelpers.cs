using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application;

internal static class IdentityHelpers
{
    public static List<Error> Map(this IEnumerable<IdentityError> errors)
    {
        return errors.Select(x => new Error(x.Code, x.Description)).ToList();
    }
}