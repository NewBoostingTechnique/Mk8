using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mk8.Core.Courses;
using Mk8.Core.Locations;
using Mk8.Core.Players;
using Mk8.Core.ProofTypes;
using Mk8.Core.Times;
using Mk8.Core.Logins;

namespace Mk8.Core.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddMk8(this IHostApplicationBuilder builder)
    {
        builder.Services
            .Configure<Mk8Settings>(builder.Configuration.GetRequiredSection("Mk8"))
            .AddCourses()
            .AddLocations()
            .AddPlayers()
            .AddProofTypes()
            .AddTimes()
            .AddLogins();

        return builder;
    }

    internal static ServiceDescriptor GetServiceDescriptor<TService>(this IServiceCollection services)
    {
        return services.LastOrDefault(x => x.ServiceType == typeof(TService))
            ?? throw new InvalidOperationException($"'{typeof(TService).FullName}' not found.");
    }
}
