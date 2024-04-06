using Microsoft.Playwright;
using Mk8.Web.Test.Mk8Instances;

namespace Mk8.Web.Test.Steps;

public class Given(Func<IPage> pageAccessor, IMk8Instance mk8Instance)
    : Step(pageAccessor, mk8Instance)
{
    internal Task ImAuthenticatedAsAnAuthorizedUserAsync()
    {
        return ImSignedInAsAsync(GoogleAccount.AuthorizedUser);
    }

    internal Task ImAuthenticatedAsAnUnauthorizedUserAsync()
    {
        return ImSignedInAsAsync(GoogleAccount.Unauthorized);
    }

    // TODO: Secrets.

    private async Task ImSignedInAsAsync(GoogleAccount googleAccount)
    {
        await GoToPathAsync("/authorization/");
        await SignIn(googleAccount);
    }

    internal async Task ImNotAuthenticatedAsync()
    {
        await Page.GotoAsync("https://accounts.google.com/SignOutOptions");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Sign out" }).ClickAsync();
    }

    internal async Task TheGoogleSignInPageIsShownAsync()
    {
        await ImNotAuthenticatedAsync();
        await GoToPathAsync("/authorization/");
    }
}