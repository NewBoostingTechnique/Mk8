using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Mk8.Core;
using Mk8.Core.Regions;
using MySql.Data.MySqlClient;

namespace Mk8.MySql.Regions;

internal sealed class MySqlRegionData(IOptions<Mk8Settings> mk8Options) : IRegionData
{
    public async Task CreateAsync(Region region, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("region_create", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", region.Id);
        command.AddParameter("Name", region.Name);
        command.AddParameter("CountryId", region.CountryId);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<Ulid?> IdentifyAsync(string name, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("region_identify", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Name", name);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        return await command.ExecuteUlidAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IImmutableList<Region>> IndexAsync(Ulid countryId, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("region_index", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("CountryId", countryId);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        ImmutableList<Region>.Builder builder = ImmutableList.CreateBuilder<Region>();
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            builder.Add(new Region
            {
                CountryId = countryId,
                Name = reader.GetString(0)
            });
        }
        return builder.ToImmutable();
    }
}
