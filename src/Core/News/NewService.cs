using System.Collections.Immutable;

namespace Mk8.Core.News;

internal class NewService(INewData newData) : INewService
{
    public Task<IImmutableList<New>> ListAsync()
    {
        return newData.ListAsync();
    }
}
