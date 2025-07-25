using CompanyName.MyMeetings.BuildingBlocks.Application.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using CompanyName.MyMeetings.Modules.UsersMI.Domain.Repositories;
using Dapper;

namespace CompanyName.MyMeetings.Modules.UsersMI.Infrastructure.Domain.Repositories;

internal class PermissionRepository : IReadOnlyPermissionRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public PermissionRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Permission>> GetPermissionsAsync(GetPermissionsOptions options, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateNewConnection();

        var commandParameters = new DynamicParameters();
        List<string> whereClauses = new();

        if (options.PermissionCodes is not null)
        {
            whereClauses.Add("[P].[Code] IN @Codes");
            commandParameters.Add("Codes", options.PermissionCodes);
        }

        var whereClause = string.Empty;
        if (Enumerable.Any(whereClauses))
        {
            whereClause = "WHERE " + string.Join(" AND ", whereClauses);
        }

        string query = $"""
                        SELECT [P].[Code]           AS {nameof(Permission.Code)},
                               [P].[Name]           AS {nameof(Permission.Name)},
                               [P].[Description]    AS {nameof(Permission.Description)}
                          FROM [usersmi].[permissions] AS [P]
                         {whereClause}
                        """;

        var permissions = await connection.QueryAsync<Permission>(new CommandDefinition(query, commandParameters, cancellationToken: cancellationToken));
        return permissions;
    }
}