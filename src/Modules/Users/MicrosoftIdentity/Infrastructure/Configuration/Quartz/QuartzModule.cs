using Autofac;
using Quartz;

namespace CompanyName.MyMeetings.Modules.UsersMI.Infrastructure.Configuration.Quartz;

public class QuartzModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(ThisAssembly)
            .Where(x => typeof(IJob).IsAssignableFrom(x)).InstancePerDependency();
    }
}