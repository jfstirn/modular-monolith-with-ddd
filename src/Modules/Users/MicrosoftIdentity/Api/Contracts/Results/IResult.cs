namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.Results;

public interface IResult<T>
{
    T? Value { get; }
}