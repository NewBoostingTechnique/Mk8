using System.Security.Cryptography;
using Mk8.Core.News;
using Mk8.Core.Players;

namespace Mk8.Core.Imports;

internal class ImportService(
    INewService newService,
    IPlayerService playerService,
    IImportData importData
) : IImportService
{
    public Task<Import?> FindAsync(Ulid id)
    {
        return importData.DetailAsync(id);
    }

    public async Task<Import> InsertAsync(Import import)
    {
        import.Id = Ulid.NewUlid();
        import.StartTime = DateTime.UtcNow;
        await importData.InsertAsync(import).ConfigureAwait(false);

        _ = Task.Run(async () =>
        {
            try
            {
                await newService.ImportAsync().ConfigureAwait(false);
                await playerService.ImportAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                import.Error = ex.ToString();
            }

            import.EndTime = DateTime.UtcNow;
            await importData.UpdateAsync(import).ConfigureAwait(false);
        });

        return import;
    }
}
