using Mk8.Web.Test.Steps;

namespace Mk8.Web.Test.Tests;

public class AccountsTests : Mk8PageTest
{
    [Test]
    public async Task GivenImAuthenticatedAsAnAuthorizedUser_WhenINavigateToAccountSignIn_ThenTheHomePageIsShown()
    {
        // Given I'm authenticated as an unauthorized user.
        await Given.ImAuthenticatedAsAnAuthorizedUserAsync();

        // When I navigate to '/authorization/'.
        await When.INavigateToAsync("/authorization/");

        // Then the 'Home' page is shown.
        await Then.TheHomePageIsShownAsync();
    }

    [Test]
    public async Task GivenImAuthenticatedAsAnUnAuthorizedUser_WhenINavigateToAccountSignIn_ThenTheHomePageIsShown()
    {
        // Given I'm authenticated as an unauthorized user.
        await Given.ImAuthenticatedAsAnUnauthorizedUserAsync();

        // When I navigate to '/authorization/'.
        await When.INavigateToAsync("/authorization/");

        // Then the 'Home' page is shown.
        await Then.TheHomePageIsShownAsync();
    }

    [Test]
    public async Task GivenImNotAuthenticated_WhenINavigateToAccountSignIn_ThenTheGoogleSignInPageIsShown()
    {
        // Given I'm not authenticated with Google.
        await Given.ImNotAuthenticatedAsync();

        // When I navigate to '/authorization'.
        await When.INavigateToAsync("/authorization/");

        // Then the 'Google Sign-in' page is shown.
        await Then.TheGoogleSignInPageIsShownAsync();
    }

    [Test]
    public async Task GivenTheGoogleSignInPageIsShown_WhenISignIn_ThenTheHomePageIsShown()
    {
        // Given the 'Google Sign-in' page is shown.
        await Given.TheGoogleSignInPageIsShownAsync();

        // When I sign in.
        await When.ISignInAsAnAuthorizedUser();

        // Then the 'Home' page is shown.
        await Then.TheHomePageIsShownAsync();
    }
}
