namespace Mk8.Core.ProofTypes;

internal interface IProofTypeDataEvents
{

    #region Inserted.

    event EventHandler<InsertedEventArgs>? Inserted;

    internal sealed class InsertedEventArgs(ProofType proofType) : EventArgs
    {
        internal ProofType ProofType { get; } = proofType;
    }

    #endregion Inserted.

}
