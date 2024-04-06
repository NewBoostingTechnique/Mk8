using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.ProofTypes.MySql;
using Mk8.Core.Extensions;

namespace Mk8.Core.ProofTypes;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddProofTypes(this IServiceCollection services)
    {
        return services
            .AddSingleton<IProofTypeData, MySqlProofTypeData>()
            .AddProofTypeCaching()
            .AddSingleton<IProofTypeService, ProofTypeService>();
    }

    private static IServiceCollection AddProofTypeCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<IProofTypeData>();

        return services.AddSingleton<IProofTypeData>
        (
            sp => ActivatorUtilities.CreateInstance<CachingProofTypeData>
            (
                sp,
                (IProofTypeData)descriptor.CreateInstance(sp)
            )
        );
    }
}