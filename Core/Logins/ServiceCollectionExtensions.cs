using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Logins.MySql;
using Mk8.Core.Extensions;

namespace Mk8.Core.Logins;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddLogins(this IServiceCollection services)
    {
        return services
            .AddSingleton<ILoginData, MySqlLoginData>()
            .AddLoginCaching()
            .AddSingleton<ILoginService, LoginService>();
    }

    private static IServiceCollection AddLoginCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<ILoginData>();

        return services.AddSingleton<ILoginData>
        (
            sp => ActivatorUtilities.CreateInstance<CachingLoginData>
            (
                sp,
                (ILoginData)descriptor.CreateInstance(sp)
            )
        );
    }
}