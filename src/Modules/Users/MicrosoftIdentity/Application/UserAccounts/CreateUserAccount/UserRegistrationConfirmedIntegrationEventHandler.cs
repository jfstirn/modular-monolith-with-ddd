using CompanyName.MyMeetings.Modules.Registrations.IntegrationEvents;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Commands;
using MediatR;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount
{
    internal class UserRegistrationConfirmedIntegrationEventHandler : INotificationHandler<UserRegistrationConfirmedIntegrationEvent>
    {
        private readonly ICommandsScheduler _commandsScheduler;

        internal UserRegistrationConfirmedIntegrationEventHandler(ICommandsScheduler commandsScheduler)
        {
            _commandsScheduler = commandsScheduler;
        }

        public Task Handle(UserRegistrationConfirmedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            return _commandsScheduler.EnqueueAsync(new
                CreateUserAccountCommand(
                    Guid.NewGuid(),
                    notification.UserId,
                    notification.Login,
                    notification.Password,
                    notification.Name,
                    notification.FirstName,
                    notification.LastName,
                    notification.Email));
        }
    }
}