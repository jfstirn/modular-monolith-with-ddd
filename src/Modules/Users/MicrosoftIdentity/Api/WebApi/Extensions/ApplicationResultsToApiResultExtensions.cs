using Microsoft.AspNetCore.Http;
using ApplicationResults = CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi;

internal static class ApplicationResultsToApiResultExtensions
{
    public static IResult ToApiResult(this ApplicationResults.IResult result)
    {
        return result.ToContractResult().ToApiResult();
    }

    public static IResult ToApiResult<T>(this ApplicationResults.Result result, T value)
    {
        return result.ToContractResult(value).ToApiResult();
    }

    public static IResult ToApiResult<T>(this ApplicationResults.Result<T> result)
    {
        return result.ToContractResult().ToApiResult();
    }

    public static IResult ToApiResult<T, TValue>(this ApplicationResults.Result<T> result, TValue value)
    {
        return result.ToContractResult(value).ToApiResult();
    }
}
