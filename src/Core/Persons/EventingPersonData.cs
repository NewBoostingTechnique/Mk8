using static Mk8.Core.Persons.IPersonDataEvents;

namespace Mk8.Core.Persons;

internal class EventingPersonData(IPersonStore innerData)
    : IPersonStore, IPersonDataEvents
{

    public Task<Ulid?> IdentifyAsync(string personName, CancellationToken cancellationToken = default)
    {
        return innerData.IdentifyAsync(personName, cancellationToken);
    }

    #region Insert.

    public event EventHandler<InsertedEventArgs>? Inserted;

    public async Task CreateAsync(Person person, CancellationToken cancellationToken = default)
    {
        await innerData.CreateAsync(person, cancellationToken).ConfigureAwait(false);
        Inserted?.Invoke(this, new InsertedEventArgs(person));
    }

    #endregion Insert.

}
