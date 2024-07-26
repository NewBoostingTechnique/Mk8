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

internal class MySqlPlayerData(IOptions<Mk8Settings> mk8Options) : IPlayerData
{
    public async Task DeleteAsync(string id)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("PlayerDelete", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new MySqlParameter("Id", id));

        await connection.OpenAsync().ConfigureAwait(false);
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }

    public async Task<Player?> DetailAsync(string id)
    {
        using var connection = new MySqlConnection(mk8Options.Value.ConnectionString);

        using var command = new MySqlCommand("PlayerDetail", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new MySqlParameter("Id", id));

        await connection.OpenAsync().ConfigureAwait(false);
        using DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

        if (!await reader.ReadAsync().ConfigureAwait(false))
            return default;

        Player player = new()
        {
            Active = reader.GetDateOnlyNullable(0),
            Id = id,
            CountryName = reader.GetString(1),
            Name = reader.GetString(2),
            ProofTypeDescription = reader.GetString(3),
            RegionName = reader.GetString(4),
        };

        if (!await reader.NextResultAsync().ConfigureAwait(false))
            return player;

        ImmutableList<Time>.Builder builder = ImmutableList.CreateBuilder<Time>();
        while (await reader.ReadAsync().ConfigureAwait(false))
        {
            Time time = new()
            {
                CourseName = reader.GetString(0),
                Span = reader.GetTimeSpanNullable(1)
            };
            builder.Add(time);
        }
        player.Times = builder.ToImmutableList();

        return player;
    }

    public async Task<bool> ExistsAsync(string name)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("PlayerExists", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new MySqlParameter("Name", name));

        await connection.OpenAsync().ConfigureAwait(false);
        return await command.ExecuteBoolAsync().ConfigureAwait(false);
    }

    public async Task<string?> IdentifyAsync(string name)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("PlayerIdentify", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new MySqlParameter("Name", name));

        await connection.OpenAsync().ConfigureAwait(false);
        return (await command.ExecuteScalarAsync().ConfigureAwait(false)) as string;
    }

    public async Task InsertAsync(Player player)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("PlayerInsert", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new MySqlParameter("CountryId", player.CountryId));
        command.Parameters.Add(new MySqlParameter("Id", player.Id));
        command.Parameters.Add(new MySqlParameter("ProofTypeId", player.ProofTypeId));
        command.Parameters.Add(new MySqlParameter("RegionId", player.RegionId));

        await connection.OpenAsync().ConfigureAwait(false);
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }

    public async Task<IImmutableList<Player>> ListAsync()
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("PlayerList", connection);
        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync().ConfigureAwait(false);
        using DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

        ImmutableList<Player>.Builder builder = ImmutableList.CreateBuilder<Player>();
        while (await reader.ReadAsync().ConfigureAwait(false))
        {
            builder.Add(new Player
            {
                Active = reader.GetDateOnlyNullable(0),
                CountryName = reader.GetString(1),
                Name = reader.GetString(2),
                ProofTypeDescription = reader.GetString(3),
                RegionName = reader.GetString(4),
            });
        }

        return builder.ToImmutable();
    }
}