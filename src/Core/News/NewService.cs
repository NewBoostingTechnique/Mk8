using System.Collections.Immutable;
using System.Transactions;
using Mk8.Core.Persons;

namespace Mk8.Core.News;

internal class NewService(
    INewData newData,
    INewSource newSource,
    IPersonData personData
) : INewService
{
    public Task<IImmutableList<New>> ListAsync()
    {
        return newData.ListAsync();
    }

    public async Task ImportAsync()
    {
        List<New> news = newSource.GetNews().ToList();

        using TransactionScope transaction = new(TransactionScopeAsyncFlowOption.Enabled);

        await newData.ClearAsync().ConfigureAwait(false);

        foreach (New @new in news)
        {
            @new.AuthorPersonId = await personData.IdentifyAsync(@new.AuthorName).ConfigureAwait(false);
            if (@new.AuthorPersonId is null)
            {
                Person authorPerson = new()
                {
                    Id = Ulid.NewUlid(),
                    Name = @new.AuthorName
                };
                await personData.InsertAsync(authorPerson).ConfigureAwait(false);
                @new.AuthorPersonId = authorPerson.Id;
            }

            @new.Id = Ulid.NewUlid();
            await newData.InsertAsync(@new).ConfigureAwait(false);
        }

        transaction.Complete();
    }
}
