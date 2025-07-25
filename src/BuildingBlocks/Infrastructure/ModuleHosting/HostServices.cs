using CompanyName.MyMeetings.BuildingBlocks.Application;
using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Emails;
using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.EventBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
#nullable enable

namespace CompanyName.MyMeetings.BuildingBlocks.Infrastructure.ModuleHosting;

public class HostServices
{
    public HostServices(ILogger logger, string connectionString, string textEncryptionKey, IApplicationBuilder applicationBuilder, IExecutionContextAccessor executionContextAccessor, EmailsConfiguration emailsConfiguration, IEventsBus? eventsBus)
    {
        Logger = logger;
        ConnectionString = connectionString;
        TextEncryptionKey = textEncryptionKey;
        ApplicationBuilder = applicationBuilder;
        ExecutionContextAccessor = executionContextAccessor;
        EventsBus = eventsBus;
        EmailsConfiguration = emailsConfiguration;
    }

    public ILogger Logger { get; }

    public string ConnectionString { get; }

    public string TextEncryptionKey { get; }

    public IApplicationBuilder ApplicationBuilder { get; }

    public IExecutionContextAccessor ExecutionContextAccessor { get; }

    public IEventsBus? EventsBus { get; }

    public EmailsConfiguration EmailsConfiguration { get; }
}