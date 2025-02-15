using Microsoft.Extensions.DependencyInjection;
using Mk8.Management.Core;

namespace Mk8.Management.MySql.Logins;

internal static class ServiceCollectionExtensions
{
    internal static void AddLogins(this IServiceCollection services)
    {
        services.AddSingleton<IStoreManager, MySqlLoginStoreManager>();
    }
}
