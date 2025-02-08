using Mk8.MySql;

namespace Mk8.Management.MySql;

public record MySqlManagementSettings : MySqlSettings
{
    public required string RootConnectionString { get; init; }
}
