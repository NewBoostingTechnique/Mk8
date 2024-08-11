using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mk8.Core.Courses;
using Mk8.Core.Locations;
using Mk8.Core.News;
using Mk8.Core.Players;
using Mk8.Core.ProofTypes;
using Mk8.Core.Times;
using Mk8.Core.Logins;
using Mk8.Core.Syncs;
using Mk8.Core.Persons;
using Mk8.Core.Seeds;

namespace Mk8.Core;

public static class HostApplicationBuilderExtensions
{
    public static void AddMk8(this IHostApplicationBuilder builder)
    {
        IServiceCollection services = builder.Services;

        services.Configure<Mk8Settings>(builder.Configuration.GetRequiredSection("Mk8"));
        services.AddCourses();
        services.AddLocations();
        services.AddNews();
        services.AddPersons();
        services.AddPlayers();
        services.AddProofTypes();
        services.AddSeeds();
        services.AddSyncs();
        services.AddTimes();
        services.AddLogins();
    }
}
