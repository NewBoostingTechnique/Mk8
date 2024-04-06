using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Users.MySql;
using Mk8.Core.Extensions;

namespace Mk8.Core.Users;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddUsers(this IServiceCollection services)
    {
        return services
            .AddSingleton<IUserData, MySqlUserData>()
            .AddUserCaching()
            .AddSingleton<IUserService, UserService>();
    }

    private static IServiceCollection AddUserCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        ServiceDescriptor? descriptor = services.GetServiceDescriptor<IUserData>();

        return services.AddSingleton<IUserData>
        (
            sp => ActivatorUtilities.CreateInstance<CachingUserData>
            (
                sp,
                (IUserData)descriptor.CreateInstance(sp)
            )
        );
    }
}