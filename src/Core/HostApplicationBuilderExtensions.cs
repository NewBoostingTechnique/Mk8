using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mk8.Core.Courses;
using Mk8.Core.News;
using Mk8.Core.Players;
using Mk8.Core.Times;
using Mk8.Core.Logins;
using Mk8.Core.Migrations;
using Mk8.Core.Persons;
using Mk8.Core.Seeds;
using Mk8.Core.Countries;
using Mk8.Core.Regions;

namespace Mk8.Core;

public static class HostApplicationBuilderExtensions
{
    public static void AddMk8Core(this IHostApplicationBuilder builder)
    {
        IServiceCollection services = builder.Services;

        services.Configure<Mk8Settings>(builder.Configuration.GetRequiredSection("Mk8"));
        services.AddCountries();
        services.AddCourses();
        services.AddNews();
        services.AddPersons();
        services.AddPlayers();
        services.AddRegions();
        services.AddSeeds();
        services.AddMigrations();
        services.AddTimes();
        services.AddLogins();
    }
}
