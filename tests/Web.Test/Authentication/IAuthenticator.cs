using Microsoft.AspNetCore.Authentication;

namespace Mk8.Web.Test.Authentication;

public interface IAuthenticator
{
    Task<AuthenticateResult> AuthenticateAsync();
}
