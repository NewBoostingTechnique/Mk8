using System.Collections.Immutable;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Players;
using NSubstitute;

namespace Mk8.Web.Test.PlayerApiTest;

public class PlayerIndexTest : EndpointTest
{
    [Test]
    public async Task ShouldReturnPlayers_WhenPlayersExist()
    {
        // Arrange.
        ImmutableList<Player> expected =
        [
            new() { Name = "John Doe" }
        ];
        playerStore.IndexAsync().Returns(expected);
        HttpRequestMessage request = new(HttpMethod.Get, "/api/players/");

        // Act.
        HttpResponseMessage response = await HttpClient.SendAsync(request);

        // Assert.
        List<Player>? actual = await response.Content.ReadFromJsonAsync<List<Player>>();
        Assert.That(actual?.Count, Is.EqualTo(1));
    }

    private readonly IPlayerStore playerStore = Substitute.For<IPlayerStore>();

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        base.ConfigureTestServices(services);
        services.AddSingleton(playerStore);
    }
}

// TODO: Add PlayerCreateTest for this one.
//     [Test]
//     public async Task GivenImAuthenticatedAsAnAuthorizedUser_WhenIPostANewPlayerToApiPlayer_ThenIReceive200Ok_AndThePlayerIsCreated()
//     {
//         // Given I'm authenticated as an authorized user.
//         await Given.ImAuthenticatedAsAnAuthorizedUserAsync();

//         // When I POST to a new player to '/api/player/create/'.
//         PostPlayerResult result = await When.IPostANewPlayerToAsync("/api/player/");

//         // Then I receive a '200 Ok' response.
//         Then.TheResponseHasStatus(result, HttpStatusCode.OK);
//         // And the player is created.
//         await Then.ThePlayerIsCreatedAsync(result.Player);
//     }

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
