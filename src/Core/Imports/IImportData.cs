namespace Mk8.Core.Imports;

public interface IImportData
{
    Task<Import?> DetailAsync(Ulid id);

    Task InsertAsync(Import sync);

    Task UpdateAsync(Import sync);
}
