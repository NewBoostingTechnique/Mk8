using System.Collections.Immutable;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Mk8.Core.Courses;

internal class CachingCourseData(
    IMemoryCache cache,
    ICourseStore innerStore,
    ICourseDataEvents courseEvents
) : ICourseStore
{

    #region ExistsAsync.

    public Task<bool> ExistsAsync(string courseName, CancellationToken cancellationToken = default)
    {
        return cache.GetOrCreateAsync
        (
            $"Course_Exists:{courseName}",
            async entry =>
            {
                bool exists = await innerStore.ExistsAsync(courseName, cancellationToken).ConfigureAwait(false);
                entry.AddExpirationToken(new ExistsChangeToken(courseEvents, courseName));
                return exists;
            }
        );
    }

    private sealed class ExistsChangeToken(
        ICourseDataEvents events,
        string courseName
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
                courseName,
                events,
                state
            );
        }

        private sealed class ExistsChangeTokenRegistration : IDisposable
        {
            private readonly Action<object?> _callback;
            private readonly string _courseName;
            private readonly ICourseDataEvents _events;
            private readonly object? _state;

            internal ExistsChangeTokenRegistration(
                Action<object?> callback,
                string courseName,
                ICourseDataEvents events,
                object? state
            )
            {
                _callback = callback;
                _courseName = courseName;
                _events = events;
                _state = state;

                events.Created += OnInserted;
            }

            private void OnInserted(object? sender, ICourseDataEvents.CreatedEventArgs e)
            {
                if (e.Course.Name == _courseName)
                    _callback(_state);
            }

            #region IDisposable.

            private bool _disposed;

            public void Dispose()
            {
                if (!_disposed)
                {
                    _events.Created -= OnInserted;
                    _disposed = true;
                }
            }

            #endregion IDisposable.

        }

        #endregion RegisterChangeCallback.

    }

    #endregion ExistsAsync.

    #region Identify.

    public Task<Ulid?> IdentifyAsync(string courseName, CancellationToken cancellationToken = default)
    {
        return cache.GetOrCreateAsync
        (
            $"Course_Identify:{courseName}",
            async entry =>
            {
                Ulid? id = await innerStore.IdentifyAsync(courseName, cancellationToken).ConfigureAwait(false);
                entry.AddExpirationToken(new IdentifyChangeToken(courseEvents, courseName));
                return id;
            }
        );
    }

    private sealed class IdentifyChangeToken(
        ICourseDataEvents events,
        string courseName
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
                courseName,
                state
            );
        }

        private sealed class IdentifyChangeTokenRegistration : IDisposable
        {
            private readonly Action<object?> _callback;
            private readonly ICourseDataEvents _events;
            private readonly string _courseName;
            private readonly object? _state;

            internal IdentifyChangeTokenRegistration(
                Action<object?> callback,
                ICourseDataEvents events,
                string courseName,
                object? state
            )
            {
                _callback = callback;
                _events = events;
                _courseName = courseName;
                _state = state;

                _events.Created += OnCreated;
            }

            private void OnCreated(object? sender, ICourseDataEvents.CreatedEventArgs e)
            {
                if (e.Course.Name == _courseName)
                    _callback(_state);
            }

            #region IDisposable.

            private bool _disposed;

            public void Dispose()
            {
                if (!_disposed)
                {
                    _events.Created -= OnCreated;
                    _disposed = true;
                }
            }

            #endregion IDisposable.

        }

        #endregion RegisterChangeCallback.

    }

    #endregion IdentifyAsync.

    public Task CreateAsync(Course course, CancellationToken cancellationToken = default)
    {
        return innerStore.CreateAsync(course, cancellationToken);
    }

    #region Index.

    public Task<ImmutableList<Course>> IndexAsync(CancellationToken cancellationToken = default)
    {
        return cache.GetOrCreateAsync
        (
            "Course_Index",
            async entry =>
            {
                ImmutableList<Course> index = await innerStore.IndexAsync(cancellationToken).ConfigureAwait(false);
                entry.AddExpirationToken(new IndexChangeToken(courseEvents));
                return index;
            }
        )!;
    }

    private sealed class IndexChangeToken(
       ICourseDataEvents events
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
                events,
                state
            );
        }

        private sealed class IndexChangeTokenRegistration : IDisposable
        {
            private readonly Action<object?> _callback;
            private readonly ICourseDataEvents _events;
            private readonly object? _state;

            internal IndexChangeTokenRegistration(
                Action<object?> callback,
                ICourseDataEvents events,
                object? state
            )
            {
                _callback = callback;
                _events = events;
                _state = state;

                _events.Created += OnCreated;
            }

            private void OnCreated(object? sender, ICourseDataEvents.CreatedEventArgs e)
            {
                _callback.Invoke(_state);
            }

            #region IDisposable.

            private bool _disposed;

            public void Dispose()
            {
                if (!_disposed)
                {
                    _events.Created -= OnCreated;

                    _disposed = true;
                }
            }

            #endregion IDisposable.

        }
    }


    #endregion ListAsync.

}
