using System.Net;
using System.Net.Http.Json;
using Mk8.Core.Players;
using NSubstitute;

namespace Mk8.Web.Test.PlayerApiTest;

public class PlayerCreateTest : EndpointTest
{
    [Test]
    public async Task GivenImAuthorized_WhenIPostANewPlayer_ThenIReceive200Ok_AndThePlayerIsCreated()
    {
        // Arrange.
        GivenImAuthorized();

        // Act.
        WhenIPostANewPlayerToApiPlayerResult result = await WhenIPostANewPlayerAsync();

        // Assert.
        Assert.Multiple(async () =>
        {
            Assert.That(result.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            await PlayerStore.Received().CreateAsync
            (
                Arg.Is<Player>
                (
                    p => p.Id != Ulid.Empty
                        && p.Name == result.Player.Name
                        && p.CountryName == result.Player.CountryName
                )
            );
        });
    }

    [Test]
    public async Task GivenImNotAuthorized_WhenIPostANewPlayer_ThenIReceive403Forbidden()
    {
        // Arrange.
        GivenImNotAuthorized();

        // Act.
        WhenIPostANewPlayerToApiPlayerResult result = await WhenIPostANewPlayerAsync();

        // Assert.
        Assert.That(result.Response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task GivenImNotAuthenticated_WhenIPostANewPlayer_ThenIReceive401Unauthorized()
    {
        // Arrange.
        GivenImNotAuthenticated();

        // Act.
        WhenIPostANewPlayerToApiPlayerResult result = await WhenIPostANewPlayerAsync();

        // Then I receive a '401 Unauthorized' response.
        Assert.That(result.Response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    private async Task<WhenIPostANewPlayerToApiPlayerResult> WhenIPostANewPlayerAsync()
    {
        // Arrange.
        Player player = new()
        {
            Name = "John Doe",
            CountryName = "England"
        };
        HttpRequestMessage request = new(HttpMethod.Post, "/api/players/")
        {
            Content = JsonContent.Create(player)
        };

        return new WhenIPostANewPlayerToApiPlayerResult
        {
            Player = player,
            Response = await HttpClient.SendAsync(request)
        };
    }

    private sealed record WhenIPostANewPlayerToApiPlayerResult
    {
        internal required Player Player { get; init; }
        internal required HttpResponseMessage Response { get; init; }
    }
}
