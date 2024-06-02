using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Extensions;

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
        services.AddSingleton<IProofTypeData>
        (
            sp => ActivatorUtilities.CreateInstance<CachingProofTypeData>
            (
                sp,
                (IProofTypeData)descriptor.CreateInstance(sp)
            )
        );
    }
}