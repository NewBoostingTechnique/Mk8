using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Mk8.Management.MySql.Deployments;

internal static class ServiceCollectionExtensions
{
    internal static void AddDeployments(this IServiceCollection services)
    {
        services.TryAddSingleton<StoreManagerAssistant>();
        services.TryAddSingleton<IDeploymentStore, MySqlDeploymentStore>();
    }
}
