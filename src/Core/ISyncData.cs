namespace Mk8.Core.Syncs;

public interface ISyncData
{
    Task<Sync?> DetailAsync(Ulid id);

    Task InsertAsync(Sync sync);

    Task UpdateAsync(Sync sync);
}
