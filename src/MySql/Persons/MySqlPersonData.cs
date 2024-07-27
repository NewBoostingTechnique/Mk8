using System.Data;
using Microsoft.Extensions.Options;
using Mk8.Core;
using Mk8.Core.Persons;
using MySql.Data.MySqlClient;

namespace Mk8.MySql.Persons;

internal class MySqlPersonData(IOptions<Mk8Settings> mk8Options) : IPersonData
{
    public async Task<Ulid?> IdentifyAsync(string personName)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("PersonIdentify", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("PersonName", personName);

        await connection.OpenAsync().ConfigureAwait(false);
        return await command.ExecuteScalarAsync().ConfigureAwait(false) as Ulid?;
    }

    public async Task InsertAsync(Person person)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("PersonInsert", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("PersonId", person.Id);
        command.AddParameter("PersonName", person.Name);

        await connection.OpenAsync().ConfigureAwait(false);
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }
}
