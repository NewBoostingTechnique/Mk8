using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mk8.Management.Core;

namespace Mk8.Management.MySql.Times;

internal static class ServiceCollectionExtensions
{
    internal static void AddTimes(this IServiceCollection services)
    {
        services.TryAddSingleton<StoreManagerAssistant>();
        services.AddSingleton<IStoreManager, MySqlTimeStoreManager>();
    }
}
