using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Locations.Countries;

namespace Mk8.MySql.Locations.Countries;

internal static class ServiceCollectionExtensions
{
    internal static void AddCountries(this IServiceCollection services)
    {
        services.AddSingleton<ICountryData, MySqlCountryData>();
    }
}