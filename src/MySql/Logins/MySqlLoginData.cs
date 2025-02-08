using Microsoft.Extensions.Options;
using Mk8.Core.Logins;
using MySql.Data.MySqlClient;
using System.Data;

namespace Mk8.MySql.Logins;

internal class MySqlLoginData(
    IOptions<MySqlSettings> options
) : ILoginStore
{
    public async Task CreateAsync(Login login, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(options.Value.ConnectionString);

        using MySqlCommand command = new("login_create", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Id", login.Id);
        command.AddParameter("Email", login.Email);
        command.AddParameter("PersonId", login.PersonId);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        using MySqlConnection connection = new(options.Value.ConnectionString);

        using MySqlCommand command = new("login_exists", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.AddParameter("Email", email);

        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        return await command.ExecuteBoolAsync(cancellationToken).ConfigureAwait(false);
    }
}
