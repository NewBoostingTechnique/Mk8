using System.Net;
using System.Net.Http.Json;
using Mk8.Core.Players;
using NSubstitute;

namespace Mk8.Web.Test.PlayerApiTest;

public class PlayerCreateTest : EndpointTest
{
    [Test]
    public async Task GivenHappyArrangement_WhenNewPlayerPosted_ThenHappyOutcome()
    {
        // Arrange.
        ArrangeAuthorization();

        // Act.
        PostNewPlayerResult result = await PostNewPlayerAsync();

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
    public async Task GivenNoAuthorization_WhenNewPlayerPosted_ThenForbiddenOutcome()
    {
        // Arrange.
        ArrangeUnauthorized();

        // Act.
        PostNewPlayerResult result = await PostNewPlayerAsync();

        // Assert.
        Assert.That(result.Response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task GivenImNotAuthentication_WhenNewPlayerPosted_ThenUnauthorizedOutcome()
    {
        // Arrange.
        ArrangeUnauthenticated();

        // Act.
        PostNewPlayerResult result = await PostNewPlayerAsync();

        // Assert.
        Assert.That(result.Response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    #region Act.

    private async Task<PostNewPlayerResult> PostNewPlayerAsync()
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

        return new PostNewPlayerResult
        {
            Player = player,
            Response = await HttpClient.SendAsync(request)
        };
    }

    private sealed record PostNewPlayerResult
    {
        internal required Player Player { get; init; }
        internal required HttpResponseMessage Response { get; init; }
    }

    #endregion Act.

}
