using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Logins;

namespace Mk8.MySql.Logins;

internal static class ServiceCollectionExtensions
{
    internal static void AddLogins(this IServiceCollection services)
    {
        services.AddSingleton<ILoginStore, MySqlLoginData>();
    }
}
