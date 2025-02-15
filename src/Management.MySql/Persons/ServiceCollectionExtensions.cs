using Microsoft.Extensions.DependencyInjection;
using Mk8.Management.Core;

namespace Mk8.Management.MySql.Persons;

internal static class ServiceCollectionExtensions
{
    internal static void AddPersons(this IServiceCollection services)
    {
        services.AddSingleton<IStoreManager, MySqlPersonStoreManager>();
    }
}
