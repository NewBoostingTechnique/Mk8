using System.Globalization;
using MySql.Data.MySqlClient;

namespace Mk8.Core.MySql;

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
}