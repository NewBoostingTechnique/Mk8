using System.Collections.Immutable;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Mk8.Core.ProofTypes;

internal class CachingProofTypeData(
    IMemoryCache cache,
    IProofTypeData innerData,
    IProofTypeDataEvents events
) : IProofTypeData
{

    #region Exists.

    public Task<bool> ExistsAsync(string description)
    {
        return cache.GetOrCreateAsync
        (
            $"ProofType_Exists:{description}",
            async entry =>
            {
                bool exists = await innerData.ExistsAsync(description).ConfigureAwait(false);
                entry.AddExpirationToken(new ExistsChangeToken(events, description));
                return exists;
            }
        );
    }

    private sealed class ExistsChangeToken(
        IProofTypeDataEvents events,
        string description
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
                description,
                state
            );
        }

        private sealed class ExistsChangeTokenRegistration : IDisposable
        {
            private readonly Action<object?> _callback;
            private readonly IProofTypeDataEvents _events;
            private readonly string _description;
            private readonly object? _state;

            internal ExistsChangeTokenRegistration(
                Action<object?> callback,
                IProofTypeDataEvents events,
                string description,
                object? state
            )
            {
                _callback = callback;
                _events = events;
                _description = description;
                _state = state;

                _events.Inserted += OnInserted;
            }

            private void OnInserted(object? sender, IProofTypeDataEvents.InsertedEventArgs e)
            {
                if (e.ProofType.Description == _description)
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

    #region Identify.

    public Task<Ulid?> IdentifyAsync(string description)
    {
        return cache.GetOrCreateAsync
        (
             $"ProofType_Identify:{description}",
            async entry =>
            {
                Ulid? id = await innerData.IdentifyAsync(description).ConfigureAwait(false);
                entry.AddExpirationToken(new IdentifyChangeToken(events, id, description));
                return id;
            }
        );
    }

    private sealed class IdentifyChangeToken(
        IProofTypeDataEvents events,
        Ulid? id,
        string description
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
                description,
                state
            );
        }

        private sealed class IdentifyChangeTokenRegistration : IDisposable
        {
            private readonly Action<object?> _callback;
            private readonly IProofTypeDataEvents _events;
            private readonly string _description;
            private readonly Ulid? _id;
            private readonly object? _state;

            internal IdentifyChangeTokenRegistration(
                Action<object?> callback,
                IProofTypeDataEvents events,
                Ulid? id,
                string description,
                object? state
            )
            {
                _callback = callback;
                _events = events;
                _description = description;
                _id = id;
                _state = state;

                _events.Inserted += OnInserted;
            }

            private void OnInserted(object? sender, IProofTypeDataEvents.InsertedEventArgs e)
            {
                if (e.ProofType.Description == _description)
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

    public Task InsertAsync(ProofType proofType)
    {
        return innerData.InsertAsync(proofType);
    }

    #region List.

    public Task<IImmutableList<ProofType>> ListAsync()
    {
        return cache.GetOrCreateAsync
        (
            "ProofTypes_List",
            async entry =>
            {
                IImmutableList<ProofType> proofTypes = await innerData.ListAsync().ConfigureAwait(false);
                entry.AddExpirationToken(new ListChangeToken(events));
                return proofTypes;
            }
        )!;
    }

    private sealed class ListChangeToken(
        IProofTypeDataEvents events
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
            private readonly IProofTypeDataEvents _events;
            private readonly object? _state;

            internal ListChangeTokenRegistration(
                Action<object?> callback,
                IProofTypeDataEvents events,
                object? state
            )
            {
                _callback = callback;
                _events = events;
                _state = state;

                _events.Inserted += OnInserted;
            }

            private void OnInserted(object? sender, IProofTypeDataEvents.InsertedEventArgs e)
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

    #endregion List.

}
