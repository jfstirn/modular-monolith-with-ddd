using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using FluentValidation;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application;

public static class FluentValidationExtensions
{
    /// <summary>
    /// Adds a new validation failure for the specified message. The failure will be associated with the current property being validated.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="validationContext">The validation context.</param>
    /// <param name="error">The error.</param>
    public static void AddFailure<T>(this ValidationContext<T> validationContext, Error error)
    {
        validationContext.AddFailure(new FluentValidation.Results.ValidationFailure()
        {
            PropertyName = validationContext.PropertyPath,
            ErrorMessage = error.Message,
            ErrorCode = error.Code
        });
    }

    public static IRuleBuilderOptions<T, TProperty> WithMessage<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, Error error)
    {
        return rule.WithMessage(error.Message);
    }

    public static IRuleBuilderOptions<T, TProperty> WithCode<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, Error error)
    {
        return rule.WithErrorCode(error.Code);
    }

    public static IRuleBuilderOptions<T, TProperty> WithMessageAndCode<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, Error error)
    {
        return rule
            .WithErrorCode(error.Code)
            .WithMessage(error.Message);
    }
}