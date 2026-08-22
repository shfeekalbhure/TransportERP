using System.Data;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using TransportERP.Contracts.Geo;

namespace TransportERP.Infrastructure.Persistence;

public sealed class Wave1CountryAuthorityRecord
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string ArabicName { get; set; } = string.Empty;
    public string? EnglishName { get; set; }
    public string? NationalityName { get; set; }
    public string? ISO2 { get; set; }
    public string? ISO3 { get; set; }
    public string? DialingCode { get; set; }
    public bool IsActive { get; set; } = true;
    public int Version { get; set; } = 1;
}

public sealed class Wave1CountryAuthorityDbContext(DbContextOptions<Wave1CountryAuthorityDbContext> options) : DbContext(options)
{
    public DbSet<Wave1CountryAuthorityRecord> Countries => Set<Wave1CountryAuthorityRecord>();
    public DbSet<AuditEvent> AuditEvents => Set<AuditEvent>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.HasDefaultSchema("transport_erp");
        var country = mb.Entity<Wave1CountryAuthorityRecord>();
        country.ToTable("countries");
        country.HasKey(x => x.Id);
        country.Property(x => x.Code).HasMaxLength(64).IsRequired();
        country.Property(x => x.ArabicName).HasMaxLength(200).IsRequired();
        country.Property(x => x.EnglishName).HasMaxLength(200);
        country.Property(x => x.NationalityName).HasMaxLength(200);
        country.Property(x => x.ISO2).HasMaxLength(2);
        country.Property(x => x.ISO3).HasMaxLength(3);
        country.Property(x => x.DialingCode).HasMaxLength(8);
        country.Property(x => x.Version).IsConcurrencyToken();
        country.HasIndex(x => x.Code).IsUnique();
        country.HasIndex(x => x.ISO2).IsUnique().HasFilter("\"ISO2\" IS NOT NULL");
        country.HasIndex(x => x.ISO3).IsUnique().HasFilter("\"ISO3\" IS NOT NULL");
        country.HasIndex(x => new { x.IsActive, x.Code });

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

public sealed class Wave1CountryAuthorityService(Wave1CountryAuthorityDbContext db)
{
    public async Task<PagedResponse<CountryDto>> ListAsync(PagedQueryRequest request, CancellationToken ct = default)
    {
        ValidatePage(request);
        var q = db.Countries.AsNoTracking().AsQueryable();
        if (request.IsActive.HasValue) q = q.Where(x => x.IsActive == request.IsActive.Value);
        if (!string.IsNullOrWhiteSpace(request.SearchText))
        {
            var term = request.SearchText.Trim();
            q = q.Where(x => x.Code.Contains(term) || x.ArabicName.Contains(term) ||
                (x.EnglishName != null && x.EnglishName.Contains(term)) ||
                (x.ISO2 != null && x.ISO2.Contains(term)) || (x.ISO3 != null && x.ISO3.Contains(term)) ||
                (x.DialingCode != null && x.DialingCode.Contains(term)));
        }
        var total = await q.CountAsync(ct);
        var rows = await q.OrderBy(x => x.Code)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .Select(x => ToDto(x)).ToListAsync(ct);
        return new(rows, request.Page, request.PageSize, total);
    }

    public async Task<CountryDto?> GetAsync(Guid id, CancellationToken ct = default)
        => await db.Countries.AsNoTracking().Where(x => x.Id == id).Select(x => ToDto(x)).SingleOrDefaultAsync(ct);

    public Task<CountryDto> CreateAsync(CreateCountryRequest request, Wave1GeoOperationContext context, CancellationToken ct = default)
        => ExecuteRequiredAsync(async () =>
        {
            var values = Normalize(request.Code, request.ArabicName, request.EnglishName, request.NationalityName, request.ISO2, request.ISO3, request.DialingCode);
            await EnsureUnique(values.Code, values.ISO2, values.ISO3, null, ct);
            var row = new Wave1CountryAuthorityRecord
            {
                Id = Guid.NewGuid(), Code = values.Code, ArabicName = values.ArabicName, EnglishName = values.EnglishName,
                NationalityName = values.NationalityName, ISO2 = values.ISO2, ISO3 = values.ISO3, DialingCode = values.DialingCode,
                IsActive = true, Version = 1
            };
            db.Countries.Add(row);
            await AppendAudit(row, "Create", null, context, null, ct);
            await db.SaveChangesAsync(ct);
            return ToDto(row);
        }, ct);

    public Task<CountryDto?> UpdateAsync(Guid id, UpdateCountryRequest request, Wave1GeoOperationContext context, CancellationToken ct = default)
        => ExecuteAsync(async () =>
        {
            var row = await db.Countries.SingleOrDefaultAsync(x => x.Id == id, ct);
            if (row is null) return null;
            if (row.Version != request.ExpectedVersion) throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");
            var values = Normalize(request.Code, request.ArabicName, request.EnglishName, request.NationalityName, request.ISO2, request.ISO3, request.DialingCode);
            await EnsureUnique(values.Code, values.ISO2, values.ISO3, id, ct);
            var before = JsonSerializer.Serialize(ToDto(row));
            row.Code = values.Code; row.ArabicName = values.ArabicName; row.EnglishName = values.EnglishName;
            row.NationalityName = values.NationalityName; row.ISO2 = values.ISO2; row.ISO3 = values.ISO3;
            row.DialingCode = values.DialingCode; row.Version++;
            await AppendAudit(row, "Update", before, context, null, ct);
            await db.SaveChangesAsync(ct);
            return ToDto(row);
        }, ct);

    public Task<CountryDto?> DisableAsync(Guid id, DisableRequest request, Wave1GeoOperationContext context, CancellationToken ct = default)
        => ExecuteAsync(async () =>
        {
            if (string.IsNullOrWhiteSpace(request.Reason)) throw new ArgumentException("REASON_REQUIRED");
            var row = await db.Countries.SingleOrDefaultAsync(x => x.Id == id, ct);
            if (row is null) return null;
            if (row.Version != request.ExpectedVersion) throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");
            var before = JsonSerializer.Serialize(ToDto(row));
            row.IsActive = false; row.Version++;
            await AppendAudit(row, "Disable", before, context, request.Reason.Trim(), ct);
            await db.SaveChangesAsync(ct);
            return ToDto(row);
        }, ct);

    private async Task EnsureUnique(string code, string iso2, string? iso3, Guid? exceptId, CancellationToken ct)
    {
        if (await db.Countries.AnyAsync(x => x.Id != exceptId && x.Code == code, ct)) throw new ArgumentException("DUPLICATE_CODE");
        if (await db.Countries.AnyAsync(x => x.Id != exceptId && x.ISO2 == iso2, ct)) throw new ArgumentException("DUPLICATE_ISO2");
        if (iso3 is not null && await db.Countries.AnyAsync(x => x.Id != exceptId && x.ISO3 == iso3, ct)) throw new ArgumentException("DUPLICATE_ISO3");
    }

    private async Task AppendAudit(Wave1CountryAuthorityRecord row, string action, string? before, Wave1GeoOperationContext context, string? reason, CancellationToken ct)
    {
        var previous = await db.AuditEvents.AsNoTracking()
            .Where(x => x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.DeviceId == context.DeviceId)
            .OrderByDescending(x => x.OccurredAt).ThenByDescending(x => x.Id).Select(x => x.Hash).FirstOrDefaultAsync(ct);
        var evt = new AuditEvent
        {
            Id = Guid.NewGuid(), OccurredAt = DateTimeOffset.UtcNow, ActorUserId = context.ActorUserId,
            CompanyId = context.CompanyId, BranchId = context.BranchId, Action = $"Country.{action}", Outcome = "SUCCESS",
            EntityType = "Country", EntityId = row.Id, CorrelationId = context.CorrelationId, DeviceId = context.DeviceId,
            BeforeJson = before, AfterJson = JsonSerializer.Serialize(ToDto(row)), Reason = reason, Ip = context.Ip,
            PreviousHash = previous, Hash = string.Empty
        };
        evt.Hash = AuditEventService.ComputeHash(evt);
        db.AuditEvents.Add(evt);
    }

    private async Task<T?> ExecuteAsync<T>(Func<Task<T?>> action, CancellationToken ct)
    {
        await using var tx = await Begin(ct);
        try { var value = await action(); if (tx is not null) await tx.CommitAsync(ct); return value; }
        catch { if (tx is not null) await tx.RollbackAsync(ct); throw; }
    }
    private async Task<T> ExecuteRequiredAsync<T>(Func<Task<T>> action, CancellationToken ct)
    {
        await using var tx = await Begin(ct);
        try { var value = await action(); if (tx is not null) await tx.CommitAsync(ct); return value; }
        catch { if (tx is not null) await tx.RollbackAsync(ct); throw; }
    }
    private Task<IDbContextTransaction?> Begin(CancellationToken ct)
        => db.Database.IsRelational() ? db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct).ContinueWith<IDbContextTransaction?>(x => x.Result, ct) : Task.FromResult<IDbContextTransaction?>(null);

    private static void ValidatePage(PagedQueryRequest request)
    {
        if (request.Page < 1 || request.PageSize is < 1 or > 200) throw new ArgumentOutOfRangeException(nameof(request));
    }

    private static (string Code, string ArabicName, string? EnglishName, string? NationalityName, string ISO2, string? ISO3, string? DialingCode) Normalize(
        string code, string arabicName, string? englishName, string? nationalityName, string? iso2, string? iso3, string? dialingCode)
    {
        var c = Required(code, 64, "INVALID_CODE").ToUpperInvariant();
        var ar = Required(arabicName, 200, "INVALID_ARABIC_NAME");
        var en = Optional(englishName, 200, "INVALID_ENGLISH_NAME");
        var nat = Optional(nationalityName, 200, "INVALID_NATIONALITY_NAME");
        var i2 = Required(iso2, 2, "INVALID_ISO2").ToUpperInvariant();
        if (i2.Length != 2 || i2.Any(ch => ch is < 'A' or > 'Z')) throw new ArgumentException("INVALID_ISO2");
        var i3 = Optional(iso3, 3, "INVALID_ISO3")?.ToUpperInvariant();
        if (i3 is not null && (i3.Length != 3 || i3.Any(ch => ch is < 'A' or > 'Z'))) throw new ArgumentException("INVALID_ISO3");
        var dial = Optional(dialingCode, 8, "INVALID_DIALING_CODE");
        if (dial is not null && (dial.Length < 2 || dial[0] != '+' || dial.Skip(1).Any(ch => !char.IsDigit(ch)))) throw new ArgumentException("INVALID_DIALING_CODE");
        return (c, ar, en, nat, i2, i3, dial);
    }
    private static string Required(string? value, int max, string code)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException(code);
        var x = value.Trim(); if (x.Length > max) throw new ArgumentException(code); return x;
    }
    private static string? Optional(string? value, int max, string code)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        var x = value.Trim(); if (x.Length > max) throw new ArgumentException(code); return x;
    }
    private static CountryDto ToDto(Wave1CountryAuthorityRecord x)
        => new(x.Id, x.Code, x.ArabicName, x.EnglishName, x.NationalityName, x.IsActive, x.Version, x.ISO2, x.ISO3, x.DialingCode);
}

[DbContext(typeof(Wave1CountryAuthorityDbContext))]
[Migration("20260823001000_Wave1CountryPhysicalPromotion")]
public sealed class Wave1CountryPhysicalPromotion : Migration
{
    protected override void Up(MigrationBuilder m)
    {
        m.Sql("ALTER TABLE transport_erp.countries ADD COLUMN IF NOT EXISTS \"ISO2\" character varying(2) NULL;");
        m.Sql("ALTER TABLE transport_erp.countries ADD COLUMN IF NOT EXISTS \"ISO3\" character varying(3) NULL;");
        m.Sql("ALTER TABLE transport_erp.countries ADD COLUMN IF NOT EXISTS \"DialingCode\" character varying(8) NULL;");
        m.Sql("CREATE UNIQUE INDEX IF NOT EXISTS \"IX_countries_ISO2\" ON transport_erp.countries (\"ISO2\") WHERE \"ISO2\" IS NOT NULL;");
        m.Sql("CREATE UNIQUE INDEX IF NOT EXISTS \"IX_countries_ISO3\" ON transport_erp.countries (\"ISO3\") WHERE \"ISO3\" IS NOT NULL;");
        m.Sql("DO $$ BEGIN IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname='ck_countries_iso2') THEN ALTER TABLE transport_erp.countries ADD CONSTRAINT ck_countries_iso2 CHECK (\"ISO2\" IS NULL OR \"ISO2\" ~ '^[A-Z]{2}$'); END IF; END $$;");
        m.Sql("DO $$ BEGIN IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname='ck_countries_iso3') THEN ALTER TABLE transport_erp.countries ADD CONSTRAINT ck_countries_iso3 CHECK (\"ISO3\" IS NULL OR \"ISO3\" ~ '^[A-Z]{3}$'); END IF; END $$;");
        m.Sql("DO $$ BEGIN IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname='ck_countries_dial') THEN ALTER TABLE transport_erp.countries ADD CONSTRAINT ck_countries_dial CHECK (\"DialingCode\" IS NULL OR \"DialingCode\" ~ '^\\+[0-9]{1,7}$'); END IF; END $$;");
    }
    protected override void Down(MigrationBuilder m)
    {
        m.Sql("DROP INDEX IF EXISTS transport_erp.\"IX_countries_ISO3\";");
        m.Sql("DROP INDEX IF EXISTS transport_erp.\"IX_countries_ISO2\";");
        m.Sql("ALTER TABLE transport_erp.countries DROP COLUMN IF EXISTS \"DialingCode\", DROP COLUMN IF EXISTS \"ISO3\", DROP COLUMN IF EXISTS \"ISO2\";");
    }
}
