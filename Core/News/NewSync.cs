using System.Transactions;
using Mk8.Core.Persons;

namespace Mk8.Core.News;

internal class NewSync(
    INewSource newSource,
    INewData newData,
    IPersonData personData
) : INewSync
{
    public async Task SyncNewsAsync()
    {
        // TODO: News import gets the author name wrong, and there are some funky characters.

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
                    Id = Identifier.Generate(),
                    Name = @new.AuthorName
                };
                await personData.InsertAsync(authorPerson).ConfigureAwait(false);
                @new.AuthorPersonId = authorPerson.Id;
            }

            @new.Id = Identifier.Generate();
            await newData.InsertAsync(@new).ConfigureAwait(false);
        }

        transaction.Complete();
    }
}
