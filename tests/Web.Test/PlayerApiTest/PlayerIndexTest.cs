using System.Collections.Immutable;
using System.Net.Http.Json;
using Mk8.Core.Players;
using NSubstitute;

namespace Mk8.Web.Test.PlayerApiTest;

public class PlayerIndexTest : PlayerApiEndpointTest
{
    [Test]
    public async Task ShouldReturnPlayers_WhenPlayersExist([Values] bool authorized)
    {
        // Arrange.
        GivenImAuthorized(authorized);
        ImmutableList<Player> expected =
        [
            new() { Name = "John Doe" }
        ];
        PlayerStore.IndexAsync().Returns(expected);
        HttpRequestMessage request = new(HttpMethod.Get, "/api/players/");

        // Act.
        HttpResponseMessage response = await HttpClient.SendAsync(request);

        // Assert.
        List<Player>? actual = await response.Content.ReadFromJsonAsync<List<Player>>();
        Assert.That(actual?.Count, Is.EqualTo(1));
    }
}
