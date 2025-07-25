using Quartz;

namespace CompanyName.MyMeetings.Modules.UsersMI.Infrastructure.Configuration.Processing.Inbox;

[DisallowConcurrentExecution]
public class ProcessInboxJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandsExecutor.Execute(new ProcessInboxCommand());
    }
}