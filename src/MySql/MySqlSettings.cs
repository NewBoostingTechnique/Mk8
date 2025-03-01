using System.ComponentModel.DataAnnotations;

namespace Mk8.MySql;

public record MySqlSettings
{
    public const string SectionName = "Mk8:MySql";

    [Required]
    public required string ConnectionString { get; init; }
}
