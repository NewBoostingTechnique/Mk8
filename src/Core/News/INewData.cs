using System.Collections.Immutable;

namespace Mk8.Core.News;

public interface INewData
{
    Task ClearAsync();

    Task InsertAsync(New @new);

    Task<IImmutableList<New>> ListAsync();
}