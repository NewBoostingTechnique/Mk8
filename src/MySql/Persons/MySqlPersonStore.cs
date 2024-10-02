using System.Data;
using Microsoft.Extensions.Options;
using Mk8.Core;
using Mk8.Core.Persons;
using MySql.Data.MySqlClient;

namespace Mk8.MySql.Persons;

internal class MySqlPersonData(IOptions<Mk8Settings> mk8Options) : IPersonStore
{
    public async Task CreateAsync(Person person, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("person_create", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", person.Id);
        command.AddParameter("Name", person.Name);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<Ulid?> IdentifyAsync(string name, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("person_identify", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Name", name);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        return await command.ExecuteUlidAsync(cancellationToken).ConfigureAwait(false);
    }
}
