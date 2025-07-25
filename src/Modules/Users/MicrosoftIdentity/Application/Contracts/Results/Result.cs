using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using CSharpFunctionalExtensions;
using MediatR;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

public class Result : Result<Result>
{
    public Result()
        : base(ResultStatus.Ok)
    {
    }

    protected Result(ResultStatus status)
        : base(status)
    {
    }

    protected Result(Error error)
        : base(ResultStatus.Error)
    {
        Errors = [error];
    }

    public static implicit operator Result(Error error)
    {
        if (error.Code.Equals(Domain.Errors.General.NotFound().Code))
        {
            return NotFound(error);
        }

        return Error(error);
    }

    public static implicit operator Result(Result<Unit, Error> result) => result.IsSuccess
        ? Ok()
        : result.Error;

    public static implicit operator Result(UnitResult<Error> result) => result.IsSuccess
        ? Ok()
        : result.Error;

    public static Result Ok() => new Result(ResultStatus.Ok);

    public static Result<T> Ok<T>(T value) => new Result<T>(value);

    public static Result NoContent() => new Result(ResultStatus.NoContent);

    public static Result<T> Created<T>(T value) => new Result<T>(value, ResultStatus.Created);

    public static Result Forbidden() => Forbidden(string.Empty);

    public static Result Forbidden(string? message)
    {
        message = message?.Trim();
        var error = string.IsNullOrEmpty(message)
            ? Domain.Errors.Authorization.Forbidden()
            : Domain.Errors.Authorization.Forbidden(message);

        return Forbidden(error);
    }

    public static Result Forbidden(Error error) => new Result(ResultStatus.Forbidden)
    {
        Errors = [error]
    };

    public static Result<T> Forbidden<T>() => Forbidden<T>(string.Empty);

    public static Result<T> Forbidden<T>(string? message)
    {
        message = message?.Trim();
        var error = string.IsNullOrEmpty(message)
            ? Domain.Errors.Authorization.Forbidden()
            : Domain.Errors.Authorization.Forbidden(message);
        return Forbidden<T>(error);
    }

    public static Result<T> Forbidden<T>(Error error) => new Result<T>(ResultStatus.Forbidden)
    {
        Errors = [error]
    };

    public static Result NotFound(Error error) => new Result(ResultStatus.NotFound)
    {
        Errors = [error]
    };

    public static Result<T> NotFound<T>(Error error) => new Result<T>(ResultStatus.NotFound)
    {
        Errors = [error]
    };

    public static new Result Error(Error error) => new Result(ResultStatus.Error)
    {
        Errors = [error]
    };

    public static Result<T> Error<T>(Error error) => new Result<T>(ResultStatus.Error)
    {
        Errors = [error]
    };
}

public class Result<T> : IResult<T>
{
    public Result(ResultStatus status)
    {
        Status = status;
    }

    public Result(T value)
    {
        Value = value;
        Status = ResultStatus.Ok;
    }

    public Result(T value, ResultStatus status)
        : this(value)
    {
        Status = status;
    }

    public Result(Error error)
        : this(error, ResultStatus.Error)
    {
    }

    public Result(Error error, ResultStatus status)
        : this([error], status)
    {
    }

    public Result(IEnumerable<Error> errors, ResultStatus status)
    {
        Errors = errors;
        Status = status;
    }

    public T? Value { get; init; }

    public ResultStatus Status { get; protected set; }

    public bool IsSuccess => Status is ResultStatus.Ok or ResultStatus.NoContent or ResultStatus.Created;

    public IEnumerable<Error> Errors { get; internal set; } = Enumerable.Empty<Error>();

    public object? GetValue()
    {
        return Value;
    }

    public IResult AddError(Error error)
    {
        ArgumentNullException.ThrowIfNull(error, nameof(error));

        var errors = new List<Error>(Errors);

        if (error.Errors != null)
        {
            foreach (var errorObj in error.Errors)
            {
                errors.Add(errorObj);
            }
        }

        if (error.Code != null)
        {
            errors.Add(error);
        }

        Errors = errors;
        Status = ResultStatus.Error;

        return this;
    }

    public static implicit operator T(Result<T> response) => response.Value!;

    public static implicit operator Result<T>(T value) => Result<T>.Ok(value);

    public static implicit operator Result<T>(Error error)
    {
        if (error.Code.Equals(Domain.Errors.General.NotFound().Code))
        {
            return Result.NotFound<T>(error);
        }

        return Error(error);
    }

    public static implicit operator Result<T>(Result<T, Error> result) => result.IsSuccess
        ? Result.Ok(result.Value)
        : result.Error;

    public static Result<T> Ok(T value) => new Result<T>(value);

    public static Result<T> Error(Error error) => new Result<T>(error);
}