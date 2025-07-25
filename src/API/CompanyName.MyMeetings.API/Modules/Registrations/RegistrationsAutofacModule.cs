using Autofac;
using CompanyName.MyMeetings.Modules.Registrations.Application.Contracts;
using CompanyName.MyMeetings.Modules.Registrations.Infrastructure;

namespace CompanyName.MyMeetings.API.Modules.Registrations;

internal class RegistrationsAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<RegistrationsModule>()
            .As<IRegistrationsModule>()
            .InstancePerLifetimeScope();
    }
}