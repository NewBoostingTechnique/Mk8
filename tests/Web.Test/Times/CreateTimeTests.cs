using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Mk8.Core.Courses;
using Mk8.Core.Players;
using Mk8.Core.Times;
using Mk8.Web.Times.Create;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace Mk8.Web.Test.Times;

public class CreateTimeTests : EndpointTest
{
    [Test]
    public async Task GivenHappyArrangement_WhenNewTimePosted_ThenHappyOutcome()
    {
        // Arrange.
        HappyArrangement arrangement = ArrangeHappyPath();

        // Act.
        CreateTimeRequest requestContent = new()
        {
            CourseName = arrangement.Course.Name,
            Date = DateOnly.FromDayNumber(1),
            PlayerName = arrangement.Player.Name,
            Span = TimeSpan.FromMinutes(1)
        };
        HttpResponseMessage response = await PostContent(JsonContent.Create(requestContent));

        // Assert.
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            TimeStore.Received(1).CreateAsync
            (
                Arg.Is<Time>(time =>
                    time.CourseId == arrangement.Course.Id &&
                    time.Date == requestContent.Date &&
                    time.PlayerId == arrangement.Player.Id &&
                    time.Span == requestContent.Span
                ),
                Arg.Any<CancellationToken>()
            );
        });
    }

    [Test]
    public async Task GivenHappyArrangement_WhenNoContentPosted_ThenPreValidationBadRequestOutcome()
    {
        // Arrange.
        ArrangeHappyPath();

        // Act.
        HttpContent? content = null;
        HttpResponseMessage response = await PostContent(content);

        // Assert.
        AssertPreValidationBadRequestOutcome(response);
    }

    [Test]
    public async Task GivenHappyArrangement_WhenMalformedJsonPosted_ThenPreValidationBadRequestOutcome()
    {
        // Arrange.
        ArrangeHappyPath();

        // Act.
        HttpContent content = new StringContent("Not json");
        content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);
        HttpResponseMessage response = await PostContent(content);

        // Assert.
        AssertPreValidationBadRequestOutcome(response);
    }

    [Test]
    public async Task GivenHappyArrangement_WhenEmptyJsonPosted_ThenValidationBadRequestOutcome()
    {
        // Arrange.
        ArrangeHappyPath();

        // Act.
        HttpContent content = JsonContent.Create(new { });
        HttpResponseMessage response = await PostContent(content);

        // Assert.
        Assert.Multiple(async () =>
        {
            AssertBadRequestOutcome(response);

            ValidationProblemDetails? details = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            Assert.That(details, Is.Not.Null);
            if (details is not null)
            {
                Assert.That(details.Status, Is.EqualTo((int)HttpStatusCode.BadRequest));
                Assert.That(details.Errors.Count, Is.EqualTo(4));

                assertRequired(nameof(CreateTimeRequest.Date));
                assertRequired(nameof(CreateTimeRequest.Span));
                assertRequired(nameof(CreateTimeRequest.CourseName));
                assertRequired(nameof(CreateTimeRequest.PlayerName));

                void assertRequired(string propertyName)
                {
                    CollectionAssert.Contains(details.Errors.Keys, propertyName);
                    if (details.Errors.TryGetValue(propertyName, out string[]? propertyErrors))
                        CollectionAssert.Contains(propertyErrors, $"{propertyName} is required.");
                }
            }
        });
    }

    #region Arrange.

    private HappyArrangement ArrangeHappyPath()
    {
        Course course = new() { Id = Ulid.NewUlid(), Name = "Mushroom Cup" };
        CourseStore.IdentifyAsync(Arg.Is(course.Name), Arg.Any<CancellationToken>())
            .Returns(_ => Task.FromResult(course.Id));

        Player player = new() { Id = Ulid.NewUlid(), Name = "John Doe" };
        PlayerStore.IdentifyAsync(Arg.Is(player.Name), Arg.Any<CancellationToken>())
            .Returns(_ => Task.FromResult(player.Id));

        TimeStore.ExistsAsync(Arg.Is(course.Id.Value), Arg.Is(player.Id.Value), Arg.Any<CancellationToken>())
            .Returns(false);

        return new HappyArrangement
        {
            Course = course,
            Player = player
        };
    }

    private sealed record HappyArrangement
    {
        internal required Course Course { get; init; }
        internal required Player Player { get; init; }
    }

    #endregion Arrange.

    #region Act.

    private Task<HttpResponseMessage> PostContent(HttpContent? content)
    {
        return HttpClient.SendAsync
        (
            new(HttpMethod.Post, "/api/times/")
            {
                Content = content
            }
        );
    }

    #endregion Act.

    #region Assert.

    private void AssertPreValidationBadRequestOutcome(HttpResponseMessage response)
    {
        Assert.Multiple(async () =>
        {
            AssertBadRequestOutcome(response);
            Assert.That(await response.Content.ReadAsStringAsync(), Is.Empty);
        });
    }

    void AssertBadRequestOutcome(HttpResponseMessage response)
    {
        Assert.Multiple(async () =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            await TimeStore.DidNotReceive().CreateAsync(Arg.Any<Time>(), Arg.Any<CancellationToken>());
        });
    }

    #endregion Assert.

}
