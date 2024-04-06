namespace Mk8.Core.Users;

internal class UserService(IUserData userData) : IUserService
{
    public Task<bool> ExistsAsync(string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        return userData.ExistsAsync(email);
    }
}