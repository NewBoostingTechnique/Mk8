using System.ComponentModel.DataAnnotations;

namespace Mk8.Management.MySql;

public record MySqlManagementSettings
{
    public const string SectionName = "Mk8:Management:MySql";

    [Required]
    public required string RootConnectionString { get; init; }

    [Required]
    public required string TargetConnectionStringTemplate { get; init; }

    public string GetTargetConnectionString(string database)
    {
        return TargetConnectionStringTemplate.Replace("${database}", database, StringComparison.OrdinalIgnoreCase);
    }
}
