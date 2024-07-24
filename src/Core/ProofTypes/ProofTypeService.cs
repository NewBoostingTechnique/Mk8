using System.Collections.Immutable;

namespace Mk8.Core.ProofTypes;

internal class ProofTypeService(IProofTypeData proofTypeData) : IProofTypeService
{
    public Task<bool> ExistsAsync(string proofTypeDescription)
    {
        return proofTypeData.ExistsAsync(proofTypeDescription);
    }

    public Task<IImmutableList<ProofType>> ListAsync()
    {
        return proofTypeData.ListAsync();
    }
}