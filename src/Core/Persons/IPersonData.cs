namespace Mk8.Core.Persons;

public interface IPersonData
{
    Task<string?> IdentifyAsync(string personName);

    Task InsertAsync(Person person);
}