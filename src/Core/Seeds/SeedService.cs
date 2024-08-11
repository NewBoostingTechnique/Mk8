using System.Transactions;
using Mk8.Core.Courses;
using Mk8.Core.Locations.Countries;
using Mk8.Core.Locations.Regions;
using Mk8.Core.Logins;
using Mk8.Core.Persons;
using Mk8.Core.ProofTypes;

namespace Mk8.Core.Seeds;

internal sealed class SeedService(
    ICourseService courseService,
    ICountryService countryService,
    IRegionService regionService,
    IPersonService personService,
    IProofTypeService proofTypeService,
    ILoginService loginService
) : ISeedService
{
    public async Task InsertAsync()
    {
        using TransactionScope transaction = new(TransactionScopeAsyncFlowOption.Enabled);

        await courseService.SeedAsync().ConfigureAwait(false);
        await countryService.SeedAsync().ConfigureAwait(false);
        await regionService.SeedAsync().ConfigureAwait(false);
        await personService.SeedAsync().ConfigureAwait(false);
        await proofTypeService.SeedAsync().ConfigureAwait(false);
        await loginService.SeedAsync().ConfigureAwait(false);

        transaction.Complete();
    }
}
