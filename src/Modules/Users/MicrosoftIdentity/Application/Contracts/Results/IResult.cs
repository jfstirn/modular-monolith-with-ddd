using CompanyName.MyMeetings.Modules.UsersMI.Domain;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

public interface IResult
{
    ResultStatus Status { get; }

    IEnumerable<Error> Errors { get; }

    bool HasError => Errors.Any();

    object? GetValue();
}

public interface IResult<T> : IResult
{
    T? Value { get; }
}