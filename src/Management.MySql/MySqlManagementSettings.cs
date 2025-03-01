using System.ComponentModel.DataAnnotations;

namespace Mk8.Management.MySql;

public record MySqlManagementSettings
{
    public const string SectionName = "Mk8:Management:MySql";

    [Required]
    public required string RootConnectionString { get; init; }

    // Then PR review....

    // Resolve TODO's on new code.
    // Add issues for TODO's on old code.

    [Required]
    public required string TargetConnectionStringTemplate { get; init; }

    public string GetTargetConnectionString(string database)
    {
        return TargetConnectionStringTemplate.Replace("${database}", database, StringComparison.OrdinalIgnoreCase);
    }
}
