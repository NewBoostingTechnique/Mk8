using System.Collections.Immutable;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Mk8.Core.Times;

namespace Mk8.Core.Players;

internal class CachingPlayerStore(
    IMemoryCache cache,
    IPlayerStore innerStore,
    IPlayerStoreEvents playerEvents,
    ITimeStoreEvents timeEvents
) : IPlayerStore
{

    public Task CreateAsync(Player player, CancellationToken cancellationToken = default)
    {
        return innerStore.CreateAsync(player, cancellationToken);
    }

    #region Delete.

    public Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        return innerStore.DeleteAsync(cancellationToken);
    }

    public Task DeleteAsync(Ulid id, CancellationToken cancellationToken = default)
    {
        return innerStore.DeleteAsync(id, cancellationToken);
    }

    #endregion Delete.

    #region Detail.

    public Task<Player?> DetailAsync(Ulid id, CancellationToken cancellationToken = default)
    {
        return cache.GetOrCreateAsync
        (
            $"Player_Detail:{id}",
            async entry =>
            {
                Player? player = await innerStore.DetailAsync(id, cancellationToken).ConfigureAwait(false);
                entry.AddExpirationToken(new DetailChangeToken(playerEvents, player, id, timeEvents));
                return player;
            }
        );
    }

    private sealed class DetailChangeToken(
        IPlayerStoreEvents playerEvents,
        Player? player,
        Ulid playerId,
        ITimeStoreEvents timeEvents
    ) : IChangeToken
    {
        public bool ActiveChangeCallbacks => true;

        public bool HasChanged { get; private set; }

        #region RegisterChangeCallback.

        public IDisposable RegisterChangeCallback(Action<object?> callback, object? state)
        {
            return new DetailChangeTokenRegistration
            (
                state =>
                {
                    HasChanged = true;
                    callback.Invoke(state);
                },
                playerEvents,
                player,
                playerId,
                state,
                timeEvents
            );
        }

        private sealed class DetailChangeTokenRegistration : IDisposable
        {
            private readonly Action<object?> _callback;
            private readonly IPlayerStoreEvents _playerEvents;
            private readonly Player? _player;
            private readonly Ulid? _id;
            private readonly object? _state;
            private readonly ITimeStoreEvents _timeEvents;

            internal DetailChangeTokenRegistration(
                Action<object?> callback,
                IPlayerStoreEvents playerEvents,
                Player? player,
                Ulid id,
                object? state,
                ITimeStoreEvents timeEvents
            )
            {
                _callback = callback;
                _playerEvents = playerEvents;
                _player = player;
                _id = id;
                _state = state;
                _timeEvents = timeEvents;

                _playerEvents.Deleted += OnPlayerDeleted;
                _playerEvents.Created += OnPlayerInserted;
                _timeEvents.Created += OnTimeInserted;
            }

            private void OnPlayerDeleted(object? sender, IPlayerStoreEvents.DeletedEventArgs e)
            {
                if (e.Id is null || e.Id == _id)
                    _callback(_state);
            }

            private void OnPlayerInserted(object? sender, IPlayerStoreEvents.CreatedEventArgs e)
            {
                if (e.Player.Id == _id)
                    _callback(_state);
            }

            private void OnTimeInserted(object? sender, ITimeStoreEvents.CreatedEventArgs e)
            {
                if (_player != null && e.Time.PlayerId == _player.Id)
                    _callback(_state);
            }

            #region IDisposable.

            private bool _disposed;

            public void Dispose()
            {
                if (!_disposed)
                {
                    _playerEvents.Deleted -= OnPlayerDeleted;
                    _playerEvents.Created -= OnPlayerInserted;
                    _timeEvents.Created -= OnTimeInserted;
                    _disposed = true;
                }
            }

            #endregion IDisposable.

        }

        #endregion RegisterChangeCallback.

    }

    #endregion Detail.

    #region Exists.

    public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return cache.GetOrCreateAsync
        (
            $"Player_Exists:{name}",
            async entry =>
            {
                bool exists = await innerStore.ExistsAsync(name, cancellationToken).ConfigureAwait(false);
                entry.AddExpirationToken(new ExistsChangeToken(playerEvents, name));
                return exists;
            }
        );
    }

    private sealed class ExistsChangeToken(
        IPlayerStoreEvents events,
        string playerName
    ) : IChangeToken
    {
        public bool ActiveChangeCallbacks => true;

        public bool HasChanged { get; private set; }

        #region RegisterChangeCallback.

        public IDisposable RegisterChangeCallback(Action<object?> callback, object? state)
        {
            return new ExistsChangeTokenRegistration
            (
                state =>
                {
                    HasChanged = true;
                    callback.Invoke(state);
                },
                events,
                playerName,
                state
            );
        }

        private sealed class ExistsChangeTokenRegistration : IDisposable
        {
            private readonly Action<object?> _callback;
            private readonly IPlayerStoreEvents _events;
            private readonly string _playerName;
            private readonly object? _state;

            internal ExistsChangeTokenRegistration(
                Action<object?> callback,
                IPlayerStoreEvents events,
                string playerName,
                object? state
            )
            {
                _callback = callback;
                _events = events;
                _playerName = playerName;
                _state = state;

                _events.Deleted += OnDeleted;
                _events.Created += OnInserted;
            }

            private void OnDeleted(object? sender, IPlayerStoreEvents.DeletedEventArgs e)
            {
                _callback(_state);
            }

            private void OnInserted(object? sender, IPlayerStoreEvents.CreatedEventArgs e)
            {
                if (e.Player.Name == _playerName)
                    _callback(_state);
            }

            #region IDisposable.

            private bool _disposed;

            public void Dispose()
            {
                if (!_disposed)
                {
                    _events.Deleted -= OnDeleted;
                    _events.Created -= OnInserted;
                    _disposed = true;
                }
            }

            #endregion IDisposable.

        }

        #endregion RegisterChangeCallback.

    }

    #endregion Exists.


    #region Identify.

    public Task<Ulid?> IdentifyAsync(string name, CancellationToken cancellationToken = default)
    {
        return cache.GetOrCreateAsync
        (
            $"Player_Identify:{name}",
            async entry =>
            {
                Ulid? id = await innerStore.IdentifyAsync(name, cancellationToken).ConfigureAwait(false);
                entry.AddExpirationToken(new IdentifyChangeToken(playerEvents, id, name));
                return id;
            }
        );
    }

    private sealed class IdentifyChangeToken(
        IPlayerStoreEvents events,
        Ulid? playerId,
        string playerName
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
                playerId,
                playerName,
                state
            );
        }

        private sealed class IdentifyChangeTokenRegistration : IDisposable
        {
            private readonly Action<object?> _callback;
            private readonly IPlayerStoreEvents _events;
            private readonly string _name;
            private readonly Ulid? _id;
            private readonly object? _state;

            internal IdentifyChangeTokenRegistration(
                Action<object?> callback,
                IPlayerStoreEvents events,
                Ulid? id,
                string name,
                object? state
            )
            {
                _callback = callback;
                _events = events;
                _name = name;
                _id = id;
                _state = state;

                _events.Deleted += OnDeleted;
                _events.Created += OnCreated;
            }

            private void OnDeleted(object? sender, IPlayerStoreEvents.DeletedEventArgs e)
            {
                if (e.Id is null || e.Id == _id)
                    _callback(_state);
            }

            private void OnCreated(object? sender, IPlayerStoreEvents.CreatedEventArgs e)
            {
                if (e.Player.Name == _name)
                    _callback(_state);
            }

            #region IDisposable.

            private bool _disposed;

            public void Dispose()
            {
                if (!_disposed)
                {
                    _events.Deleted -= OnDeleted;
                    _events.Created -= OnCreated;
                    _disposed = true;
                }
            }

            #endregion IDisposable.

        }

        #endregion RegisterChangeCallback.

    }

    #endregion Identify.

    #region Index.

    public Task<ImmutableList<Player>> IndexAsync(CancellationToken cancellationToken = default)
    {
        return cache.GetOrCreateAsync
        (
            "Player.Index",
            async entry =>
            {
                ImmutableList<Player> players = await innerStore.IndexAsync(cancellationToken).ConfigureAwait(false);
                entry.AddExpirationToken(new IndexChangeToken(playerEvents, timeEvents));
                return players;
            }
        )!;
    }

    private sealed class IndexChangeToken(
        IPlayerStoreEvents playerEvents,
        ITimeStoreEvents timeEvents
    ) : IChangeToken
    {
        public bool ActiveChangeCallbacks => true;

        public bool HasChanged { get; private set; }

        public IDisposable RegisterChangeCallback(Action<object?> callback, object? state)
        {
            return new IndexChangeTokenRegistration
            (
                state =>
                {
                    HasChanged = true;
                    callback.Invoke(state);
                },
                playerEvents,
                state,
                timeEvents
            );
        }

        private sealed class IndexChangeTokenRegistration : IDisposable
        {
            private readonly Action<object?> _callback;
            private readonly IPlayerStoreEvents _playerEvents;
            private readonly object? _state;
            private readonly ITimeStoreEvents _timeEvents;

            internal IndexChangeTokenRegistration(
                Action<object?> callback,
                IPlayerStoreEvents playerEvents,
                object? state,
                ITimeStoreEvents timeEvents
            )
            {
                _callback = callback;
                _playerEvents = playerEvents;
                _state = state;
                _timeEvents = timeEvents;

                _playerEvents.Deleted += OnPlayerDeleted;
                _playerEvents.Created += OnPlayerCreated;
                _timeEvents.Created += OnTimeCreated;
            }

            private void OnPlayerDeleted(object? sender, IPlayerStoreEvents.DeletedEventArgs e)
            {
                _callback.Invoke(_state);
            }

            private void OnPlayerCreated(object? sender, IPlayerStoreEvents.CreatedEventArgs e)
            {
                _callback.Invoke(_state);
            }

            private void OnTimeCreated(object? sender, ITimeStoreEvents.CreatedEventArgs e)
            {
                _callback.Invoke(_state);
            }

            #region IDisposable.

            private bool _disposed;

            public void Dispose()
            {
                if (!_disposed)
                {
                    _playerEvents.Deleted -= OnPlayerDeleted;
                    _playerEvents.Created -= OnPlayerCreated;
                    _timeEvents.Created -= OnTimeCreated;

                    _disposed = true;
                }
            }

            #endregion IDisposable.

        }
    }

    #endregion Index.

}
