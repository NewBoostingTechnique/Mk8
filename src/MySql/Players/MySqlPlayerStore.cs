using Microsoft.Extensions.Options;
using Mk8.Core;
using Mk8.Core.Players;
using Mk8.Core.Times;
using Mk8.Data.Common;
using MySql.Data.MySqlClient;
using System.Collections.Immutable;
using System.Data;
using System.Data.Common;

namespace Mk8.MySql.Players;

internal class MySqlPlayerStore(IOptions<Mk8Settings> mk8Options) : IPlayerStore
{
    public async Task CreateAsync(Player player, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("player_create", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", player.Id);
        command.AddParameter("CountryId", player.CountryId);
        command.AddParameter("RegionId", player.RegionId);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    #region Delete.

    public Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        return deleteAsync(null, cancellationToken);
    }

    public Task DeleteAsync(Ulid id, CancellationToken cancellationToken = default)
    {
        return deleteAsync(id, cancellationToken);
    }

    private async Task deleteAsync(Ulid? id, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("player_delete", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", id);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    #endregion Delete.

    public async Task<Player?> DetailAsync(Ulid id, CancellationToken cancellationToken = default)
    {
        using var connection = new MySqlConnection(mk8Options.Value.ConnectionString);

        using var command = new MySqlCommand("player_detail", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", id);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            throw new KeyNotFoundException();

        Player player = new()
        {
            Id = id,
            Name = reader.GetString("Name"),
            Active = reader.GetDateOnlyNullable("Active"),
            CountryName = reader.GetString("CountryName"),
            RegionName = reader.GetString("RegionName")
        };

        ImmutableList<Time>.Builder builder = ImmutableList.CreateBuilder<Time>();
        if (await reader.NextResultAsync(cancellationToken).ConfigureAwait(false))
        {
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                Time time = new()
                {
                    CourseName = reader.GetString("CourseName"),
                    Span = reader.GetTimeSpanNullable("Span")
                };
                builder.Add(time);
            }
        }

        return player with
        {
            Times = builder.ToImmutable()
        };
    }

    public async Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("player_exists", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Name", name);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        return await command.ExecuteBoolAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<Ulid?> IdentifyAsync(string name, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("player_identify", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Name", name);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        return await command.ExecuteUlidAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IImmutableList<Player>> IndexAsync(CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("player_index", connection);
        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        ImmutableList<Player>.Builder builder = ImmutableList.CreateBuilder<Player>();
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            builder.Add(new Player
            {
                Active = reader.GetDateOnlyNullable("Active"),
                CountryName = reader.GetString("CountryName"),
                Name = reader.GetString("Name"),
                RegionName = reader.GetString("RegionName")
            });
        }

        return builder.ToImmutable();
    }
}
