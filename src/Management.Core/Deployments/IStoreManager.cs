namespace Mk8.Management.Core;

public interface IStoreManager
{
    Task DeployAsync(string deploymentName, CancellationToken cancellationToken = default);
}
