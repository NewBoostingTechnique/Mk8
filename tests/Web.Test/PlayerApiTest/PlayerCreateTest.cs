using System.Net;
using System.Net.Http.Json;
using Mk8.Core.Players;
using NSubstitute;

namespace Mk8.Web.Test.PlayerApiTest;

public class PlayerCreateTest : PlayerApiEndpointTest
{
    [Test]
    public async Task GivenImAuthorized_WhenIPostANewPlayerToApiPlayer_ThenIReceive200Ok_AndThePlayerIsCreated()
    {
        // Arrange.
        WhenAuthenticated();
        Player player = new()
        {
            Name = "John Doe",
            CountryName = "England"
        };
        HttpRequestMessage request = new(HttpMethod.Post, "/api/players/")
        {
            Content = JsonContent.Create(player)
        };

        // Act.
        HttpResponseMessage response = await HttpClient.SendAsync(request);

        // Assert.
        Assert.Multiple(async () =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            await PlayerStore.Received().CreateAsync
            (
                Arg.Is<Player>
                (
                    p => p.Id != Ulid.Empty
                        && p.Name == player.Name
                        && p.CountryName == player.CountryName
                )
            );
        });
    }
}

//     [Test]
//     public async Task GivenImAuthenticatedAsAnUnauthorizedUser_WhenIPostToApiPlayer_ThenIReceive403Forbidden()
//     {
//         // Given I'm authenticated as an unauthorized user.
//         await Given.ImAuthenticatedAsAnUnauthorizedUserAsync();

//         // When I POST to '/api/player/'.
//         PostPlayerResult result = await When.IPostANewPlayerToAsync("/api/player/");

//         // Then I receive a '403 Forbidden' response.
//         Then.TheResponseHasStatus(result, HttpStatusCode.Forbidden);
//     }

//     [Test]
//     public async Task GivenImAuthenticatedAsAnUnauthorizedUser_WhenINavigateToPlayer_ThenThePlayerListPageIsShown_AndTheCreateButtonIsNotShown()
//     {
//         // Given I'm authenticated as an unauthorized user.
//         await Given.ImAuthenticatedAsAnUnauthorizedUserAsync();

//         // When I navigate to '/player/'.
//         await When.INavigateToAsync("/player/");

//         // Then the 'Player List' page is shown.
//         await Then.ThePlayerListPageIsShownAsync();
//         // And the 'Create' button is NOT shown.
//         await Then.TheCreateButtonIsNotShownAsync();
//     }

//     [Test]
//     public async Task GivenImNotAuthenticated_WhenINavigateToPlayer_ThenThePlayerListPageIsShown_AndTheCreateButtonIsNotShown()
//     {
//         // Given I'm not authenticated.
//         await Given.ImNotAuthenticatedAsync();

//         // When I navigate To '/player/'.
//         await When.INavigateToAsync("/player/");

//         // Then the 'Player List' page is shown.
//         await Then.ThePlayerListPageIsShownAsync();
//         // And the 'Create' button is NOT shown.
//         await Then.TheCreateButtonIsNotShownAsync();
//     }

//     [Test]
//     public async Task GivenImNotAuthenticated_WhenIPostToApiPlayerCreate_ThenIReceive401Unauthorized()
//     {
//         // Given I'm NOT authenticated.
//         await Given.ImNotAuthenticatedAsync();

//         // When I POST to '/api/player/'.
//         PostPlayerResult result = await When.IPostANewPlayerToAsync("/api/player/");

//         // Then I receive a '401 Unauthorized' response.
//         Then.TheResponseHasStatus(result, HttpStatusCode.Unauthorized);
//     }
