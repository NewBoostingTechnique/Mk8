namespace Mk8.Core.Players;

internal static class PlayerDataExtensions
{
    internal static async Task<Ulid> IdentifyRequiredAsync(this IPlayerData playerData, string playerName)
    {
        return await playerData.IdentifyAsync(playerName).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"Player '{playerName}' not found.");
    }
}
