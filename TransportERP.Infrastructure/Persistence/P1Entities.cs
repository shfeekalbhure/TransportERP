namespace TransportERP.Infrastructure.Persistence;

public interface IP1Voucher
{
    Guid CompanyId { get; set; }
    Guid BranchId { get; set; }
    string VoucherNo { get; set; }
    DateTime VoucherDate { get; set; }
    string ReferenceType { get; set; }
    Guid? ReferenceId { get; set; }
    string PaymentMethodCode { get; set; }
    decimal Amount { get; set; }
    Guid CurrencyId { get; set; }
    string Status { get; set; }
    Guid? CashBoxId { get; set; }
    string? Notes { get; set; }
    string? ExternalReference { get; set; }
}

public abstract class P1Entity
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public sealed class Currency : P1Entity
{
    public string Code { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public int MinorUnit { get; set; }
    public bool IsBase { get; set; }
    public string Status { get; set; } = "ACTIVE";
}

public sealed class Company : P1Entity
{
    public string Code { get; set; } = string.Empty;
    public string LegalNameAr { get; set; } = string.Empty;
    public string? LegalNameEn { get; set; }
    public string? TaxIdentifier { get; set; }
    public Guid BaseCurrencyId { get; set; }
    public Guid DefaultCalendarId { get; set; }
    public string Status { get; set; } = "ACTIVE";
    public Currency? BaseCurrency { get; set; }
    public ICollection<Branch> Branches { get; set; } = new List<Branch>();
}

public sealed class Branch : P1Entity
{
    public Guid CompanyId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public string? BranchType { get; set; }
    public string? Address { get; set; }
    public string Timezone { get; set; } = "UTC";
    public string Status { get; set; } = "ACTIVE";
    public Company? Company { get; set; }
}

public sealed class User : P1Entity
{
    public string UserName { get; set; } = string.Empty;
    public string NormalizedUserName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? NormalizedEmail { get; set; }
    public string? Phone { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string Status { get; set; } = "ACTIVE";
    public Guid? CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public DateTimeOffset? LastLoginAt { get; set; }
    public int AccessFailedCount { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public string SecurityStamp { get; set; } = Guid.NewGuid().ToString("N");
    public int AuthVersion { get; set; } = 1;
    public DateTimeOffset? DeletedAt { get; set; }
}

public sealed class AuthSession : P1Entity
{
    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public string Mode { get; set; } = "LOCAL";
    public string SecurityStampAtIssue { get; set; } = string.Empty;
    public int AuthVersionAtIssue { get; set; }
    public string RefreshTokenHash { get; set; } = string.Empty;
    public Guid RefreshTokenFamilyId { get; set; }
    public Guid? ReplacedBySessionId { get; set; }
    public DateTimeOffset IssuedAt { get; set; }
    public DateTimeOffset AccessTokenExpiresAt { get; set; }
    public DateTimeOffset RefreshTokenExpiresAt { get; set; }
    public DateTimeOffset? LastUsedAt { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }
    public string? RevokeReason { get; set; }
    public Guid? RegisteredDeviceId { get; set; }
    public int? DeviceCredentialVersion { get; set; }
}

public sealed class RegisteredDevice : P1Entity
{
    public Guid CompanyId { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public string AppVersion { get; set; } = string.Empty;
    public string? DeviceModel { get; set; }
    public string? OsVersion { get; set; }
    public string RegistrationRequestId { get; set; } = string.Empty;
    public string CredentialHash { get; set; } = string.Empty;
    public int CredentialVersion { get; set; } = 1;
    public string Status { get; set; } = "PENDING";
    public Guid RegisteredByUserId { get; set; }
    public Guid? ApprovedByUserId { get; set; }
    public DateTimeOffset? ApprovedAt { get; set; }
    public DateTimeOffset? SuspendedAt { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }
    public DateTimeOffset? LastSeenAt { get; set; }
}

public sealed class RegisteredDeviceAssignment : P1Entity
{
    public Guid RegisteredDeviceId { get; set; }
    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid BranchId { get; set; }
    public string Status { get; set; } = "ACTIVE";
    public Guid AssignedByUserId { get; set; }
    public Guid? RemovedByUserId { get; set; }
    public DateTimeOffset AssignedAt { get; set; }
    public DateTimeOffset? RemovedAt { get; set; }
}

public sealed class Role : P1Entity
{
    public string Code { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public string? Description { get; set; }
    public bool IsSystem { get; set; }
    public Guid? CompanyId { get; set; }
    public string Status { get; set; } = "ACTIVE";
    public DateTimeOffset? DeletedAt { get; set; }
}

public sealed class Permission : P1Entity
{
    public string Code { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string Resource { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string ScopeType { get; set; } = "PLATFORM";
    public bool IsSystem { get; set; }
    public string Status { get; set; } = "ACTIVE";
    public DateTimeOffset? DeletedAt { get; set; }
}

public sealed class RolePermission
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    public string ScopeType { get; set; } = "PLATFORM";
    public Guid? CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public sealed class UserRole
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public sealed class UserPermissionOverride
{
    public Guid UserId { get; set; }
    public Guid PermissionId { get; set; }
    public bool IsAllowed { get; set; }
    public string? Reason { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public abstract class ScopedSetting : P1Entity
{
    public string Key { get; set; } = string.Empty;
    public string ValueJson { get; set; } = string.Empty;
    public string ValueType { get; set; } = string.Empty;
    public int Version { get; set; }
    public string Status { get; set; } = "ACTIVE";
}

public sealed class GlobalSetting : ScopedSetting
{
    public bool IsSecret { get; set; }
}

public sealed class CompanySetting : ScopedSetting
{
    public Guid CompanyId { get; set; }
}

public sealed class BranchSetting : ScopedSetting
{
    public Guid CompanyId { get; set; }
    public Guid BranchId { get; set; }
}

public sealed class ChartOfAccount : P1Entity
{
    public Guid CompanyId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public Guid? ParentId { get; set; }
    public string AccountType { get; set; } = string.Empty;
    public bool PostingAllowed { get; set; }
    public Guid? CurrencyId { get; set; }
    public string Status { get; set; } = "DRAFT";
    public DateTimeOffset? DeletedAt { get; set; }
}

public sealed class FiscalPeriod : P1Entity
{
    public Guid CompanyId { get; set; }
    public string Code { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = "OPEN";
    public DateTimeOffset? ClosedAt { get; set; }
    public Guid? ClosedBy { get; set; }
}

public sealed class FinancialDimension : P1Entity
{
    public Guid CompanyId { get; set; }
    public string DimensionCode { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
    public string ValueCode { get; set; } = string.Empty;
    public string ValueNameAr { get; set; } = string.Empty;
    public string Status { get; set; } = "ACTIVE";
    public DateTime ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
}

public sealed class JournalEntry : P1Entity
{
    public Guid CompanyId { get; set; }
    public Guid BranchId { get; set; }
    public string DocumentNo { get; set; } = string.Empty;
    public Guid FiscalPeriodId { get; set; }
    public DateTime EntryDate { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = "DRAFT";
    public string SourceType { get; set; } = string.Empty;
    public Guid? SourceId { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public Guid CurrencyId { get; set; }
    public decimal ExchangeRate { get; set; }
    public Guid? ReversalOfId { get; set; }
    public ICollection<JournalEntryLine> Lines { get; set; } = new List<JournalEntryLine>();
}

public sealed class JournalEntryLine
{
    public Guid JournalEntryId { get; set; }
    public int LineNo { get; set; }
    public Guid AccountId { get; set; }
    public Guid? FinancialDimensionId { get; set; }
    public string? Description { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public decimal ForeignAmount { get; set; }
    public Guid CurrencyId { get; set; }
}

public sealed class ReceiptVoucher : P1Entity, IP1Voucher
{
    public Guid CompanyId { get; set; }
    public Guid BranchId { get; set; }
    public string VoucherNo { get; set; } = string.Empty;
    public DateTime VoucherDate { get; set; }
    public string PayerName { get; set; } = string.Empty;
    public string ReferenceType { get; set; } = string.Empty;
    public Guid? ReferenceId { get; set; }
    public string PaymentMethodCode { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public Guid CurrencyId { get; set; }
    public string Status { get; set; } = "DRAFT";
    public Guid CollectedBy { get; set; }
    public Guid? CashBoxId { get; set; }
    public string? Notes { get; set; }
    public string? ExternalReference { get; set; }
}

public sealed class PaymentVoucher : P1Entity, IP1Voucher
{
    public Guid CompanyId { get; set; }
    public Guid BranchId { get; set; }
    public string VoucherNo { get; set; } = string.Empty;
    public DateTime VoucherDate { get; set; }
    public string PayeeName { get; set; } = string.Empty;
    public string ReferenceType { get; set; } = string.Empty;
    public Guid? ReferenceId { get; set; }
    public string PaymentMethodCode { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public Guid CurrencyId { get; set; }
    public string Status { get; set; } = "DRAFT";
    public Guid PaidBy { get; set; }
    public Guid? CashBoxId { get; set; }
    public string? Notes { get; set; }
    public string? ExternalReference { get; set; }
}

public sealed class AuditEvent
{
    public Guid Id { get; set; }
    public long SequenceNo { get; set; }
    public DateTimeOffset OccurredAt { get; set; }
    public Guid? ActorUserId { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Outcome { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public Guid? EntityId { get; set; }
    public Guid CorrelationId { get; set; }
    public Guid? OperationCorrelationId { get; set; }
    public string? DeviceId { get; set; }
    public string? BeforeJson { get; set; }
    public string? AfterJson { get; set; }
    public string? Reason { get; set; }
    public string? Ip { get; set; }
    public string Hash { get; set; } = string.Empty;
    public string? PreviousHash { get; set; }
}

public sealed class AuditStreamHead
{
    public string StreamKey { get; set; } = string.Empty;
    public string? LastHash { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public sealed class ConflictCase : P1Entity
{
    public Guid SyncOperationId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public long? BaseVersion { get; set; }
    public string DeviceSnapshot { get; set; } = string.Empty;
    public string ServerSnapshot { get; set; } = string.Empty;
    public string ConflictReason { get; set; } = string.Empty;
    public string? Resolution { get; set; }
    public string? ResolvedBy { get; set; }
    public DateTimeOffset? ResolvedAt { get; set; }
    public Guid? ReplacedByOperationId { get; set; }
    public string Status { get; set; } = "OPEN";
    public SyncOperation? SyncOperation { get; set; }
}

public sealed class SyncOperation : P1Entity
{
    public string DeviceId { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public string OperationType { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public Guid? EntityId { get; set; }
    public Guid? ResultEntityId { get; set; }
    public string ClientOperationId { get; set; } = string.Empty;
    public string PayloadJson { get; set; } = string.Empty;
    public string PayloadHash { get; set; } = string.Empty;
    public DateTimeOffset ClientOccurredAt { get; set; }
    public DateTimeOffset? ServerReceivedAt { get; set; }
    public long? BaseVersion { get; set; }
    public long? ResultVersion { get; set; }
    public string Status { get; set; } = "QUEUED";
    public int RetryCount { get; set; }
    public DateTimeOffset? NextRetryAt { get; set; }
    public string? ErrorCode { get; set; }
    public Guid? RegisteredDeviceId { get; set; }
    public int? RegisteredDeviceCredentialVersion { get; set; }
    public string? ActionCode { get; set; }
    public string? ProtocolVersion { get; set; }
    public Guid? OperationCorrelationId { get; set; }
    public string? RequestFingerprintVersion { get; set; }
    public byte[]? RequestFingerprintHash { get; set; }
    public int? ProofKeyVersion { get; set; }
    public string? ProofKeyThumbprint { get; set; }
    public Guid? AcceptedProofReplayId { get; set; }
    public ConflictCase? ConflictCase { get; set; }
}

public sealed class SyncProofNonce
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid RegisteredDeviceId { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public int ProofKeyVersion { get; set; }
    public byte[] NonceHash { get; set; } = [];
    public DateTimeOffset IssuedAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
}

public sealed class SyncProofReplay
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid RegisteredDeviceId { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public Guid DeviceAssignmentId { get; set; }
    public Guid UserId { get; set; }
    public Guid BranchId { get; set; }
    public int ProofKeyVersion { get; set; }
    public string ProofKeyThumbprint { get; set; } = string.Empty;
    public byte[] JtiHash { get; set; } = [];
    public byte[] HtuHash { get; set; } = [];
    public string HttpMethod { get; set; } = string.Empty;
    public Guid NonceRecordId { get; set; }
    public DateTimeOffset IssuedAt { get; set; }
    public DateTimeOffset FirstSeenAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public Guid AttemptCorrelationId { get; set; }
}
