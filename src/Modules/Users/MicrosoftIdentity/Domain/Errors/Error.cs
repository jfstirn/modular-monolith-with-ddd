using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace CompanyName.MyMeetings.Modules.UsersMI.Domain
{
    /// <summary>
    /// Class that represents an error.
    ///
    /// Errors should not be instantiated arbitrarily, but should be taken from a predefined set of errors.
    /// Error messages should not be handled by the domain layer. This is an application concern as you might
    /// want to translate them based on user settings. The Error it self is a value object meaning that error will
    /// not have an identity and can be use interchangeably.
    /// Further more as the Error class is part of the domain layer this is technically a violation of domain model
    /// purity and as said it shouldn't deal with application concerns. But this is a minor concession.
    /// </summary>
    public class Error : ValueObject, ICombine
    {
        private readonly List<Error> _errors = new();

        public Error(string code, string? message)
            : this()
        {
            Code = code ?? "unknown";
            Message = message;
        }

        internal Error()
            : base()
        {
            Code = string.Empty;
        }

        internal Error(IEnumerable<Error> errors)
            : this()
        {
            ArgumentNullException.ThrowIfNull(errors);

            _errors.AddRange(errors);
        }

        /// <summary>
        /// Error code.
        /// The code is going to be part of the contract between the API and it's clients.
        /// Once published error codes should not be changed.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// The error message.
        /// Error messages are just for informational purposes. So we can specify some additional information for debugging.
        /// Ideally the client should not show that message to the end user and instead should map there own error messages
        /// onto the code.
        /// </summary>
        public string? Message { get; }

        [JsonIgnore]
        public IReadOnlyCollection<Error> Errors => _errors;

        public ICombine Combine(ICombine value)
        {
            if (value is not Error error)
            {
                throw new ArgumentException($"Value is not of type {nameof(Error)}");
            }

            var errorList = new List<Error>();
            if (!string.IsNullOrEmpty(Code))
            {
                errorList.Add(new Error(Code, Message));
            }

            if (_errors is not null)
            {
                errorList.AddRange(_errors);
            }

            if (!string.IsNullOrEmpty(error.Code))
            {
                errorList.Add(new Error(error.Code, error.Message));
            }

            if (error._errors.Count > 0)
            {
                errorList.AddRange(error._errors);
            }

            return new Error(errorList);
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Message))
            {
                return Code;
            }

            return $"{Code}: {Message}";
        }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            // Only the code field is required as the code will be part of the contract between the API and it's clients.
            yield return Code;
        }
    }
}