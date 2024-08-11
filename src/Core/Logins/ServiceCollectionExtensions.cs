using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.DependencyInjection;

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

        services.AddSingleton(sp => ActivatorUtilities.CreateInstance<EventingLoginData>
        (
            sp,
            (ILoginData)descriptor.CreateInstance(sp)
        ));

        services.AddSingleton<ILoginDataEvents>(sp => sp.GetRequiredService<EventingLoginData>());

        services.AddSingleton<ILoginData>
        (
            sp =>
            {
                EventingLoginData eventingLoginData = sp.GetRequiredService<EventingLoginData>();
                return ActivatorUtilities.CreateInstance<CachingLoginData>
                (
                    sp,
                    eventingLoginData,
                    eventingLoginData
                );
            }
        );
    }
}
