using System.Net;
using System.Net.Http.Json;
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
    public async Task ShouldCreateTime_WhenOnHappyPath()
    {
        // Arrange.

        // TODO: Entities should not be public.
        // There should be a test API for creating entities.

        Course course = new() { Id = Ulid.NewUlid(), Name = "Mushroom Cup" };
        CourseStore.IdentifyAsync(Arg.Is(course.Name), Arg.Any<CancellationToken>())
            .Returns(_ => Task.FromResult(course.Id));

        Player player = new() { Id = Ulid.NewUlid(), Name = "John Doe" };
        PlayerStore.IdentifyAsync(Arg.Is(player.Name), Arg.Any<CancellationToken>())
            .Returns(_ => Task.FromResult(player.Id));

        TimeStore.ExistsAsync(Arg.Is(course.Id.Value), Arg.Is(player.Id.Value), Arg.Any<CancellationToken>())
            .Returns(false);

        CreateTimeRequest requestContent = new()
        {
            CourseName = course.Name,
            Date = DateOnly.FromDayNumber(1),
            PlayerName = player.Name,
            Span = TimeSpan.FromMinutes(1)
        };

        // Act.
        HttpResponseMessage response = await HttpClient.SendAsync
        (
            new(HttpMethod.Post, "/api/times/") { Content = JsonContent.Create(requestContent) }
        );

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            TimeStore.Received(1).CreateAsync
            (
                Arg.Is<Time>(time =>
                    time.CourseId == course.Id &&
                    time.Date == requestContent.Date &&
                    time.PlayerId == player.Id &&
                    time.Span == requestContent.Span
                ),
                Arg.Any<CancellationToken>()
            );
        });
    }
}
