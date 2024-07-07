namespace Mk8.Core.Syncs;

public interface ISyncService
{
    Task<Sync?> FindAsync(string id);

    Task<Sync> InsertAsync(Sync sync);
}
