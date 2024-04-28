using System.Collections.Immutable;
using Microsoft.Extensions.Caching.Memory;

namespace Mk8.Core.News;

internal class CachingNewsData(
    IMemoryCache cache,
    INewsData innerData
) : INewsData
{
    public Task<IImmutableList<News>> ListAsync()
    {
        return cache.GetOrCreateAsync
        (
            "News_List",
            entry => innerData.ListAsync()
        )!;
    }
}