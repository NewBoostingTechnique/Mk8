using Microsoft.Extensions.Caching.Memory;

namespace Mk8.Core.Users;

internal class CachingUserData(IMemoryCache cache, IUserData innerData) : IUserData
{
    public Task<bool> ExistsAsync(string email)
    {
        return cache.GetOrCreateAsync
        (
            $"Course_Exists:{email}",
            entry => innerData.ExistsAsync(email)
        );
    }
}