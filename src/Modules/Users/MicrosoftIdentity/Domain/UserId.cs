using CompanyName.MyMeetings.BuildingBlocks.Domain;

namespace CompanyName.MyMeetings.Modules.UsersMI.Domain
{
    public class UserId : TypedIdValueBase
    {
        public UserId(Guid value)
            : base(value)
        {
        }
    }
}