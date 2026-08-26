namespace TransportERP.Mobile.Driver.Offline;

/// <summary>
/// One-use, memory-only capability created after the server explicitly authorizes BIND or RECOVER.
/// It contains identities only and cannot sign, authenticate, or be serialized.
/// </summary>
internal sealed class DriverDeviceKeyEnrollmentAuthorization
{
    private int _consumed;

    internal DriverDeviceKeyEnrollmentAuthorization(
        Guid companyId,
        Guid branchId,
        Guid userId,
        Guid registeredDeviceId,
        Guid sessionId,
        string changeType,
        DateTimeOffset expiresAt)
    {
        if (companyId == Guid.Empty || branchId == Guid.Empty || userId == Guid.Empty ||
            registeredDeviceId == Guid.Empty || sessionId == Guid.Empty ||
            changeType is not ("BIND" or "RECOVER") || expiresAt <= DateTimeOffset.UtcNow)
            throw new DriverOfflineUnavailableException("DEVICE_KEY_ENROLLMENT_AUTHORITY_INVALID");
        CompanyId = companyId;
        BranchId = branchId;
        UserId = userId;
        RegisteredDeviceId = registeredDeviceId;
        SessionId = sessionId;
        ChangeType = changeType;
        ExpiresAt = expiresAt;
    }

    internal Guid CompanyId { get; }
    internal Guid BranchId { get; }
    internal Guid UserId { get; }
    internal Guid RegisteredDeviceId { get; }
    internal Guid SessionId { get; }
    internal string ChangeType { get; }
    internal DateTimeOffset ExpiresAt { get; }

    internal void Consume()
    {
        if (ExpiresAt <= DateTimeOffset.UtcNow || Interlocked.Exchange(ref _consumed, 1) != 0)
            throw new DriverOfflineUnavailableException("DEVICE_KEY_ENROLLMENT_AUTHORITY_INVALID");
    }
}
