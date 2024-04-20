using System.Data;
using Microsoft.Extensions.Options;
using Mk8.Core.Extensions;
using MySql.Data.MySqlClient;

namespace Mk8.Core.Logins.MySql;

internal class MySqlLoginData(IOptions<Mk8Settings> mk8Options) : ILoginData
{
    public async Task<bool> ExistsAsync(string email)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("LoginExists", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new MySqlParameter("LoginEmail", email));

        await connection.OpenAsync().ConfigureAwait(false);
        return await command.ExecuteBoolAsync().ConfigureAwait(false);
    }
}