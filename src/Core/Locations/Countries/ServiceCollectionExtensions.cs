using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.DependencyInjection;

namespace Mk8.Core.Locations.Countries;

internal static class ServiceCollectionExtensions
{
    internal static void AddCountries(this IServiceCollection services)
    {
        services.AddCountryCaching();
        services.AddSingleton<ICountryService, CountryService>();
    }

    private static void AddCountryCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<ICountryData>();

        services.AddSingleton(sp => ActivatorUtilities.CreateInstance<EventingCountryData>
        (
            sp,
            (ICountryData)descriptor.CreateInstance(sp)
        ));

        services.AddSingleton<ICountryDataEvents>(sp => sp.GetRequiredService<EventingCountryData>());

        services.AddSingleton<ICountryData>
        (
            sp =>
            {
                EventingCountryData eventingRegionData = sp.GetRequiredService<EventingCountryData>();
                return ActivatorUtilities.CreateInstance<CachingCountryData>
                (
                    sp,
                    eventingRegionData,
                    eventingRegionData
                );
            }
        );

    }
}
