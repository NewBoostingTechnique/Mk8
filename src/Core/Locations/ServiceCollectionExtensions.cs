using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Locations.Countries;
using Mk8.Core.Locations.Regions;

namespace Mk8.Core.Locations;

internal static class ServiceCollectionExtensions
{
    internal static void AddLocations(this IServiceCollection services)
    {
        services.AddCountries();
        services.AddRegions();
    }
}
