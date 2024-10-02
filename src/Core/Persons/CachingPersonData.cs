using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Mk8.Core.Persons;

internal class CachingPersonData(
    IMemoryCache cache,
    IPersonStore innerData,
    IPersonDataEvents personEvents
) : IPersonStore
{

    #region IdentifyAsync.

    public Task<Ulid?> IdentifyAsync(string personName, CancellationToken cancellationToken = default)
    {
        return cache.GetOrCreateAsync
        (
            $"Person_Identify_{personName}",
            async entry =>
            {
                Ulid? id = await innerData.IdentifyAsync(personName, cancellationToken).ConfigureAwait(false);
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

    public Task CreateAsync(Person person, CancellationToken cancellationToken = default)
    {
        return innerData.CreateAsync(person, cancellationToken);
    }
}
