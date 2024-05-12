namespace Mk8.Core.Persons;

internal interface IPersonDataEvents
{

    #region Inserted.

    event EventHandler<InsertedEventArgs>? Inserted;

    internal sealed class InsertedEventArgs(Person person) : EventArgs
    {
        internal Person Person { get; } = person;
    }

    #endregion Inserted.

}