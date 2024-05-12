namespace Mk8.Core.Persons;

internal interface IPersonData
{
    Task<string?> IdentifyAsync(string personName);

    Task InsertAsync(Person person);
}