using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Mk8.Core;
using Mk8.Core.Countries;
using MySql.Data.MySqlClient;

namespace Mk8.MySql.Countries;

internal class MySqlCountryStore(IOptions<Mk8Settings> mk8Options) : ICountryStore
{
    public async Task CreateAsync(Country country, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("country_create", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", country.Id);
        command.AddParameter("Name", country.Name);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<Ulid?> IdentifyAsync(string name, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("country_identify", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Name", name);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        return await command.ExecuteUlidAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IImmutableList<Country>> IndexAsync(CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("country_index", connection);
        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        ImmutableList<Country>.Builder builder = ImmutableList.CreateBuilder<Country>();
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            builder.Add(new Country
            {
                Name = reader.GetString(0)
            });
        }
        return builder.ToImmutable();
    }
}
