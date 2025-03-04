namespace Mk8.Core.Logins;

public interface ILoginService
{
    Task<bool> ExistsAsync(string email);
}
