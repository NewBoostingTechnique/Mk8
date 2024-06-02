using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Extensions;

namespace Mk8.Core.Logins;

internal static class ServiceCollectionExtensions
{
    internal static void AddLogins(this IServiceCollection services)
    {
        services.AddLoginCaching();
        services.AddSingleton<ILoginService, LoginService>();
    }

    private static void AddLoginCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<ILoginData>();
        services.AddSingleton<ILoginData>
        (
            sp => ActivatorUtilities.CreateInstance<CachingLoginData>
            (
                sp,
                (ILoginData)descriptor.CreateInstance(sp)
            )
        );
    }
}