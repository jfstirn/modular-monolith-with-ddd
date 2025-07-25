using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.EventBus;

namespace CompanyName.MyMeetings.Modules.Registrations.IntegrationEvents
{
    public class UserRegistrationConfirmedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; }

        public string Login { get; }

        public string Password { get; }

        public string Email { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string Name { get; }

        public UserRegistrationConfirmedIntegrationEvent(Guid id, DateTime occurredOn, Guid userId, string login, string password, string email, string firstName, string lastName, string name)
            : base(id, occurredOn)
        {
            UserId = userId;
            Login = login;
            Password = password;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Name = name;
        }
    }
}
