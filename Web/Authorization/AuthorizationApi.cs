using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mk8.Core.Users;
using Mk8.Web.Authorization;

namespace Mk8.Web.Access;

[Route("/api/authorization")]
public class AuthorizationApi(
    IHttpContextAccessor httpContextAccessor,
    IUserService userService
) : ControllerBase
{
    [HttpPost("")]
    public IActionResult Create()
    {
        return Ok();
    }

    [AllowAnonymous, HttpGet("")]
    public async Task<IActionResult> GetAsync()
    {
        return Ok
        (
            await userService.IsCurrentUserAuthorizedAsync(httpContextAccessor.HttpContext)
        );
    }
}