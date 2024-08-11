using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Mk8.Core.Logins;

internal class CachingLoginData(
    IMemoryCache cache,
    ILoginData innerData,
    ILoginDataEvents events
) : ILoginData
{

    #region Exists.

    public Task<bool> ExistsAsync(string email)
    {
        return cache.GetOrCreateAsync
        (
            $"Course_Exists:{email}",
            async entry =>
            {
                bool exists = await innerData.ExistsAsync(email).ConfigureAwait(false);
                entry.AddExpirationToken(new ExistsChangeToken(events, email));
                return exists;
            }
        );
    }

    private sealed class ExistsChangeToken(
        ILoginDataEvents events,
        string email
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
                email,
                state
            );
        }

        private sealed class ExistsChangeTokenRegistration : IDisposable
        {
            private readonly Action<object?> _callback;
            private readonly ILoginDataEvents _events;
            private readonly string _email;
            private readonly object? _state;

            internal ExistsChangeTokenRegistration(
                Action<object?> callback,
                ILoginDataEvents events,
                string email,
                object? state
            )
            {
                _callback = callback;
                _events = events;
                _email = email;
                _state = state;

                _events.Inserted += OnInserted;
            }

            private void OnInserted(object? sender, ILoginDataEvents.InsertedEventArgs e)
            {
                if (e.Login.Email == _email)
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

    #endregion Exists.

    public Task InsertAsync(Login login)
    {
        return innerData.InsertAsync(login);
    }
}
