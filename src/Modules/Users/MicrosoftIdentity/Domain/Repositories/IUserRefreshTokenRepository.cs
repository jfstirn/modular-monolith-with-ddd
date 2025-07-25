namespace CompanyName.MyMeetings.Modules.UsersMI.Domain.Repositories
{
    public interface IUserRefreshTokenRepository
    {
        Task<UserRefreshToken?> GetByJwtIdAsync(string id, CancellationToken cancellationToken);

        void Add(UserRefreshToken userRefreshToken);

        Task<int> DeleteAsync(UserRefreshToken userRefreshToken, CancellationToken cancellationToken);
    }
}