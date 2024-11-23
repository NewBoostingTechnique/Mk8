using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace Mk8.Web.Authentication;

public class AuthenticationHandler(
    IOptionsMonitor<GoogleOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder
) : GoogleHandler(options, logger, encoder)
{
    protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
    {
        properties.SetString(".redirect", "/authorization/");
        return base.BuildChallengeUrl(properties, redirectUri);
    }

    protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        return Task.CompletedTask;
    }
}
