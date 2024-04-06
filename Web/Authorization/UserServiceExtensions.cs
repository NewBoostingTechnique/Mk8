using System.Security.Claims;
using Mk8.Core.Users;

namespace Mk8.Web.Authorization;

internal static class UserServiceExtensions
{
    internal static async Task<bool> IsCurrentUserAuthorizedAsync(this IUserService userService, HttpContext? httpContext)
    {
        string? email = httpContext?.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        return !string.IsNullOrWhiteSpace(email) && await userService.ExistsAsync(email);
    }
}