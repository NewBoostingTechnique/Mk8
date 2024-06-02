using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Extensions;

namespace Mk8.Core.Persons;

internal static class ServiceCollectionExtensions
{
    internal static void AddPersons(this IServiceCollection services)
    {
        services.AddPersonCaching();
    }

    private static void AddPersonCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<IPersonData>();

        services.AddSingleton(sp => ActivatorUtilities.CreateInstance<EventingPersonData>
        (
            sp,
            (IPersonData)descriptor.CreateInstance(sp))
        );

        services.AddSingleton<IPersonDataEvents>(sp => sp.GetRequiredService<EventingPersonData>());

        services.AddSingleton<IPersonData>
        (
            sp =>
            {
                EventingPersonData eventingPersonData = sp.GetRequiredService<EventingPersonData>();
                return ActivatorUtilities.CreateInstance<CachingPersonData>
                (
                    sp,
                    eventingPersonData,
                    eventingPersonData
                );
            }
        );
    }
}