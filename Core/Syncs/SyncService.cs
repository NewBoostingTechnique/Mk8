using Mk8.Core.News;

namespace Mk8.Core.Syncs;

internal class SyncService(
    INewSync newSync,
    ISyncData syncData
) : ISyncService
{
    public Task<Sync?> FindAsync(string id)
    {
        return syncData.DetailAsync(id);
    }

    public async Task<Sync> InsertAsync(Sync sync)
    {
        sync.Id = Identifier.Generate();
        sync.StartTime = DateTime.UtcNow;
        await syncData.InsertAsync(sync).ConfigureAwait(false);

        await newSync.SyncNewsAsync().ConfigureAwait(false);

        sync.EndTime = DateTime.UtcNow;
        await syncData.UpdateAsync(sync).ConfigureAwait(false);

        return sync;
    }
}
