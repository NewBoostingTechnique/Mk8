using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
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

public static class HostApplicationBuilderExtensions
{
    public static void AddMySqlManagement(this IHostApplicationBuilder builder)
    {
        builder.Services.AddManagement();
        builder.Services.Configure<MySqlManagementSettings>(builder.Configuration.GetRequiredSection(MySqlManagementSettings.SectionName));

        // TODO: Currently these need to be in dependency order.
        // Consider making this all independent units somehow.
        builder.Services.AddCountries();
        builder.Services.AddCourses();
        builder.Services.AddDeployments();
        builder.Services.AddPersons();
        builder.Services.AddLogins();
        builder.Services.AddMigrations();
        builder.Services.AddNews();
        builder.Services.AddRegions();
        builder.Services.AddPlayers();
        builder.Services.AddTimes();
    }
}
