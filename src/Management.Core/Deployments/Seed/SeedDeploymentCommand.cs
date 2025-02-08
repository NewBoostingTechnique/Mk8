using System.Collections.Immutable;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Mk8.Management.Core.Deployments.Seed;

public record SeedDeploymentCommand : ICommand<Result>
{

    #region Courses.

    internal ImmutableArray<Course> Courses { get; } =
    [
        new Course
        {
            Name = "Mario Kart Stadium"
        },
        new Course
        {
            Name = "Water Park"
        }
    ];

    internal record Course
    {
        internal required string Name { get; init; }
    }

    #endregion Courses.

    #region Countries.

    internal ImmutableArray<Country> Countries { get; } =
    [
        new Country
        {
            Name = "France",
            Regions = []
        },
        new Country
        {
            Name = "United Kingdom",
            Regions =
            [
                new Region
                {
                    Name = "Guildford"
                }
            ]
        }
    ];

    internal record Country
    {
        internal required string Name { get; init; }

        internal required ImmutableArray<Region> Regions { get; init; }
    }

    internal record Region
    {
        internal required string Name { get; init; }
    }

    #endregion Countries.

    #region Persons.

    internal ImmutableArray<Person> Persons { get; } =
    [
        new Person
        {
            Name = "Russell Horwood",
            Logins =
            [
                new Login
                {
                    Email = "russell.horwood@gmail.com"
                }
            ]
        }
    ];

    internal record Person
    {
        internal required string Name { get; init; }

        internal required ImmutableArray<Login> Logins { get; init; }
    }

    internal record Login
    {
        internal required string Email { get; init; }
    }

    #endregion Persons.

}
