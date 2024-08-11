namespace Mk8.Core.Players;

internal interface IPlayerDataEvents
{

    #region Deleted.

    event EventHandler<DeletedEventArgs>? Deleted;

    internal sealed class DeletedEventArgs(Ulid id) : EventArgs
    {
        internal Ulid Id { get; } = id;
    }

    #endregion Deleted.

    #region Inserted.

    event EventHandler<InsertedEventArgs>? Inserted;

    internal sealed class InsertedEventArgs(Player player) : EventArgs
    {
        internal Player Player { get; } = player;
    }

    #endregion Inserted.

}
