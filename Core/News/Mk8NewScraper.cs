using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Mk8.Core.News;

internal partial class Mk8NewsScraper : INewSource
{
    public IEnumerable<New> GetNews()
    {
        HtmlWeb web = new();

        var htmlDoc = web.Load("https://mariokart64.com/mk8/");

        IEnumerable<HtmlNode> nodes = htmlDoc.DocumentNode.SelectNodes("//div[@id='body_panel']/div[@class='info_box grey']")
            .Skip(1);

        foreach (var node in nodes)
        {
            // TODO: Inline these in the yield new where possible.
            string body = string.Join(Environment.NewLine, node.ChildNodes.Where(child => child.Name != "#text").Skip(2).Select(n => n.OuterHtml));
            Match? match = AuthorDateRegex().Match(node.SelectSingleNode("div[@class='small_text']").InnerText);
            string authorName = match.Groups[1].Value;
            string dateString = match.Groups[2].Value;
            DateOnly date = DateOnly.Parse(dateString);
            string title = node.SelectSingleNode("h3").InnerText;

            yield return new New
            {
                AuthorName = authorName,
                Body = body,
                Date = date,
                Title = title
            };
        }
    }

    [GeneratedRegex("(By .* on )(.*)")]
    private static partial Regex AuthorDateRegex();
}