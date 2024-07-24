using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.ProofTypes;

namespace Mk8.MySql.ProofTypes;

internal static class ServiceCollectionExtensions
{
    internal static void AddProofTypes(this IServiceCollection services)
    {
        services.AddSingleton<IProofTypeData, MySqlProofTypeData>();
    }
}