using System.Collections.Immutable;

namespace Mk8.Core.ProofTypes;

public interface IProofTypeData
{
    Task<bool> ExistsAsync(string description);

    Task<Ulid?> IdentifyAsync(string description);

    Task InsertAsync(ProofType proofType);

    Task<IImmutableList<ProofType>> ListAsync();
}
