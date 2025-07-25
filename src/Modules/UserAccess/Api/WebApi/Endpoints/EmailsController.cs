using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Authorization;
using CompanyName.MyMeetings.Modules.UserAccess.Application.Contracts;
using CompanyName.MyMeetings.Modules.UserAccess.Application.Emails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.Modules.UserAccess.WebApi.Endpoints
{
    [Route("api/userAccess/emails")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly IUserAccessModule _userAccessModule;

        public EmailsController(IUserAccessModule userAccessModule)
        {
            _userAccessModule = userAccessModule;
        }

        [NoPermissionRequired]
        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetEmails()
        {
            var allEmails = await _userAccessModule.ExecuteQueryAsync(new GetAllEmailsQuery());

            return Ok(allEmails);
        }
    }
}
