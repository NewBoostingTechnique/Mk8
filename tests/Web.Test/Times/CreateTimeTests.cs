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
    public async Task GivenHappyArrangement_WhenNewPosted_ThenHappyOutcome()
    {
        // Arrange.
        Entities entities = ArrangeHappyPath();

        // Act.
        HttpResponseMessage response = await PostTimeAsync(entities);

        // Assert.
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            TimeStore.Received(1).CreateAsync
            (
                Arg.Is<Time>(time =>
                    time.CourseId == entities.Course.Id &&
                    time.Date == entities.Time.Date &&
                    time.PlayerId == entities.Player.Id &&
                    time.Span == entities.Time.Span
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
        HttpResponseMessage response = await PostContentAsync(content);

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
        HttpResponseMessage response = await PostContentAsync(content);

        // Assert.
        AssertPreValidationBadRequestOutcome(response);
    }

    [Test]
    public async Task GivenHappyArrangement_WhenEmptyJsonPosted_ThenValidationBadRequestOutcome()
    {
        // Arrange.
        ArrangeHappyPath();

        // Act.
        HttpResponseMessage response = await PostContentAsync(JsonContent.Create(new { }));

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
    public async Task GivenPlayerDoesNotExist_WhenTimePosted_ThenNotFoundOutcome()
    {
        // Arrange.
        Entities entities = GetEntities();
        InsertCourse(entities.Course);

        // Act.
        HttpResponseMessage response = await PostTimeAsync(entities);

        // Assert.
        AssertNotFoundOutcome(response);
    }

    [Test]
    public async Task GivenCourseDoesNotExist_WhenTimePosted_ThenNotFoundOutcome()
    {
        // Arrange.
        Entities entities = GetEntities();
        InsertPlayer(entities.Player);

        // Act.
        HttpResponseMessage response = await PostTimeAsync(entities);

        // Assert.
        AssertNotFoundOutcome(response);
    }

    [Test]
    public async Task GivenTimeAlreadyExists_WhenTimePosted_ThenConflictOutcome()
    {
        // Arrange.
        Entities entities = ArrangeHappyPath();
        InsertTime(entities.Time);

        // Act.
        HttpResponseMessage response = await PostTimeAsync(entities);

        // Assert.
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
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
        Course course = new()
        {
            Id = Ulid.NewUlid(),
            Name = "Mushroom Cup"
        };

        Player player = new()
        {
            Id = Ulid.NewUlid(),
            Name = "John Doe"
        };

        Time time = new()
        {
            Date = DateOnly.FromDayNumber(1),
            Span = TimeSpan.FromSeconds(1),
            CourseId = course.Id.Value,
            PlayerId = player.Id.Value
        };

        return new Entities
        {
            Course = course,
            Player = player,
            Time = time
        };
    }

    private sealed record Entities
    {
        internal required Course Course { get; init; }
        internal required Player Player { get; init; }
        internal required Time Time { get; init; }
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

    #endregion Arrange.

    #region Act.

    private Task<HttpResponseMessage> PostContentAsync(HttpContent? content)
    {
        HttpRequestMessage message = new(HttpMethod.Post, "/api/times/")
        {
            Content = content
        };
        return HttpClient.SendAsync(message);
    }

    private Task<HttpResponseMessage> PostTimeAsync(Entities entities)
    {
        CreateTimeRequest request = new()
        {
            Date = entities.Time.Date,
            Span = entities.Time.Span,
            CourseName = entities.Course.Name,
            PlayerName = entities.Player.Name
        };
        return PostTimeAsync(request);
    }

    private Task<HttpResponseMessage> PostTimeAsync(CreateTimeRequest request)
    {
        JsonContent content = JsonContent.Create(request);
        return PostContentAsync(content);
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
