namespace Mk8.Core.Syncs;

public interface ISyncService
{
    Task<Sync?> FindAsync(Ulid id);

    Task<Sync> InsertAsync(Sync sync);
}
