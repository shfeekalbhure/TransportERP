namespace TransportERP.Contracts.Identity;

public sealed record RegisterDeviceRequest(
    string DeviceId,
    string DisplayName,
    string Platform,
    string AppVersion,
    string? DeviceModel,
    string? OsVersion,
    string RegistrationRequestId,
    string Credential);

public sealed record DeviceStatusRequest(string? Reason = null);
public sealed record AddDeviceAssignmentRequest(Guid UserId, Guid BranchId);
public sealed record RotateDeviceCredentialRequest(string Credential, int ExpectedCredentialVersion);

public sealed record RegisteredDeviceResponse(
    Guid Id, Guid CompanyId, string DeviceId, string DisplayName, string Platform, string AppVersion,
    string? DeviceModel, string? OsVersion, int CredentialVersion,
    string Status, DateTimeOffset? LastSeenAt, DateTimeOffset? ExpiresAt,
    DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);

public sealed record RegisteredDeviceAssignmentResponse(
    Guid Id, Guid RegisteredDeviceId, Guid UserId, Guid CompanyId, Guid BranchId,
    string Status, DateTimeOffset AssignedAt, DateTimeOffset? RemovedAt);
