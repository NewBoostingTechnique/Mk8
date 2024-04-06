using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Extensions;
using Mk8.Core.Locations.Countries.MySql;

namespace Mk8.Core.Locations.Countries;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddCountries(this IServiceCollection services)
    {
        return services
            .AddSingleton<ICountryData, MySqlCountryData>()
            .AddCountryCaching()
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
