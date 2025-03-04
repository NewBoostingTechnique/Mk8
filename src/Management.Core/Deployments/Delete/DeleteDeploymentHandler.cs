using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Logging;
using Mk8.Management.Core.Deployments.Delete.Storage;

namespace Mk8.Management.Core.Deployments.Delete;

internal class DeleteDeploymentHandler(
    IDeleteDeploymentStorage storage,
    ILogger<DeleteDeploymentHandler> logger
) : ICommandHandler<DeleteDeploymentCommand, Result>
{
    public async Task<Result> Handle(DeleteDeploymentCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting Deployment ...");

        await storage.DeleteDeploymentAsync
        (
            new()
            {
                Name = request.Name
            },
            cancellationToken
        )
        .ConfigureAwait(false);

        return Result.Success();
    }
}
