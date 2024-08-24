using System.Data.Common;

namespace Mk8.Data.Common;

public static class DbDataReaderExtensions
{
    public static DateOnly GetDateOnly(this DbDataReader reader, int ordinal)
    {
        return DateOnly.FromDateTime(reader.GetFieldValue<DateTime>(ordinal));
    }

    public static DateOnly? GetDateOnlyNullable(this DbDataReader reader, int ordinal)
    {
        return reader.IsDBNull(ordinal)
            ? null
            : GetDateOnly(reader, ordinal);
    }

    public static DateTime? GetDateTimeNullable(this DbDataReader reader, int ordinal)
    {
        return reader.IsDBNull(ordinal)
            ? null
            : reader.GetDateTime(ordinal);
    }

    public static string? GetStringNullable(this DbDataReader reader, int ordinal)
    {
        return reader.IsDBNull(ordinal)
            ? null
            : reader.GetString(ordinal);
    }

    public static TimeSpan? GetTimeSpanNullable(this DbDataReader reader, int ordinal)
    {
        return reader.IsDBNull(ordinal)
            ? null
            : reader.GetFieldValue<TimeSpan>(ordinal);
    }
}
