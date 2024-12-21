using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.DependencyInjection;

namespace Mk8.Core.Countries;

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

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<ICountryStore>();

        services.AddSingleton(sp => ActivatorUtilities.CreateInstance<EventingCountryStore>
        (
            sp,
            (ICountryStore)descriptor.CreateInstance(sp)
        ));

        services.AddSingleton<ICountryStoreEvents>(sp => sp.GetRequiredService<EventingCountryStore>());

        services.AddSingleton<ICountryStore>
        (
            sp =>
            {
                EventingCountryStore eventingCountryStore = sp.GetRequiredService<EventingCountryStore>();
                return ActivatorUtilities.CreateInstance<CachingCountryStore>
                (
                    sp,
                    eventingCountryStore,
                    eventingCountryStore
                );
            }
        );

    }
}
