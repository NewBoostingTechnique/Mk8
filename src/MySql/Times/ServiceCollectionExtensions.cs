using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Times;

namespace Mk8.MySql.Times;

internal static class ServiceCollectionExtensions
{
    internal static void AddTimes(this IServiceCollection services)
    {
        services.AddSingleton<ITimeStore, MySqlTimeStore>();
    }
}
