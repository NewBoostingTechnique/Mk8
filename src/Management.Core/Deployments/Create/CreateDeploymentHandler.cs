using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Logging;
using Mk8.Management.Core.Deployments.Create.Storage;

namespace Mk8.Management.Core.Deployments.Create;

internal class CreateDeploymentHandler(
    ICreateDeploymentStorage storage,
    ILogger<CreateDeploymentHandler> logger,
    IEnumerable<IStoreManager> storeManagers
) : ICommandHandler<CreateDeploymentCommand, Result>
{
    public async Task<Result> Handle(CreateDeploymentCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating Deployment ...");

        await storage.CreateDeploymentAsync
        (
            new()
            {
                Name = request.Name
            },
            cancellationToken
        )
        .ConfigureAwait(false);

        foreach (IStoreManager storeManager in storeManagers)
        {
            await storeManager.DeployAsync(request.Name, cancellationToken).ConfigureAwait(false);
        }

        return Result.Success();
    }
}
