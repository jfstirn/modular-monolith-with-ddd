using CompanyName.MyMeetings.Modules.UsersMI.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Mvc;
using ApplicationResult = CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

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
    protected new Microsoft.AspNetCore.Http.IResult Ok()
    {
        return Result.Ok().ToApiResult();
    }

    protected new Microsoft.AspNetCore.Http.IResult Ok(object? value)
    {
        return Result.Ok(value).ToApiResult();
    }

    protected Microsoft.AspNetCore.Http.IResult Ok<T>(T value)
    {
        return Result.Ok(value).ToApiResult();
    }

    protected Microsoft.AspNetCore.Http.IResult NotFound(Error error)
    {
        return Result.NotFound(error.Translate()).ToApiResult();
    }

    protected Microsoft.AspNetCore.Http.IResult Error(Error error)
    {
        return Result.Error(error.Translate()).ToApiResult();
    }

    protected Microsoft.AspNetCore.Http.IResult ToApiResult(ApplicationResult.Result result)
    {
        return result.ToApiResult();
    }

    protected Microsoft.AspNetCore.Http.IResult ToApiResult<T>(ApplicationResult.Result<T> result)
    {
        return result.ToApiResult();
    }
}