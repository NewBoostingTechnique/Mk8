namespace Mk8.Core.Countries;

internal interface ICountryStoreEvents
{

    #region Inserted.

    event EventHandler<InsertedEventArgs>? Inserted;

    internal sealed class InsertedEventArgs(Country country) : EventArgs
    {
        internal Country Country { get; } = country;
    }

    #endregion Inserted.

}
