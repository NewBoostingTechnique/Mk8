namespace Mk8.Core.Imports;

public interface IImportService
{
    Task<Import?> FindAsync(Ulid id);

    Task<Import> InsertAsync(Import import);
}
