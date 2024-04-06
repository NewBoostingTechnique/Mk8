using System.Collections.Immutable;

namespace Mk8.Core.ProofTypes;

internal interface IProofTypeData
{
    Task<bool> ExistsAsync(string proofTypeDescription);

    Task<string?> IdentifyAsync(string proofTypeDescription);

    Task<IImmutableList<ProofType>> ListAsync();
}
