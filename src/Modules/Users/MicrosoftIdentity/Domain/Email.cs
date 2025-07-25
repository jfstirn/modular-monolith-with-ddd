using CompanyName.MyMeetings.BuildingBlocks.Domain;

namespace CompanyName.MyMeetings.Modules.UsersMI.Domain
{
    public class Email : ValueObject
    {
        public string Address { get; }

        private Email(string address)
        {
            Address = address;
        }

        public static bool IsValid(string emailAddress)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(emailAddress);
                return mailAddress.Address == emailAddress;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValid(string emailAddress, out Error? error)
        {
            error = null;
            var result = IsValid(emailAddress);
            if (!result)
            {
                error = Errors.General.ValueIsInvalid($"'{emailAddress}' is not a valid email address.");
            }

            return result;
        }

        public static Email Parse(string emailAddress)
        {
            if (!IsValid(emailAddress, out Error? error))
            {
                throw new InvalidCastException(error!.Message);
            }

            return new Email(emailAddress);
        }

        public static bool TryParse(string emailAddress, out Email? email)
        {
            email = null;
            if (!IsValid(emailAddress))
            {
                return false;
            }

            email = new Email(emailAddress);
            return true;
        }

        public static implicit operator string?(Email? email)
            => email?.Address;

        public static explicit operator Email?(string? emailAddress)
            => emailAddress is not null ? Parse(emailAddress) : null;
    }
}
