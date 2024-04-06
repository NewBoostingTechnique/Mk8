using Microsoft.AspNetCore.Authorization;

namespace Mk8.Web.Authorization;

internal static class AuthorizationBuilder
{
    internal static void AddAuthorization(this WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Services.AddScoped<IAuthorizationHandler, AuthorizationHandler>();
    }
}