using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Countries;
using Mk8.Core.Courses;
using Mk8.Core.Persons;
using Mk8.Core.Players;
using Mk8.Core.Regions;
using Mk8.Core.Times;

namespace Mk8.MySql.Test.Players;

public class DetailTest : MySqlTest
{
    [Test]
    public async Task ShouldReturnTimeSheet_WhenPlayerHasNoTimes_AndAnotherPlayerDoes()
    {
        // Arrange.
        IServiceProvider serviceProvider = await ServiceProviderTask;
        ICountryStore countryStore = serviceProvider.GetRequiredService<ICountryStore>();
        ICourseStore courseStore = serviceProvider.GetRequiredService<ICourseStore>();
        IPersonStore personStore = serviceProvider.GetRequiredService<IPersonStore>();
        IPlayerStore playerStore = serviceProvider.GetRequiredService<IPlayerStore>();
        IRegionStore regionStore = serviceProvider.GetRequiredService<IRegionStore>();
        ITimeStore timeStore = serviceProvider.GetRequiredService<ITimeStore>();

        // Create courses.
        Course[] courses =
        [
            new Course
            {
                Id = Ulid.NewUlid(),
                Name = "Mario Circuit"
            },
            new Course
            {
                Id = Ulid.NewUlid(),
                Name = "Toad's Turnpike"
            }
        ];
        foreach (Course course in courses)
        {
            await courseStore.CreateAsync(course);
        }

        // Create location for players.
        Country country = new()
        {
            Id = Ulid.NewUlid(),
            Name = "United Kingdom"
        };
        await countryStore.CreateAsync(country);
        Region region = new()
        {
            Id = Ulid.NewUlid(),
            Name = "Guildford",
            CountryId = country.Id.Value
        };
        await regionStore.CreateAsync(region);

        // Create a player with times.
        Person playerWithTimes = new()
        {
            Id = Ulid.NewUlid(),
            Name = "Player with times"
        };
        await personStore.CreateAsync(playerWithTimes);
        await playerStore.CreateAsync(new()
        {
            Id = playerWithTimes.Id,
            Name = playerWithTimes.Name,
            CountryId = country.Id,
            RegionId = region.Id
        });
        foreach (Ulid courseId in courses.Select(c => c.Id!.Value))
        {
            await timeStore.CreateAsync(new()
            {
                Id = Ulid.NewUlid(),
                Date = DateOnly.FromDayNumber(1),
                Span = TimeSpan.FromSeconds(1),
                CourseId = courseId,
                PlayerId = playerWithTimes.Id.Value,
            });
        }

        // Create a player without times.
        Person playerWithoutTimes = new()
        {
            Id = Ulid.NewUlid(),
            Name = "Player without times"
        };
        await personStore.CreateAsync(playerWithoutTimes);
        await playerStore.CreateAsync(new()
        {
            Id = playerWithoutTimes.Id,
            Name = playerWithoutTimes.Name,
            CountryId = country.Id,
            RegionId = region.Id
        });

        // Act.
        Player? result = await playerStore.DetailAsync(playerWithoutTimes.Id.Value);

        // Assert.
        Assert.That(result?.Times.Count, Is.EqualTo(courses.Length));
    }

    // TODO: There's another bug, where the player does not have a region specified.
}
