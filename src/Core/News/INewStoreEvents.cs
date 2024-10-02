namespace Mk8.Core.News;

internal interface INewStoreEvents
{
    event EventHandler? Cleared;

    event EventHandler? Inserted;
}
