using System.Linq.Expressions;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using CSharpFunctionalExtensions;
using FluentValidation;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, TProperty> CustomNotNull<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return DefaultValidatorExtensions.NotNull(ruleBuilder)
                .WithMessageAndCode(Errors.General.ValueIsRequired());
        }

        public static IRuleBuilderOptions<T, TProperty> CustomNotEmpty<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return DefaultValidatorExtensions.NotEmpty(ruleBuilder)
                .WithMessageAndCode(Errors.General.ValueIsRequired());
        }

        public static IRuleBuilderOptions<T, string> CustomLength<T>(this IRuleBuilder<T, string> ruleBuilder, int min, int max)
        {
            return DefaultValidatorExtensions.Length(ruleBuilder, min, max)
                .WithMessageAndCode(Errors.General.InvalidLength());
        }

        public static IRuleBuilderOptions<T, string> CustomMaximumLength<T>(this IRuleBuilder<T, string> ruleBuilder, int maximumLength)
        {
            return DefaultValidatorExtensions.MaximumLength(ruleBuilder, maximumLength)
                .WithMessageAndCode(Errors.General.InvalidLength(maxLength: maximumLength));
        }

        public static IRuleBuilderOptions<T, TProperty> CustomGreaterThanOrEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, TProperty valueToCompare)
            where TProperty : IComparable<TProperty>, IComparable
        {
            return DefaultValidatorExtensions.GreaterThanOrEqualTo(ruleBuilder, valueToCompare)
                .WithMessageAndCode(Errors.General.ValueIsInvalid());
        }

        public static IRuleBuilderOptions<T, int?> CustomGreaterThanOrEqualTo<T>(this IRuleBuilder<T, int?> ruleBuilder, int valueToCompare)
        {
            return DefaultValidatorExtensions.GreaterThanOrEqualTo(ruleBuilder, valueToCompare)
                .WithMessageAndCode(Errors.General.ValueIsInvalid());
        }

        public static IRuleBuilderOptions<T, TProperty> CustomGreaterThan<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, TProperty valueToCompare)
            where TProperty : IComparable<TProperty>, IComparable
        {
            return DefaultValidatorExtensions.GreaterThan(ruleBuilder, valueToCompare)
                .WithMessageAndCode(Errors.General.ValueIsInvalid());
        }

        public static IRuleBuilderOptions<T, string> CustomEmailAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return DefaultValidatorExtensions.EmailAddress(ruleBuilder)
                .WithMessageAndCode(Errors.General.ValueIsInvalid());
        }

        public static IRuleBuilderOptions<T, string> CustomEqual<T>(this IRuleBuilder<T, string> ruleBuilder, Expression<Func<T, string>> expression, IEqualityComparer<string>? comparer = null)
        {
            return DefaultValidatorExtensions.Equal(ruleBuilder, expression, comparer)
                .WithMessage(Errors.General.ValueIsInvalid());
        }

        public static IRuleBuilderOptions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
            this IRuleBuilder<T, TElement> ruleBuilder,
            Func<TElement, Result<TValueObject, Error>> factoryMethod)
            where TValueObject : ValueObject
        {
            return (IRuleBuilderOptions<T, TElement>)ruleBuilder.Custom((value, context) =>
            {
                Result<TValueObject, Error> result = factoryMethod(value);

                if (result.IsFailure)
                {
                    if (result.Error.Errors != null)
                    {
                        foreach (var error in result.Error.Errors)
                        {
                            context.AddFailure(error);
                        }
                    }

                    if (result.Error.Code != null)
                    {
                        context.AddFailure(result.Error);
                    }
                }
            });
        }

        public static IRuleBuilderOptionsConditions<T, IList<TElement>> ListMustContainNumberOfItems<T, TElement>(
            this IRuleBuilder<T, IList<TElement>> ruleBuilder, int? min = null, int? max = null)
        {
            return ruleBuilder.Custom((list, context) =>
            {
                if (min.HasValue && list.Count < min.Value)
                {
                    context.AddFailure(Errors.General.CollectionIsTooSmall(min.Value, list.Count));
                }

                if (max.HasValue && list.Count > max.Value)
                {
                    context.AddFailure(Errors.General.CollectionIsTooLarge(max.Value, list.Count));
                }
            });
        }
    }
}