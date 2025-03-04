using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Courses;
using Mk8.Core.News;
using Mk8.Core.Players;
using Mk8.Core.Times;
using Mk8.Core.Logins;
using Mk8.Core.Migrations;
using Mk8.Core.Persons;
using Mk8.Core.Countries;
using Mk8.Core.Regions;

namespace Mk8.Core;

public static class ServiceCollectionExtensions
{
    public static void AddMk8Core(this IServiceCollection services)
    {
        services.AddCountries();
        services.AddCourses();
        services.AddNews();
        services.AddPersons();
        services.AddPlayers();
        services.AddRegions();
        services.AddMigrations();
        services.AddTimes();
        services.AddLogins();

        services.AddOptions<Mk8Settings>()
            .BindConfiguration(Mk8Settings.SectionName);
    }
}
