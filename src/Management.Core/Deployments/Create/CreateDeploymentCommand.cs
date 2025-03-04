using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Mk8.Management.Core.Deployments.Create;

public record CreateDeploymentCommand : ICommand<Result>
{
    public required string Name { get; init; }
}
