using System.Transactions;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Logging;
using Mk8.Core.Countries;
using Mk8.Core.Courses;
using Mk8.Core.Logins;
using Mk8.Core.Persons;
using Mk8.Core.Regions;

namespace Mk8.Management.Core.Deployments.Seed;

internal class SeedDeploymentHandler(
    ICourseStore courseStore,
    ICountryStore countryStore,
    ILogger<SeedDeploymentHandler> logger,
    ILoginStore loginStore,
    IPersonStore personStore,
    IRegionStore regionStore
) : ICommandHandler<SeedDeploymentCommand, Result>
{
    public async Task<Result> Handle(SeedDeploymentCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Seeding deployment ...");

        using TransactionScope transaction = new(TransactionScopeAsyncFlowOption.Enabled);

        await Task.WhenAll
        (
            seedCoursesAsync(),
            seedCountriesAsync(),
            seedPersonsAsync()
        )
        .ConfigureAwait(false);

        transaction.Complete();

        return Result.Success();

        Task seedCoursesAsync()
        {
            logger.LogInformation("Seeding courses ...");
            return Task.WhenAll
            (
                request.Courses.Select
                (
                    c => courseStore.CreateAsync
                    (
                        new Course
                        {
                            Id = Ulid.NewUlid(),
                            Name = c.Name
                        },
                        cancellationToken
                    )
                )
            );
        }

        Task seedCountriesAsync()
        {
            logger.LogInformation("Seeding countries ...");
            return Task.WhenAll
            (
                request.Countries.Select
                (
                    async c =>
                    {
                        Country country = new()
                        {
                            Id = Ulid.NewUlid(),
                            Name = c.Name
                        };
                        await countryStore.CreateAsync(country, cancellationToken).ConfigureAwait(false);
                        await Task.WhenAll
                        (
                            c.Regions.Select
                            (
                                r => regionStore.CreateAsync
                                (
                                    new Region
                                    {
                                        Id = Ulid.NewUlid(),
                                        Name = r.Name,
                                        CountryId = country.Id.Value
                                    },
                                    cancellationToken
                                )
                            )
                        )
                        .ConfigureAwait(false);
                    }
                )
            );
        }

        Task seedPersonsAsync()
        {
            logger.LogInformation("Seeing persons ...");
            return Task.WhenAll
            (
                request.Persons.Select
                (
                    async p =>
                    {
                        Person person = new()
                        {
                            Id = Ulid.NewUlid(),
                            Name = p.Name
                        };
                        await personStore.CreateAsync(person, cancellationToken).ConfigureAwait(false);
                        await Task.WhenAll
                        (
                            p.Logins.Select
                            (
                                l => loginStore.CreateAsync
                                (
                                    new Login
                                    {
                                        Id = Ulid.NewUlid(),
                                        Email = l.Email,
                                        PersonId = person.Id.Value
                                    },
                                    cancellationToken
                                )
                            )
                        )
                        .ConfigureAwait(false);
                    }
                )
            );
        }
    }
}
