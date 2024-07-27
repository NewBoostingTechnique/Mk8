namespace Mk8.Core.Persons;

public interface IPersonData
{
    Task<Ulid?> IdentifyAsync(string personName);

    Task InsertAsync(Person person);
}
