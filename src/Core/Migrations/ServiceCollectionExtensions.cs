using Microsoft.Extensions.DependencyInjection;

namespace Mk8.Core.Migrations;

internal static class ServiceCollectionExtensions
{
    internal static void AddMigrations(this IServiceCollection services)
    {
        services.AddSingleton<IMigrationService, MigrationService>();
    }
}
