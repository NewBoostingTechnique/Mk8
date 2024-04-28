using System.Collections.Immutable;

namespace Mk8.Core.News;

internal interface INewsData
{
    Task<IImmutableList<News>> ListAsync();
}