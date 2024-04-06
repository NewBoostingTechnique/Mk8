using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace Mk8.Web.Authentication;

internal static class AuthenticationBuilder
{
    internal static void AddAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddOAuth<GoogleOptions, AuthenticationHandler>
        (
            GoogleDefaults.AuthenticationScheme,
            GoogleDefaults.DisplayName,
            options =>
            {
                string getRequiredConfig(string key)
                {
                    return builder.Configuration[key]
                        ?? throw new InvalidOperationException($"Google Authentication cannot be configured without '{key}'.");
                }

                options.ClientId = getRequiredConfig("Authentication:Google:ClientId");
                options.ClientSecret = getRequiredConfig("Authentication:Google:ClientSecret");
                options.Events = new OAuthEvents
                {
                    OnRedirectToAuthorizationEndpoint = async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(context.RedirectUri));
                    }
                };
                options.Scope.Add("email");
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }
        );
    }
}