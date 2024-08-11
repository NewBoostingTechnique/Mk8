namespace Mk8.Core.Logins;

internal interface ILoginDataEvents
{

    #region Inserted.

    event EventHandler<InsertedEventArgs>? Inserted;

    internal sealed class InsertedEventArgs(Login login) : EventArgs
    {
        internal Login Login { get; } = login;
    }

    #endregion Inserted.

}
