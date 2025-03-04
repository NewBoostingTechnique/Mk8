using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mk8.Management.Core.Deployments.Create;
using Mk8.Management.Core.Deployments.Delete;

namespace Mk8.Management.Core.Deployments.Seed;

internal static class ServiceCollectionExtensions
{
    internal static void AddDeployments(this IServiceCollection services)
    {
        services.TryAddSingleton<ICommandHandler<CreateDeploymentCommand, Result>, CreateDeploymentHandler>();
        services.TryAddSingleton<ICommandHandler<DeleteDeploymentCommand, Result>, DeleteDeploymentHandler>();
        services.TryAddSingleton<ICommandHandler<SeedDeploymentCommand, Result>, SeedDeploymentHandler>();
    }
}
