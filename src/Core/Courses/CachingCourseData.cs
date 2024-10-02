using System.Collections.Immutable;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Mk8.Core.Courses;

internal class CachingCourseData(
    IMemoryCache cache,
    ICourseData innerData,
    ICourseDataEvents courseEvents
) : ICourseData
{

    #region ExistsAsync.

    public Task<bool> ExistsAsync(string courseName)
    {
        return cache.GetOrCreateAsync
        (
            $"Course_Exists:{courseName}",
            async entry =>
            {
                bool exists = await innerData.ExistsAsync(courseName).ConfigureAwait(false);
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

                events.Inserted += OnInserted;
            }

            private void OnInserted(object? sender, ICourseDataEvents.InsertedEventArgs e)
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

    public Task<Ulid?> IdentifyAsync(string courseName)
    {
        return cache.GetOrCreateAsync
        (
            $"Course_Identify:{courseName}",
            async entry =>
            {
                Ulid? id = await innerData.IdentifyAsync(courseName).ConfigureAwait(false);
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

                _events.Inserted += OnInserted;
            }

            private void OnInserted(object? sender, ICourseDataEvents.InsertedEventArgs e)
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
                    _events.Inserted -= OnInserted;
                    _disposed = true;
                }
            }

            #endregion IDisposable.

        }

        #endregion RegisterChangeCallback.

    }

    #endregion IdentifyAsync.

    public Task CreateAsync(Course course)
    {
        return innerData.CreateAsync(course);
    }

    #region ListAsync.

    public Task<IImmutableList<Course>> IndexAsync()
    {
        return cache.GetOrCreateAsync
        (
            "Course_List",
            async entry =>
            {
                IImmutableList<Course> list = await innerData.IndexAsync().ConfigureAwait(false);
                entry.AddExpirationToken(new ListChangeToken(courseEvents));
                return list;
            }
        )!;
    }

    private sealed class ListChangeToken(
       ICourseDataEvents events
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
            private readonly ICourseDataEvents _events;
            private readonly object? _state;

            internal ListChangeTokenRegistration(
                Action<object?> callback,
                ICourseDataEvents events,
                object? state
            )
            {
                _callback = callback;
                _events = events;
                _state = state;

                _events.Inserted += OnInserted;
            }

            private void OnInserted(object? sender, ICourseDataEvents.InsertedEventArgs e)
            {
                _callback.Invoke(_state);
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
    }


    #endregion ListAsync.

}
