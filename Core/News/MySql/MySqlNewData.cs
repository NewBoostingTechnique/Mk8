using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Mk8.Core.Extensions;
using Mk8.Core.MySql;
using MySql.Data.MySqlClient;

namespace Mk8.Core.News.MySql;

internal sealed class MySqlNewData(IOptions<Mk8Settings> options) : INewData
{
    public async Task ClearAsync()
    {
        using MySqlConnection connection = new(options.Value.ConnectionString);

        using MySqlCommand clearCommand = new("NewClear", connection);
        clearCommand.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync().ConfigureAwait(false);
        await clearCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
    }

    public async Task InsertAsync(New @new)
    {
        ArgumentException.ThrowIfNullOrEmpty(@new.AuthorPersonId);

        using MySqlConnection connection = new(options.Value.ConnectionString);

        using MySqlCommand insertCommand = new("NewInsert", connection);
        insertCommand.CommandType = CommandType.StoredProcedure;
        insertCommand.AddParameter("AuthorPersonId", @new.AuthorPersonId);
        insertCommand.AddParameter("NewBody", @new.Body);
        insertCommand.AddParameter("NewDate", @new.Date);
        insertCommand.AddParameter("NewId", @new.Id);
        insertCommand.AddParameter("NewTitle", @new.Title);

        await connection.OpenAsync().ConfigureAwait(false);
        await insertCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
    }

    public async Task<IImmutableList<New>> ListAsync()
    {
        using MySqlConnection connection = new(options.Value.ConnectionString);

        using MySqlCommand command = new("NewList", connection);
        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync().ConfigureAwait(false);
        using DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

        ImmutableList<New>.Builder builder = ImmutableList.CreateBuilder<New>();
        while (await reader.ReadAsync().ConfigureAwait(false))
        {
            builder.Add(new New
            {
                AuthorName = reader.GetString(0),
                Body = reader.GetString(1),
                Date = reader.GetDateOnly(2),
                Title = reader.GetString(3),
            });
        }
        return builder.ToImmutable();
    }
}