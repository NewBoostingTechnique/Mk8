using System.Collections.Immutable;

namespace Mk8.Core.News;

internal class EventingNewData(INewData innerData) : INewData, INewDataEvents
{
    public Task<IImmutableList<New>> ListAsync()
    {
        return innerData.ListAsync();
    }

    #region Clear.

    public event EventHandler? Cleared;

    public async Task ClearAsync()
    {
        await innerData.ClearAsync().ConfigureAwait(false);
        Cleared?.Invoke(this, EventArgs.Empty);
    }

    #endregion Clear.

    #region Insert.

    public event EventHandler? Inserted;

    public async Task InsertAsync(New @new)
    {
        await innerData.InsertAsync(@new).ConfigureAwait(false);
        Inserted?.Invoke(this, EventArgs.Empty);
    }

    #endregion Insert.

}