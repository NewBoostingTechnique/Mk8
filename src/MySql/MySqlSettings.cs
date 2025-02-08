namespace Mk8.MySql;

public record MySqlSettings
{
    public const string SectionName = "Mk8:MySql";

    public required string ConnectionString { get; init; }
}
