using System.Collections.Immutable;

namespace Mk8.Core.News;

public interface INewStore
{
    Task CreateAsync(New @new, CancellationToken cancellationToken = default);

    Task DeleteAsync(CancellationToken cancellationToken = default);

    Task<IImmutableList<New>> IndexAsync(CancellationToken cancellationToken = default);
}
