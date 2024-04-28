using System.Data.Common;

namespace Mk8.Core.Extensions;

internal static class DbDataReaderExtensions
{
    internal static DateOnly GetDateOnly(this DbDataReader reader, int ordinal)
    {
        return DateOnly.FromDateTime(reader.GetFieldValue<DateTime>(ordinal));
    }

    internal static DateOnly? GetDateOnlyNullable(this DbDataReader reader, int ordinal)
    {
        return reader.IsDBNull(ordinal)
            ? null
            : GetDateOnly(reader, ordinal);
    }

    internal static DateTime? GetDateTimeNullable(this DbDataReader reader, int ordinal)
    {
        return reader.IsDBNull(ordinal)
            ? null
            : reader.GetDateTime(ordinal);
    }

    internal static TimeSpan? GetTimeSpanNullable(this DbDataReader reader, int ordinal)
    {
        return reader.IsDBNull(ordinal)
            ? null
            : reader.GetFieldValue<TimeSpan>(ordinal);
    }
}