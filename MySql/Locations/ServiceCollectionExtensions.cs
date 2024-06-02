using Microsoft.Extensions.DependencyInjection;
using Mk8.MySql.Locations.Countries;
using Mk8.MySql.Locations.Regions;

namespace Mk8.MySql.Locations;

internal static class ServiceCollectionExtensions
{
    internal static void AddLocations(this IServiceCollection services)
    {
        services.AddCountries();
        services.AddRegions();
    }
}