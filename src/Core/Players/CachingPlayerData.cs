using System.Collections.Immutable;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Mk8.Core.Times;

namespace Mk8.Core.Players;

internal class CachingPlayerData(
    IMemoryCache cache,
    IPlayerData innerData,
    IPlayerDataEvents playerEvents,
    ITimeDataEvents timeEvents
) : IPlayerData
{
    public Task DeleteAsync(Ulid id)
    {
        return innerData.DeleteAsync(id);
    }

    #region DetailAsync.

    public Task<Player?> DetailAsync(Ulid id)
    {
        return cache.GetOrCreateAsync
        (
            $"Player_Detail:{id}",
            async entry =>
            {
                Player? player = await innerData.DetailAsync(id).ConfigureAwait(false);
                entry.AddExpirationToken(new DetailChangeToken(playerEvents, player, id, timeEvents));
                return player;
            }
        );
    }

    private sealed class DetailChangeToken(
        IPlayerDataEvents playerEvents,
        Player? player,
        Ulid playerId,
        ITimeDataEvents timeEvents
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
            private readonly IPlayerDataEvents _playerEvents;
            private readonly Player? _player;
            private readonly Ulid? _playerId;
            private readonly object? _state;
            private readonly ITimeDataEvents _timeEvents;

            internal DetailChangeTokenRegistration(
                Action<object?> callback,
                IPlayerDataEvents playerEvents,
                Player? player,
                Ulid playerId,
                object? state,
                ITimeDataEvents timeEvents
            )
            {
                _callback = callback;
                _playerEvents = playerEvents;
                _player = player;
                _playerId = playerId;
                _state = state;
                _timeEvents = timeEvents;

                _playerEvents.Deleted += OnPlayerDeleted;
                _playerEvents.Inserted += OnPlayerInserted;
                _timeEvents.Inserted += OnTimeInserted;
            }

            private void OnPlayerDeleted(object? sender, IPlayerDataEvents.DeletedEventArgs e)
            {
                if (e.Id == _playerId)
                    _callback(_state);
            }

            private void OnPlayerInserted(object? sender, IPlayerDataEvents.InsertedEventArgs e)
            {
                if (e.Player.Id == _playerId)
                    _callback(_state);
            }

            private void OnTimeInserted(object? sender, ITimeDataEvents.InsertedEventArgs e)
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
                    _playerEvents.Inserted -= OnPlayerInserted;
                    _timeEvents.Inserted -= OnTimeInserted;
                    _disposed = true;
                }
            }

            #endregion IDisposable.

        }

        #endregion RegisterChangeCallback.

    }


    #endregion DetailAsync.

    #region ExistsAsync.

    public Task<bool> ExistsAsync(string name)
    {
        return cache.GetOrCreateAsync
        (
            $"Player_Exists:{name}",
            async entry =>
            {
                bool exists = await innerData.ExistsAsync(name).ConfigureAwait(false);
                entry.AddExpirationToken(new ExistsChangeToken(playerEvents, name));
                return exists;
            }
        );
    }

    private sealed class ExistsChangeToken(
        IPlayerDataEvents events,
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
            private readonly IPlayerDataEvents _events;
            private readonly string _playerName;
            private readonly object? _state;

            internal ExistsChangeTokenRegistration(
                Action<object?> callback,
                IPlayerDataEvents events,
                string playerName,
                object? state
            )
            {
                _callback = callback;
                _events = events;
                _playerName = playerName;
                _state = state;

                _events.Deleted += OnDeleted;
                _events.Inserted += OnInserted;
            }

            private void OnDeleted(object? sender, IPlayerDataEvents.DeletedEventArgs e)
            {
                _callback(_state);
            }

            private void OnInserted(object? sender, IPlayerDataEvents.InsertedEventArgs e)
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
                    _events.Inserted -= OnInserted;
                    _disposed = true;
                }
            }

            #endregion IDisposable.

        }

        #endregion RegisterChangeCallback.

    }

    #endregion ExistsAsync.

    #region IdentifyAsync.

    public Task<Ulid?> IdentifyAsync(string name)
    {
        return cache.GetOrCreateAsync
        (
            $"Player_Identify:{name}",
            async entry =>
            {
                Ulid? id = await innerData.IdentifyAsync(name).ConfigureAwait(false);
                entry.AddExpirationToken(new IdentifyChangeToken(playerEvents, id, name));
                return id;
            }
        );
    }

    private sealed class IdentifyChangeToken(
        IPlayerDataEvents events,
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
            private readonly IPlayerDataEvents _events;
            private readonly string _playerName;
            private readonly Ulid? _playerId;
            private readonly object? _state;

            internal IdentifyChangeTokenRegistration(
                Action<object?> callback,
                IPlayerDataEvents events,
                Ulid? playerId,
                string playerName,
                object? state
            )
            {
                _callback = callback;
                _events = events;
                _playerName = playerName;
                _playerId = playerId;
                _state = state;

                _events.Deleted += OnDeleted;
                _events.Inserted += OnInserted;
            }

            private void OnDeleted(object? sender, IPlayerDataEvents.DeletedEventArgs e)
            {
                if (e.Id == _playerId)
                    _callback(_state);
            }

            private void OnInserted(object? sender, IPlayerDataEvents.InsertedEventArgs e)
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
                    _events.Inserted -= OnInserted;
                    _disposed = true;
                }
            }

            #endregion IDisposable.

        }

        #endregion RegisterChangeCallback.

    }

    #endregion IdentifyAsync.

    public Task InsertAsync(Player player)
    {
        return innerData.InsertAsync(player);
    }

    #region ListAsync.

    public Task<IImmutableList<Player>> ListAsync()
    {
        return cache.GetOrCreateAsync
        (
            "Player_List",
            async entry =>
            {
                IImmutableList<Player> players = await innerData.ListAsync().ConfigureAwait(false);
                entry.AddExpirationToken(new ListChangeToken(playerEvents, timeEvents));
                return players;
            }
        )!;
    }

    private sealed class ListChangeToken(
        IPlayerDataEvents playerEvents,
        ITimeDataEvents timeEvents
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
                playerEvents,
                state,
                timeEvents
            );
        }

        private sealed class ListChangeTokenRegistration : IDisposable
        {
            private readonly Action<object?> _callback;
            private readonly IPlayerDataEvents _playerEvents;
            private readonly object? _state;
            private readonly ITimeDataEvents _timeEvents;

            internal ListChangeTokenRegistration(
                Action<object?> callback,
                IPlayerDataEvents playerEvents,
                object? state,
                ITimeDataEvents timeEvents
            )
            {
                _callback = callback;
                _playerEvents = playerEvents;
                _state = state;
                _timeEvents = timeEvents;

                _playerEvents.Deleted += OnPlayerDeleted;
                _playerEvents.Inserted += OnPlayerInserted;
                _timeEvents.Inserted += OnTimeInserted;
            }

            private void OnPlayerDeleted(object? sender, IPlayerDataEvents.DeletedEventArgs e)
            {
                _callback.Invoke(_state);
            }

            private void OnPlayerInserted(object? sender, IPlayerDataEvents.InsertedEventArgs e)
            {
                _callback.Invoke(_state);
            }

            private void OnTimeInserted(object? sender, ITimeDataEvents.InsertedEventArgs e)
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
                    _playerEvents.Inserted -= OnPlayerInserted;
                    _timeEvents.Inserted -= OnTimeInserted;

                    _disposed = true;
                }
            }

            #endregion IDisposable.

        }
    }

    #endregion ListAsync.

}
