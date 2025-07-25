using Microsoft.AspNetCore.Authorization;

namespace CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public const string HasPermissionPolicyName = "HasPermission";

        public HasPermissionAttribute(string name)
            : base(HasPermissionPolicyName)
        {
            Name = name;
        }

        public string Name { get; }
    }
}