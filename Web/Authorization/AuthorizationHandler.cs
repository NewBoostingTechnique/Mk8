using Microsoft.AspNetCore.Authorization;
using Mk8.Core.Logins;
using System.Net;

namespace Mk8.Web.Authorization;

internal class AuthorizationHandler(
    ILoginService loginService,
    IHttpContextAccessor httpContextAccessor
) : IAuthorizationHandler
{
    public async Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (!await loginService.IsCurrentLoginAuthorizedAsync(httpContextAccessor.HttpContext))
        {
            if (context.Resource is HttpContext httpContext)
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

            context.Fail();
        }
    }
}