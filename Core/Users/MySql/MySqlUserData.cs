using System.Data;
using Microsoft.Extensions.Options;
using Mk8.Core.Extensions;
using MySql.Data.MySqlClient;

namespace Mk8.Core.Users.MySql;

internal class MySqlUserData(IOptions<Mk8Settings> mk8Options) : IUserData
{
    public async Task<bool> ExistsAsync(string email)
    {
        using MySqlConnection connection = new(mk8Options.Value.ConnectionString);

        using MySqlCommand command = new("UserExists", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new MySqlParameter("UserEmail", email));

        await connection.OpenAsync().ConfigureAwait(false);
        return await command.ExecuteBoolAsync().ConfigureAwait(false);
    }
}