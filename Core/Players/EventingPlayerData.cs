using System.Collections.Immutable;
using static Mk8.Core.Players.IPlayerDataEvents;

namespace Mk8.Core.Players;

internal class EventingPlayerData(IPlayerData innerData)
    : IPlayerData, IPlayerDataEvents
{

    #region Delete.

    public event EventHandler<DeletedEventArgs>? Deleted;

    public async Task DeleteAsync(string id)
    {
        await innerData.DeleteAsync(id).ConfigureAwait(false);
        Deleted?.Invoke(this, new DeletedEventArgs(id));
    }

    #endregion Delete.

    public Task<bool> ExistsAsync(string name)
    {
        return innerData.ExistsAsync(name);
    }

    public Task<Player?> DetailAsync(string id)
    {
        return innerData.DetailAsync(id);
    }

    public Task<string?> IdentifyAsync(string name)
    {
        return innerData.IdentifyAsync(name);
    }

    #region Insert.

    public event EventHandler<InsertedEventArgs>? Inserted;

    public async Task InsertAsync(Player player)
    {
        await innerData.InsertAsync(player).ConfigureAwait(false);
        Inserted?.Invoke(this, new InsertedEventArgs(player));
    }

    #endregion Insert.

    public Task<IImmutableList<Player>> ListAsync()
    {
        return innerData.ListAsync();
    }
}