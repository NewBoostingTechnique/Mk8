namespace Mk8.Core.Players;

internal interface IPlayerStoreEvents
{

    #region Created.

    event EventHandler<CreatedEventArgs>? Created;

    internal sealed class CreatedEventArgs(Player player) : EventArgs
    {
        internal Player Player { get; } = player;
    }

    #endregion Created.

    #region Deleted.

    event EventHandler<DeletedEventArgs>? Deleted;

    internal sealed class DeletedEventArgs : EventArgs
    {
        internal Ulid? Id { get; init; }
    }

    #endregion Deleted.

}
