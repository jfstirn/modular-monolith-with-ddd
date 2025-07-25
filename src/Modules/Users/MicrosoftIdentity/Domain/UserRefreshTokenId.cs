using CompanyName.MyMeetings.BuildingBlocks.Domain;

namespace CompanyName.MyMeetings.Modules.UsersMI.Domain
{
    public class UserRefreshTokenId : TypedIdValueBase
    {
        public UserRefreshTokenId(Guid value)
            : base(value)
        {
        }
    }
}