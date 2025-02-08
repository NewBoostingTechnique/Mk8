using Mk8.Management.Core.Deployments;

namespace Mk8.Management.MySql.Deployments;

public interface IDeploymentStore
{
    Task CreateAsync(Deployment deployment, CancellationToken cancellationToken = default);
}
