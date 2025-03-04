using Microsoft.Extensions.DependencyInjection;
using Mk8.Management.Core;
using Mk8.Management.MySql.Countries;
using Mk8.Management.MySql.Courses;
using Mk8.Management.MySql.Deployments;
using Mk8.Management.MySql.Logins;
using Mk8.Management.MySql.Migrations;
using Mk8.Management.MySql.News;
using Mk8.Management.MySql.Persons;
using Mk8.Management.MySql.Players;
using Mk8.Management.MySql.Regions;
using Mk8.Management.MySql.Times;

namespace Mk8.Management.MySql;

public static class ServiceCollectionExtensions
{
    public static void AddMySqlManagement(this IServiceCollection services)
    {
        services.AddManagement();

        services.AddOptions<MySqlManagementSettings>()
            .BindConfiguration(MySqlManagementSettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddCountries();
        services.AddCourses();
        services.AddDeployments();
        services.AddPersons();
        services.AddLogins();
        services.AddMigrations();
        services.AddNews();
        services.AddRegions();
        services.AddPlayers();
        services.AddTimes();
    }
}
