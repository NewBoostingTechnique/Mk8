namespace Mk8.Core.News;

internal interface INewDataEvents
{
    event EventHandler? Cleared;

    event EventHandler? Inserted;
}