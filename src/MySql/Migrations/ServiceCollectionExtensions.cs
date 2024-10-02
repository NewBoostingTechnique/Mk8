using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Migrations;

namespace Mk8.MySql.Migrations;

internal static class ServiceCollectionExtensions
{
    internal static void AddMigrations(this IServiceCollection services)
    {
        services.AddSingleton<IMigrationStore, MySqlMigrationStore>();
    }
}
