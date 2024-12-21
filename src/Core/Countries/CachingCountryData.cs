using System.Collections.Immutable;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Mk8.Core.Countries;

internal class CachingCountryStore(
    IMemoryCache cache,
    ICountryStore innerStore,
    ICountryStoreEvents events
) : ICountryStore
{

    #region Identify.

    public Task<Ulid?> IdentifyAsync(string name, CancellationToken cancellationToken = default)
    {
        return cache.GetOrCreateAsync
        (
            $"Country_Identify:{name}",
            async entry =>
            {
                Ulid? id = await innerStore.IdentifyAsync(name, cancellationToken).ConfigureAwait(false);
                entry.AddExpirationToken(new IdentifyChangeToken(events, name));
                return id;
            }
        );
    }

    private sealed class IdentifyChangeToken(
        ICountryStoreEvents events,
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
            private readonly ICountryStoreEvents _events;
            private readonly string _name;
            private readonly object? _state;

            internal IdentifyChangeTokenRegistration(
                Action<object?> callback,
                ICountryStoreEvents events,
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

            private void OnInserted(object? sender, ICountryStoreEvents.InsertedEventArgs e)
            {
                if (e.Country.Name == _name)
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

    public Task CreateAsync(Country country, CancellationToken cancellationToken = default)
    {
        return innerStore.CreateAsync(country, cancellationToken);
    }

    #region List.

    public Task<IImmutableList<Country>> IndexAsync(CancellationToken cancellationToken = default)
    {
        return cache.GetOrCreateAsync
        (
            "Country_List",
            async entry =>
            {
                IImmutableList<Country> list = await innerStore.IndexAsync(cancellationToken).ConfigureAwait(false);
                entry.AddExpirationToken(new ListChangeToken(events));
                return list;
            }
        )!;
    }

    private sealed class ListChangeToken(
        ICountryStoreEvents events
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
            private readonly ICountryStoreEvents _events;
            private readonly object? _state;

            internal ListChangeTokenRegistration(
                Action<object?> callback,
                ICountryStoreEvents events,
                object? state
            )
            {
                _callback = callback;
                _events = events;
                _state = state;

                _events.Inserted += OnCountryInserted;
            }

            private void OnCountryInserted(object? sender, ICountryStoreEvents.InsertedEventArgs e)
            {
                _callback.Invoke(_state);
            }

            #region IDisposable.

            private bool _disposed;

            public void Dispose()
            {
                if (!_disposed)
                {
                    _events.Inserted -= OnCountryInserted;

                    _disposed = true;
                }
            }

            #endregion IDisposable.

        }
    }

    #endregion List.

}
