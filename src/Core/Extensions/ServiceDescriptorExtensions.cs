using Microsoft.Extensions.DependencyInjection;

namespace Mk8.Core.Extensions;

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
}