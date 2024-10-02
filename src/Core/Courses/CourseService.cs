using System.Collections.Immutable;
using System.Transactions;
using Microsoft.Extensions.Logging;

namespace Mk8.Core.Courses;

internal class CourseService(
    ICourseData courseData,
    ILogger<CourseService> logger
) : ICourseService
{
    public Task<bool> ExistsAsync(string courseName)
    {
        return courseData.ExistsAsync(courseName);
    }

    public Task<IImmutableList<Course>> ListAsync()
    {
        return courseData.IndexAsync();
    }

    public async Task SeedAsync()
    {
        logger.LogInformation("Seeding Courses...");

        using TransactionScope transaction = new(TransactionScopeAsyncFlowOption.Enabled);

        await insertAsync("Mario Kart Stadium").ConfigureAwait(false);
        await insertAsync("Water Park").ConfigureAwait(false);

        transaction.Complete();

        Task insertAsync(string name)
        {
            return courseData.CreateAsync
            (
                new Course
                {
                    Id = Ulid.NewUlid(),
                    Name = name
                }
            );
        }
    }
}
