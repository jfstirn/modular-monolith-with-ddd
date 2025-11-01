using ApplicationResults = CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using ContractResults = CompanyName.MyMeetings.Modules.UsersMI.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi;

internal static class ApplicationResultsToContractResultExtensions
{
    public static ContractResults.Result ToContractResult(this ApplicationResults.IResult result)
    {
        return result.Status switch
        {
            ApplicationResults.ResultStatus.Ok => ContractResults.Result.Ok(),
            ApplicationResults.ResultStatus.NotFound => ContractResults.Result.NotFound(result.Errors.Translate()),
            ApplicationResults.ResultStatus.Unauthorized => ContractResults.Result.Unauthorized(result.Errors.Translate()),
            ApplicationResults.ResultStatus.Forbidden => ContractResults.Result.Forbidden(result.Errors.Translate()),
            ApplicationResults.ResultStatus.Invalid => ContractResults.Result.Invalid(result.Errors.Translate()),
            ApplicationResults.ResultStatus.Error => ContractResults.Result.Error(result.Errors.Translate()),
            ApplicationResults.ResultStatus.Conflict => ContractResults.Result.Conflict(result.Errors.Translate()),
            ApplicationResults.ResultStatus.Created => ContractResults.Result.Created(),
            ApplicationResults.ResultStatus.NoContent => ContractResults.Result.NoContent(result.Errors.Translate()),
            _ => throw new NotSupportedException($"Application Result {result.Status} conversion is not supported."),
        };
    }

    public static ContractResults.Result<T> ToContractResult<T>(this ApplicationResults.IResult result, T value)
    {
        return result.Status switch
        {
            ApplicationResults.ResultStatus.Ok => ContractResults.Result.Ok(value),
            ApplicationResults.ResultStatus.NotFound => ContractResults.Result.NotFound<T>(result.Errors.Translate()),
            ApplicationResults.ResultStatus.Unauthorized => ContractResults.Result.Unauthorized<T>(result.Errors.Translate()),
            ApplicationResults.ResultStatus.Forbidden => ContractResults.Result.Forbidden<T>(result.Errors.Translate()),
            ApplicationResults.ResultStatus.Invalid => ContractResults.Result.Invalid<T>(result.Errors.Translate()),
            ApplicationResults.ResultStatus.Error => ContractResults.Result.Error<T>(result.Errors.Translate()),
            ApplicationResults.ResultStatus.Conflict => ContractResults.Result.Conflict<T>(result.Errors.Translate()),
            ApplicationResults.ResultStatus.Created => ContractResults.Result.Created(value),
            ApplicationResults.ResultStatus.NoContent => ContractResults.Result.NoContent<T>(result.Errors.Translate()),
            _ => throw new NotSupportedException($"Application Result {result.Status} conversion is not supported."),
        };
    }

    public static ContractResults.Result ToContractResult<T>(this ApplicationResults.IResult<T> result)
    {
        return result.Status switch
        {
            ApplicationResults.ResultStatus.Ok => ContractResults.Result.Ok(),
            ApplicationResults.ResultStatus.NotFound => ContractResults.Result.NotFound(result.Errors.Translate()),
            ApplicationResults.ResultStatus.Unauthorized => ContractResults.Result.Unauthorized(result.Errors.Translate()),
            ApplicationResults.ResultStatus.Forbidden => ContractResults.Result.Forbidden(result.Errors.Translate()),
            ApplicationResults.ResultStatus.Invalid => ContractResults.Result.Invalid(result.Errors.Translate()),
            ApplicationResults.ResultStatus.Error => ContractResults.Result.Error(result.Errors.Translate()),
            ApplicationResults.ResultStatus.Conflict => ContractResults.Result.Conflict(result.Errors.Translate()),
            ApplicationResults.ResultStatus.Created => ContractResults.Result.Created(),
            ApplicationResults.ResultStatus.NoContent => ContractResults.Result.NoContent(result.Errors.Translate()),
            _ => throw new NotSupportedException($"Application Result {result.Status} conversion is not supported."),
        };
    }
}