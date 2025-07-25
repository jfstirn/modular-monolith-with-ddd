using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi.Endpoints;

/// <summary>
/// Custom base class for API Controllers
///
/// Decorating the controller class with the ApiController attribute unleashes more magic power the framework offers.
/// This automates the model state checks and the model state IsValid property doesn't need to be checked manually.
/// Further more the [FromBody] attribute isn't needed anymore since the controller now automatically infers the binding source for the incoming data.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ApplicationController : ControllerBase
{
    protected new Microsoft.AspNetCore.Http.IResult Ok(object? result = null)
    {
        return Result.Ok(result).ToApiResult();
    }

    /*
    protected IActionResult Ok(string successMessage, object result = null)
    {
        return ApiResult.Ok(result, successMessage);
    }
    */

    protected Microsoft.AspNetCore.Http.IResult NotFound(Error error, string? invalidField = null)
    {
        return Result.NotFound(error).ToApiResult();
    }

    protected Microsoft.AspNetCore.Http.IResult Error(Error error, string? invalidField = null)
    {
        return Result.Error(error).ToApiResult();
    }

    protected Microsoft.AspNetCore.Http.IResult Error(Error error)
    {
        return Result.Error(error).ToApiResult();
    }

    protected Microsoft.AspNetCore.Http.IResult FromResponse(Result response)
    {
        return response.ToApiResult();
    }

    protected Microsoft.AspNetCore.Http.IResult FromResponse<T>(Result<T> response)
    {
        return response.ToApiResult();
    }

    protected Microsoft.AspNetCore.Http.IResult FromResult<T>(CSharpFunctionalExtensions.Result<T, Error> result)
    {
        if (result.IsSuccess)
        {
            return Ok();
        }

        return Error(result.Error);
    }
}