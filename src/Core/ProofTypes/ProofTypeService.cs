using System.Collections.Immutable;
using System.Transactions;
using Microsoft.Extensions.Logging;

namespace Mk8.Core.ProofTypes;

internal class ProofTypeService(
    ILogger<ProofTypeService> logger,
    IProofTypeData proofTypeData
) : IProofTypeService
{
    public Task<bool> ExistsAsync(string proofTypeDescription)
    {
        return proofTypeData.ExistsAsync(proofTypeDescription);
    }

    public Task<IImmutableList<ProofType>> ListAsync()
    {
        return proofTypeData.ListAsync();
    }

    public async Task SeedAsync()
    {
        logger.LogInformation("Seeding Proof Types...");

        using TransactionScope transaction = new(TransactionScopeAsyncFlowOption.Enabled);

        await insertAsync("No Proof").ConfigureAwait(false);
        await insertAsync("Ghost").ConfigureAwait(false);
        await insertAsync("Video").ConfigureAwait(false);
        await insertAsync("Screenshot").ConfigureAwait(false);
        await insertAsync("Time Scroll").ConfigureAwait(false);

        transaction.Complete();

        Task insertAsync(string description)
        {
            return proofTypeData.InsertAsync(new ProofType
            {
                Id = Ulid.NewUlid(),
                Description = description
            });
        }
    }
}
