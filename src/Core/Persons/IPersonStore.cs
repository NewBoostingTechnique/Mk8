namespace Mk8.Core.Persons;

public interface IPersonStore
{
    Task CreateAsync(Person person, CancellationToken cancellationToken = default);

    Task<Ulid?> IdentifyAsync(string name, CancellationToken cancellationToken = default);
}
