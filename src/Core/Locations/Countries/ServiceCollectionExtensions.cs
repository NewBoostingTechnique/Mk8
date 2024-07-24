using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Extensions;

namespace Mk8.Core.Locations.Countries;

internal static class ServiceCollectionExtensions
{
    internal static void AddCountries(this IServiceCollection services)
    {
        services.AddCountryCaching()
            .AddSingleton<ICountryService, CountryService>();
    }

    private static IServiceCollection AddCountryCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<ICountryData>();

        return services.AddSingleton<ICountryData>
        (
            sp => ActivatorUtilities.CreateInstance<CachingCountryData>
            (
                sp,
                (ICountryData)descriptor.CreateInstance(sp)
            )
        );
    }
}
