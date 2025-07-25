using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Commands;

public interface ICommandsScheduler
{
    Task EnqueueAsync(ICommand command);

    Task EnqueueAsync<T>(ICommand<T> command);
}