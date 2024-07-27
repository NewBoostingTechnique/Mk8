using static Mk8.Core.Persons.IPersonDataEvents;

namespace Mk8.Core.Persons;

internal class EventingPersonData(IPersonData innerData)
    : IPersonData, IPersonDataEvents
{

    public Task<Ulid?> IdentifyAsync(string personName)
    {
        return innerData.IdentifyAsync(personName);
    }

    #region Insert.

    public event EventHandler<InsertedEventArgs>? Inserted;

    public async Task InsertAsync(Person person)
    {
        await innerData.InsertAsync(person).ConfigureAwait(false);
        Inserted?.Invoke(this, new InsertedEventArgs(person));
    }

    #endregion Insert.

}
