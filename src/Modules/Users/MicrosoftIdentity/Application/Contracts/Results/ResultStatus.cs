namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

public enum ResultStatus
{
    /// <summary>
    /// Successful result.
    /// </summary>
    Ok,

    /// <summary>
    /// General error result.
    /// </summary>
    Error,

    /// <summary>
    /// Successful created result.
    /// </summary>
    Created,

    /// <summary>
    /// Not found error result.
    /// </summary>
    NotFound,

    /// <summary>
    /// Forbidden error result.
    /// </summary>
    Forbidden,

    /// <summary>
    /// Unauthorized error result.
    /// </summary>
    Unauthorized,

    /// <summary>
    /// Invalid result, typically used for validation errors.
    /// </summary>
    Invalid,

    /// <summary>
    /// Successful result with no content.
    /// </summary>
    NoContent,

    /// <summary>
    /// Conflict error result.
    /// </summary>
    Conflict
}