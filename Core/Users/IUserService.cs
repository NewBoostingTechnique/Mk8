namespace Mk8.Core.Users;

public interface IUserService
{
    Task<bool> ExistsAsync(string email);
}