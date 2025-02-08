using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mk8.MySql.Countries;
using Mk8.MySql.Courses;
using Mk8.MySql.Logins;
using Mk8.MySql.Migrations;
using Mk8.MySql.News;
using Mk8.MySql.Persons;
using Mk8.MySql.Players;
using Mk8.MySql.Regions;
using Mk8.MySql.Times;

namespace Mk8.MySql;

public static class HostApplicationBuilderExtensions
{
    public static void AddMySql(this IHostApplicationBuilder builder)
    {
        builder.Services.AddCountries();
        builder.Services.AddCourses();
        builder.Services.AddLogins();
        builder.Services.AddNews();
        builder.Services.AddPersons();
        builder.Services.AddPlayers();
        builder.Services.AddRegions();
        builder.Services.AddTimes();
        builder.Services.AddMigrations();
        builder.Services.Configure<MySqlSettings>(builder.Configuration.GetRequiredSection(MySqlSettings.SectionName));
    }
}
