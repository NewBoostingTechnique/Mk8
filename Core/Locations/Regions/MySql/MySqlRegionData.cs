using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Mk8.Core.Locations.Regions;

internal class MySqlRegionData(IOptions<Mk8Settings> mk8Options) : IRegionData
{
    public async Task<string?> IdentifyAsync(string regionName)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("RegionIdentify", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("RegionName", regionName);

        await connection.OpenAsync().ConfigureAwait(false);
        return await command.ExecuteScalarAsync().ConfigureAwait(false) as string;
    }

    public async Task<IImmutableList<Region>> ListAsync(string countryId)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("RegionList", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("CountryId", countryId);

        await connection.OpenAsync().ConfigureAwait(false);
        using DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
        ImmutableList<Region>.Builder builder = ImmutableList.CreateBuilder<Region>();
        while (await reader.ReadAsync().ConfigureAwait(false))
        {
            builder.Add(new Region
            {
                Name = reader.GetString(0)
            });
        }
        return builder.ToImmutable();
    }
}