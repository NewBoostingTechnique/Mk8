using Microsoft.Extensions.Caching.Memory;

namespace Mk8.Core.Logins;

internal class CachingLoginData(IMemoryCache cache, ILoginData innerData) : ILoginData
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