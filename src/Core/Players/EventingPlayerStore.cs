using System.Collections.Immutable;
using static Mk8.Core.Players.IPlayerStoreEvents;

namespace Mk8.Core.Players;

internal class EventingPlayerStore(IPlayerStore innerData)
    : IPlayerStore, IPlayerStoreEvents
{

    #region Create.

    public event EventHandler<CreatedEventArgs>? Created;

    public async Task CreateAsync(Player player, CancellationToken cancellationToken = default)
    {
        await innerData.CreateAsync(player, cancellationToken).ConfigureAwait(false);
        Created?.Invoke(this, new CreatedEventArgs(player));
    }

    #endregion Create.

    #region Delete.

    public event EventHandler<DeletedEventArgs>? Deleted;

    public async Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        await innerData.DeleteAsync(cancellationToken).ConfigureAwait(false);
        Deleted?.Invoke(this, new DeletedEventArgs());
    }

    public async Task DeleteAsync(Ulid id, CancellationToken cancellationToken = default)
    {
        await innerData.DeleteAsync(id, cancellationToken).ConfigureAwait(false);
        Deleted?.Invoke(this, new DeletedEventArgs { Id = id });
    }

    #endregion Delete.

    public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return innerData.ExistsAsync(name, cancellationToken);
    }

    public Task<Player?> FindAsync(Ulid id, CancellationToken cancellationToken = default)
    {
        return innerData.FindAsync(id, cancellationToken);
    }

    public Task<Ulid?> IdentifyAsync(string name, CancellationToken cancellationToken = default)
    {
        return innerData.IdentifyAsync(name, cancellationToken);
    }

    public Task<IImmutableList<Player>> IndexAsync(CancellationToken cancellationToken = default)
    {
        return innerData.IndexAsync(cancellationToken);
    }
}
