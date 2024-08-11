using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.DependencyInjection;

namespace Mk8.Core.ProofTypes;

internal static class ServiceCollectionExtensions
{
    internal static void AddProofTypes(this IServiceCollection services)
    {
        services.AddProofTypeCaching();
        services.AddSingleton<IProofTypeService, ProofTypeService>();
    }

    private static void AddProofTypeCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<IProofTypeData>();

        services.AddSingleton(sp => ActivatorUtilities.CreateInstance<EventingProofTypeData>
        (
            sp,
            (IProofTypeData)descriptor.CreateInstance(sp)
        ));

        services.AddSingleton<IProofTypeDataEvents>(sp => sp.GetRequiredService<EventingProofTypeData>());

        services.AddSingleton<IProofTypeData>
        (
            sp =>
            {
                EventingProofTypeData eventingProofTypeData = sp.GetRequiredService<EventingProofTypeData>();
                return ActivatorUtilities.CreateInstance<CachingProofTypeData>
                (
                    sp,
                    eventingProofTypeData,
                    eventingProofTypeData
                );
            }
        );
    }
}
