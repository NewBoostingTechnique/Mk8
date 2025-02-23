using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mk8.Management.Core.Deployments.Create.Storage;
using Mk8.Management.Core.Deployments.Delete.Storage;
using Mk8.Management.MySql.Deployments.Create.Storage;
using Mk8.Management.MySql.Deployments.Storage.Delete;

namespace Mk8.Management.MySql.Deployments;

internal static class ServiceCollectionExtensions
{
    internal static void AddDeployments(this IServiceCollection services)
    {
        services.TryAddSingleton<ICreateDeploymentStorage, MySqlCreateDeploymentStorage>();
        services.TryAddSingleton<IDeleteDeploymentStorage, MySqlDeleteDeploymentStorage>();
    }
}
