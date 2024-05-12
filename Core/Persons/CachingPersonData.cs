using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Mk8.Core.Persons;

internal class CachingPersonData(
    IMemoryCache cache,
    IPersonData innerData,
    IPersonDataEvents personEvents
) : IPersonData
{

    #region IdentifyAsync.

    public Task<string?> IdentifyAsync(string personName)
    {
        return cache.GetOrCreateAsync
        (
            $"Person_Identify_{personName}",
            async entry =>
            {
                string? id = await innerData.IdentifyAsync(personName).ConfigureAwait(false);
                entry.AddExpirationToken(new IdentifyChangeToken(personEvents, personName));
                return id;
            }
        );
    }

    private sealed class IdentifyChangeToken(
        IPersonDataEvents events,
        string personName
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
                personName,
                state
            );
        }

        private sealed class IdentifyChangeTokenRegistration : IDisposable
        {
            private readonly Action<object?> _callback;
            private readonly IPersonDataEvents _events;
            private readonly string _personName;
            private readonly object? _state;

            public IdentifyChangeTokenRegistration(
                Action<object?> callback,
                IPersonDataEvents events,
                string personName,
                object? state
            )
            {
                _callback = callback;
                _events = events;
                _personName = personName;
                _state = state;

                _events.Inserted += OnInserted;
            }

            private void OnInserted(object? sender, IPersonDataEvents.InsertedEventArgs e)
            {
                if (e.Person.Name == _personName)
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

    public Task InsertAsync(Person person)
    {
        return innerData.InsertAsync(person);
    }
}