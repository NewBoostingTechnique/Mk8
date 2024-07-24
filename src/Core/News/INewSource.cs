namespace Mk8.Core.News;

internal interface INewSource
{
    IEnumerable<New> GetNews();
}