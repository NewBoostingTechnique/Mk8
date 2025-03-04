
using static Mk8.Core.Logins.ILoginDataEvents;

namespace Mk8.Core.Logins;

internal class EventingLoginData(
    ILoginStore innerData
) : ILoginStore, ILoginDataEvents
{
    public Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return innerData.ExistsAsync(email, cancellationToken);
    }

    #region Insert.

    public event EventHandler<InsertedEventArgs>? Inserted;

    public async Task CreateAsync(Login login, CancellationToken cancellationToken = default)
    {
        await innerData.CreateAsync(login, cancellationToken).ConfigureAwait(false);
        Inserted?.Invoke(this, new InsertedEventArgs(login));
    }

    #endregion Insert.

}
