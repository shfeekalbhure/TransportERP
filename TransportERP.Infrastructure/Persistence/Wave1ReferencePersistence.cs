using System.Data;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using TransportERP.Contracts.Core;
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

public sealed class Wave1LanguageEntity : Wave1ReferenceEntity
{
    public string Code { get; set; } = string.Empty;
    public string ArabicName { get; set; } = string.Empty;
    public string? EnglishName { get; set; }
    public bool IsRtl { get; set; }
    public ICollection<Wave1TranslationEntity> Translations { get; set; } = new List<Wave1TranslationEntity>();
}

public sealed class Wave1TranslationEntity : Wave1ReferenceEntity
{
    public Guid LanguageId { get; set; }
    public string ResourceKey { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public Wave1LanguageEntity? Language { get; set; }
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
    public DbSet<Wave1TranslationEntity> Translations => Set<Wave1TranslationEntity>();
    public DbSet<Wave1AccountClassificationEntity> AccountClassifications => Set<Wave1AccountClassificationEntity>();
    public DbSet<Wave1AccountingOpenItemEntity> OpenItems => Set<Wave1AccountingOpenItemEntity>();
    public DbSet<AuditEvent> AuditEvents => Set<AuditEvent>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.HasDefaultSchema("transport_erp");

        var language = mb.Entity<Wave1LanguageEntity>();
        language.ToTable("languages");
        language.HasKey(x => x.Id);
        language.Property(x => x.Code).HasMaxLength(20).IsRequired();
        language.Property(x => x.ArabicName).HasMaxLength(120).IsRequired();
        language.Property(x => x.EnglishName).HasMaxLength(120);
        language.Property(x => x.Version).IsConcurrencyToken();
        language.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        language.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        language.HasIndex(x => x.Code).IsUnique();
        language.HasIndex(x => new { x.IsActive, x.Code });

        var translation = mb.Entity<Wave1TranslationEntity>();
        translation.ToTable("translations");
        translation.HasKey(x => x.Id);
        translation.Property(x => x.ResourceKey).HasMaxLength(240).IsRequired();
        translation.Property(x => x.Text).HasColumnType("text").IsRequired();
        translation.Property(x => x.Version).IsConcurrencyToken();
        translation.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        translation.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        translation.HasIndex(x => new { x.LanguageId, x.ResourceKey }).IsUnique();
        translation.HasOne(x => x.Language).WithMany(x => x.Translations)
            .HasForeignKey(x => x.LanguageId).OnDelete(DeleteBehavior.Restrict);

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

    public async Task<Wave1ReferencePage<LanguageDto>> ListLanguagesAsync(int skip, int take, CancellationToken ct = default)
    {
        ValidatePage(skip, take);
        var q = db.Languages.AsNoTracking().OrderBy(x => x.Code);
        var total = await q.CountAsync(ct);
        var items = await q.Skip(skip).Take(take).Select(x => ToDto(x)).ToListAsync(ct);
        return new(items, total, skip, take);
    }

    public Task<LanguageDto> CreateLanguageAsync(OperationContext context, CreateLanguageRequest request, CancellationToken ct = default)
        => ExecuteRequiredAsync(async () =>
        {
            context.EnsureComplete();
            var code = NormalizeCode(request.Code, 20);
            if (await db.Languages.AnyAsync(x => x.Code == code, ct)) throw new ArgumentException("DUPLICATE_CODE");
            var now = DateTimeOffset.UtcNow;
            var entity = new Wave1LanguageEntity
            {
                Id = Guid.NewGuid(), Code = code, ArabicName = Required(request.ArabicName, 120),
                EnglishName = Optional(request.EnglishName, 120), IsRtl = request.IsRtl,
                IsActive = true, Version = 1, CreatedAt = now, UpdatedAt = now
            };
            db.Languages.Add(entity);
            await AppendAuditAsync(context, "Language.Create", "Language", entity.Id, null, JsonSerializer.Serialize(ToDto(entity)), null, ct);
            await db.SaveChangesAsync(ct);
            return ToDto(entity);
        }, ct);

    public Task<LanguageDto?> UpdateLanguageAsync(OperationContext context, Guid id, UpdateLanguageRequest request, CancellationToken ct = default)
        => ExecuteAsync(async () =>
        {
            context.EnsureComplete();
            var entity = await db.Languages.SingleOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null) return null;
            if (entity.Version != request.ExpectedVersion) throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");
            var code = NormalizeCode(request.Code, 20);
            if (await db.Languages.AnyAsync(x => x.Id != id && x.Code == code, ct)) throw new ArgumentException("DUPLICATE_CODE");
            var before = JsonSerializer.Serialize(ToDto(entity));
            entity.Code = code; entity.ArabicName = Required(request.ArabicName, 120);
            entity.EnglishName = Optional(request.EnglishName, 120); entity.IsRtl = request.IsRtl;
            entity.Version++; entity.UpdatedAt = DateTimeOffset.UtcNow;
            await AppendAuditAsync(context, "Language.Update", "Language", entity.Id, before, JsonSerializer.Serialize(ToDto(entity)), null, ct);
            await db.SaveChangesAsync(ct);
            return ToDto(entity);
        }, ct);

    public Task<LanguageDto?> DisableLanguageAsync(OperationContext context, Guid id, DisableReferenceRequest request, CancellationToken ct = default)
        => ExecuteAsync(async () =>
        {
            context.EnsureComplete();
            if (string.IsNullOrWhiteSpace(request.Reason)) throw new ArgumentException("REASON_REQUIRED");
            var entity = await db.Languages.SingleOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null) return null;
            if (entity.Version != request.ExpectedVersion) throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");
            var before = JsonSerializer.Serialize(ToDto(entity));
            entity.IsActive = false; entity.Version++; entity.UpdatedAt = DateTimeOffset.UtcNow;
            await AppendAuditAsync(context, "Language.Disable", "Language", entity.Id, before, JsonSerializer.Serialize(ToDto(entity)), request.Reason, ct);
            await db.SaveChangesAsync(ct);
            return ToDto(entity);
        }, ct);

    public Task<TranslationDto> UpsertTranslationAsync(OperationContext context, Guid languageId, UpsertTranslationRequest request, CancellationToken ct = default)
        => ExecuteRequiredAsync(async () =>
        {
            context.EnsureComplete();
            if (!await db.Languages.AnyAsync(x => x.Id == languageId && x.IsActive, ct)) throw new ArgumentException("LANGUAGE_NOT_FOUND_OR_INACTIVE");
            var key = Required(request.ResourceKey, 240);
            var text = Required(request.Text, int.MaxValue);
            var entity = await db.Translations.SingleOrDefaultAsync(x => x.LanguageId == languageId && x.ResourceKey == key, ct);
            var now = DateTimeOffset.UtcNow;
            string? before = null;
            if (entity is null)
            {
                entity = new Wave1TranslationEntity { Id = Guid.NewGuid(), LanguageId = languageId, ResourceKey = key, Text = text, IsActive = true, Version = 1, CreatedAt = now, UpdatedAt = now };
                db.Translations.Add(entity);
            }
            else
            {
                if (!request.ExpectedVersion.HasValue || entity.Version != request.ExpectedVersion.Value) throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");
                before = JsonSerializer.Serialize(ToDto(entity));
                entity.Text = text; entity.IsActive = true; entity.Version++; entity.UpdatedAt = now;
            }
            await AppendAuditAsync(context, "Translation.Upsert", "Translation", entity.Id, before, JsonSerializer.Serialize(ToDto(entity)), null, ct);
            await db.SaveChangesAsync(ct);
            return ToDto(entity);
        }, ct);

    public async Task<Wave1ReferencePage<AccountClassificationDto>> ListClassificationsAsync(Guid companyId, int skip, int take, CancellationToken ct = default)
    {
        ValidatePage(skip, take);
        var q = db.AccountClassifications.AsNoTracking().Where(x => x.CompanyId == companyId).OrderBy(x => x.Code);
        var total = await q.CountAsync(ct);
        var items = await q.Skip(skip).Take(take).Select(x => ToDto(x)).ToListAsync(ct);
        return new(items, total, skip, take);
    }

    public Task<AccountClassificationDto> CreateClassificationAsync(OperationContext context, CreateAccountClassificationRequest request, CancellationToken ct = default)
        => ExecuteRequiredAsync(async () =>
        {
            context.EnsureComplete();
            var code = NormalizeCode(request.Code, 60); var type = NormalizeAccountType(request.AccountType);
            if (await db.AccountClassifications.AnyAsync(x => x.CompanyId == context.CompanyId && x.Code == code, ct)) throw new ArgumentException("DUPLICATE_CODE");
            var now = DateTimeOffset.UtcNow;
            var entity = new Wave1AccountClassificationEntity
            {
                Id = Guid.NewGuid(), CompanyId = context.CompanyId, Code = code,
                ArabicName = Required(request.ArabicName, 200), EnglishName = Optional(request.EnglishName, 200),
                AccountType = type, IsActive = true, Version = 1, CreatedAt = now, UpdatedAt = now
            };
            db.AccountClassifications.Add(entity);
            await AppendAuditAsync(context, "AccountClassification.Create", "AccountClassification", entity.Id, null, JsonSerializer.Serialize(ToDto(entity)), null, ct);
            await db.SaveChangesAsync(ct); return ToDto(entity);
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
            var before = JsonSerializer.Serialize(ToDto(entity));
            entity.Code = code; entity.ArabicName = Required(request.ArabicName, 200); entity.EnglishName = Optional(request.EnglishName, 200);
            entity.AccountType = NormalizeAccountType(request.AccountType); entity.Version++; entity.UpdatedAt = DateTimeOffset.UtcNow;
            await AppendAuditAsync(context, "AccountClassification.Update", "AccountClassification", entity.Id, before, JsonSerializer.Serialize(ToDto(entity)), null, ct);
            await db.SaveChangesAsync(ct); return ToDto(entity);
        }, ct);

    public Task<AccountClassificationDto?> DisableClassificationAsync(OperationContext context, Guid id, DisableReferenceRequest request, CancellationToken ct = default)
        => ExecuteAsync(async () =>
        {
            context.EnsureComplete();
            if (string.IsNullOrWhiteSpace(request.Reason)) throw new ArgumentException("REASON_REQUIRED");
            var entity = await db.AccountClassifications.SingleOrDefaultAsync(x => x.Id == id && x.CompanyId == context.CompanyId, ct);
            if (entity is null) return null;
            if (entity.Version != request.ExpectedVersion) throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");
            var before = JsonSerializer.Serialize(ToDto(entity));
            entity.IsActive = false; entity.Version++; entity.UpdatedAt = DateTimeOffset.UtcNow;
            await AppendAuditAsync(context, "AccountClassification.Disable", "AccountClassification", entity.Id, before, JsonSerializer.Serialize(ToDto(entity)), request.Reason, ct);
            await db.SaveChangesAsync(ct); return ToDto(entity);
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

    private Task<IDbContextTransaction?> BeginTransactionIfSupportedAsync(CancellationToken ct)
        => db.Database.IsRelational() ? db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct).ContinueWith<IDbContextTransaction?>(x => x.Result, ct) : Task.FromResult<IDbContextTransaction?>(null);

    private async Task AppendAuditAsync(OperationContext context, string action, string entityType, Guid entityId, string? before, string? after, string? reason, CancellationToken ct)
    {
        var previousHash = await db.AuditEvents.AsNoTracking()
            .Where(x => x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.DeviceId == null)
            .OrderByDescending(x => x.OccurredAt).ThenByDescending(x => x.Id).Select(x => x.Hash).FirstOrDefaultAsync(ct);
        var evt = new AuditEvent
        {
            Id = Guid.NewGuid(), OccurredAt = DateTimeOffset.UtcNow, ActorUserId = context.UserId,
            CompanyId = context.CompanyId, BranchId = context.BranchId, Action = action, Outcome = "SUCCESS",
            EntityType = entityType, EntityId = entityId, CorrelationId = context.CorrelationId,
            BeforeJson = before, AfterJson = after, Reason = string.IsNullOrWhiteSpace(reason) ? null : reason.Trim(), PreviousHash = previousHash
        };
        evt.Hash = AuditEventService.ComputeHash(evt); db.AuditEvents.Add(evt);
    }

    private static void ValidatePage(int skip, int take) { if (skip < 0 || take is < 1 or > 500) throw new ArgumentOutOfRangeException(nameof(take)); }
    private static string NormalizeCode(string value, int max) => Required(value, max).ToUpperInvariant();
    private static string Required(string value, int max) { if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("VALUE_REQUIRED"); var x = value.Trim(); if (x.Length > max) throw new ArgumentException("VALUE_TOO_LONG"); return x; }
    private static string? Optional(string? value, int max) { if (string.IsNullOrWhiteSpace(value)) return null; var x = value.Trim(); if (x.Length > max) throw new ArgumentException("VALUE_TOO_LONG"); return x; }
    private static string NormalizeAccountType(string value) { var x = Required(value, 20).ToUpperInvariant(); if (!AccountTypes.Contains(x)) throw new ArgumentException("INVALID_ACCOUNT_TYPE"); return x; }
    private static LanguageDto ToDto(Wave1LanguageEntity x) => new(x.Id, x.Code, x.ArabicName, x.EnglishName, x.IsRtl, x.IsActive, x.Version);
    private static TranslationDto ToDto(Wave1TranslationEntity x) => new(x.Id, x.LanguageId, x.ResourceKey, x.Text, x.IsActive, x.Version);
    private static AccountClassificationDto ToDto(Wave1AccountClassificationEntity x) => new(x.Id, x.CompanyId, x.Code, x.ArabicName, x.EnglishName, x.AccountType, x.IsActive, x.Version);
}

[DbContext(typeof(Wave1ReferenceDbContext))]
[Migration("20260822013000_Wave1ReferenceAndOpenItems")]
public sealed class Wave1ReferenceAndOpenItems : Migration
{
    protected override void Up(MigrationBuilder m)
    {
        m.EnsureSchema("transport_erp");
        m.CreateTable("languages", "transport_erp", columns: t => new
        {
            Id=t.Column<Guid>(), Code=t.Column<string>(maxLength:20), ArabicName=t.Column<string>(maxLength:120), EnglishName=t.Column<string>(maxLength:120, nullable:true), IsRtl=t.Column<bool>(), IsActive=t.Column<bool>(), Version=t.Column<long>(), CreatedAt=t.Column<DateTimeOffset>(), UpdatedAt=t.Column<DateTimeOffset>()
        }, constraints: t => t.PrimaryKey("PK_languages", x => x.Id));
        m.CreateIndex("IX_languages_Code", "transport_erp", "languages", "Code", unique:true);
        m.CreateTable("translations", "transport_erp", columns: t => new
        {
            Id=t.Column<Guid>(), LanguageId=t.Column<Guid>(), ResourceKey=t.Column<string>(maxLength:240), Text=t.Column<string>(type:"text"), IsActive=t.Column<bool>(), Version=t.Column<long>(), CreatedAt=t.Column<DateTimeOffset>(), UpdatedAt=t.Column<DateTimeOffset>()
        }, constraints: t => { t.PrimaryKey("PK_translations", x => x.Id); t.ForeignKey("FK_translations_languages_LanguageId", x => x.LanguageId, "transport_erp", "languages", "Id", onDelete:ReferentialAction.Restrict); });
        m.CreateIndex("IX_translations_LanguageId_ResourceKey", "transport_erp", "translations", new[]{"LanguageId","ResourceKey"}, unique:true);
        m.CreateTable("account_classifications", "transport_erp", columns: t => new
        {
            Id=t.Column<Guid>(), CompanyId=t.Column<Guid>(), Code=t.Column<string>(maxLength:60), ArabicName=t.Column<string>(maxLength:200), EnglishName=t.Column<string>(maxLength:200, nullable:true), AccountType=t.Column<string>(maxLength:20), IsActive=t.Column<bool>(), Version=t.Column<long>(), CreatedAt=t.Column<DateTimeOffset>(), UpdatedAt=t.Column<DateTimeOffset>()
        }, constraints: t => t.PrimaryKey("PK_account_classifications", x => x.Id));
        m.CreateIndex("IX_account_classifications_CompanyId_Code", "transport_erp", "account_classifications", new[]{"CompanyId","Code"}, unique:true);
        m.CreateTable("accounting_open_items", "transport_erp", columns: t => new
        {
            Id=t.Column<Guid>(), CompanyId=t.Column<Guid>(), BranchId=t.Column<Guid>(), PartyId=t.Column<Guid>(), PartyCode=t.Column<string>(maxLength:80), PartyName=t.Column<string>(maxLength:250), Side=t.Column<string>(maxLength:20), SourceType=t.Column<string>(maxLength:80), SourceId=t.Column<Guid>(), DocumentNo=t.Column<string>(maxLength:80), DocumentDate=t.Column<DateTime>(), DueDate=t.Column<DateTime>(), CurrencyId=t.Column<Guid>(), OriginalAmount=t.Column<decimal>(type:"numeric(19,4)"), SettledAmount=t.Column<decimal>(type:"numeric(19,4)"), Status=t.Column<string>(maxLength:20), Version=t.Column<long>(), CreatedAt=t.Column<DateTimeOffset>(), UpdatedAt=t.Column<DateTimeOffset>()
        }, constraints: t => t.PrimaryKey("PK_accounting_open_items", x => x.Id));
        m.CreateIndex("IX_accounting_open_items_Source", "transport_erp", "accounting_open_items", new[]{"CompanyId","SourceType","SourceId","Side"}, unique:true);
        m.CreateIndex("IX_accounting_open_items_Aging", "transport_erp", "accounting_open_items", new[]{"CompanyId","BranchId","Side","Status","DueDate"});
    }
    protected override void Down(MigrationBuilder m)
    {
        m.DropTable("accounting_open_items", "transport_erp"); m.DropTable("account_classifications", "transport_erp"); m.DropTable("translations", "transport_erp"); m.DropTable("languages", "transport_erp");
    }
}
