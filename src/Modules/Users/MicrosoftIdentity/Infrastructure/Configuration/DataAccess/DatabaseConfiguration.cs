namespace CompanyName.MyMeetings.Modules.UsersMI.Infrastructure.Configuration.DataAccess
{
    public class DatabaseConfiguration : IDatabaseConfiguration
    {
        public DatabaseConfiguration(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }
    }
}