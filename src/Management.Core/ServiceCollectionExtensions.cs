using Microsoft.Extensions.DependencyInjection;
using Mk8.Management.Core.Deployments.Seed;

namespace Mk8.Management.Core;

public static class ServiceCollectionExtensions
{
    public static void AddManagement(this IServiceCollection services)
    {
        services.AddDeployments();
    }
}
