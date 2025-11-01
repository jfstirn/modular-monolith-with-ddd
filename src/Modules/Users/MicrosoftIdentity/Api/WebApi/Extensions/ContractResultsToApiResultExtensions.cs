using Microsoft.AspNetCore.Http;
using ContractResults = CompanyName.MyMeetings.Modules.UsersMI.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi;

internal static class ContractResultsToApiResultExtensions
{
    public static IResult ToApiResult<T>(this ContractResults.Result<T> result)
    {
        return result.Status switch
        {
            ContractResults.ResultStatus.Ok => Results.Ok(result),
            ContractResults.ResultStatus.NotFound => Results.NotFound(result),
            ContractResults.ResultStatus.Unauthorized => Results.Unauthorized(),
            ContractResults.ResultStatus.Forbidden => Results.Forbid(),
            ContractResults.ResultStatus.Invalid => Results.BadRequest(result),
            ContractResults.ResultStatus.Error => Results.UnprocessableEntity(result),
            ContractResults.ResultStatus.Conflict => Results.Conflict(result),
            ContractResults.ResultStatus.Created => Results.Created((string?)null, result),
            ContractResults.ResultStatus.NoContent => Results.NoContent(),
            _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported."),
        };
    }
}