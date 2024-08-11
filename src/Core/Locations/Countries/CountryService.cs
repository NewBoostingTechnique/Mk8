using System.Collections.Immutable;
using System.Transactions;
using Microsoft.Extensions.Logging;

namespace Mk8.Core.Locations.Countries;

internal class CountryService(
    ICountryData countryData,
    ILogger<CountryService> logger
) : ICountryService
{
    public Task<IImmutableList<Country>> ListAsync()
    {
        return countryData.ListAsync();
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
            return countryData.InsertAsync
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
