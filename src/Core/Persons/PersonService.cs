namespace Mk8.Core.Persons;

internal sealed class PersonService(IPersonStore personData) : IPersonService
{
    public Task SeedAsync()
    {
        return personData.CreateAsync
        (
            new Person
            {
                Id = Ulid.NewUlid(),
                Name = "Russell Horwood"
            }
        );
    }
}
