using System.Text.Json.Serialization;

namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.Results;

/// <summary>
/// Represents the status of a result in the application.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ResultStatus>))]
public enum ResultStatus
{
    /// <summary>
    /// The operation completed successfully.
    /// </summary>
    Ok,

    /// <summary>
    /// The operation encountered an error.
    /// </summary>
    Error,

    /// <summary>
    /// The resource was successfully created.
    /// </summary>
    Created,

    /// <summary>
    /// The requested resource was not found.
    /// </summary>
    NotFound,

    /// <summary>
    /// Access to the resource is forbidden.
    /// </summary>
    Forbidden,

    /// <summary>
    /// The user is not authorized to perform the operation.
    /// </summary>
    Unauthorized,

    /// <summary>
    /// The request is invalid.
    /// </summary>
    Invalid,

    /// <summary>
    /// The operation completed successfully but there is no content to return.
    /// </summary>
    NoContent,

    /// <summary>
    /// There is a conflict with the current state of the resource.
    /// </summary>
    Conflict
}