namespace TransportERP.Api.Security;

public sealed record DeviceTrustRequest(
    Guid UserId,
    Guid CompanyId,
    Guid? BranchId,
    Guid? SessionId,
    string RequestedDeviceId,
    string? ClaimedDeviceSelector);

public sealed record DeviceTrustResolution(bool IsTrusted, string FailureCode)
{
    public static DeviceTrustResolution Trusted() => new(true, string.Empty);
    public static DeviceTrustResolution Denied(string failureCode = "DEVICE_NOT_REGISTERED")
        => new(false, failureCode);
}

/// <summary>
/// Server-authoritative device/session trust boundary. JWT device claims are
/// narrowing selectors only and cannot establish enrollment, ownership or
/// proof-of-possession.
/// </summary>
public interface IDeviceTrustAuthority
{
    Task<DeviceTrustResolution> ResolveAsync(
        DeviceTrustRequest request,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Production-safe default while DBP-003/006 persistent registry, session and
/// proof adapters remain unapproved. Tests may replace this service explicitly.
/// </summary>
public sealed class DefaultDenyDeviceTrustAuthority : IDeviceTrustAuthority
{
    public Task<DeviceTrustResolution> ResolveAsync(
        DeviceTrustRequest request,
        CancellationToken cancellationToken = default)
        => Task.FromResult(DeviceTrustResolution.Denied());
}
