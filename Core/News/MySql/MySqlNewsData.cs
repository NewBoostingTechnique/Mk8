using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Mk8.Core.Extensions;
using MySql.Data.MySqlClient;

namespace Mk8.Core.News.MySql;

internal class MySqlNewsData(IOptions<Mk8Settings> mk8Options) : INewsData
{
    public async Task<IImmutableList<News>> ListAsync()
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("NewsList", connection);
        command.CommandType = CommandType.StoredProcedure;

        await connection.OpenAsync().ConfigureAwait(false);
        using DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

        ImmutableList<News>.Builder builder = ImmutableList.CreateBuilder<News>();
        while (await reader.ReadAsync().ConfigureAwait(false))
        {
            builder.Add(new News
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