namespace Mk8.Core.Persons;

internal static class PersonDataExtensions
{
    internal static async Task<Ulid> IdentifyRequiredAsync(this IPersonStore personData, string name)
    {
        return await personData.IdentifyAsync(name).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"Person '{name}' not found.");
    }
}
