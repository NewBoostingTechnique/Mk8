using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Persons;

namespace Mk8.MySql.Persons;

internal static class ServiceCollectionExtensions
{
    internal static void AddPersons(this IServiceCollection services)
    {
        services.AddSingleton<IPersonStore, MySqlPersonStore>();
    }
}
