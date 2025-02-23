namespace Mk8.Management.Core.Deployments.Create.Storage;

public interface ICreateDeploymentStorage
{
    Task CreateDeploymentAsync(CreateDeploymentDto dto, CancellationToken cancellationToken = default);
}
