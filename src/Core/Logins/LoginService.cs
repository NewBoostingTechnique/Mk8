namespace Mk8.Core.Logins;

internal class LoginService(
    ILoginStore loginStore
) : ILoginService
{
    public Task<bool> ExistsAsync(string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        return loginStore.ExistsAsync(email);
    }
}
