using System.Collections.Immutable;
using System.Net.Http.Json;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using Mk8.Core.Locations.Countries;
using Mk8.Core.Locations.Regions;
using Mk8.Core.Persons;

namespace Mk8.Core.Players;

internal class PlayerService(
    ICountryData countryData,
    IHttpClientFactory httpClientFactory,
    IOptionsMonitor<Mk8Settings> options,
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

    #region Import.

    public async Task ImportAsync(CancellationToken cancellationToken = default)
    {
        using var client = httpClientFactory.CreateClient();

        HtmlWeb htmlWeb = new();
        client.BaseAddress = new Uri($"{options.CurrentValue.ImportFromBaseUrl}api/users");

        int page = 1;

        while (true)
        {
            GetUsersResponse? response = await client.GetFromJsonAsync<GetUsersResponse>
            (
                $"?page={page}",
                cancellationToken
            )
            .ConfigureAwait(false);

            if (response?.Data is null || response.Data.Count == 0)
                return;

            foreach (GetUserResponseItem data in response.Data)
            {
                if (await playerData.ExistsAsync(data.UserName).ConfigureAwait(false))
                    continue;

                HtmlDocument document = await htmlWeb.LoadFromWebAsync($"{options.CurrentValue.ImportFromBaseUrl}users/{data.Url_Name}", cancellationToken).ConfigureAwait(false);

                await InsertAsync
                (
                    new()
                    {
                        Name = data.UserName,
                        CountryName = data.Country_Name,
                        RegionName = string.Join
                        (
                            ",",
                            document.DocumentNode.SelectSingleNode("(//div[@class='info_box user_info'])[1]/table/tr[3]/td[2]").InnerText.Split(",")[..^1]
                        )
                        .Trim()
                    }
                )
                .ConfigureAwait(false);
            }

            page++;
        }
    }

    private sealed class GetUsersResponse
    {
        public required List<GetUserResponseItem> Data { get; init; } = [];
    }

    internal sealed class GetUserResponseItem
    {
        public required string Country_Name { get; init; }

        public required string Url_Name { get; init; }

        public required string UserName { get; init; }
    }

    #endregion Import.

    public async Task<Player> InsertAsync(Player player)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(player.Name);

        if (await ExistsAsync(player.Name).ConfigureAwait(false))
            throw new InvalidOperationException($"Player with name '{player.Name}' already exists.");

        ArgumentException.ThrowIfNullOrWhiteSpace(player.CountryName);
        player.CountryId = await countryData.IdentifyAsync(player.CountryName).ConfigureAwait(false);
        if (player.CountryId is null)
        {
            Country country = new()
            {
                Id = Ulid.NewUlid(),
                Name = player.CountryName
            };
            await countryData.InsertAsync(country).ConfigureAwait(false);
            player.CountryId = country.Id;
        }

        if (!string.IsNullOrWhiteSpace(player.RegionName))
        {
            player.RegionId = await regionData.IdentifyAsync(player.RegionName).ConfigureAwait(false);
            if (player.RegionId is null)
            {
                Region region = new()
                {
                    CountryId = player.CountryId.Value,
                    Id = Ulid.NewUlid(),
                    Name = player.RegionName
                };
                await regionData.InsertAsync(region).ConfigureAwait(false);
                player.RegionId = region.Id;
            }
        }

        player.Id = await personData.IdentifyAsync(player.Name).ConfigureAwait(false);
        if (player.Id is null)
        {
            player.Id = Ulid.NewUlid();
            await personData.InsertAsync(player).ConfigureAwait(false);
        }

        await playerData.InsertAsync(player).ConfigureAwait(false);

        return player;
    }

    public Task<IImmutableList<Player>> ListAsync()
    {
        return playerData.ListAsync();
    }

    #endregion IPlayerService.

}
