using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Imports;

namespace Mk8.MySql.Imports;

internal static class ServiceCollectionExtensions
{
    internal static void AddImports(this IServiceCollection services)
    {
        services.AddSingleton<IImportData, MySqlImportData>();
    }
}
