using Microsoft.Extensions.DependencyInjection;

namespace Mk8.Core.Imports;

internal static class ServiceCollectionExtensions
{
    internal static void AddImports(this IServiceCollection services)
    {
        services.AddSingleton<IImportService, ImportService>();
    }
}
