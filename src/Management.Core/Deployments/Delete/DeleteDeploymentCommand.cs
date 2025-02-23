using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Mk8.Management.Core.Deployments.Delete;

public record DeleteDeploymentCommand : ICommand<Result>
{
    public required string Name { get; init; }
}
