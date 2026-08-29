namespace TransportERP.Infrastructure.Persistence;

public sealed class AuditStreamHead
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public string StreamKey { get; set; } = string.Empty;
    public long LastSequence { get; set; }
    public byte[]? LastHashV2 { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public long ConcurrencyVersion { get; set; } = 1;
}

public sealed class IntegrationOutbox
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public Guid OperationId { get; set; }
    public string Topic { get; set; } = string.Empty;
    public short ContractVersion { get; set; } = 1;
    public string PayloadJson { get; set; } = string.Empty;
    public byte[] PayloadSha256 { get; set; } = [];
    public DateTimeOffset OccurredAt { get; set; }
    public DateTimeOffset AvailableAt { get; set; }
    public string Status { get; set; } = "PENDING";
    public int AttemptCount { get; set; }
    public string? LeaseOwner { get; set; }
    public Guid? LeaseToken { get; set; }
    public DateTimeOffset? LeaseExpiresAt { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }
    public string? LastError { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public long ConcurrencyVersion { get; set; } = 1;
}
