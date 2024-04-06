using Microsoft.AspNetCore.Authorization;
using Mk8.Core.Users;
using System.Net;

namespace Mk8.Web.Authorization;

internal class AuthorizationHandler(
    IUserService userService,
    IHttpContextAccessor httpContextAccessor
) : IAuthorizationHandler
{
    public async Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (!await userService.IsCurrentUserAuthorizedAsync(httpContextAccessor.HttpContext))
        {
            if (context.Resource is HttpContext httpContext)
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

            context.Fail();
        }
    }
}