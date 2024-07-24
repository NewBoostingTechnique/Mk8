using System.Collections.Immutable;

namespace Mk8.Core.News;

public interface INewService
{
    Task<IImmutableList<New>> ListAsync();
}