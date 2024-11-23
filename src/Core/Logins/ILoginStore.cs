namespace Mk8.Core.Logins;

public interface ILoginStore
{
    Task CreateAsync(Login login);

    Task<bool> ExistsAsync(string email);
}
