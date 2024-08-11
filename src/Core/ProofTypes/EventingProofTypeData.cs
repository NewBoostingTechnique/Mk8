using System.Collections.Immutable;
using static Mk8.Core.ProofTypes.IProofTypeDataEvents;

namespace Mk8.Core.ProofTypes;

internal class EventingProofTypeData(
    IProofTypeData innerData
) : IProofTypeData, IProofTypeDataEvents
{
    public Task<Ulid?> IdentifyAsync(string description)
    {
        return innerData.IdentifyAsync(description);
    }

    #region Insert.

    public event EventHandler<InsertedEventArgs>? Inserted;

    public async Task InsertAsync(ProofType proofType)
    {
        await innerData.InsertAsync(proofType).ConfigureAwait(false);
        Inserted?.Invoke(this, new InsertedEventArgs(proofType));
    }

    #endregion Insert.

    public Task<IImmutableList<ProofType>> ListAsync()
    {
        return innerData.ListAsync();
    }

    public Task<bool> ExistsAsync(string description)
    {
        return innerData.ExistsAsync(description);
    }
}
