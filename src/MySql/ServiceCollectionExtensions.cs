using Microsoft.Extensions.DependencyInjection;
using Mk8.MySql.Courses;
using Mk8.MySql.Locations;
using Mk8.MySql.Logins;
using Mk8.MySql.News;
using Mk8.MySql.Persons;
using Mk8.MySql.Players;
using Mk8.MySql.ProofTypes;
using Mk8.MySql.Syncs;
using Mk8.MySql.Times;

namespace Mk8.MySql;

public static class ServiceCollectionExtensions
{
    public static void AddMySql(this IServiceCollection services)
    {
        services.AddCourses();
        services.AddLocations();
        services.AddLogins();
        services.AddNews();
        services.AddPersons();
        services.AddPlayers();
        services.AddProofTypes();
        services.AddTimes();
        services.AddSyncs();
    }
}
