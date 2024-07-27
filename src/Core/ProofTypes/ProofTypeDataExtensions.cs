namespace Mk8.Core.ProofTypes;

internal static class ProofTypeDataExtensions
{
    internal static async Task<Ulid> IdentifyRequiredAsync(this IProofTypeData proofTypeData, string proofTypeName)
    {
        return await proofTypeData.IdentifyAsync(proofTypeName).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"ProofType '{proofTypeName}' not found.");
    }
}
