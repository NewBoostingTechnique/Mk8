using System.Collections.Immutable;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Mk8.Core.News;

internal class CachingNewData(
    IMemoryCache cache,
    INewData innerData,
    INewDataEvents newEvents
) : INewData
{
    public Task ClearAsync()
    {
        return innerData.ClearAsync();
    }

    public Task InsertAsync(New @new)
    {
        return innerData.InsertAsync(@new);
    }

    #region ListAsync.

    public Task<IImmutableList<New>> ListAsync()
    {
        return cache.GetOrCreateAsync
        (
            "News_List",
            async entry =>
            {
                IImmutableList<New> list = await innerData.ListAsync().ConfigureAwait(false);
                entry.AddExpirationToken(new ListChangeToken(newEvents));
                return list;
            }
        )!;
    }

    private sealed class ListChangeToken(INewDataEvents newEvents) : IChangeToken
    {
        public bool ActiveChangeCallbacks => true;

        public bool HasChanged { get; private set; }

        public IDisposable RegisterChangeCallback(Action<object?> callback, object? state)
        {
            return new ListChangeTokenRegistration
            (
                state =>
                {
                    HasChanged = true;
                    callback.Invoke(state);
                },
                newEvents,
                state
            );
        }

        private sealed class ListChangeTokenRegistration : IDisposable
        {
            private readonly Action<object?> _callback;
            private readonly INewDataEvents _newDataEvents;
            private readonly object? _state;

            public ListChangeTokenRegistration(
                Action<object?> callback,
                INewDataEvents newDataEvents,
                object? state
            )
            {
                _callback = callback;
                _newDataEvents = newDataEvents;
                _state = state;

                _newDataEvents.Cleared += OnListInvalidated;
                _newDataEvents.Inserted += OnListInvalidated;
            }

            private void OnListInvalidated(object? sender, EventArgs e)
            {
                _callback.Invoke(_state);
            }

            #region IDisposable.

            private bool _disposed;

            public void Dispose()
            {
                if (!_disposed)
                {
                    _newDataEvents.Cleared -= OnListInvalidated;
                    _newDataEvents.Inserted -= OnListInvalidated;
                    _disposed = true;
                }
            }

            #endregion IDisposable.

        }
    }

    #endregion ListAsync.

}