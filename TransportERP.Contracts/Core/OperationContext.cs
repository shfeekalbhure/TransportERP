namespace TransportERP.Contracts.Core;

/// <summary>
/// Immutable caller and organizational scope carried by an operation.
/// Authorization remains server-side; this contract never grants authority.
/// </summary>
public sealed record OperationContext(
    Guid UserId,
    Guid CompanyId,
    Guid BranchId,
    Guid CorrelationId)
{
    public void EnsureComplete()
    {
        if (UserId == Guid.Empty)
        {
            throw new ArgumentException("A user identity is required.", nameof(UserId));
        }

        if (CompanyId == Guid.Empty)
        {
            throw new ArgumentException("A company scope is required.", nameof(CompanyId));
        }

        if (BranchId == Guid.Empty)
        {
            throw new ArgumentException("A branch scope is required.", nameof(BranchId));
        }

        if (CorrelationId == Guid.Empty)
        {
            throw new ArgumentException("A correlation identifier is required.", nameof(CorrelationId));
        }
    }
}
