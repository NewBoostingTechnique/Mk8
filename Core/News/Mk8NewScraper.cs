using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Mk8.Core.News;

internal partial class Mk8NewsScraper : INewSource
{
    public IEnumerable<New> GetNews()
    {
        HtmlWeb web = new();

        // TODO: path to come from config?
        HtmlDocument htmlDocument = web.Load("https://mariokart64.com/mk8/");

        IEnumerable<HtmlNode> nodes = htmlDocument.DocumentNode.SelectNodes("//div[@id='body_panel']/div[@class='info_box grey']")
            .Skip(1);

        foreach (var node in nodes)
        {
            Match? subTitleMatch = AuthorDateRegex().Match(node.SelectSingleNode("div[@class='small_text']").InnerText);

            yield return new New
            {
                AuthorName = subTitleMatch.Groups[1].Value,
                Body = string.Join
                (
                    Environment.NewLine,
                    node.ChildNodes.Where(child => child.Name != "#text").Skip(2).Select(n => n.OuterHtml)
                ),
                // TODO: CultureInfo.GetCultureInfo
                Date = DateOnly.Parse(subTitleMatch.Groups[2].Value),
                Title = node.SelectSingleNode("h3").InnerText
            };
        }
    }

    [GeneratedRegex("(By .* on )(.*)")]
    private static partial Regex AuthorDateRegex();
}
