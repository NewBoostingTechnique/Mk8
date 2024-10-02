using System.Collections.Immutable;
using Mk8.Core.Migrations;

namespace Mk8.Core.News;

public interface INewService
{
    Task CreateAsync(New @new, CancellationToken cancellationToken = default);

    Task<IImmutableList<New>> IndexAsync(CancellationToken cancellationToken = default);

    Task<Migration> MigrateAsync(CancellationToken cancellationToken = default);
}
