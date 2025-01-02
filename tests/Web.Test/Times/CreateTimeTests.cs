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
        Entities entities = ArrangeHappyPath();

        // Act.
        CreateTimeRequest request = GetCreateTimeRequest(entities);
        HttpResponseMessage response = await PostAsync(request);

        // Assert.
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            TimeStore.Received(1).CreateAsync
            (
                Arg.Is<Time>(time =>
                    time.CourseId == entities.Course.Id &&
                    time.Date == request.Date &&
                    time.PlayerId == entities.Player.Id &&
                    time.Span == request.Span
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
        HttpResponseMessage response = await PostAsync(content);

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
        HttpResponseMessage response = await PostAsync(content);

        // Assert.
        AssertPreValidationBadRequestOutcome(response);
    }

    [Test]
    public async Task GivenHappyArrangement_WhenEmptyJsonPosted_ThenValidationBadRequestOutcome()
    {
        // Arrange.
        ArrangeHappyPath();

        // Act.
        HttpResponseMessage response = await PostAsync(JsonContent.Create(new { }));

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

    [Test]
    public async Task GivenPlayerDoesNotExist_WhenNewTimePosted_ThenNotFoundOutcome()
    {
        // Arrange.
        Entities entities = GetEntities();
        InsertCourse(entities.Course);

        // Act.
        HttpResponseMessage response = await PostAsync(entities);

        // Assert.
        AssertNotFoundOutcome(response);
    }

    [Test]
    public async Task GivenCourseDoesNotExist_WhenNewTimePosted_ThenNotFoundOutcome()
    {
        // Arrange.
        Entities entities = GetEntities();
        InsertPlayer(entities.Player);

        // Act.
        HttpResponseMessage response = await PostAsync(entities);

        // Assert.
        AssertNotFoundOutcome(response);
    }

    #region Arrange.

    private Entities ArrangeHappyPath()
    {
        Entities entities = GetEntities();
        InsertCourse(entities.Course);
        InsertPlayer(entities.Player);
        return entities;
    }

    private static Entities GetEntities()
    {
        return new Entities
        {
            Course = new() { Id = Ulid.NewUlid(), Name = "Mushroom Cup" },
            Player = new() { Id = Ulid.NewUlid(), Name = "John Doe" }
        };
    }

    private static CreateTimeRequest GetCreateTimeRequest(Entities entities)
    {
        return new CreateTimeRequest
        {
            Date = DateOnly.FromDayNumber(1),
            Span = TimeSpan.FromSeconds(1),
            CourseName = entities.Course.Name,
            PlayerName = entities.Player.Name
        };
    }

    private void InsertCourse(Course course)
    {
        CourseStore.IdentifyAsync(Arg.Is(course.Name), Arg.Any<CancellationToken>())
            .Returns(_ => Task.FromResult(course.Id));
    }

    private void InsertPlayer(Player player)
    {
        PlayerStore.IdentifyAsync(Arg.Is(player.Name), Arg.Any<CancellationToken>())
            .Returns(_ => Task.FromResult(player.Id));
    }

    private void InsertTime(Time time)
    {
        TimeStore.ExistsAsync(Arg.Is(time.CourseId), Arg.Is(time.PlayerId), Arg.Any<CancellationToken>())
            .Returns(true);
    }

    private sealed record Entities
    {
        internal required Course Course { get; init; }
        internal required Player Player { get; init; }
    }

    #endregion Arrange.

    #region Act.

    private Task<HttpResponseMessage> PostAsync(HttpContent? content)
    {
        HttpRequestMessage message = new(HttpMethod.Post, "/api/times/")
        {
            Content = content
        };
        return HttpClient.SendAsync(message);
    }

    private Task<HttpResponseMessage> PostAsync(Entities entities)
    {
        CreateTimeRequest request = GetCreateTimeRequest(entities);
        return PostAsync(request);
    }

    private Task<HttpResponseMessage> PostAsync(CreateTimeRequest request)
    {
        JsonContent content = JsonContent.Create(request);
        return PostAsync(content);
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

    private static void AssertNotFoundOutcome(HttpResponseMessage response)
    {
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    #endregion Assert.

}
