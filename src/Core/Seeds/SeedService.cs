using System.Transactions;
using Mk8.Core.Countries;
using Mk8.Core.Courses;
using Mk8.Core.Logins;
using Mk8.Core.Persons;
using Mk8.Core.Regions;

namespace Mk8.Core.Seeds;

internal sealed class SeedService(
    ICountryService countryService,
    ICourseService courseService,
    ILoginService loginService,
    IPersonService personService,
    IRegionService regionService
) : ISeedService
{
    public async Task InsertAsync()
    {
        using TransactionScope transaction = new(TransactionScopeAsyncFlowOption.Enabled);

        await courseService.SeedAsync().ConfigureAwait(false);
        await countryService.SeedAsync().ConfigureAwait(false);
        await regionService.SeedAsync().ConfigureAwait(false);
        await personService.SeedAsync().ConfigureAwait(false);
        await loginService.SeedAsync().ConfigureAwait(false);

        transaction.Complete();
    }
}
