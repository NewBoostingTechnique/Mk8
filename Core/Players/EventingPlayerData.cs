using System.Collections.Immutable;
using static Mk8.Core.Players.IPlayerDataEvents;

namespace Mk8.Core.Players;

internal class EventingPlayerData(IPlayerData innerData)
    : IPlayerData, IPlayerDataEvents
{

    #region Delete.

    public event EventHandler<DeletedEventArgs>? Deleted;

    public async Task DeleteAsync(string playerId)
    {
        await innerData.DeleteAsync(playerId).ConfigureAwait(false);
        Deleted?.Invoke(this, new DeletedEventArgs(playerId));
    }

    #endregion Delete.

    public Task<bool> ExistsAsync(string playerName)
    {
        return innerData.ExistsAsync(playerName);
    }

    public Task<Player?> DetailAsync(string playerId)
    {
        return innerData.DetailAsync(playerId);
    }

    public Task<string?> IdentifyAsync(string playerName)
    {
        return innerData.IdentifyAsync(playerName);
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