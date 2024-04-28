using System.Collections.Immutable;

namespace Mk8.Core.News;

public interface INewsService
{
    Task<IImmutableList<News>> ListAsync();
}