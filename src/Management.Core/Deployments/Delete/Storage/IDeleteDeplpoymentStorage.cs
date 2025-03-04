using Mk8.Management.MySql.Deployments.Delete.Storage;

namespace Mk8.Management.Core.Deployments.Delete.Storage;

public interface IDeleteDeploymentStorage
{
    Task DeleteDeploymentAsync(DeleteDeploymentDto dto, CancellationToken cancellationToken = default);
}
