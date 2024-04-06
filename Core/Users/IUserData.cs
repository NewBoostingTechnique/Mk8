namespace Mk8.Core.Users;

public interface IUserData
{
    Task<bool> ExistsAsync(string email);
}