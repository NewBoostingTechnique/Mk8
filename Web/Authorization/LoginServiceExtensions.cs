using System.Security.Claims;
using Mk8.Core.Logins;

namespace Mk8.Web.Authorization;

internal static class LoginServiceExtensions
{
    internal static async Task<bool> IsCurrentLoginAuthorizedAsync(this ILoginService loginService, HttpContext? httpContext)
    {
        string? email = httpContext?.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        return !string.IsNullOrWhiteSpace(email) && await loginService.ExistsAsync(email);
    }
}