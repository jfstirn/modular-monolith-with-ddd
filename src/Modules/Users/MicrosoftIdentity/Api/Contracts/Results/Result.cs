namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.Results;

public class Result : Result<Result>
{
    public Result()
    {
    }

    public Result(Result value)
        : base(value)
    {
    }

    public Result(ResultStatus status)
        : base(status)
    {
    }

    public Result(IDictionary<string, IEnumerable<ErrorMessage>> errors)
        : base(errors)
    {
    }

    public Result(ResultStatus status, Result value)
        : base(status, value)
    {
    }

    public Result(ResultStatus status, IDictionary<string, IEnumerable<ErrorMessage>> errors)
        : base(status, errors)
    {
    }

    public static Result Conflict()
    {
        return new Result(ResultStatus.Conflict);
    }

    public static Result Conflict(IDictionary<string, IEnumerable<ErrorMessage>> errors)
    {
        return new Result(ResultStatus.Conflict, errors);
    }

    public static Result<T> Conflict<T>(IDictionary<string, IEnumerable<ErrorMessage>> errors)
    {
        return new Result<T>(ResultStatus.Conflict, errors);
    }

    public static Result Created()
    {
        return new Result(ResultStatus.Created);
    }

    public static Result Created(IDictionary<string, IEnumerable<ErrorMessage>> errors)
    {
        return new Result(ResultStatus.Created, errors);
    }

    public static Result<T> Created<T>(T value)
    {
        return new Result<T>(ResultStatus.Created, value);
    }

    public static Result Error()
    {
        return new Result(ResultStatus.Error);
    }

    public static Result<T> Error<T>()
    {
        return new Result<T>(ResultStatus.Error);
    }

    public static Result Error(IDictionary<string, IEnumerable<ErrorMessage>> errors)
    {
        return new Result(ResultStatus.Error, errors);
    }

    public static Result<T> Error<T>(IDictionary<string, IEnumerable<ErrorMessage>> errors)
    {
        return new Result<T>(ResultStatus.Error, errors);
    }

    public static Result Forbidden()
    {
        return new Result(ResultStatus.Forbidden);
    }

    public static Result Forbidden(IDictionary<string, IEnumerable<ErrorMessage>> errors)
    {
        return new Result(ResultStatus.Forbidden, errors);
    }

    public static Result<T> Forbidden<T>(IDictionary<string, IEnumerable<ErrorMessage>> errors)
    {
        return new Result<T>(ResultStatus.Forbidden, errors);
    }

    public static Result Invalid()
    {
        return new Result(ResultStatus.Invalid);
    }

    public static Result Invalid(IDictionary<string, IEnumerable<ErrorMessage>> errors)
    {
        return new Result(ResultStatus.Invalid, errors);
    }

    public static Result<T> Invalid<T>(IDictionary<string, IEnumerable<ErrorMessage>> errors)
    {
        return new Result<T>(ResultStatus.Invalid, errors);
    }

    public static Result NoContent()
    {
        return new Result(ResultStatus.NoContent);
    }

    public static Result NoContent(IDictionary<string, IEnumerable<ErrorMessage>> errors)
    {
        return new Result(ResultStatus.NoContent, errors);
    }

    public static Result<T> NoContent<T>(IDictionary<string, IEnumerable<ErrorMessage>> errors)
    {
        return new Result<T>(ResultStatus.NoContent, errors);
    }

    public static Result NotFound()
    {
        return new Result(ResultStatus.NotFound);
    }

    public static Result NotFound(IDictionary<string, IEnumerable<ErrorMessage>> errors)
    {
        return new Result(ResultStatus.NotFound, errors);
    }

    public static Result<T> NotFound<T>(IDictionary<string, IEnumerable<ErrorMessage>> errors)
    {
        return new Result<T>(ResultStatus.NotFound, errors);
    }

    public static Result Ok()
    {
        return new Result(ResultStatus.Ok);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(value);
    }

    public static Result Unauthorized()
    {
        return new Result(ResultStatus.Unauthorized);
    }

    public static Result Unauthorized(IDictionary<string, IEnumerable<ErrorMessage>> errors)
    {
        return new Result(ResultStatus.Unauthorized, errors);
    }

    public static Result<T> Unauthorized<T>(IDictionary<string, IEnumerable<ErrorMessage>> errors)
    {
        return new Result<T>(ResultStatus.Unauthorized, errors);
    }
}

public class Result<T> : IResult<T>
{
    public Result()
    {
    }

    public Result(ResultStatus status)
    {
        Status = status;
        TimeGeneratedUtc = DateTime.UtcNow;
    }

    public Result(T value)
        : this(ResultStatus.Ok)
    {
        Value = value;
    }

    public Result(ResultStatus status, T value)
    : this(status)
    {
        Value = value;
    }

    public Result(IDictionary<string, IEnumerable<ErrorMessage>> errors)
        : this(ResultStatus.Error)
    {
        Errors = errors;
    }

    public Result(ResultStatus status, IDictionary<string, IEnumerable<ErrorMessage>> errors)
        : this(status)
    {
        Errors = errors;
    }

    public T? Value { get; set; }

    public ResultStatus Status { get; set; }

    public DateTime TimeGeneratedUtc { get; set; }

    public IDictionary<string, IEnumerable<ErrorMessage>>? Errors { get; set; }

    public bool IsSuccess => Status is ResultStatus.Ok || Status is ResultStatus.NoContent || Status == ResultStatus.Created;
}