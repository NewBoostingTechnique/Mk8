namespace Mk8.Core.Logins;

internal class LoginService(ILoginData loginData) : ILoginService
{
    public Task<bool> ExistsAsync(string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        return loginData.ExistsAsync(email);
    }
}