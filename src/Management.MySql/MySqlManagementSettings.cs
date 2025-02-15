namespace Mk8.Management.MySql;

public record MySqlManagementSettings
{
    public const string SectionName = "Mk8:Management:MySql";

    public required string RootConnectionString { get; init; }

    // TODO: Add validation so we actually know these are not null.

    public required string TargetConnectionStringTemplate { get; init; }

    public string GetTargetConnectionString(string database)
    {
        return TargetConnectionStringTemplate.Replace("${database}", database, StringComparison.OrdinalIgnoreCase);
    }
}
