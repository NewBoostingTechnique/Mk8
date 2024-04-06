using MySql.Data.MySqlClient;

namespace Mk8.Core.Extensions;

internal static class MySqlCommandExtensions
{
    internal static async Task<bool> ExecuteBoolAsync(this MySqlCommand command)
    {
        object? scalarObject = await command.ExecuteScalarAsync().ConfigureAwait(false);
        return scalarObject is long scalarLong && scalarLong == 1;
    }
}