using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mk8.Management.Core;

namespace Mk8.Management.MySql.Migrations;

internal static class ServiceCollectionExtensions
{
    internal static void AddMigrations(this IServiceCollection services)
    {
        services.TryAddSingleton<StoreManagerAssistant>();
        services.AddSingleton<IStoreManager, MySqlMigrationStoreManager>();
    }
}
