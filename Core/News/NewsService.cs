using System.Collections.Immutable;

namespace Mk8.Core.News;

internal class NewsService(INewsData newsData) : INewsService
{
    public Task<IImmutableList<News>> ListAsync()
    {
        // TODO: Pagination or continuation tokens.
        return newsData.ListAsync();
    }
}