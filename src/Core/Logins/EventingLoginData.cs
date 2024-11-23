
using static Mk8.Core.Logins.ILoginDataEvents;

namespace Mk8.Core.Logins;

internal class EventingLoginData(
    ILoginStore innerData
) : ILoginStore, ILoginDataEvents
{
    public Task<bool> ExistsAsync(string email)
    {
        return innerData.ExistsAsync(email);
    }

    #region Insert.

    public event EventHandler<InsertedEventArgs>? Inserted;

    public async Task CreateAsync(Login login)
    {
        await innerData.CreateAsync(login).ConfigureAwait(false);
        Inserted?.Invoke(this, new InsertedEventArgs(login));
    }

    #endregion Insert.

}
