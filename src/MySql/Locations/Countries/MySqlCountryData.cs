using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Mk8.Core;
using Mk8.Core.Locations.Countries;
using MySql.Data.MySqlClient;

namespace Mk8.MySql.Locations.Countries;

internal class MySqlCountryData(IOptions<Mk8Settings> mk8Options) : ICountryData
{
    public async Task<Ulid?> IdentifyAsync(string countryName)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("CountryIdentify", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("CountryName", countryName);

        await connection.OpenAsync().ConfigureAwait(false);
        return await command.ExecuteScalarAsync().ConfigureAwait(false) as Ulid?;
    }

    public async Task<IImmutableList<Country>> ListAsync()
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("CountryList", connection);
        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync().ConfigureAwait(false);
        using DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
        ImmutableList<Country>.Builder builder = ImmutableList.CreateBuilder<Country>();
        while (await reader.ReadAsync().ConfigureAwait(false))
        {
            builder.Add(new Country
            {
                Name = reader.GetString(0)
            });
        }
        return builder.ToImmutable();
    }
}
