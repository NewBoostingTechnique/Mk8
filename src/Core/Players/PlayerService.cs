using System.Collections.Immutable;
using Mk8.Core.Locations.Countries;
using Mk8.Core.Locations.Regions;
using Mk8.Core.Persons;
using Mk8.Core.ProofTypes;

namespace Mk8.Core.Players;

internal class PlayerService(
    ICountryData countryData,
    IProofTypeData proofTypeData,
    IPersonData personData,
    IPlayerData playerData,
    IRegionData regionData
) : IPlayerService
{

    #region IPlayerService.

    public async Task DeleteAsync(string playerName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(playerName);

        Ulid? id = await playerData.IdentifyAsync(playerName).ConfigureAwait(false);

        if (id.HasValue)
            await playerData.DeleteAsync(id.Value).ConfigureAwait(false);
    }

    public Task<bool> ExistsAsync(string playerName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(playerName);

        return playerData.ExistsAsync(playerName);
    }

    public async Task<Player?> FindAsync(string playerName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(playerName);

        Ulid? id = await playerData.IdentifyAsync(playerName).ConfigureAwait(false);

        return id.HasValue
            ? await playerData.DetailAsync(id.Value).ConfigureAwait(false)
            : default;
    }

    public async Task<Player> InsertAsync(Player player)
    {
        ArgumentNullException.ThrowIfNull(player);
        ArgumentException.ThrowIfNullOrWhiteSpace(player.CountryName);
        ArgumentException.ThrowIfNullOrWhiteSpace(player.Name);
        ArgumentException.ThrowIfNullOrWhiteSpace(player.ProofTypeDescription);
        ArgumentException.ThrowIfNullOrWhiteSpace(player.RegionName);

        if (await ExistsAsync(player.Name).ConfigureAwait(false))
            throw new InvalidOperationException($"Player with name '{player.Name}' already exists.");

        player.Id = await personData.IdentifyAsync(player.Name).ConfigureAwait(false);
        if (player.Id is null)
        {
            player.Id = Ulid.NewUlid();
            await personData.InsertAsync(player).ConfigureAwait(false);
        }

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
