using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Mk8.Core.Locations.Countries.MySql;

internal class MySqlCountryData(IOptions<Mk8Settings> mk8Options) : ICountryData
{
    public async Task<string?> IdentifyAsync(string countryName)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("CountryIdentify", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("CountryName", countryName);

        await connection.OpenAsync().ConfigureAwait(false);
        return await command.ExecuteScalarAsync().ConfigureAwait(false) as string;
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