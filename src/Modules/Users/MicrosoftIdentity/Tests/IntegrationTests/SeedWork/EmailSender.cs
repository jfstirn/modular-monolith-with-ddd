using CompanyName.MyMeetings.BuildingBlocks.Application.Emails;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;

public class EmailSender : IEmailSender
{
    /// <summary>
    /// Last sent email message.
    /// </summary>
    public EmailMessage? EmailMessage { get; private set; }

    public Task SendEmail(EmailMessage message)
    {
        EmailMessage = message;
        return Task.CompletedTask;
    }
}