using System.Collections.Immutable;
using Mk8.Core.Locations.Countries;
using Mk8.Core.Locations.Regions;
using Mk8.Core.ProofTypes;

namespace Mk8.Core.Players;

internal class PlayerService(
    ICountryData countryData,
    IProofTypeData proofTypeData,
    IPlayerData playerData,
    IRegionData regionData
) : IPlayerService
{

    #region IPlayerService.

    public async Task DeleteAsync(string playerName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(playerName);

        string? id = await playerData.IdentifyAsync(playerName).ConfigureAwait(false);

        if (id is not null)
            await playerData.DeleteAsync(id).ConfigureAwait(false);
    }

    public Task<bool> ExistsAsync(string playerName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(playerName);

        return playerData.ExistsAsync(playerName);
    }

    public async Task<Player?> FindAsync(string playerName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(playerName);

        string? id = await playerData.IdentifyAsync(playerName).ConfigureAwait(false);

        return id is null
            ? default
            : await playerData.DetailAsync(id).ConfigureAwait(false);
    }

    public async Task<Player> InsertAsync(Player player)
    {
        ArgumentNullException.ThrowIfNull(player);
        ArgumentException.ThrowIfNullOrWhiteSpace(player.Name);
        ArgumentException.ThrowIfNullOrWhiteSpace(player.CountryName);
        ArgumentException.ThrowIfNullOrWhiteSpace(player.ProofTypeDescription);
        ArgumentException.ThrowIfNullOrWhiteSpace(player.RegionName);

        if (await ExistsAsync(player.Name))
            throw new InvalidOperationException($"Player with name '{player.Name}' already exists.");

        player.Id = Guid.NewGuid().ToString("N");
        player.CountryId = await countryData.IdentifyRequiredAsync(player.CountryName).ConfigureAwait(false);
        player.ProofTypeId = await proofTypeData.IdentifyRequiredAsync(player.ProofTypeDescription).ConfigureAwait(false);
        player.RegionId = await regionData.IdentifyRequiredAsync(player.RegionName).ConfigureAwait(false);

        await playerData.InsertAsync(player).ConfigureAwait(false);

        return player;
    }

    public Task<IImmutableList<Player>> ListAsync()
    {
        return playerData.ListAsync();
    }

    #endregion IPlayerService.

}
