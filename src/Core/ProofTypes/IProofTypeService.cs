using System.Collections.Immutable;

namespace Mk8.Core.ProofTypes;

public interface IProofTypeService
{
    Task<bool> ExistsAsync(string proofTypeDescription);

    Task<IImmutableList<ProofType>> ListAsync();

    Task SeedAsync();
}
