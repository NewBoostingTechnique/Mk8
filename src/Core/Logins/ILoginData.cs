namespace Mk8.Core.Logins;

public interface ILoginData
{
    Task CreateAsync(Login login);

    Task<bool> ExistsAsync(string email);
}
