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
    public async Task<Ulid?> IdentifyAsync(string name)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("CountryIdentify", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("CountryName", name);

        await connection.OpenAsync().ConfigureAwait(false);
        return await command.ExecuteUlidAsync().ConfigureAwait(false);
    }

    public async Task InsertAsync(Country country)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("CountryInsert", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", country.Id);
        command.AddParameter("Name", country.Name);

        await connection.OpenAsync().ConfigureAwait(false);
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
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
