using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using Mk8.Core.Migrations;
using Mk8.Core.Persons;

namespace Mk8.Core.News;

internal sealed partial class NewService(
    IMigrationStore migrationStore,
    INewStore newStore,
    IPersonStore personStore,
    IOptionsMonitor<Mk8Settings> settings
) : INewService
{
    public async Task CreateAsync(New @new, CancellationToken cancellationToken = default)
    {
        Ulid? authorPersonId = await personStore.IdentifyAsync(@new.AuthorName, cancellationToken).ConfigureAwait(false);
        if (authorPersonId is null)
        {
            authorPersonId = Ulid.NewUlid();
            await personStore.CreateAsync
            (
                new()
                {
                    Id = authorPersonId,
                    Name = @new.AuthorName
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        }

        await newStore.CreateAsync
        (
            @new with
            {
                Id = Ulid.NewUlid(),
                AuthorPersonId = authorPersonId
            },
            cancellationToken
        )
        .ConfigureAwait(false);
    }

    public Task<IImmutableList<New>> IndexAsync(CancellationToken cancellationToken = default)
    {
        return newStore.IndexAsync(cancellationToken);
    }

    #region Migrate.

    public async Task<Migration> MigrateAsync(CancellationToken cancellationToken = default)
    {
        Migration migration = await migrationStore.StartAsync("News Migration", cancellationToken).ConfigureAwait(false);

        _ = Task.Run
        (
            async () =>
            {
                string? error = null;
                byte progress = 0;

                try
                {
                    await newStore.DeleteAsync(cancellationToken).ConfigureAwait(false);

                    HtmlWeb web = new();

                    IEnumerable<HtmlNode> nodes = web.Load(settings.CurrentValue.ImportFromBaseUrl)
                        .DocumentNode
                        .SelectNodes("//div[@id='body_panel']/div[@class='info_box grey']")
                        .Skip(1);

                    foreach (HtmlNode node in nodes)
                    {
                        Match? subTitleMatch = AuthorDateRegex().Match(node.SelectSingleNode("div[@class='small_text']").InnerText);

                        await CreateAsync
                        (
                            new()
                            {
                                Title = HttpUtility.HtmlDecode(node.SelectSingleNode("h3").InnerText),
                                Date = DateOnly.Parse(subTitleMatch.Groups[2].Value, CultureInfo.GetCultureInfo("en-US")),
                                Body = string.Join
                                (
                                    Environment.NewLine,
                                    node.ChildNodes.Where(child => child.Name != "#text").Skip(2).Select(n => HttpUtility.HtmlDecode(n.OuterHtml))
                                ),
                                AuthorName = subTitleMatch.Groups[1].Value
                            },
                            cancellationToken
                        )
                        .ConfigureAwait(false);
                    }

                    progress = 100;
                }
                catch (Exception ex)
                {
                    error = ex.ToString();
                }

                await migrationStore.UpdateAsync
                (
                    migration with
                    {
                        EndTime = DateTime.UtcNow,
                        Error = error,
                        Progress = progress
                    },
                    CancellationToken.None
                )
                .ConfigureAwait(false);
            },
            cancellationToken
        );

        return migration;
    }

    [GeneratedRegex("By (.*) on (.*)")]
    private static partial Regex AuthorDateRegex();

    #endregion Migrate.

}
