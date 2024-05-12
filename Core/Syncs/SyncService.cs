using Mk8.Core.News;

namespace Mk8.Core.Syncs;

internal class SyncService(INewSync newSync) : ISyncService
{
    public void Insert()
    {
        newSync.SyncNews();
    }
}