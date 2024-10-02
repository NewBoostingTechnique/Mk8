using System.Data;
using System.Data.Common;

namespace Mk8.Data.Common;

public static class DbDataReaderExtensions
{
    public static DateOnly GetDateOnly(this DbDataReader reader, string name)
    {
        return DateOnly.FromDateTime(reader.GetFieldValue<DateTime>(name));
    }

    public static DateOnly? GetDateOnlyNullable(this DbDataReader reader, string name)
    {
        return reader.IsDBNull(name)
            ? null
            : GetDateOnly(reader, name);
    }

    public static DateTime? GetDateTimeNullable(this DbDataReader reader, string name)
    {
        return reader.IsDBNull(name)
            ? null
            : reader.GetDateTime(name);
    }

    public static string? GetStringNullable(this DbDataReader reader, string name)
    {
        return reader.IsDBNull(name)
            ? null
            : reader.GetString(name);
    }

    public static TimeSpan? GetTimeSpanNullable(this DbDataReader reader, string name)
    {
        return reader.IsDBNull(name)
            ? null
            : reader.GetFieldValue<TimeSpan>(name);
    }

    public static Ulid GetUlid(this DbDataReader reader, string name)
    {
        return new(reader.GetFieldValue<byte[]>(name));
    }
}
