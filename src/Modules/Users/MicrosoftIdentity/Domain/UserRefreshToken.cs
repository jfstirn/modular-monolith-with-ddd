using CompanyName.MyMeetings.BuildingBlocks.Domain;

namespace CompanyName.MyMeetings.Modules.UsersMI.Domain
{
    public class UserRefreshToken : Entity, IAggregateRoot
    {
        private UserRefreshToken()
            : base()
        {
            Id = new UserRefreshTokenId(Guid.NewGuid());
            IsRevoked = false;
            AddedDate = DateTime.UtcNow;
            ExpiryDate = DateTime.UtcNow.AddMonths(6);
        }

        private UserRefreshToken(ApplicationUser user, string jwtId, string token)
            : this()
        {
            User = user;
            JwtId = jwtId;
            Token = token;
        }

        public static UserRefreshToken Create(ApplicationUser user, string jwtId, string token)
        {
            return new UserRefreshToken(user, jwtId, token);
        }

        public UserRefreshTokenId Id { get; protected set; }

        public ApplicationUser User { get; protected set; } = null!;

        public string Token { get; protected set; } = null!;

        public string JwtId { get; protected set; } = null!;

        public bool IsRevoked { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}