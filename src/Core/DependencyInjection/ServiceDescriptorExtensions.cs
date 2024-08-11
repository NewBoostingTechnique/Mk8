using Microsoft.Extensions.DependencyInjection;

namespace Mk8.Core.DependencyInjection;

internal static class ServiceDescriptorExtensions
{
    internal static object CreateInstance(
        this ServiceDescriptor serviceDescriptor,
        IServiceProvider serviceProvider
    )
    {
        if (serviceDescriptor.ImplementationInstance is not null)
            return serviceDescriptor.ImplementationInstance;

        if (serviceDescriptor.ImplementationType is not null)
            return ActivatorUtilities.CreateInstance(serviceProvider, serviceDescriptor.ImplementationType);

        if (serviceDescriptor.ImplementationFactory is not null)
            return serviceDescriptor.ImplementationFactory.Invoke(serviceProvider);

        throw new InvalidOperationException("No implementation found.");
    }

    internal static ServiceDescriptor GetServiceDescriptor<TService>(this IServiceCollection services)
    {
        return services.LastOrDefault(x => x.ServiceType == typeof(TService))
            ?? throw new InvalidOperationException($"'{typeof(TService).FullName}' not found.");
    }
}
