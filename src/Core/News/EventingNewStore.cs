using System.Collections.Immutable;

namespace Mk8.Core.News;

internal class EventingNewData(INewStore innerData) : INewStore, INewStoreEvents
{
    public Task<IImmutableList<New>> IndexAsync(CancellationToken cancellationToken = default)
    {
        return innerData.IndexAsync(cancellationToken);
    }

    #region Clear.

    public event EventHandler? Cleared;

    public async Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        await innerData.DeleteAsync(cancellationToken).ConfigureAwait(false);
        Cleared?.Invoke(this, EventArgs.Empty);
    }

    #endregion Clear.

    #region Insert.

    public event EventHandler? Inserted;

    public async Task CreateAsync(New @new, CancellationToken cancellationToken = default)
    {
        await innerData.CreateAsync(@new, cancellationToken).ConfigureAwait(false);
        Inserted?.Invoke(this, EventArgs.Empty);
    }

    #endregion Insert.

}
