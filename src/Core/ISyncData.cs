namespace Mk8.Core.Syncs;

public interface ISyncData
{
    Task<Sync?> DetailAsync(string id);

    Task InsertAsync(Sync sync);

    Task UpdateAsync(Sync sync);
}
