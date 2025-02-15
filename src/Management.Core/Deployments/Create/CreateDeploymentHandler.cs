using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Logging;
using Mk8.Management.MySql.Deployments;

namespace Mk8.Management.Core.Deployments.Create;

internal class CreateDeploymentHandler(
    IDeploymentStore deploymentStore,
    ILogger<CreateDeploymentHandler> logger,
    IEnumerable<IStoreManager> storeManagers
) : ICommandHandler<CreateDeploymentCommand, Result>
{
    public async Task<Result> Handle(CreateDeploymentCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating Deployment ...");
        Deployment deployment = new()
        {
            Name = request.Name
        };
        await deploymentStore.CreateAsync(deployment, cancellationToken).ConfigureAwait(false);

        foreach (IStoreManager storeManager in storeManagers)
        {
            await storeManager.DeployAsync(deployment.Name, cancellationToken).ConfigureAwait(false);
        }

        return Result.Success();
    }
}
