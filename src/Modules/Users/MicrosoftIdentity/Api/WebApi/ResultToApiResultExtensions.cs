using System.Net;
using System.Text;
using CompanyName.MyMeetings.Modules.UsersMI.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi;

public static class ResultToApiResultExtensions
{
    public static IResult ToApiResult<T>(this Result<T> result)
    {
        return result.Status switch
        {
            ResultStatus.Ok => Results.Ok(result),
            ResultStatus.NotFound => Results.NotFound(result),
            ResultStatus.Unauthorized => Results.Unauthorized(),
            ResultStatus.Forbidden => Results.Forbid(),
            ResultStatus.Invalid => Results.BadRequest(result),
            ResultStatus.Error => Results.UnprocessableEntity(result),
            ResultStatus.Conflict => Results.Conflict(result),
            ResultStatus.Created => Results.Created(),
            ResultStatus.NoContent => Results.NoContent(),
            _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported."),
        };
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return InternalActionResult.FromResult(result);
    }
}

internal class InternalActionResult : IActionResult
{
    private readonly object? _result;
    private readonly HttpStatusCode _statusCode;

    private InternalActionResult(HttpStatusCode statusCode)
    {
        _statusCode = statusCode;
    }

    private InternalActionResult(object result, HttpStatusCode statusCode)
        : this(statusCode)
    {
        _result = result;
    }

    public Task ExecuteResultAsync(ActionContext context)
    {
        var result = new ObjectResult(_result)
        {
            StatusCode = (int)_statusCode
        };

        return result.ExecuteResultAsync(context);
    }

    public static IActionResult FromResult<T>(Result<T> result)
    {
        return result.Status switch
        {
            ResultStatus.Ok => new InternalActionResult(result, HttpStatusCode.OK),
            ResultStatus.NotFound => new InternalActionResult(result, HttpStatusCode.NotFound),
            ResultStatus.Unauthorized => new InternalActionResult(result, HttpStatusCode.Unauthorized),
            ResultStatus.Forbidden => new InternalActionResult(result, HttpStatusCode.Forbidden),
            ResultStatus.Error => new InternalActionResult(result, HttpStatusCode.BadRequest),
            ResultStatus.Conflict => new InternalActionResult(result, HttpStatusCode.Conflict),
            ResultStatus.Created => new InternalActionResult(HttpStatusCode.Created),
            ResultStatus.NoContent => new InternalActionResult(HttpStatusCode.NoContent),
            _ => throw new NotSupportedException($"Result {result.GetType()} conversion is not supported."),
        };
    }
}

internal static class ResponseToApiResultExtensions
{
    /// <summary>
    /// Convert an AppResults.Result to a Result.
    /// </summary>
    /// <param name="result">The AppResults.Result to convert.</param>
    /// <returns>The Result.</returns>
    public static IResult ToApiResult(this Application.Contracts.Results.Result result)
    {
        return result.ToResult().ToApiResult();
    }

    /// <summary>
    /// Convert an AppResults.Result to a Result.
    /// </summary>
    /// <typeparam name="T">The value type being returned.</typeparam>
    /// <param name="result">The AppResults.Result to convert.</param>
    /// <param name="value">The value being returned.</param>
    /// <returns>The Result.</returns>
    public static IResult ToApiResult<T>(this Application.Contracts.Results.Result result, T value)
    {
        return result.ToResult(value).ToApiResult();
    }

    /// <summary>
    /// Convert an AppResults.Result to a Result.
    /// </summary>
    /// <typeparam name="T">The value being returned.</typeparam>
    /// <param name="result">The AppResults.Result to convert.</param>
    /// <returns>The Result.</returns>
    public static IResult ToApiResult<T>(this Application.Contracts.Results.Result<T> result)
    {
        return result.ToResult().ToApiResult();
    }
}

internal static class ResponseToResultExtensions
{
    /// <summary>
    /// Convert an AppResults.Result to a Result.
    /// </summary>
    /// <param name="result">The AppResults.Result to convert.</param>
    /// <returns>The Result.</returns>
    public static Result ToResult(this Application.Contracts.Results.IResult result)
    {
        return result.Status switch
        {
            Application.Contracts.Results.ResultStatus.Ok => Result.Ok(),
            Application.Contracts.Results.ResultStatus.NotFound => Result.NotFound(result.Errors.Translate()),
            Application.Contracts.Results.ResultStatus.Unauthorized => Result.Unauthorized(result.Errors.Translate()),
            Application.Contracts.Results.ResultStatus.Forbidden => Result.Forbidden(result.Errors.Translate()),
            Application.Contracts.Results.ResultStatus.Invalid => Result.Invalid(result.Errors.Translate()),
            Application.Contracts.Results.ResultStatus.Error => Result.Error(result.Errors.Translate()),
            Application.Contracts.Results.ResultStatus.Conflict => Result.Conflict(result.Errors.Translate()),
            Application.Contracts.Results.ResultStatus.Created => Result.Created(),
            Application.Contracts.Results.ResultStatus.NoContent => Result.NoContent(),
            _ => throw new NotSupportedException($"AppResults.Result {result.Status} conversion is not supported."),
        };
    }

    /// <summary>
    /// Convert an AppResults.Result to a Result.
    /// </summary>
    /// <typeparam name="T">The value type being returned.</typeparam>
    /// <param name="result">The AppResults.Result to convert.</param>
    /// <param name="value">The value being returned.</param>
    /// <returns>The Result.</returns>
    public static Result<T> ToResult<T>(this Application.Contracts.Results.IResult result, T value)
    {
        return result.Status switch
        {
            Application.Contracts.Results.ResultStatus.Ok => Result.Ok(value),
            Application.Contracts.Results.ResultStatus.NotFound => Result.NotFound<T>(result.Errors.Translate()),
            Application.Contracts.Results.ResultStatus.Unauthorized => Result.Unauthorized<T>(result.Errors.Translate()),
            Application.Contracts.Results.ResultStatus.Forbidden => Result.Forbidden<T>(result.Errors.Translate()),
            Application.Contracts.Results.ResultStatus.Invalid => Result.Invalid<T>(result.Errors.Translate()),
            Application.Contracts.Results.ResultStatus.Error => Result.Error<T>(result.Errors.Translate()),
            Application.Contracts.Results.ResultStatus.Conflict => Result.Conflict<T>(result.Errors.Translate()),
            Application.Contracts.Results.ResultStatus.Created => Result.Created(value),
            Application.Contracts.Results.ResultStatus.NoContent => Result.NoContent<T>(result.Errors.Translate()),
            _ => throw new NotSupportedException($"AppResults.Result {result.Status} conversion is not supported."),
        };
    }

    /// <summary>
    /// Convert an AppResults.Result to a Result.
    /// </summary>
    /// <typeparam name="T">The value being returned.</typeparam>
    /// <param name="result">The AppResults.Result to convert.</param>
    /// <returns>The Result.</returns>
    public static Result ToResult<T>(this Application.Contracts.Results.IResult<T> result)
    {
        return result.Status switch
        {
            Application.Contracts.Results.ResultStatus.Ok => Result.Ok(),
            Application.Contracts.Results.ResultStatus.NotFound => Result.NotFound(result.Errors.Translate()),
            Application.Contracts.Results.ResultStatus.Unauthorized => Result.Unauthorized(result.Errors.Translate()),
            Application.Contracts.Results.ResultStatus.Forbidden => Result.Forbidden(result.Errors.Translate()),
            Application.Contracts.Results.ResultStatus.Invalid => Result.Invalid(result.Errors.Translate()),
            Application.Contracts.Results.ResultStatus.Error => Result.Error(result.Errors.Translate()),
            Application.Contracts.Results.ResultStatus.Conflict => Result.Conflict(result.Errors.Translate()),
            Application.Contracts.Results.ResultStatus.Created => Result.Created(),
            Application.Contracts.Results.ResultStatus.NoContent => Result.NoContent(result.Errors.Translate()),
            _ => throw new NotSupportedException($"AppResults.Result {result.Status} conversion is not supported."),
        };
    }
}

internal static class ResponseExtensions
{
    public static IResult ToApiResult<T>(this Application.Contracts.Results.IResult<T> result) => result.ConvertToApiResult();

    public static IResult ToApiResult<T>(this Application.Contracts.Results.IResult<T> result, object value) => result.ConvertToApiResult(value);

    public static IResult ToApiResult(this Application.Contracts.Results.IResult result) => result.ConvertToApiResult();

    internal static IResult ConvertToApiResult(this Application.Contracts.Results.IResult result, object? value = null)
    {
        return result.Status switch
        {
            Application.Contracts.Results.ResultStatus.Ok => typeof(Application.Contracts.Results.Result).IsInstanceOfType(result)
                ? Microsoft.AspNetCore.Http.Results.Ok()
                : Microsoft.AspNetCore.Http.Results.Ok(value ?? result.GetValue()),
            Application.Contracts.Results.ResultStatus.NotFound => NotFoundEntity(result),
            Application.Contracts.Results.ResultStatus.Unauthorized => Microsoft.AspNetCore.Http.Results.Unauthorized(),
            Application.Contracts.Results.ResultStatus.Forbidden => Microsoft.AspNetCore.Http.Results.Forbid(),
            Application.Contracts.Results.ResultStatus.Invalid => Microsoft.AspNetCore.Http.Results.BadRequest(result.Errors),
            Application.Contracts.Results.ResultStatus.Error => UnprocessableEntity(result),
            Application.Contracts.Results.ResultStatus.Conflict => ConflictEntity(result),
            _ => throw new NotSupportedException($"AppResults.Result {result.Status} conversion is not supported."),
        };
    }

    private static IResult UnprocessableEntity(Application.Contracts.Results.IResult result)
    {
        StringBuilder stringBuilder = new("Next error(s) occurred:");
        foreach (Error error in result.Errors)
        {
            stringBuilder.Append("* ").Append(error).AppendLine();
        }

        return Microsoft.AspNetCore.Http.Results.UnprocessableEntity(
            new ProblemDetails { Title = "Something went wrong.", Detail = stringBuilder.ToString() });
    }

    private static IResult NotFoundEntity(Application.Contracts.Results.IResult result)
    {
        StringBuilder stringBuilder = new("Next error(s) occurred:");
        if (result.Errors.Any())
        {
            foreach (Error error in result.Errors)
            {
                stringBuilder.Append("* ").Append(error).AppendLine();
            }

            return Microsoft.AspNetCore.Http.Results.NotFound(
                new ProblemDetails { Title = "Resource not found.", Detail = stringBuilder.ToString() });
        }

        return Microsoft.AspNetCore.Http.Results.NotFound();
    }

    private static IResult ConflictEntity(Application.Contracts.Results.IResult result)
    {
        StringBuilder stringBuilder = new("Next error(s) occurred:");
        if (result.Errors.Any())
        {
            foreach (Error error in result.Errors)
            {
                stringBuilder.Append("* ").Append(error).AppendLine();
            }

            return Microsoft.AspNetCore.Http.Results.Conflict(
                new ProblemDetails { Title = "There was a conflict.", Detail = stringBuilder.ToString() });
        }

        return Microsoft.AspNetCore.Http.Results.Conflict();
    }

    private static IResult CriticalEntity(Application.Contracts.Results.IResult result)
    {
        StringBuilder stringBuilder = new("Next error(s) occurred:");
        if (result.Errors.Any())
        {
            foreach (Error error in result.Errors)
            {
                stringBuilder.Append("* ").Append(error).AppendLine();
            }

            return Microsoft.AspNetCore.Http.Results.Problem(
                new ProblemDetails { Title = "Something went wrong.", Detail = stringBuilder.ToString(), Status = 500 });
        }

        return Microsoft.AspNetCore.Http.Results.StatusCode(500);
    }

    private static IResult UnavailableEntity(Application.Contracts.Results.IResult result)
    {
        StringBuilder stringBuilder = new("Next error(s) occurred:");
        if (result.Errors.Any())
        {
            foreach (Error error in result.Errors)
            {
                stringBuilder.Append("* ").Append(error).AppendLine();
            }

            return Microsoft.AspNetCore.Http.Results.Problem(
                new ProblemDetails { Title = "Service unavailable.", Detail = stringBuilder.ToString(), Status = 503 });
        }

        return Microsoft.AspNetCore.Http.Results.StatusCode(503);
    }
}