using Microsoft.Extensions.Options;
using Mk8.Core;
using Mk8.Core.Logins;
using MySql.Data.MySqlClient;
using System.Data;

namespace Mk8.MySql.Logins;

internal class MySqlLoginData(IOptions<Mk8Settings> mk8Options) : ILoginData
{
    public async Task<bool> ExistsAsync(string email)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("LoginExists", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Email", email);

        await connection.OpenAsync().ConfigureAwait(false);
        return await command.ExecuteBoolAsync().ConfigureAwait(false);
    }

    public async Task InsertAsync(Login login)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("LoginInsert", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", login.Id);
        command.AddParameter("Email", login.Email);
        command.AddParameter("PersonId", login.PersonId);

        await connection.OpenAsync().ConfigureAwait(false);
        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }
}
