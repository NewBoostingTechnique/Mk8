using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Countries;

namespace Mk8.MySql.Countries;

internal static class ServiceCollectionExtensions
{
    internal static void AddCountries(this IServiceCollection services)
    {
        services.AddSingleton<ICountryStore, MySqlCountryStore>();
    }
}
