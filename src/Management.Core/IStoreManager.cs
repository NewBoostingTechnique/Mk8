using Mk8.Management.Core.Deployments;

namespace Mk8.Management.Core;

public interface IStoreManager
{
    Task DeployAsync(Deployment deployment, CancellationToken cancellationToken = default);
}
