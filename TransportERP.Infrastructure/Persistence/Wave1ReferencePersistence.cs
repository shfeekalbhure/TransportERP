using System.Data;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Wave1;

namespace TransportERP.Infrastructure.Persistence;

public abstract class Wave1ReferenceEntity
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; } = true;
    public long Version { get; set; } = 1;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public sealed class Wave1LanguageEntity
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string CultureCode { get; set; } = string.Empty;
    public string Direction { get; set; } = "LTR";
    public bool IsActive { get; set; } = true;
    public int Version { get; set; } = 1;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public sealed class Wave1AccountClassificationEntity : Wave1ReferenceEntity
{
    public Guid CompanyId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string ArabicName { get; set; } = string.Empty;
    public string? EnglishName { get; set; }
    public string AccountType { get; set; } = string.Empty;
}

public sealed class Wave1AccountingOpenItemEntity
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid BranchId { get; set; }
    public Guid PartyId { get; set; }
    public string PartyCode { get; set; } = string.Empty;
    public string PartyName { get; set; } = string.Empty;
    public string Side { get; set; } = string.Empty;
    public string SourceType { get; set; } = string.Empty;
    public Guid SourceId { get; set; }
    public string DocumentNo { get; set; } = string.Empty;
    public DateTime DocumentDate { get; set; }
    public DateTime DueDate { get; set; }
    public Guid CurrencyId { get; set; }
    public decimal OriginalAmount { get; set; }
    public decimal SettledAmount { get; set; }
    public string Status { get; set; } = "OPEN";
    public long Version { get; set; } = 1;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public sealed class Wave1ReferenceDbContext(DbContextOptions<Wave1ReferenceDbContext> options) : DbContext(options)
{
    public DbSet<Wave1LanguageEntity> Languages => Set<Wave1LanguageEntity>();
    public DbSet<Wave1AccountClassificationEntity> AccountClassifications => Set<Wave1AccountClassificationEntity>();
    public DbSet<Wave1AccountingOpenItemEntity> OpenItems => Set<Wave1AccountingOpenItemEntity>();
    public DbSet<AuditEvent> AuditEvents => Set<AuditEvent>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.HasDefaultSchema("transport_erp");

        var language = mb.Entity<Wave1LanguageEntity>();
        language.ToTable("languages", t => t.HasCheckConstraint("ck_languages_direction", "\"Direction\" IN ('RTL','LTR')"));
        language.HasKey(x => x.Id);
        language.Property(x => x.Code).HasColumnType("text").IsRequired();
        language.Property(x => x.CultureCode).HasColumnType("text").IsRequired();
        language.Property(x => x.Direction).HasColumnType("text").IsRequired();
        language.Property(x => x.Version).IsConcurrencyToken();
        language.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        language.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        language.HasIndex(x => x.Code).IsUnique();
        language.HasIndex(x => x.CultureCode).IsUnique();
        language.HasIndex(x => new { x.IsActive, x.Code });

        var classification = mb.Entity<Wave1AccountClassificationEntity>();
        classification.ToTable("account_classifications", t =>
            t.HasCheckConstraint("ck_account_classifications_type", "\"AccountType\" IN ('ASSET','LIABILITY','EQUITY','REVENUE','EXPENSE')"));
        classification.HasKey(x => x.Id);
        classification.Property(x => x.Code).HasMaxLength(60).IsRequired();
        classification.Property(x => x.ArabicName).HasMaxLength(200).IsRequired();
        classification.Property(x => x.EnglishName).HasMaxLength(200);
        classification.Property(x => x.AccountType).HasMaxLength(20).IsRequired();
        classification.Property(x => x.Version).IsConcurrencyToken();
        classification.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        classification.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        classification.HasIndex(x => new { x.CompanyId, x.Code }).IsUnique();
        classification.HasIndex(x => new { x.CompanyId, x.AccountType, x.IsActive });

        var openItem = mb.Entity<Wave1AccountingOpenItemEntity>();
        openItem.ToTable("accounting_open_items", t =>
        {
            t.HasCheckConstraint("ck_accounting_open_items_side", "\"Side\" IN ('RECEIVABLE','PAYABLE')");
            t.HasCheckConstraint("ck_accounting_open_items_status", "\"Status\" IN ('OPEN','SETTLED','CANCELLED')");
            t.HasCheckConstraint("ck_accounting_open_items_amounts", "\"OriginalAmount\" >= 0 AND \"SettledAmount\" >= 0 AND \"SettledAmount\" <= \"OriginalAmount\"");
        });
        openItem.HasKey(x => x.Id);
        openItem.Property(x => x.PartyCode).HasMaxLength(80).IsRequired();
        openItem.Property(x => x.PartyName).HasMaxLength(250).IsRequired();
        openItem.Property(x => x.Side).HasMaxLength(20).IsRequired();
        openItem.Property(x => x.SourceType).HasMaxLength(80).IsRequired();
        openItem.Property(x => x.DocumentNo).HasMaxLength(80).IsRequired();
        openItem.Property(x => x.OriginalAmount).HasPrecision(19, 4);
        openItem.Property(x => x.SettledAmount).HasPrecision(19, 4);
        openItem.Property(x => x.Status).HasMaxLength(20).IsRequired();
        openItem.Property(x => x.Version).IsConcurrencyToken();
        openItem.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        openItem.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        openItem.HasIndex(x => new { x.CompanyId, x.SourceType, x.SourceId, x.Side }).IsUnique();
        openItem.HasIndex(x => new { x.CompanyId, x.BranchId, x.Side, x.Status, x.DueDate });
        openItem.HasIndex(x => new { x.CompanyId, x.PartyId, x.Side, x.Status });

        var audit = mb.Entity<AuditEvent>();
        audit.ToTable("audit_events");
        audit.HasKey(x => x.Id);
        audit.Property(x => x.OccurredAt).HasColumnType("timestamptz");
        audit.Property(x => x.Action).HasMaxLength(120).IsRequired();
        audit.Property(x => x.Outcome).HasMaxLength(40).IsRequired();
        audit.Property(x => x.EntityType).HasMaxLength(120).IsRequired();
        audit.Property(x => x.Hash).HasMaxLength(128).IsRequired();
        audit.Property(x => x.PreviousHash).HasMaxLength(128);
    }
}

public sealed class Wave1ReferenceService(Wave1ReferenceDbContext db)
{
    private static readonly HashSet<string> AccountTypes = new(StringComparer.Ordinal)
    { "ASSET", "LIABILITY", "EQUITY", "REVENUE", "EXPENSE" };

    public async Task<PagedResponse<LanguageListItemDto>> ListLanguagesAsync(LanguageQueryRequest request, CancellationToken ct = default)
    {
        var page = ValidatePage(request.Page, request.PageSize, out var pageSize);
        var q = db.Languages.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.SearchText))
        {
            var term = request.SearchText.Trim();
            q = q.Where(x => x.Code.Contains(term) || x.CultureCode.Contains(term));
        }
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            var active = ParseStatus(request.Status);
            q = q.Where(x => x.IsActive == active);
        }
        if (!string.IsNullOrWhiteSpace(request.Direction))
        {
            var direction = NormalizeDirection(request.Direction);
            q = q.Where(x => x.Direction == direction);
        }
        q = ApplyLanguageSort(q, request.SortBy, request.SortDirection);
        var total = await q.CountAsync(ct);
        var rows = await q.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(x => new LanguageListItemDto(x.Id, x.Code, x.CultureCode, x.Direction, x.IsActive ? "Active" : "Stopped", x.Version))
            .ToListAsync(ct);
        return new(rows, page, pageSize, total);
    }

    public async Task<LanguageDto?> GetLanguageAsync(Guid id, CancellationToken ct = default)
        => await db.Languages.AsNoTracking().Where(x => x.Id == id)
            .Select(x => new LanguageDto(x.Id, x.Code, x.CultureCode, x.Direction, x.IsActive ? "Active" : "Stopped", x.Version))
            .SingleOrDefaultAsync(ct);

    public Task<LanguageDto> CreateLanguageAsync(OperationContext context, CreateLanguageRequest request, CancellationToken ct = default)
        => ExecuteRequiredAsync(async () =>
        {
            context.EnsureComplete();
            var code = Required(request.Code);
            var cultureCode = Required(request.CultureCode);
            var direction = NormalizeDirection(request.Direction);
            if (await db.Languages.AnyAsync(x => x.Code == code || x.CultureCode == cultureCode, ct))
                throw new Wave1ReferenceRuleException("CONFLICT");
            var now = DateTimeOffset.UtcNow;
            var entity = new Wave1LanguageEntity { Id = Guid.NewGuid(), Code = code, CultureCode = cultureCode, Direction = direction, IsActive = true, Version = 1, CreatedAt = now, UpdatedAt = now };
            db.Languages.Add(entity);
            await AppendAuditAsync(context, "Language.Create", "Language", entity.Id, null, JsonSerializer.Serialize(ToLanguageDto(entity)), null, ct);
            await db.SaveChangesAsync(ct);
            return ToLanguageDto(entity);
        }, ct);

    public Task<LanguageDto?> UpdateLanguageAsync(OperationContext context, Guid id, UpdateLanguageRequest request, CancellationToken ct = default)
        => ExecuteAsync(async () =>
        {
            context.EnsureComplete();
            var entity = await db.Languages.SingleOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null) return null;
            if (entity.Version != request.ExpectedVersion) throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");
            var code = Required(request.Code);
            var cultureCode = Required(request.CultureCode);
            var direction = NormalizeDirection(request.Direction);
            if (await db.Languages.AnyAsync(x => x.Id != id && (x.Code == code || x.CultureCode == cultureCode), ct))
                throw new Wave1ReferenceRuleException("CONFLICT");
            var before = JsonSerializer.Serialize(ToLanguageDto(entity));
            entity.Code = code; entity.CultureCode = cultureCode; entity.Direction = direction; entity.Version++; entity.UpdatedAt = DateTimeOffset.UtcNow;
            await AppendAuditAsync(context, "Language.Update", "Language", entity.Id, before, JsonSerializer.Serialize(ToLanguageDto(entity)), null, ct);
            await db.SaveChangesAsync(ct);
            return ToLanguageDto(entity);
        }, ct);

    public Task<LanguageDto?> DisableLanguageAsync(OperationContext context, Guid id, DisableRequest request, CancellationToken ct = default)
        => ExecuteAsync(async () =>
        {
            context.EnsureComplete();
            if (string.IsNullOrWhiteSpace(request.Reason)) throw new Wave1ReferenceRuleException("VALIDATION_FAILED");
            var entity = await db.Languages.SingleOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null) return null;
            if (entity.Version != request.ExpectedVersion) throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");
            var before = JsonSerializer.Serialize(ToLanguageDto(entity));
            entity.IsActive = false; entity.Version++; entity.UpdatedAt = DateTimeOffset.UtcNow;
            await AppendAuditAsync(context, "Language.Disable", "Language", entity.Id, before, JsonSerializer.Serialize(ToLanguageDto(entity)), request.Reason.Trim(), ct);
            await db.SaveChangesAsync(ct);
            return ToLanguageDto(entity);
        }, ct);

    public async Task<Wave1ReferencePage<AccountClassificationDto>> ListClassificationsAsync(Guid companyId, int skip, int take, CancellationToken ct = default)
    {
        ValidateLegacyPage(skip, take);
        var q = db.AccountClassifications.AsNoTracking().Where(x => x.CompanyId == companyId).OrderBy(x => x.Code);
        var total = await q.CountAsync(ct);
        var raw = await q.Skip(skip).Take(take).ToListAsync(ct);
        return new(raw.Select(ToClassificationDto).ToList(), total, skip, take);
    }

    public Task<AccountClassificationDto> CreateClassificationAsync(OperationContext context, CreateAccountClassificationRequest request, CancellationToken ct = default)
        => ExecuteRequiredAsync(async () =>
        {
            context.EnsureComplete();
            var code = NormalizeCode(request.Code, 60); var type = NormalizeAccountType(request.AccountType);
            if (await db.AccountClassifications.AnyAsync(x => x.CompanyId == context.CompanyId && x.Code == code, ct)) throw new ArgumentException("DUPLICATE_CODE");
            var now = DateTimeOffset.UtcNow;
            var entity = new Wave1AccountClassificationEntity { Id = Guid.NewGuid(), CompanyId = context.CompanyId, Code = code, ArabicName = Required(request.ArabicName, 200), EnglishName = Optional(request.EnglishName, 200), AccountType = type, IsActive = true, Version = 1, CreatedAt = now, UpdatedAt = now };
            db.AccountClassifications.Add(entity);
            await AppendAuditAsync(context, "AccountClassification.Create", "AccountClassification", entity.Id, null, JsonSerializer.Serialize(ToClassificationDto(entity)), null, ct);
            await db.SaveChangesAsync(ct); return ToClassificationDto(entity);
        }, ct);

    public Task<AccountClassificationDto?> UpdateClassificationAsync(OperationContext context, Guid id, UpdateAccountClassificationRequest request, CancellationToken ct = default)
        => ExecuteAsync(async () =>
        {
            context.EnsureComplete();
            var entity = await db.AccountClassifications.SingleOrDefaultAsync(x => x.Id == id && x.CompanyId == context.CompanyId, ct);
            if (entity is null) return null;
            if (entity.Version != request.ExpectedVersion) throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");
            var code = NormalizeCode(request.Code, 60);
            if (await db.AccountClassifications.AnyAsync(x => x.Id != id && x.CompanyId == context.CompanyId && x.Code == code, ct)) throw new ArgumentException("DUPLICATE_CODE");
            var before = JsonSerializer.Serialize(ToClassificationDto(entity));
            entity.Code = code; entity.ArabicName = Required(request.ArabicName, 200); entity.EnglishName = Optional(request.EnglishName, 200); entity.AccountType = NormalizeAccountType(request.AccountType); entity.Version++; entity.UpdatedAt = DateTimeOffset.UtcNow;
            await AppendAuditAsync(context, "AccountClassification.Update", "AccountClassification", entity.Id, before, JsonSerializer.Serialize(ToClassificationDto(entity)), null, ct);
            await db.SaveChangesAsync(ct); return ToClassificationDto(entity);
        }, ct);

    public Task<AccountClassificationDto?> DisableClassificationAsync(OperationContext context, Guid id, DisableReferenceRequest request, CancellationToken ct = default)
        => ExecuteAsync(async () =>
        {
            context.EnsureComplete();
            if (string.IsNullOrWhiteSpace(request.Reason)) throw new ArgumentException("REASON_REQUIRED");
            var entity = await db.AccountClassifications.SingleOrDefaultAsync(x => x.Id == id && x.CompanyId == context.CompanyId, ct);
            if (entity is null) return null;
            if (entity.Version != request.ExpectedVersion) throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");
            var before = JsonSerializer.Serialize(ToClassificationDto(entity)); entity.IsActive = false; entity.Version++; entity.UpdatedAt = DateTimeOffset.UtcNow;
            await AppendAuditAsync(context, "AccountClassification.Disable", "AccountClassification", entity.Id, before, JsonSerializer.Serialize(ToClassificationDto(entity)), request.Reason, ct);
            await db.SaveChangesAsync(ct); return ToClassificationDto(entity);
        }, ct);

    private async Task<T?> ExecuteAsync<T>(Func<Task<T?>> action, CancellationToken ct)
    {
        await using var tx = await BeginTransactionIfSupportedAsync(ct);
        try { var value = await action(); if (tx is not null) await tx.CommitAsync(ct); return value; }
        catch { if (tx is not null) await tx.RollbackAsync(ct); throw; }
    }
    private async Task<T> ExecuteRequiredAsync<T>(Func<Task<T>> action, CancellationToken ct)
    {
        await using var tx = await BeginTransactionIfSupportedAsync(ct);
        try { var value = await action(); if (tx is not null) await tx.CommitAsync(ct); return value; }
        catch { if (tx is not null) await tx.RollbackAsync(ct); throw; }
    }
    private async Task<IDbContextTransaction?> BeginTransactionIfSupportedAsync(CancellationToken ct)
        => db.Database.IsRelational() ? await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct) : null;

    private async Task AppendAuditAsync(OperationContext context, string action, string entityType, Guid entityId, string? before, string? after, string? reason, CancellationToken ct)
    {
        var previousHash = await db.AuditEvents.AsNoTracking().Where(x => x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.DeviceId == null).OrderByDescending(x => x.OccurredAt).ThenByDescending(x => x.Id).Select(x => x.Hash).FirstOrDefaultAsync(ct);
        var evt = new AuditEvent { Id = Guid.NewGuid(), OccurredAt = DateTimeOffset.UtcNow, ActorUserId = context.UserId, CompanyId = context.CompanyId, BranchId = context.BranchId, Action = action, Outcome = "SUCCESS", EntityType = entityType, EntityId = entityId, CorrelationId = context.CorrelationId, BeforeJson = before, AfterJson = after, Reason = string.IsNullOrWhiteSpace(reason) ? null : reason.Trim(), PreviousHash = previousHash };
        evt.Hash = AuditEventService.ComputeHash(evt); db.AuditEvents.Add(evt);
    }

    private static int ValidatePage(int page, int requestedPageSize, out int effectivePageSize)
    { if (page < 1 || requestedPageSize <= 0) throw new Wave1ReferenceRuleException("VALIDATION_FAILED"); effectivePageSize = Math.Min(requestedPageSize, 200); return page; }
    private static IQueryable<Wave1LanguageEntity> ApplyLanguageSort(IQueryable<Wave1LanguageEntity> q, string? sortBy, string? sortDirection)
    {
        var key = string.IsNullOrWhiteSpace(sortBy) ? "code" : sortBy.Trim().ToLowerInvariant();
        if (key is not ("code" or "culturecode" or "direction" or "status")) throw new Wave1ReferenceRuleException("VALIDATION_FAILED");
        var desc = string.Equals(sortDirection?.Trim(), "desc", StringComparison.OrdinalIgnoreCase);
        if (!string.IsNullOrWhiteSpace(sortDirection) && !desc && !string.Equals(sortDirection.Trim(), "asc", StringComparison.OrdinalIgnoreCase)) throw new Wave1ReferenceRuleException("VALIDATION_FAILED");
        return (key, desc) switch { ("code", false) => q.OrderBy(x => x.Code), ("code", true) => q.OrderByDescending(x => x.Code), ("culturecode", false) => q.OrderBy(x => x.CultureCode), ("culturecode", true) => q.OrderByDescending(x => x.CultureCode), ("direction", false) => q.OrderBy(x => x.Direction).ThenBy(x => x.Code), ("direction", true) => q.OrderByDescending(x => x.Direction).ThenBy(x => x.Code), ("status", false) => q.OrderByDescending(x => x.IsActive).ThenBy(x => x.Code), _ => q.OrderBy(x => x.IsActive).ThenBy(x => x.Code) };
    }
    private static bool ParseStatus(string status) => status.Trim().ToLowerInvariant() switch { "active" => true, "stopped" => false, _ => throw new Wave1ReferenceRuleException("VALIDATION_FAILED") };
    private static string NormalizeDirection(string value) { var x = Required(value).ToUpperInvariant(); return x is "RTL" or "LTR" ? x : throw new Wave1ReferenceRuleException("VALIDATION_FAILED"); }
    private static string Required(string value) { if (string.IsNullOrWhiteSpace(value)) throw new Wave1ReferenceRuleException("VALIDATION_FAILED"); return value.Trim(); }
    private static void ValidateLegacyPage(int skip, int take) { if (skip < 0 || take is < 1 or > 500) throw new ArgumentOutOfRangeException(nameof(take)); }
    private static string NormalizeCode(string value, int max) => Required(value, max).ToUpperInvariant();
    private static string Required(string value, int max) { if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("VALUE_REQUIRED"); var x = value.Trim(); if (x.Length > max) throw new ArgumentException("VALUE_TOO_LONG"); return x; }
    private static string? Optional(string? value, int max) { if (string.IsNullOrWhiteSpace(value)) return null; var x = value.Trim(); if (x.Length > max) throw new ArgumentException("VALUE_TOO_LONG"); return x; }
    private static string NormalizeAccountType(string value) { var x = Required(value, 20).ToUpperInvariant(); if (!AccountTypes.Contains(x)) throw new ArgumentException("INVALID_ACCOUNT_TYPE"); return x; }
    private static LanguageDto ToLanguageDto(Wave1LanguageEntity x) => new(x.Id, x.Code, x.CultureCode, x.Direction, x.IsActive ? "Active" : "Stopped", x.Version);
    private static AccountClassificationDto ToClassificationDto(Wave1AccountClassificationEntity x) => new(x.Id, x.CompanyId, x.Code, x.ArabicName, x.EnglishName, x.AccountType, x.IsActive, x.Version);
}

public sealed class Wave1ReferenceRuleException(string code) : Exception(code) { public string Code { get; } = code; }

[DbContext(typeof(Wave1ReferenceDbContext))]
[Migration("20260822013000_Wave1ReferenceAndOpenItems")]
public sealed class Wave1ReferenceAndOpenItems : Migration
{
    protected override void Up(MigrationBuilder m)
    {
        m.EnsureSchema("transport_erp");
        m.CreateTable(name: "languages", schema: "transport_erp", columns: table => new { Id = table.Column<Guid>(type: "uuid", nullable: false), Code = table.Column<string>(type: "text", nullable: false), CultureCode = table.Column<string>(type: "text", nullable: false), Direction = table.Column<string>(type: "text", nullable: false), IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true), Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1), CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false), UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false) }, constraints: table => { table.PrimaryKey("PK_languages", x => x.Id); table.CheckConstraint("ck_languages_direction", "\"Direction\" IN ('RTL','LTR')"); });
        m.CreateIndex(name: "IX_languages_Code", schema: "transport_erp", table: "languages", column: "Code", unique: true);
        m.CreateIndex(name: "IX_languages_CultureCode", schema: "transport_erp", table: "languages", column: "CultureCode", unique: true);

        m.CreateTable(name: "account_classifications", schema: "transport_erp", columns: table => new { Id = table.Column<Guid>(type: "uuid", nullable: false), CompanyId = table.Column<Guid>(type: "uuid", nullable: false), Code = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false), ArabicName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false), EnglishName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true), AccountType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false), IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true), Version = table.Column<long>(type: "bigint", nullable: false), CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false), UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false) }, constraints: table => table.PrimaryKey("PK_account_classifications", x => x.Id));
        m.CreateIndex(name: "IX_account_classifications_CompanyId_Code", schema: "transport_erp", table: "account_classifications", columns: new[] { "CompanyId", "Code" }, unique: true);

        m.CreateTable(name: "accounting_open_items", schema: "transport_erp", columns: table => new { Id = table.Column<Guid>(type: "uuid", nullable: false), CompanyId = table.Column<Guid>(type: "uuid", nullable: false), BranchId = table.Column<Guid>(type: "uuid", nullable: false), PartyId = table.Column<Guid>(type: "uuid", nullable: false), PartyCode = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false), PartyName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false), Side = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false), SourceType = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false), SourceId = table.Column<Guid>(type: "uuid", nullable: false), DocumentNo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false), DocumentDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false), DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false), CurrencyId = table.Column<Guid>(type: "uuid", nullable: false), OriginalAmount = table.Column<decimal>(type: "numeric(19,4)", nullable: false), SettledAmount = table.Column<decimal>(type: "numeric(19,4)", nullable: false), Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false), Version = table.Column<long>(type: "bigint", nullable: false), CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false), UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false) }, constraints: table => table.PrimaryKey("PK_accounting_open_items", x => x.Id));
        m.CreateIndex(name: "IX_accounting_open_items_Source", schema: "transport_erp", table: "accounting_open_items", columns: new[] { "CompanyId", "SourceType", "SourceId", "Side" }, unique: true);
        m.CreateIndex(name: "IX_accounting_open_items_Aging", schema: "transport_erp", table: "accounting_open_items", columns: new[] { "CompanyId", "BranchId", "Side", "Status", "DueDate" });
    }
    protected override void Down(MigrationBuilder m) { m.DropTable(name: "accounting_open_items", schema: "transport_erp"); m.DropTable(name: "account_classifications", schema: "transport_erp"); m.DropTable(name: "languages", schema: "transport_erp"); }
}
