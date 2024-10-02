using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Mk8.Core.Times;

internal class CachingTimeStore(
    IMemoryCache cache,
    ITimeStore innerData,
    ITimeStoreEvents timeEvents
) : ITimeStore
{
    public Task<bool> ExistsAsync(Ulid courseId, Ulid playerId)
    {
        return cache.GetOrCreateAsync
        (
            $"Time_Exists:{courseId}:{playerId}",
            async entry =>
            {
                bool exists = await innerData.ExistsAsync(courseId, playerId).ConfigureAwait(false);
                entry.AddExpirationToken(new ExistsChangeToken(courseId, playerId, timeEvents));
                return exists;
            }
        );
    }

    private sealed class ExistsChangeToken(
        Ulid courseId,
        Ulid playerId,
        ITimeStoreEvents timeEvents
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
                courseId,
                playerId,
                state,
                timeEvents
            );
        }

        private sealed class ExistsChangeTokenRegistration : IDisposable
        {
            private readonly Action<object?> _callback;
            private readonly Ulid _courseId;
            private readonly Ulid _playerId;
            private readonly object? _state;
            private readonly ITimeStoreEvents _timeEvents;

            internal ExistsChangeTokenRegistration(
                Action<object?> callback,
                Ulid courseId,
                Ulid playerId,
                object? state,
                ITimeStoreEvents timeEvents
            )
            {
                _callback = callback;
                _courseId = courseId;
                _playerId = playerId;
                _state = state;
                _timeEvents = timeEvents;

                _timeEvents.Created += OnInserted;
            }

            private void OnInserted(object? sender, ITimeStoreEvents.CreatedEventArgs e)
            {
                if (e.Time.CourseId == _courseId && e.Time.PlayerId == _playerId)
                    _callback(_state);
            }

            #region IDisposable.

            private bool _disposed;

            public void Dispose()
            {
                if (!_disposed)
                {
                    _timeEvents.Created -= OnInserted;
                    _disposed = true;
                }
            }

            #endregion IDisposable.

        }

        #endregion RegisterChangeCallback.

    }

    public Task CreateAsync(Time time)
    {
        return innerData.CreateAsync(time);
    }
}
