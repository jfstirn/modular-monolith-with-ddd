using CompanyName.MyMeetings.BuildingBlocks.Application.Data;
using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.EventBus;
using CompanyName.MyMeetings.Modules.Registrations.Application.UserRegistrations.GetUserRegistration;
using CompanyName.MyMeetings.Modules.Registrations.IntegrationEvents;
using MediatR;

namespace CompanyName.MyMeetings.Modules.Registrations.Application.UserRegistrations.ConfirmUserRegistration
{
    internal class UserRegistrationConfirmedPublishEventHandler : INotificationHandler<UserRegistrationConfirmedNotification>
    {
        private readonly IEventsBus _eventsBus;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public UserRegistrationConfirmedPublishEventHandler(IEventsBus eventsBus, ISqlConnectionFactory sqlConnectionFactory)
        {
            _eventsBus = eventsBus;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task Handle(UserRegistrationConfirmedNotification notification, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            var registration = await UserRegistrationProvider.GetById(
                connection,
                notification.DomainEvent.UserRegistrationId.Value);

            await _eventsBus.Publish(new UserRegistrationConfirmedIntegrationEvent(
                notification.Id,
                notification.DomainEvent.OccurredOn,
                notification.DomainEvent.UserRegistrationId.Value,
                registration.Login,
                registration.Password,
                registration.Email,
                registration.FirstName,
                registration.LastName,
                registration.Name));
        }
    }
}