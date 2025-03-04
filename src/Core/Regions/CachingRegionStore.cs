using System.Collections.Immutable;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Mk8.Core.Regions;

internal class CachingRegionStore(
    IMemoryCache cache,
    IRegionStore innerStore,
    IRegionDataEvents events
) : IRegionStore
{

    #region Identify.

    public Task<Ulid?> IdentifyAsync(string name, CancellationToken cancellationToken = default)
    {
        return cache.GetOrCreateAsync
        (
            $"Region_Identify:{name}",
            async entry =>
            {
                Ulid? id = await innerStore.IdentifyAsync(name, cancellationToken).ConfigureAwait(false);
                entry.AddExpirationToken(new IdentifyChangeToken(events, name));
                return id;
            }
        );
    }

    private sealed class IdentifyChangeToken(
      IRegionDataEvents events,
      string name
  ) : IChangeToken
    {
        public bool ActiveChangeCallbacks => true;

        public bool HasChanged { get; private set; }

        #region RegisterChangeCallback.

        public IDisposable RegisterChangeCallback(Action<object?> callback, object? state)
        {
            return new IdentifyChangeTokenRegistration
            (
                state =>
                {
                    HasChanged = true;
                    callback.Invoke(state);
                },
                events,
                name,
                state
            );
        }

        private sealed class IdentifyChangeTokenRegistration : IDisposable
        {
            private readonly Action<object?> _callback;
            private readonly IRegionDataEvents _events;
            private readonly string _name;
            private readonly object? _state;

            internal IdentifyChangeTokenRegistration(
                Action<object?> callback,
                IRegionDataEvents events,
                string name,
                object? state
            )
            {
                _callback = callback;
                _events = events;
                _name = name;
                _state = state;

                _events.Inserted += OnInserted;
            }

            private void OnInserted(object? sender, IRegionDataEvents.InsertedEventArgs e)
            {
                if (e.Region.Name == _name)
                    _callback(_state);
            }

            #region IDisposable.

            private bool _disposed;

            public void Dispose()
            {
                if (!_disposed)
                {
                    _events.Inserted -= OnInserted;
                    _disposed = true;
                }
            }

            #endregion IDisposable.

        }

        #endregion RegisterChangeCallback.

    }

    #endregion Identify.

    public Task CreateAsync(Region region, CancellationToken cancellationToken = default)
    {
        return innerStore.CreateAsync(region, cancellationToken);
    }

    #region List.

    public Task<IImmutableList<Region>> IndexAsync(Ulid countryId, CancellationToken cancellationToken = default)
    {
        return cache.GetOrCreateAsync
        (
            $"Region_List:{countryId}",
            async entry =>
            {
                IImmutableList<Region> regions = await innerStore.IndexAsync(countryId, cancellationToken).ConfigureAwait(false);
                entry.AddExpirationToken(new ListChangeToken(events));
                return regions;
            }
        )!;
    }

    private sealed class ListChangeToken(
        IRegionDataEvents events
    ) : IChangeToken
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
                events,
                state
            );
        }

        private sealed class ListChangeTokenRegistration : IDisposable
        {
            private readonly Action<object?> _callback;
            private readonly IRegionDataEvents _events;
            private readonly object? _state;

            internal ListChangeTokenRegistration(
                Action<object?> callback,
                IRegionDataEvents events,
                object? state
            )
            {
                _callback = callback;
                _events = events;
                _state = state;

                _events.Inserted += OnRegionInserted;
            }

            private void OnRegionInserted(object? sender, IRegionDataEvents.InsertedEventArgs e)
            {
                _callback.Invoke(_state);
            }

            #region IDisposable.

            private bool _disposed;

            public void Dispose()
            {
                if (!_disposed)
                {
                    _events.Inserted -= OnRegionInserted;
                    _disposed = true;
                }
            }

            #endregion IDisposable.

        }
    }

    #endregion List.

}
