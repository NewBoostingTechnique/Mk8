using System.Collections.Immutable;
using System.Transactions;
using Microsoft.Extensions.Logging;

namespace Mk8.Core.Courses;

internal class CourseService(
    ICourseStore courseStore,
    ILogger<CourseService> logger
) : ICourseService
{
    public Task<bool> ExistsAsync(string courseName, CancellationToken cancellationToken = default)
    {
        return courseStore.ExistsAsync(courseName, cancellationToken);
    }

    public Task<ImmutableList<Course>> IndexAsync(CancellationToken cancellationToken = default)
    {
        return courseStore.IndexAsync(cancellationToken);
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Seeding Courses...");

        using TransactionScope transaction = new(TransactionScopeAsyncFlowOption.Enabled);

        await insertAsync("Mario Kart Stadium", cancellationToken).ConfigureAwait(false);
        await insertAsync("Water Park", cancellationToken).ConfigureAwait(false);

        transaction.Complete();

        Task insertAsync(string name, CancellationToken cancellationToken = default)
        {
            return courseStore.CreateAsync
            (
                new Course
                {
                    Id = Ulid.NewUlid(),
                    Name = name
                },
                cancellationToken
            );
        }
    }
}
