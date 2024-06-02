using System.Globalization;
using MySql.Data.MySqlClient;

namespace Mk8.MySql;

internal static class MySqlCommandExtensions
{
    internal static void AddParameter(this MySqlCommand command, string name, DateOnly? value)
    {
        command.Parameters.AddWithValue
        (
            name,
            value?.ToString("o", CultureInfo.InvariantCulture)
        );
    }

    internal static void AddParameter(this MySqlCommand command, string name, object? value)
    {
        command.Parameters.AddWithValue
        (
            name,
            value
        );
    }

    internal static async Task<bool> ExecuteBoolAsync(this MySqlCommand command)
    {
        object? scalarObject = await command.ExecuteScalarAsync().ConfigureAwait(false);
        return scalarObject is long scalarLong && scalarLong == 1;
    }
}