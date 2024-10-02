using System.Collections.Immutable;
using System.Transactions;
using Microsoft.Extensions.Logging;

namespace Mk8.Core.Countries;

internal class CountryService(
    ICountryData countryData,
    ILogger<CountryService> logger
) : ICountryService
{
    public Task<IImmutableList<Country>> IndexAsync()
    {
        return countryData.IndexAsync();
    }

    public async Task SeedAsync()
    {
        logger.LogInformation("Seeding Countries...");

        using TransactionScope transaction = new(TransactionScopeAsyncFlowOption.Enabled);

        await insertAsync("France").ConfigureAwait(false);
        await insertAsync("United Kingdom").ConfigureAwait(false);

        transaction.Complete();

        Task insertAsync(string name)
        {
            return countryData.CreateAsync
            (
                new Country
                {
                    Id = Ulid.NewUlid(),
                    Name = name
                }
            );
        }
    }
}
