using System.Collections.Immutable;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Mk8.Core.Locations.Countries;

internal class CachingCountryData(
    IMemoryCache cache,
    ICountryData innerData,
    ICountryDataEvents events
) : ICountryData
{

    #region Identify.

    public Task<Ulid?> IdentifyAsync(string name)
    {
        return cache.GetOrCreateAsync
        (
            $"Country_Identify:{name}",
            async entry =>
            {
                Ulid? id = await innerData.IdentifyAsync(name).ConfigureAwait(false);
                entry.AddExpirationToken(new IdentifyChangeToken(events, id, name));
                return id;
            }
        );
    }

    private sealed class IdentifyChangeToken(
        ICountryDataEvents events,
        Ulid? id,
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
                id,
                name,
                state
            );
        }

        private sealed class IdentifyChangeTokenRegistration : IDisposable
        {
            private readonly Action<object?> _callback;
            private readonly ICountryDataEvents _events;
            private readonly string _name;
            private readonly Ulid? _id;
            private readonly object? _state;

            internal IdentifyChangeTokenRegistration(
                Action<object?> callback,
                ICountryDataEvents events,
                Ulid? playerId,
                string playerName,
                object? state
            )
            {
                _callback = callback;
                _events = events;
                _name = playerName;
                _id = playerId;
                _state = state;

                _events.Inserted += OnInserted;
            }

            private void OnInserted(object? sender, ICountryDataEvents.InsertedEventArgs e)
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

    public Task InsertAsync(Country country)
    {
        return innerData.InsertAsync(country);
    }

    #region List.

    public Task<IImmutableList<Country>> ListAsync()
    {
        return cache.GetOrCreateAsync
        (
            "Country_List",
            async entry =>
            {
                IImmutableList<Country> list = await innerData.ListAsync().ConfigureAwait(false);
                entry.AddExpirationToken(new ListChangeToken(events));
                return list;
            }
        )!;
    }

    private sealed class ListChangeToken(
        ICountryDataEvents events
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
            private readonly ICountryDataEvents _events;
            private readonly object? _state;

            internal ListChangeTokenRegistration(
                Action<object?> callback,
                ICountryDataEvents events,
                object? state
            )
            {
                _callback = callback;
                _events = events;
                _state = state;

                _events.Inserted += OnCountryInserted;
            }

            private void OnCountryInserted(object? sender, ICountryDataEvents.InsertedEventArgs e)
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
