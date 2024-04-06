using System.Net;
using Microsoft.Playwright;
using Mk8.Core.Players;
using Mk8.Web.Test.Extensions;
using Mk8.Web.Test.Mk8Instances;

namespace Mk8.Web.Test.Steps;

public class Then(Func<IPage> pageAccessor, IMk8Instance softwareUnderTest)
    : Step(pageAccessor, softwareUnderTest)
{
    internal Task TheGoogleSignInPageIsShownAsync()
    {
        return Assertions.Expect(Page)
            .ToHaveTitleAsync("Sign in - Google Accounts");
    }

    internal async Task TheHomePageIsShownAsync()
    {
        await Assertions.Expect(Page).ToHaveTitleAsync("Mario Kart 8 Players' Page");
        await Assertions.Expect(Page).ToHaveURLAsync(await Mk8Instance.GetAbsoluteUrlAsync("/"));
    }

    # region The 'Create' Button.

    internal Task TheCreateButtonIsShownAsync()
    {
        return ExpectTheCreateButton.ToBeVisibleAsync();
    }

    internal Task TheCreateButtonIsNotShownAsync()
    {
        return ExpectTheCreateButton.Not.ToBeVisibleAsync();
    }

    private ILocatorAssertions ExpectTheCreateButton
    {
        get
        {
            return Assertions.Expect
            (
                Page.GetByRole
                (
                    AriaRole.Button,
                    new PageGetByRoleOptions { Name = "Create" }
                )
            );
        }
    }

    #endregion The 'Create' Button.

    internal Task ThePlayerListPageIsShownAsync()
    {
        return Assertions.Expect
        (
            Page.GetByRole
            (
                AriaRole.Heading,
                new PageGetByRoleOptions { Name = "Players" }
            )
        )
        .ToBeVisibleAsync();
    }

    internal async Task ThePlayerIsCreatedAsync(Player player)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(player.Name);

        await GoToPathAsync("/player/");

        await Assertions.Expect
        (
            Page.GetByRole
            (
                AriaRole.Cell,
                new PageGetByRoleOptions { Name = player.Name }
            )
        )
        .ToBeVisibleAsync();
    }

    internal static void TheResponseHasStatus(PostPlayerResult result, HttpStatusCode statusCode)
    {
        Assert.That(result.HttpStatusCode, Is.EqualTo((int)statusCode));
    }
}