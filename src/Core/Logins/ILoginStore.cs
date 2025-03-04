namespace Mk8.Core.Logins;

public interface ILoginStore
{
    Task CreateAsync(Login login, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default);
}
