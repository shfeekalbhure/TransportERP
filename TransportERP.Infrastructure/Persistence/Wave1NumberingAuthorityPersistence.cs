using System.Data;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Numbering;
using TransportERP.Contracts.Wave1;

namespace TransportERP.Infrastructure.Persistence;

public sealed class Wave1NumberSequenceRecord
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string? Prefix { get; set; }
    public long NextValue { get; set; } = 1;
    public string ResetPolicy { get; set; } = "NONE";
    public string Status { get; set; } = "ACTIVE";
    public long Version { get; set; } = 1;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public sealed class Wave1NumberSequenceMetadataRecord
{
    public Guid SequenceId { get; set; }
    public Guid CompanyId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? ArabicName { get; set; }
    public string? EnglishName { get; set; }
    public string? Notes { get; set; }
    public Guid? FiscalYearId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public sealed class Wave1NumberReservationRecord
{
    public Guid Id { get; set; }
    public Guid SequenceId { get; set; }
    public Guid? WaybillId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public string IdempotencyKey { get; set; } = string.Empty;
    public long NumberValue { get; set; }
    public string RenderedNumber { get; set; } = string.Empty;
    public DateTimeOffset ReservedAt { get; set; }
    public DateTimeOffset? CommittedAt { get; set; }
    public DateTimeOffset? VoidedAt { get; set; }
    public string? VoidReason { get; set; }
    public string State { get; set; } = NumberReservationStates.Reserved;
    public string? LastTransitionKey { get; set; }
}

public sealed class Wave1ApprovalRequestRecord
{
    public Guid Id { get; set; }
    public string TargetType { get; set; } = string.Empty;
    public Guid TargetId { get; set; }
    public string RequestedAction { get; set; } = string.Empty;
    public string Status { get; set; } = "PENDING";
    public string? Reason { get; set; }
    public Guid RequestedBy { get; set; }
    public DateTimeOffset RequestedAt { get; set; }
    public long TargetExpectedVersion { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public long Version { get; set; } = 1;
    public DateTimeOffset UpdatedAt { get; set; }
}

public sealed class Wave1ApprovalActionRecord
{
    public Guid Id { get; set; }
    public Guid ApprovalRequestId { get; set; }
    public string Decision { get; set; } = string.Empty;
    public Guid DecidedBy { get; set; }
    public DateTimeOffset DecidedAt { get; set; }
    public string? Reason { get; set; }
}

public static class Wave1NumberingApprovalContract
{
    public const string TargetType = "NumberSequence";
    public const string RequestedAction = "Override/Reset";
    public const string ApprovedStatus = "APPROVED";
    public const string ApproveDecision = "APPROVE";
}

public sealed class Wave1NumberingAuthorityDbContext(DbContextOptions<Wave1NumberingAuthorityDbContext> options) : DbContext(options)
{
    public DbSet<Wave1NumberSequenceRecord> Sequences => Set<Wave1NumberSequenceRecord>();
    public DbSet<Wave1NumberSequenceMetadataRecord> Metadata => Set<Wave1NumberSequenceMetadataRecord>();
    public DbSet<Wave1NumberReservationRecord> Reservations => Set<Wave1NumberReservationRecord>();
    public DbSet<Wave1ApprovalRequestRecord> ApprovalRequests => Set<Wave1ApprovalRequestRecord>();
    public DbSet<Wave1ApprovalActionRecord> ApprovalActions => Set<Wave1ApprovalActionRecord>();
    public DbSet<AuditEvent> AuditEvents => Set<AuditEvent>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.HasDefaultSchema("transport_erp");
        var s = mb.Entity<Wave1NumberSequenceRecord>();
        s.ToTable("number_sequences"); s.HasKey(x => x.Id);
        s.Property(x => x.DocumentType).HasMaxLength(60).IsRequired(); s.Property(x => x.Prefix).HasMaxLength(30);
        s.Property(x => x.ResetPolicy).HasMaxLength(40).IsRequired(); s.Property(x => x.Status).HasMaxLength(20).IsRequired();
        s.Property(x => x.Version).IsConcurrencyToken(); s.Property(x => x.CreatedAt).HasColumnType("timestamptz"); s.Property(x => x.UpdatedAt).HasColumnType("timestamptz");

        var meta = mb.Entity<Wave1NumberSequenceMetadataRecord>();
        meta.ToTable("number_sequence_metadata"); meta.HasKey(x => x.SequenceId);
        meta.Property(x => x.Code).HasMaxLength(60).IsRequired(); meta.Property(x => x.ArabicName).HasMaxLength(200);
        meta.Property(x => x.EnglishName).HasMaxLength(200); meta.Property(x => x.Notes).HasMaxLength(1000);
        meta.Property(x => x.CreatedAt).HasColumnType("timestamptz"); meta.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        meta.HasIndex(x => new { x.CompanyId, x.Code }).IsUnique();
        meta.HasOne<Wave1NumberSequenceRecord>().WithOne().HasForeignKey<Wave1NumberSequenceMetadataRecord>(x => x.SequenceId).OnDelete(DeleteBehavior.Cascade);

        var r = mb.Entity<Wave1NumberReservationRecord>();
        r.ToTable("number_reservations"); r.HasKey(x => x.Id); r.Property(x => x.IdempotencyKey).HasMaxLength(160).IsRequired();
        r.Property(x => x.RenderedNumber).HasMaxLength(60).IsRequired(); r.Property(x => x.State).HasMaxLength(20).IsRequired();
        r.Property(x => x.ReservedAt).HasColumnType("timestamptz"); r.Property(x => x.CommittedAt).HasColumnType("timestamptz"); r.Property(x => x.VoidedAt).HasColumnType("timestamptz");
        r.HasIndex(x => new { x.SequenceId, x.NumberValue }).IsUnique(); r.HasIndex(x => new { x.CompanyId, x.IdempotencyKey }).IsUnique();
        r.HasOne<Wave1NumberSequenceRecord>().WithMany().HasForeignKey(x => x.SequenceId).OnDelete(DeleteBehavior.Restrict);

        var approval = mb.Entity<Wave1ApprovalRequestRecord>();
        approval.ToTable("approval_requests"); approval.HasKey(x => x.Id);
        approval.Property(x => x.TargetType).HasMaxLength(120).IsRequired(); approval.Property(x => x.RequestedAction).HasMaxLength(120).IsRequired();
        approval.Property(x => x.Status).HasMaxLength(20).IsRequired(); approval.Property(x => x.Reason).HasMaxLength(1000);
        approval.Property(x => x.RequestedAt).HasColumnType("timestamptz"); approval.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        approval.Property(x => x.Version).IsConcurrencyToken();
        approval.HasIndex(x => new { x.CompanyId, x.TargetType, x.TargetId, x.Status });

        var approvalAction = mb.Entity<Wave1ApprovalActionRecord>();
        approvalAction.ToTable("approval_actions"); approvalAction.HasKey(x => x.Id);
        approvalAction.Property(x => x.Decision).HasMaxLength(20).IsRequired(); approvalAction.Property(x => x.Reason).HasMaxLength(1000);
        approvalAction.Property(x => x.DecidedAt).HasColumnType("timestamptz");
        approvalAction.HasIndex(x => new { x.ApprovalRequestId, x.DecidedAt });
        approvalAction.HasOne<Wave1ApprovalRequestRecord>().WithMany().HasForeignKey(x => x.ApprovalRequestId).OnDelete(DeleteBehavior.Restrict);

        var a = mb.Entity<AuditEvent>(); a.ToTable("audit_events"); a.HasKey(x => x.Id);
        a.Property(x => x.OccurredAt).HasColumnType("timestamptz"); a.Property(x => x.Action).HasMaxLength(120).IsRequired();
        a.Property(x => x.Outcome).HasMaxLength(40).IsRequired(); a.Property(x => x.EntityType).HasMaxLength(120).IsRequired();
        a.Property(x => x.Hash).HasMaxLength(128).IsRequired(); a.Property(x => x.PreviousHash).HasMaxLength(128);
    }
}

public sealed class Wave1NumberingAuthorityService(Wave1NumberingAuthorityDbContext db)
{
    public async Task<IReadOnlyList<NumberSequenceDto>> ListAsync(OperationContext context, CancellationToken ct = default)
    {
        context.EnsureComplete();
        var sequences = await Scoped(context).AsNoTracking().OrderBy(x => x.DocumentType).ThenBy(x => x.BranchId).ToListAsync(ct);
        var ids = sequences.Select(x => x.Id).ToArray();
        var metadata = await db.Metadata.AsNoTracking().Where(x => ids.Contains(x.SequenceId)).ToDictionaryAsync(x => x.SequenceId, ct);
        var maxes = await db.Reservations.AsNoTracking().Where(x => ids.Contains(x.SequenceId))
            .GroupBy(x => x.SequenceId).Select(g => new { SequenceId = g.Key, Max = g.Max(x => x.NumberValue) }).ToDictionaryAsync(x => x.SequenceId, x => x.Max, ct);
        return sequences.Select(x => ToDto(x, metadata.GetValueOrDefault(x.Id), maxes.GetValueOrDefault(x.Id))).ToArray();
    }

    public Task<NumberSequenceDto?> UpdateAsync(OperationContext context, Guid id, UpdateNumberSequenceRequest request, CancellationToken ct = default)
        => ExecuteAsync(async () =>
        {
            context.EnsureComplete();
            if (string.IsNullOrWhiteSpace(request.Reason)) throw new ArgumentException("REASON_REQUIRED");
            var sequence = await Scoped(context).SingleOrDefaultAsync(x => x.Id == id, ct);
            if (sequence is null) return null;
            if (sequence.Version != request.ExpectedVersion) throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");
            var meta = await db.Metadata.SingleOrDefaultAsync(x => x.SequenceId == id, ct);
            var code = Required(request.Code ?? meta?.Code, 60, "INVALID_CODE").ToUpperInvariant();
            var arabic = Required(request.ArabicName ?? meta?.ArabicName, 200, "INVALID_ARABIC_NAME");
            var english = Optional(request.EnglishName ?? meta?.EnglishName, 200, "INVALID_ENGLISH_NAME");
            var notes = Optional(request.Notes ?? meta?.Notes, 1000, "INVALID_NOTES");
            if (await db.Metadata.AnyAsync(x => x.SequenceId != id && x.CompanyId == sequence.CompanyId && x.Code == code, ct)) throw new ArgumentException("DUPLICATE_CODE");
            var before = JsonSerializer.Serialize(await BuildDto(sequence, meta, ct));
            sequence.Prefix = Optional(request.Prefix, 30, "INVALID_PREFIX");
            sequence.ResetPolicy = Required(request.ResetPolicy, 40, "INVALID_RESET_POLICY").ToUpperInvariant();
            sequence.Status = NormalizeStatus(request.Status); sequence.Version++; sequence.UpdatedAt = DateTimeOffset.UtcNow;
            if (meta is null)
            {
                meta = new Wave1NumberSequenceMetadataRecord { SequenceId = id, CompanyId = sequence.CompanyId, CreatedAt = DateTimeOffset.UtcNow };
                db.Metadata.Add(meta);
            }
            meta.Code = code; meta.ArabicName = arabic; meta.EnglishName = english; meta.Notes = notes; meta.FiscalYearId = request.FiscalYearId; meta.UpdatedAt = DateTimeOffset.UtcNow;
            await AppendAudit(context, "NumberSequence.Update", id, before, null, request.Reason.Trim(), ct);
            await db.SaveChangesAsync(ct);
            return await BuildDto(sequence, meta, ct);
        }, ct);

    public Task<NumberReservationDto> ReserveAsync(OperationContext context, Guid sequenceId, NumberReservationCommandRequest request, CancellationToken ct = default)
        => ExecuteRequiredAsync(async () =>
        {
            context.EnsureComplete();
            if (string.IsNullOrWhiteSpace(request.IdempotencyKey)) throw new ArgumentException("IDEMPOTENCY_KEY_REQUIRED");
            var key = request.IdempotencyKey.Trim();
            var existing = await db.Reservations.SingleOrDefaultAsync(x => x.CompanyId == context.CompanyId && x.IdempotencyKey == key, ct);
            if (existing is not null)
            {
                if (existing.SequenceId != sequenceId) throw new InvalidOperationException("IDEMPOTENCY_CONFLICT");
                return ReservationDto(existing);
            }
            var sequence = await Scoped(context).SingleOrDefaultAsync(x => x.Id == sequenceId && x.Status == "ACTIVE", ct)
                ?? throw new KeyNotFoundException("NOT_FOUND");
            if (sequence.NextValue < 1) throw new InvalidOperationException("NUMBER_SEQUENCE_INACTIVE");
            var value = sequence.NextValue;
            sequence.NextValue = checked(value + 1); sequence.Version++; sequence.UpdatedAt = DateTimeOffset.UtcNow;
            var row = new Wave1NumberReservationRecord
            {
                Id = Guid.NewGuid(), SequenceId = sequence.Id, CompanyId = context.CompanyId, BranchId = sequence.BranchId ?? context.BranchId,
                IdempotencyKey = key, NumberValue = value, RenderedNumber = $"{sequence.Prefix}{value:D8}", ReservedAt = DateTimeOffset.UtcNow,
                State = NumberReservationStates.Reserved
            };
            db.Reservations.Add(row);
            await AppendAudit(context, "NumberSequence.Reserve", row.Id, null, JsonSerializer.Serialize(ReservationDto(row)), request.Reason, ct);
            await db.SaveChangesAsync(ct);
            return ReservationDto(row);
        }, ct);

    public Task<NumberReservationDto> CommitAsync(OperationContext context, Guid reservationId, NumberReservationTransitionCommandRequest request, CancellationToken ct = default)
        => Transition(context, reservationId, request, NumberReservationStates.Committed, ct);

    public Task<NumberReservationDto> CancelAsync(OperationContext context, Guid reservationId, NumberReservationTransitionCommandRequest request, CancellationToken ct = default)
        => Transition(context, reservationId, request, NumberReservationStates.Void, ct);

    public Task<NumberSequenceDto?> ProtectedActionAsync(OperationContext context, Guid id, NumberingProtectedActionRequest request, CancellationToken ct = default)
        => ExecuteAsync(async () =>
        {
            context.EnsureComplete();
            if (request.LastNumber < 0) throw new ArgumentException("INVALID_LAST_NUMBER");
            if (string.IsNullOrWhiteSpace(request.Reason)) throw new ArgumentException("REASON_REQUIRED");
            if (request.ApprovalRequestId == Guid.Empty) throw new InvalidOperationException("APPROVAL_STATE_INVALID");

            var sequence = await Scoped(context).SingleOrDefaultAsync(x => x.Id == id, ct);
            if (sequence is null) return null;
            if (sequence.Version != request.ExpectedVersion) throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");
            if (!string.Equals(sequence.Status, "ACTIVE", StringComparison.Ordinal)) throw new InvalidOperationException("NUMBERING_STATE_INVALID");

            var approval = await db.ApprovalRequests.AsNoTracking().SingleOrDefaultAsync(x =>
                x.Id == request.ApprovalRequestId &&
                x.CompanyId == context.CompanyId &&
                x.TargetType == Wave1NumberingApprovalContract.TargetType &&
                x.TargetId == id &&
                x.RequestedAction == Wave1NumberingApprovalContract.RequestedAction, ct);
            if (approval is null ||
                !string.Equals(approval.Status, Wave1NumberingApprovalContract.ApprovedStatus, StringComparison.Ordinal) ||
                approval.TargetExpectedVersion != request.ExpectedVersion ||
                (sequence.BranchId.HasValue ? approval.BranchId != sequence.BranchId : approval.BranchId.HasValue))
                throw new InvalidOperationException("APPROVAL_STATE_INVALID");

            var approvedAction = await db.ApprovalActions.AsNoTracking()
                .Where(x => x.ApprovalRequestId == approval.Id && x.Decision == Wave1NumberingApprovalContract.ApproveDecision)
                .OrderByDescending(x => x.DecidedAt).ThenByDescending(x => x.Id)
                .FirstOrDefaultAsync(ct);
            if (approvedAction is null) throw new InvalidOperationException("APPROVAL_STATE_INVALID");

            var allocated = await db.Reservations.AsNoTracking().Where(x => x.SequenceId == id).Select(x => (long?)x.NumberValue).MaxAsync(ct) ?? 0;
            var currentLast = Math.Max(sequence.NextValue - 1, allocated);
            if (request.LastNumber < currentLast) throw new InvalidOperationException("NUMBER_REUSE_FORBIDDEN");

            var before = JsonSerializer.Serialize(await BuildDto(sequence, null, ct));
            sequence.NextValue = checked(request.LastNumber + 1); sequence.Version++; sequence.UpdatedAt = DateTimeOffset.UtcNow;
            var after = await BuildDto(sequence, null, ct);
            await AppendAudit(context, "NumberSequence.ProtectedAction", id, before, JsonSerializer.Serialize(new
            {
                Result = after,
                Approval = new
                {
                    ApprovalRequestId = approval.Id,
                    ApprovalRequestVersion = approval.Version,
                    approval.RequestedAction,
                    ApprovedBy = approvedAction.DecidedBy,
                    ApprovedAt = approvedAction.DecidedAt,
                    Decision = approvedAction.Decision
                }
            }), request.Reason.Trim(), ct);
            await db.SaveChangesAsync(ct);
            return after;
        }, ct);

    private Task<NumberReservationDto> Transition(OperationContext context, Guid id, NumberReservationTransitionCommandRequest request, string target, CancellationToken ct)
        => ExecuteRequiredAsync(async () =>
        {
            context.EnsureComplete();
            if (string.IsNullOrWhiteSpace(request.IdempotencyKey)) throw new ArgumentException("IDEMPOTENCY_KEY_REQUIRED");
            var row = await db.Reservations.SingleOrDefaultAsync(x => x.Id == id && x.CompanyId == context.CompanyId && (x.BranchId == null || x.BranchId == context.BranchId), ct)
                ?? throw new KeyNotFoundException("NOT_FOUND");
            if (row.State == target) return ReservationDto(row);
            if (row.State != NumberReservationStates.Reserved) throw new InvalidOperationException("NUMBERING_STATE_INVALID");
            row.State = target; row.LastTransitionKey = request.IdempotencyKey.Trim();
            if (target == NumberReservationStates.Committed) row.CommittedAt = DateTimeOffset.UtcNow;
            else { row.VoidedAt = DateTimeOffset.UtcNow; row.VoidReason = request.Reason; }
            await AppendAudit(context, target == NumberReservationStates.Committed ? "NumberReservation.Commit" : "NumberReservation.Cancel", row.Id, null, JsonSerializer.Serialize(ReservationDto(row)), request.Reason, ct);
            await db.SaveChangesAsync(ct); return ReservationDto(row);
        }, ct);

    private IQueryable<Wave1NumberSequenceRecord> Scoped(OperationContext context)
        => db.Sequences.Where(x => x.CompanyId == context.CompanyId && (x.BranchId == null || x.BranchId == context.BranchId));

    private async Task<NumberSequenceDto> BuildDto(Wave1NumberSequenceRecord sequence, Wave1NumberSequenceMetadataRecord? meta, CancellationToken ct)
    {
        meta ??= await db.Metadata.AsNoTracking().SingleOrDefaultAsync(x => x.SequenceId == sequence.Id, ct);
        var allocated = await db.Reservations.AsNoTracking().Where(x => x.SequenceId == sequence.Id).Select(x => (long?)x.NumberValue).MaxAsync(ct) ?? 0;
        return ToDto(sequence, meta, allocated);
    }

    private static NumberSequenceDto ToDto(Wave1NumberSequenceRecord x, Wave1NumberSequenceMetadataRecord? m, long maximumAllocated)
        => new(x.Id, x.CompanyId, x.BranchId, x.DocumentType, x.Prefix, x.NextValue, x.ResetPolicy, x.Status, x.Version)
        {
            Code = m?.Code ?? x.DocumentType,
            ArabicName = m?.ArabicName,
            EnglishName = m?.EnglishName,
            Notes = m?.Notes,
            FiscalYearId = m?.FiscalYearId,
            Scope = Scope(x, m),
            LastNumber = Math.Max(Math.Max(x.NextValue - 1, maximumAllocated), 0)
        };

    private static string Scope(Wave1NumberSequenceRecord x, Wave1NumberSequenceMetadataRecord? m)
    {
        var dimensions = new List<string> { "COMPANY" };
        if (x.BranchId.HasValue) dimensions.Add("BRANCH");
        if (m?.FiscalYearId.HasValue == true) dimensions.Add("FISCAL_YEAR");
        dimensions.Add("DOCUMENT_TYPE");
        return string.Join('+', dimensions);
    }

    private async Task AppendAudit(OperationContext context, string action, Guid id, string? before, string? after, string? reason, CancellationToken ct)
    {
        var previous = await db.AuditEvents.AsNoTracking().Where(x => x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.DeviceId == null)
            .OrderByDescending(x => x.OccurredAt).ThenByDescending(x => x.Id).Select(x => x.Hash).FirstOrDefaultAsync(ct);
        var evt = new AuditEvent { Id = Guid.NewGuid(), OccurredAt = DateTimeOffset.UtcNow, ActorUserId = context.UserId, CompanyId = context.CompanyId,
            BranchId = context.BranchId, Action = action, Outcome = "SUCCESS", EntityType = action.StartsWith("NumberSequence", StringComparison.Ordinal) ? "NumberSequence" : "NumberReservation",
            EntityId = id, CorrelationId = context.CorrelationId, BeforeJson = before, AfterJson = after, Reason = string.IsNullOrWhiteSpace(reason) ? null : reason.Trim(), PreviousHash = previous, Hash = string.Empty };
        evt.Hash = AuditEventService.ComputeHash(evt); db.AuditEvents.Add(evt);
    }

    private async Task<IDbContextTransaction?> Begin(CancellationToken ct)
        => db.Database.IsRelational() ? await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct) : null;
    private async Task<T?> ExecuteAsync<T>(Func<Task<T?>> action, CancellationToken ct)
    { await using var tx = await Begin(ct); try { var value = await action(); if (tx is not null) await tx.CommitAsync(ct); return value; } catch { if (tx is not null) await tx.RollbackAsync(ct); throw; } }
    private async Task<T> ExecuteRequiredAsync<T>(Func<Task<T>> action, CancellationToken ct)
    { await using var tx = await Begin(ct); try { var value = await action(); if (tx is not null) await tx.CommitAsync(ct); return value; } catch { if (tx is not null) await tx.RollbackAsync(ct); throw; } }
    private static string NormalizeStatus(string value) { var x = Required(value, 20, "INVALID_STATUS").ToUpperInvariant(); return x is "ACTIVE" or "INACTIVE" ? x : throw new ArgumentException("INVALID_STATUS"); }
    private static string Required(string? value, int max, string code) { if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException(code); var x = value.Trim(); if (x.Length > max) throw new ArgumentException(code); return x; }
    private static string? Optional(string? value, int max, string code) { if (string.IsNullOrWhiteSpace(value)) return null; var x = value.Trim(); if (x.Length > max) throw new ArgumentException(code); return x; }
    private static NumberReservationDto ReservationDto(Wave1NumberReservationRecord x)
        => new(x.Id, x.SequenceId, checked((ulong)x.NumberValue), x.RenderedNumber, x.State);
}

[DbContext(typeof(Wave1NumberingAuthorityDbContext))]
[Migration("20260823002000_Wave1NumberingMetadata")]
public sealed class Wave1NumberingMetadata : Migration
{
    protected override void Up(MigrationBuilder m)
    {
        m.CreateTable(name: "number_sequence_metadata", schema: "transport_erp", columns: t => new
        {
            SequenceId = t.Column<Guid>(type: "uuid", nullable: false), CompanyId = t.Column<Guid>(type: "uuid", nullable: false),
            Code = t.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
            ArabicName = t.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
            EnglishName = t.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
            Notes = t.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
            FiscalYearId = t.Column<Guid>(type: "uuid", nullable: true), CreatedAt = t.Column<DateTimeOffset>(type: "timestamptz", nullable: false), UpdatedAt = t.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
        }, constraints: t =>
        {
            t.PrimaryKey("PK_number_sequence_metadata", x => x.SequenceId);
            t.ForeignKey("FK_number_sequence_metadata_number_sequences_SequenceId", x => x.SequenceId, "transport_erp", "number_sequences", "Id", onDelete: ReferentialAction.Cascade);
        });
        m.CreateIndex(name: "IX_number_sequence_metadata_CompanyId_Code", schema: "transport_erp", table: "number_sequence_metadata", columns: new[] { "CompanyId", "Code" }, unique: true);
        // Legacy DocumentType is a technical sequence identifier and can seed Code. ArabicName remains unknown until governed reconciliation/touch; it is never guessed.
        m.Sql("INSERT INTO transport_erp.number_sequence_metadata (\"SequenceId\",\"CompanyId\",\"Code\",\"ArabicName\",\"EnglishName\",\"Notes\",\"FiscalYearId\",\"CreatedAt\",\"UpdatedAt\") SELECT \"Id\",\"CompanyId\",\"DocumentType\",NULL,NULL,NULL,NULL,COALESCE(\"CreatedAt\",NOW()),COALESCE(\"UpdatedAt\",NOW()) FROM transport_erp.number_sequences ON CONFLICT (\"SequenceId\") DO NOTHING;");
    }
    protected override void Down(MigrationBuilder m) => m.DropTable(name: "number_sequence_metadata", schema: "transport_erp");
}

[DbContext(typeof(Wave1NumberingAuthorityDbContext))]
[Migration("20260823002100_Wave1NumberingApprovalBinding")]
public sealed class Wave1NumberingApprovalBinding : Migration
{
    protected override void Up(MigrationBuilder m)
    {
        m.CreateTable(name: "approval_requests", schema: "transport_erp", columns: t => new
        {
            Id = t.Column<Guid>(type: "uuid", nullable: false),
            TargetType = t.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
            TargetId = t.Column<Guid>(type: "uuid", nullable: false),
            RequestedAction = t.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
            Status = t.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
            Reason = t.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
            RequestedBy = t.Column<Guid>(type: "uuid", nullable: false),
            RequestedAt = t.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
            TargetExpectedVersion = t.Column<long>(type: "bigint", nullable: false),
            CompanyId = t.Column<Guid>(type: "uuid", nullable: false),
            BranchId = t.Column<Guid>(type: "uuid", nullable: true),
            Version = t.Column<long>(type: "bigint", nullable: false, defaultValue: 1L),
            UpdatedAt = t.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
        }, constraints: t =>
        {
            t.PrimaryKey("PK_approval_requests", x => x.Id);
            t.CheckConstraint("ck_approval_requests_status", "\"Status\" IN ('PENDING','APPROVED','REJECTED','RETURNED','CANCELLED')");
        });
        m.CreateIndex(name: "IX_approval_requests_Target", schema: "transport_erp", table: "approval_requests", columns: new[] { "CompanyId", "TargetType", "TargetId", "Status" });

        m.CreateTable(name: "approval_actions", schema: "transport_erp", columns: t => new
        {
            Id = t.Column<Guid>(type: "uuid", nullable: false),
            ApprovalRequestId = t.Column<Guid>(type: "uuid", nullable: false),
            Decision = t.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
            DecidedBy = t.Column<Guid>(type: "uuid", nullable: false),
            DecidedAt = t.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
            Reason = t.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
        }, constraints: t =>
        {
            t.PrimaryKey("PK_approval_actions", x => x.Id);
            t.ForeignKey("FK_approval_actions_approval_requests", x => x.ApprovalRequestId, "transport_erp", "approval_requests", "Id", onDelete: ReferentialAction.Restrict);
            t.CheckConstraint("ck_approval_actions_decision", "\"Decision\" IN ('APPROVE','REJECT','RETURN','CANCEL')");
        });
        m.CreateIndex(name: "IX_approval_actions_Request_Time", schema: "transport_erp", table: "approval_actions", columns: new[] { "ApprovalRequestId", "DecidedAt" });
    }

    protected override void Down(MigrationBuilder m)
    {
        m.DropTable(name: "approval_actions", schema: "transport_erp");
        m.DropTable(name: "approval_requests", schema: "transport_erp");
    }
}
