using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Mk8.Core;
using Mk8.Core.News;
using Mk8.Data.Common;
using MySql.Data.MySqlClient;

namespace Mk8.MySql.News;

internal sealed class MySqlNewData(IOptions<Mk8Settings> options) : INewStore
{
    public async Task CreateAsync(New @new, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(@new.AuthorPersonId);

        using MySqlConnection connection = new(options.Value.ConnectionString);

        using MySqlCommand insertCommand = new("new_create", connection);
        insertCommand.CommandType = CommandType.StoredProcedure;
        insertCommand.AddParameter(nameof(@new.Id), @new.Id);
        insertCommand.AddParameter(nameof(@new.Title), @new.Title);
        insertCommand.AddParameter(nameof(@new.Date), @new.Date);
        insertCommand.AddParameter(nameof(@new.Body), @new.Body);
        insertCommand.AddParameter(nameof(@new.AuthorPersonId), @new.AuthorPersonId);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await insertCommand.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(options.Value.ConnectionString);

        using MySqlCommand clearCommand = new("new_delete", connection);
        clearCommand.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await clearCommand.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IImmutableList<New>> IndexAsync(CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(options.Value.ConnectionString);

        using MySqlCommand command = new("new_index", connection);
        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        ImmutableList<New>.Builder builder = ImmutableList.CreateBuilder<New>();
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            builder.Add(new New
            {
                AuthorName = reader.GetString(nameof(New.AuthorName)),
                Body = reader.GetString(nameof(New.Body)),
                Date = reader.GetDateOnly(nameof(New.Date)),
                Title = reader.GetString(nameof(New.Title)),
            });
        }
        return builder.ToImmutable();
    }
}
