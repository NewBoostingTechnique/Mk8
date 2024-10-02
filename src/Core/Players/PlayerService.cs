using System.Collections.Immutable;
using System.Net.Http.Json;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using Mk8.Core.Countries;
using Mk8.Core.Migrations;
using Mk8.Core.Persons;
using Mk8.Core.Regions;

namespace Mk8.Core.Players;

internal class PlayerService(
    ICountryData countryData,
    IHttpClientFactory httpClientFactory,
    IMigrationStore migrationData,
    IOptionsMonitor<Mk8Settings> options,
    IPersonStore personData,
    IPlayerStore playerStore,
    IRegionData regionData
) : IPlayerService
{
    public async Task<Player> CreateAsync(Player player, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(player.Name);

        if (await ExistsAsync(player.Name, cancellationToken).ConfigureAwait(false))
            throw new InvalidOperationException($"Player with name '{player.Name}' already exists.");

        ArgumentException.ThrowIfNullOrWhiteSpace(player.CountryName);
        Ulid? countryId = await countryData.IdentifyAsync(player.CountryName, cancellationToken).ConfigureAwait(false);
        if (countryId is null)
        {
            Country country = new()
            {
                Id = Ulid.NewUlid(),
                Name = player.CountryName
            };
            await countryData.CreateAsync(country, cancellationToken).ConfigureAwait(false);
            countryId = country.Id;
        }

        Ulid? regionId = null;
        if (!string.IsNullOrWhiteSpace(player.RegionName))
        {
            regionId = await regionData.IdentifyAsync(player.RegionName, cancellationToken).ConfigureAwait(false);
            if (regionId is null)
            {
                Region region = new()
                {
                    CountryId = countryId.Value,
                    Id = Ulid.NewUlid(),
                    Name = player.RegionName
                };
                await regionData.CreateAsync(region, cancellationToken).ConfigureAwait(false);
                regionId = region.Id;
            }
        }

        Ulid? personId = await personData.IdentifyAsync(player.Name, cancellationToken).ConfigureAwait(false);
        if (personId is null)
        {
            personId = Ulid.NewUlid();
            player = getPlayer();
            await personData.CreateAsync(player, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            player = getPlayer();
        }

        await playerStore.CreateAsync(player, cancellationToken).ConfigureAwait(false);

        Player getPlayer() => player with
        {
            Id = personId,
            CountryId = countryId,
            RegionId = regionId
        };

        return player;
    }

    public async Task DeleteAsync(string playerName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(playerName);

        Ulid? id = await playerStore.IdentifyAsync(playerName, cancellationToken).ConfigureAwait(false);

        if (id.HasValue)
            await playerStore.DeleteAsync(id.Value, cancellationToken).ConfigureAwait(false);
    }

    public Task<bool> ExistsAsync(string playerName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(playerName);

        return playerStore.ExistsAsync(playerName, cancellationToken);
    }

    public async Task<Player?> FindAsync(string playerName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(playerName);

        Ulid? id = await playerStore.IdentifyAsync(playerName, cancellationToken).ConfigureAwait(false);

        return id.HasValue
            ? await playerStore.FindAsync(id.Value, cancellationToken).ConfigureAwait(false)
            : default;
    }

    #region Migrate.

    public async Task<Migration> MigrateAsync(CancellationToken cancellationToken = default)
    {
        Migration migration = await migrationData.StartAsync("Players Migration", cancellationToken).ConfigureAwait(false);

        _ = Task.Run
        (
            async () =>
            {
                string? error = null;

                try
                {
                    await playerStore.DeleteAsync(cancellationToken).ConfigureAwait(false);

                    using var client = httpClientFactory.CreateClient();

                    HtmlWeb htmlWeb = new();
                    client.BaseAddress = new Uri($"{options.CurrentValue.ImportFromBaseUrl}api/users");

                    int page = 1;
                    double processed = 0;

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
                            if (!await playerStore.ExistsAsync(data.UserName, cancellationToken).ConfigureAwait(false))
                            {
                                HtmlDocument document = await htmlWeb.LoadFromWebAsync($"{options.CurrentValue.ImportFromBaseUrl}users/{data.Url_Name}", cancellationToken).ConfigureAwait(false);

                                await CreateAsync
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
                                    },
                                    cancellationToken
                                )
                                .ConfigureAwait(false);
                            }

                            byte progress = (byte)Math.Floor(Math.Min(++processed, response.Total) * 100 / response.Total);
                            if (progress > migration.Progress)
                            {
                                migration = migration with
                                {
                                    Progress = progress
                                };
                                await migrationData.UpdateAsync(migration, CancellationToken.None).ConfigureAwait(false);
                            }
                        }

                        page++;
                    }
                }
                catch (Exception ex)
                {
                    error = ex.ToString();
                }

                await migrationData.UpdateAsync
                (
                    migration with
                    {
                        EndTime = DateTime.UtcNow,
                        Error = error
                    },
                    CancellationToken.None
                )
                .ConfigureAwait(false);
            },
            cancellationToken
        );

        return migration;
    }

    private sealed class GetUsersResponse
    {
        public required List<GetUserResponseItem> Data { get; init; } = [];

        public required int Total { get; init; }
    }

    internal sealed class GetUserResponseItem
    {
        public required string Country_Name { get; init; }

        public required string Url_Name { get; init; }

        public required string UserName { get; init; }
    }

    #endregion Import.

    public Task<IImmutableList<Player>> IndexAsync(CancellationToken cancellationToken = default)
    {
        return playerStore.IndexAsync(cancellationToken);
    }
}
