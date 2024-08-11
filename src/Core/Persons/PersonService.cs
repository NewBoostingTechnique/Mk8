namespace Mk8.Core.Persons;

internal sealed class PersonService(IPersonData personData) : IPersonService
{
    public Task SeedAsync()
    {
        return personData.InsertAsync
        (
            new Person
            {
                Id = Ulid.NewUlid(),
                Name = "Russell Horwood"
            }
        );
    }
}
