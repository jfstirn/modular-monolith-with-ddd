namespace CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Authorization
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class NoPermissionRequiredAttribute : Attribute
    {
    }
}