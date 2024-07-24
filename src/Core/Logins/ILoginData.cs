namespace Mk8.Core.Logins;

public interface ILoginData
{
    Task<bool> ExistsAsync(string email);
}