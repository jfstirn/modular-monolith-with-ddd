using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Domain
{
    public class Role : IdentityRole<Guid>
    {
        public Role()
            : base()
        {
        }

        public Role(string roleName)
            : base(roleName)
        {
        }
    }
}