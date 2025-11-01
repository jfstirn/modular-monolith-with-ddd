using CompanyName.MyMeetings.Modules.UsersMI.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi;

public static class ErrorExtensions
{
    public static IDictionary<string, IEnumerable<ErrorMessage>> Translate(this Error error)
    {
        var errors = new List<Error>
        {
            error
        };
        return errors.Translate();
    }

    public static IDictionary<string, IEnumerable<ErrorMessage>> Translate(this IEnumerable<Error> errors)
        => errors.ToErrorMessages().Wrap();

    public static IDictionary<string, IEnumerable<ErrorMessage>> ToErrorMessages(this IDictionary<string, IEnumerable<Error>> errors)
    {
        var errorMessages = new Dictionary<string, IEnumerable<ErrorMessage>>();
        foreach (var keyValue in errors)
        {
            errorMessages.Add(keyValue.Key, keyValue.Value.ToErrorMessages());
        }

        return errorMessages;
    }

    public static IEnumerable<ErrorMessage> ToErrorMessages(this IEnumerable<Error> errors)
    {
        var errorMessages = new List<ErrorMessage>();
        foreach (var error in errors)
        {
            errorMessages.AddRange(error.ToErrorMessages());
        }

        return errorMessages;
    }

    public static IEnumerable<ErrorMessage> ToErrorMessages(this Error error, List<ErrorMessage>? errorMessages = null)
    {
        if (error is null)
        {
            throw new ArgumentNullException(nameof(error));
        }

        if (errorMessages is null)
        {
            errorMessages = new List<ErrorMessage>();
        }

        if (!string.IsNullOrEmpty(error.Code + error.Message))
        {
            errorMessages.Add(error.ToErrorMessage());
        }

        if (error.Errors is not null)
        {
            foreach (var subError in error.Errors)
            {
                subError.ToErrorMessages(errorMessages);
            }
        }

        return errorMessages;
    }

    private static IDictionary<string, IEnumerable<ErrorMessage>> Wrap(this IEnumerable<ErrorMessage> errors)
    {
        if (errors.Count() <= 0)
        {
            return new Dictionary<string, IEnumerable<ErrorMessage>>();
        }

        return new Dictionary<string, IEnumerable<ErrorMessage>> { { string.Empty, errors } };
    }

    private static ErrorMessage ToErrorMessage(this Error error)
    {
        if (error is null)
        {
            throw new ArgumentNullException(nameof(error));
        }

        return new ErrorMessage(error.Code, error.Message);
    }
}