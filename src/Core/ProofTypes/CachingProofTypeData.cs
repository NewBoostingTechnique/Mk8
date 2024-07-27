using System.Collections.Immutable;
using Microsoft.Extensions.Caching.Memory;

namespace Mk8.Core.ProofTypes;

internal class CachingProofTypeData(IMemoryCache cache, IProofTypeData innerData) : IProofTypeData
{
    public Task<bool> ExistsAsync(string proofTypeDescription)
    {
        return cache.GetOrCreateAsync
        (
            $"ProofType_Exists:{proofTypeDescription}",
            entry => innerData.ExistsAsync(proofTypeDescription)
        );
    }

    public Task<Ulid?> IdentifyAsync(string proofTypeDescription)
    {
        return cache.GetOrCreateAsync
        (
             $"ProofType_Identify:{proofTypeDescription}",
            entry => innerData.IdentifyAsync(proofTypeDescription)
        );
    }

    public Task<IImmutableList<ProofType>> ListAsync()
    {
        return cache.GetOrCreateAsync
        (
            "ProofTypes_List",
            entry => innerData.ListAsync()
        )!;
    }
}
