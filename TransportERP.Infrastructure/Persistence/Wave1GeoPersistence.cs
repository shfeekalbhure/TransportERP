using System.Data;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TransportERP.Contracts.Geo;
using TransportERP.Domain.Geo;

namespace TransportERP.Infrastructure.Persistence;

public sealed class Wave1GeoDbContext(DbContextOptions<Wave1GeoDbContext> options) : DbContext(options)
{
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Governorate> Governorates => Set<Governorate>();
    public DbSet<Directorate> Directorates => Set<Directorate>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Area> Areas => Set<Area>();
    public DbSet<AuditEvent> AuditEvents => Set<AuditEvent>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.HasDefaultSchema("transport_erp");

        var root = mb.Entity<GeoEntity>();
        root.UseTpcMappingStrategy();
        root.HasKey(x => x.Id);
        root.Property(x => x.Code).HasMaxLength(64).IsRequired();
        root.Property(x => x.ArabicName).HasMaxLength(200).IsRequired();
        root.Property(x => x.EnglishName).HasMaxLength(200);
        root.Property(x => x.IsActive).HasDefaultValue(true).IsRequired();
        root.Property(x => x.Version).IsConcurrencyToken().IsRequired();

        var country = mb.Entity<Country>();
        country.ToTable("countries");
        country.Property(x => x.NationalityName).HasMaxLength(200);
        country.HasIndex(x => x.Code).IsUnique();
        country.HasIndex(x => new { x.IsActive, x.Code });

        ConfigureChild<Governorate>(mb, "governorates", x => x.CountryId);
        mb.Entity<Governorate>().HasOne(x => x.Country).WithMany(x => x.Governorates).HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Restrict);

        ConfigureChild<Directorate>(mb, "directorates", x => x.GovernorateId);
        mb.Entity<Directorate>().HasOne(x => x.Governorate).WithMany(x => x.Directorates).HasForeignKey(x => x.GovernorateId).OnDelete(DeleteBehavior.Restrict);

        ConfigureChild<City>(mb, "cities", x => x.DirectorateId);
        mb.Entity<City>().HasOne(x => x.Directorate).WithMany(x => x.Cities).HasForeignKey(x => x.DirectorateId).OnDelete(DeleteBehavior.Restrict);

        ConfigureChild<Area>(mb, "areas", x => x.CityId);
        mb.Entity<Area>().HasOne(x => x.City).WithMany(x => x.Areas).HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.Restrict);

        var audit = mb.Entity<AuditEvent>();
        audit.ToTable("audit_events");
        audit.HasKey(x => x.Id);
        audit.Property(x => x.Action).HasMaxLength(120).IsRequired();
        audit.Property(x => x.Outcome).HasMaxLength(40).IsRequired();
        audit.Property(x => x.EntityType).HasMaxLength(120).IsRequired();
        audit.Property(x => x.DeviceId).HasMaxLength(120);
        audit.Property(x => x.Reason).HasMaxLength(500);
        audit.Property(x => x.Ip).HasMaxLength(64);
        audit.Property(x => x.Hash).HasMaxLength(64).IsRequired();
        audit.Property(x => x.PreviousHash).HasMaxLength(64);
        audit.HasIndex(x => new { x.CompanyId, x.BranchId, x.DeviceId, x.OccurredAt, x.Id });
    }

    private static void ConfigureChild<TEntity>(
        ModelBuilder mb,
        string table,
        System.Linq.Expressions.Expression<Func<TEntity, Guid>> parent)
        where TEntity : GeoEntity
    {
        var b = mb.Entity<TEntity>();
        b.ToTable(table);
        b.HasIndex(parent);
        b.HasIndex(x => new { x.IsActive, x.Code });
    }
}

public enum Wave1GeoResource { Countries, Governorates, Directorates, Cities, Areas }

public sealed record Wave1GeoOperationContext(
    Guid? ActorUserId,
    Guid? CompanyId,
    Guid? BranchId,
    Guid CorrelationId,
    string? DeviceId = null,
    string? Ip = null);

public sealed class Wave1GeoService(Wave1GeoDbContext db)
{
    public async Task<PagedResponse<GeoDto>> ListAsync(Wave1GeoResource resource, PagedQueryRequest query, CancellationToken ct = default)
    {
        if (query.Page < 1 || query.PageSize is < 1 or > 200) throw new ArgumentOutOfRangeException(nameof(query));
        var rows = await Query(resource)
            .Where(x => (!query.IsActive.HasValue || x.IsActive == query.IsActive.Value)
                        && (string.IsNullOrWhiteSpace(query.SearchText)
                            || x.Code.Contains(query.SearchText)
                            || x.ArabicName.Contains(query.SearchText)
                            || (x.EnglishName != null && x.EnglishName.Contains(query.SearchText))))
            .OrderBy(x => x.Code)
            .ToListAsync(ct);
        if (query.ParentId.HasValue)
            rows = rows.Where(x => ParentId(x) == query.ParentId.Value).ToList();
        var total = rows.Count;
        return new PagedResponse<GeoDto>(
            rows.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).Select(ToDto).ToArray(),
            query.Page, query.PageSize, total);
    }

    public async Task<GeoDto?> GetAsync(Wave1GeoResource resource, Guid id, CancellationToken ct = default)
        => (await Query(resource).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct)) is { } row ? ToDto(row) : null;

    public async Task<GeoDto> CreateAsync(Wave1GeoResource resource, object request, Wave1GeoOperationContext context, CancellationToken ct = default)
    {
        await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
        var entity = FromCreate(resource, request);
        NormalizeAndValidate(entity);
        await EnsureParentAndCodeAsync(resource, entity, null, ct);
        entity.Id = Guid.NewGuid();
        entity.Version = 1;
        db.Add(entity);
        AppendAudit(entity, "Create", null, context, null);
        await db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
        return ToDto(entity);
    }

    public async Task<GeoDto?> UpdateAsync(Wave1GeoResource resource, Guid id, object request, Wave1GeoOperationContext context, CancellationToken ct = default)
    {
        await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
        var entity = await Query(resource).FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return null;
        var expected = ExpectedVersion(request);
        if (entity.Version != expected) throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");
        var before = JsonSerializer.Serialize(ToDto(entity));
        ApplyUpdate(resource, entity, request);
        NormalizeAndValidate(entity);
        await EnsureParentAndCodeAsync(resource, entity, id, ct);
        entity.Version++;
        AppendAudit(entity, "Update", before, context, null);
        await db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
        return ToDto(entity);
    }

    public async Task<GeoDto?> DisableAsync(Wave1GeoResource resource, Guid id, DisableRequest request, Wave1GeoOperationContext context, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Reason)) throw new ArgumentException("Disable reason is required.");
        await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
        var entity = await Query(resource).FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return null;
        if (entity.Version != request.ExpectedVersion) throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");
        var before = JsonSerializer.Serialize(ToDto(entity));
        entity.IsActive = false;
        entity.Version++;
        AppendAudit(entity, "Disable", before, context, request.Reason.Trim());
        await db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
        return ToDto(entity);
    }

    private IQueryable<GeoEntity> Query(Wave1GeoResource resource) => resource switch
    {
        Wave1GeoResource.Countries => db.Countries,
        Wave1GeoResource.Governorates => db.Governorates,
        Wave1GeoResource.Directorates => db.Directorates,
        Wave1GeoResource.Cities => db.Cities,
        Wave1GeoResource.Areas => db.Areas,
        _ => throw new ArgumentOutOfRangeException(nameof(resource))
    };

    private async Task EnsureParentAndCodeAsync(Wave1GeoResource resource, GeoEntity entity, Guid? exceptId, CancellationToken ct)
    {
        var parent = ParentId(entity);
        var parentExists = resource switch
        {
            Wave1GeoResource.Countries => true,
            Wave1GeoResource.Governorates => parent.HasValue && await db.Countries.AnyAsync(x => x.Id == parent.Value && x.IsActive, ct),
            Wave1GeoResource.Directorates => parent.HasValue && await db.Governorates.AnyAsync(x => x.Id == parent.Value && x.IsActive, ct),
            Wave1GeoResource.Cities => parent.HasValue && await db.Directorates.AnyAsync(x => x.Id == parent.Value && x.IsActive, ct),
            Wave1GeoResource.Areas => parent.HasValue && await db.Cities.AnyAsync(x => x.Id == parent.Value && x.IsActive, ct),
            _ => false
        };
        if (!parentExists) throw new ArgumentException("PARENT_NOT_FOUND_OR_INACTIVE");

        var duplicate = resource switch
        {
            Wave1GeoResource.Countries => await db.Countries.AnyAsync(x => x.Code == entity.Code && x.Id != exceptId, ct),
            Wave1GeoResource.Governorates => await db.Governorates.AnyAsync(x => x.CountryId == parent && x.Code == entity.Code && x.Id != exceptId, ct),
            Wave1GeoResource.Directorates => await db.Directorates.AnyAsync(x => x.GovernorateId == parent && x.Code == entity.Code && x.Id != exceptId, ct),
            Wave1GeoResource.Cities => await db.Cities.AnyAsync(x => x.DirectorateId == parent && x.Code == entity.Code && x.Id != exceptId, ct),
            Wave1GeoResource.Areas => await db.Areas.AnyAsync(x => x.CityId == parent && x.Code == entity.Code && x.Id != exceptId, ct),
            _ => false
        };
        if (duplicate) throw new ArgumentException("DUPLICATE_CODE");
    }

    private void AppendAudit(GeoEntity entity, string action, string? before, Wave1GeoOperationContext context, string? reason)
    {
        var previousHash = db.AuditEvents.AsNoTracking()
            .Where(x => x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.DeviceId == context.DeviceId)
            .OrderByDescending(x => x.OccurredAt).ThenByDescending(x => x.Id)
            .Select(x => x.Hash).FirstOrDefault();
        var audit = new AuditEvent
        {
            Id = Guid.NewGuid(),
            OccurredAt = DateTimeOffset.UtcNow,
            ActorUserId = context.ActorUserId,
            CompanyId = context.CompanyId,
            BranchId = context.BranchId,
            Action = $"Geo.{action}",
            Outcome = "SUCCESS",
            EntityType = entity.GetType().Name,
            EntityId = entity.Id,
            CorrelationId = context.CorrelationId,
            DeviceId = context.DeviceId,
            BeforeJson = before,
            AfterJson = JsonSerializer.Serialize(ToDto(entity)),
            Reason = reason,
            Ip = context.Ip,
            PreviousHash = previousHash,
            Hash = string.Empty
        };
        audit.Hash = AuditEventService.ComputeHash(audit);
        db.AuditEvents.Add(audit);
    }

    private static void NormalizeAndValidate(GeoEntity entity)
    {
        entity.Code = entity.Code.Trim().ToUpperInvariant();
        entity.ArabicName = entity.ArabicName.Trim();
        entity.EnglishName = string.IsNullOrWhiteSpace(entity.EnglishName) ? null : entity.EnglishName.Trim();
        if (string.IsNullOrWhiteSpace(entity.Code) || entity.Code.Length > 64) throw new ArgumentException("INVALID_CODE");
        if (string.IsNullOrWhiteSpace(entity.ArabicName) || entity.ArabicName.Length > 200) throw new ArgumentException("INVALID_ARABIC_NAME");
        if (entity.EnglishName?.Length > 200) throw new ArgumentException("INVALID_ENGLISH_NAME");
        if (entity is Country c)
        {
            c.NationalityName = string.IsNullOrWhiteSpace(c.NationalityName) ? null : c.NationalityName.Trim();
            if (c.NationalityName?.Length > 200) throw new ArgumentException("INVALID_NATIONALITY_NAME");
        }
    }

    private static Guid? ParentId(GeoEntity e) => e switch
    {
        Governorate x => x.CountryId,
        Directorate x => x.GovernorateId,
        City x => x.DirectorateId,
        Area x => x.CityId,
        _ => null
    };

    private static int ExpectedVersion(object request) => request switch
    {
        UpdateCountryRequest x => x.ExpectedVersion,
        UpdateGovernorateRequest x => x.ExpectedVersion,
        UpdateDirectorateRequest x => x.ExpectedVersion,
        UpdateCityRequest x => x.ExpectedVersion,
        UpdateAreaRequest x => x.ExpectedVersion,
        _ => throw new ArgumentException("REQUEST_RESOURCE_MISMATCH")
    };

    private static GeoEntity FromCreate(Wave1GeoResource resource, object request) => (resource, request) switch
    {
        (Wave1GeoResource.Countries, CreateCountryRequest x) => new Country { Code = x.Code, ArabicName = x.ArabicName, EnglishName = x.EnglishName, NationalityName = x.NationalityName },
        (Wave1GeoResource.Governorates, CreateGovernorateRequest x) => new Governorate { CountryId = x.CountryId, Code = x.Code, ArabicName = x.ArabicName, EnglishName = x.EnglishName },
        (Wave1GeoResource.Directorates, CreateDirectorateRequest x) => new Directorate { GovernorateId = x.GovernorateId, Code = x.Code, ArabicName = x.ArabicName, EnglishName = x.EnglishName },
        (Wave1GeoResource.Cities, CreateCityRequest x) => new City { DirectorateId = x.DirectorateId, Code = x.Code, ArabicName = x.ArabicName, EnglishName = x.EnglishName },
        (Wave1GeoResource.Areas, CreateAreaRequest x) => new Area { CityId = x.CityId, Code = x.Code, ArabicName = x.ArabicName, EnglishName = x.EnglishName },
        _ => throw new ArgumentException("REQUEST_RESOURCE_MISMATCH")
    };

    private static void ApplyUpdate(Wave1GeoResource resource, GeoEntity entity, object request)
    {
        switch (resource, entity, request)
        {
            case (Wave1GeoResource.Countries, Country a, UpdateCountryRequest b): a.Code=b.Code; a.ArabicName=b.ArabicName; a.EnglishName=b.EnglishName; a.NationalityName=b.NationalityName; break;
            case (Wave1GeoResource.Governorates, Governorate a, UpdateGovernorateRequest b): a.CountryId=b.CountryId; a.Code=b.Code; a.ArabicName=b.ArabicName; a.EnglishName=b.EnglishName; break;
            case (Wave1GeoResource.Directorates, Directorate a, UpdateDirectorateRequest b): a.GovernorateId=b.GovernorateId; a.Code=b.Code; a.ArabicName=b.ArabicName; a.EnglishName=b.EnglishName; break;
            case (Wave1GeoResource.Cities, City a, UpdateCityRequest b): a.DirectorateId=b.DirectorateId; a.Code=b.Code; a.ArabicName=b.ArabicName; a.EnglishName=b.EnglishName; break;
            case (Wave1GeoResource.Areas, Area a, UpdateAreaRequest b): a.CityId=b.CityId; a.Code=b.Code; a.ArabicName=b.ArabicName; a.EnglishName=b.EnglishName; break;
            default: throw new ArgumentException("REQUEST_RESOURCE_MISMATCH");
        }
    }

    private static GeoDto ToDto(GeoEntity e) => e switch
    {
        Country x => new CountryDto(x.Id, x.Code, x.ArabicName, x.EnglishName, x.NationalityName, x.IsActive, x.Version),
        Governorate x => new GovernorateDto(x.Id, x.CountryId, x.Code, x.ArabicName, x.EnglishName, x.IsActive, x.Version),
        Directorate x => new DirectorateDto(x.Id, x.GovernorateId, x.Code, x.ArabicName, x.EnglishName, x.IsActive, x.Version),
        City x => new CityDto(x.Id, x.DirectorateId, x.Code, x.ArabicName, x.EnglishName, x.IsActive, x.Version),
        Area x => new AreaDto(x.Id, x.CityId, x.Code, x.ArabicName, x.EnglishName, x.IsActive, x.Version),
        _ => throw new ArgumentOutOfRangeException(nameof(e))
    };
}

[DbContext(typeof(Wave1GeoDbContext))]
[Migration("20260822024000_Wave1Geography")]
public sealed class Wave1GeographyMigration : Migration
{
    protected override void Up(MigrationBuilder m)
    {
        m.EnsureSchema("transport_erp");
        m.CreateTable("countries", "transport_erp", table => new
        {
            Id = table.Column<Guid>(type: "uuid", nullable: false),
            Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
            ArabicName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
            EnglishName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
            IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
            Version = table.Column<int>(type: "integer", nullable: false),
            NationalityName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
        }, constraints: table => table.PrimaryKey("PK_countries", x => x.Id));
        m.CreateIndex("IX_countries_Code", "countries", "Code", "transport_erp", unique: true);
        m.CreateIndex("IX_countries_IsActive_Code", "countries", new[] { "IsActive", "Code" }, "transport_erp");

        CreateChild(m, "governorates", "CountryId", "countries");
        CreateChild(m, "directorates", "GovernorateId", "governorates");
        CreateChild(m, "cities", "DirectorateId", "directorates");
        CreateChild(m, "areas", "CityId", "cities");
    }

    protected override void Down(MigrationBuilder m)
    {
        m.DropTable("areas", "transport_erp");
        m.DropTable("cities", "transport_erp");
        m.DropTable("directorates", "transport_erp");
        m.DropTable("governorates", "transport_erp");
        m.DropTable("countries", "transport_erp");
    }

    private static void CreateChild(MigrationBuilder m, string tableName, string parentColumn, string parentTable)
    {
        m.CreateTable(tableName, "transport_erp", table => new
        {
            Id = table.Column<Guid>(type: "uuid", nullable: false),
            Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
            ArabicName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
            EnglishName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
            IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
            Version = table.Column<int>(type: "integer", nullable: false),
            ParentId = table.Column<Guid>(name: parentColumn, type: "uuid", nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey($"PK_{tableName}", x => x.Id);
            table.ForeignKey($"FK_{tableName}_{parentTable}_{parentColumn}", x => x.ParentId, "transport_erp", parentTable, "Id", onDelete: ReferentialAction.Restrict);
        });
        m.CreateIndex($"IX_{tableName}_{parentColumn}", tableName, parentColumn, "transport_erp");
        m.CreateIndex($"IX_{tableName}_IsActive_Code", tableName, new[] { "IsActive", "Code" }, "transport_erp");
        m.CreateIndex($"UX_{tableName}_{parentColumn}_Code", tableName, new[] { parentColumn, "Code" }, "transport_erp", unique: true);
    }
}
