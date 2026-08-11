using Microsoft.EntityFrameworkCore;
using TransportERP.Application.Geo;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Geo;
using TransportERP.Domain.Geo;
using TransportERP.Domain.Org;
using System.Text.Json;

namespace TransportERP.Infrastructure.Geo;

public sealed class TransportErpDbContext(DbContextOptions<TransportErpDbContext> options) : DbContext(options)
{
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Governorate> Governorates => Set<Governorate>();
    public DbSet<Directorate> Directorates => Set<Directorate>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Area> Areas => Set<Area>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<ExchangeRate> ExchangeRates => Set<ExchangeRate>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<FiscalYear> FiscalYears => Set<FiscalYear>();
    public DbSet<NumberSequence> NumberSequences => Set<NumberSequence>();
    public DbSet<NumberReservation> NumberReservations => Set<NumberReservation>();
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<SettingDefinition> SettingDefinitions => Set<SettingDefinition>();
    public DbSet<SettingOverride> SettingOverrides => Set<SettingOverride>();
    internal DbSet<BusinessAuditEvent> BusinessAuditEvents => Set<BusinessAuditEvent>();

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        EnsureAuditEventsAreAppendOnly();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        EnsureAuditEventsAreAppendOnly();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Each approved geography entity owns its explicitly named table; no hierarchy table or
        // discriminator is permitted by the GEO execution contract.
        ConfigureGeoRoot(modelBuilder);
        Configure<Country>(modelBuilder, "countries", builder => { builder.Property(x => x.NationalityName).HasColumnName("nationality_name").HasMaxLength(200); });
        Configure<Governorate>(modelBuilder, "governorates", builder => { builder.Property(x => x.CountryId).HasColumnName("country_id").HasColumnType("binary(16)"); builder.HasIndex(x => new { x.CountryId, x.Code }).IsUnique(); builder.HasIndex(x => new { x.CountryId, x.IsActive, x.Code }); builder.HasOne(x => x.Country).WithMany(x => x.Governorates).HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Restrict); });
        Configure<Directorate>(modelBuilder, "directorates", builder => { builder.Property(x => x.GovernorateId).HasColumnName("governorate_id").HasColumnType("binary(16)"); builder.HasIndex(x => new { x.GovernorateId, x.Code }).IsUnique(); builder.HasIndex(x => new { x.GovernorateId, x.IsActive, x.Code }); builder.HasOne(x => x.Governorate).WithMany(x => x.Directorates).HasForeignKey(x => x.GovernorateId).OnDelete(DeleteBehavior.Restrict); });
        Configure<City>(modelBuilder, "cities", builder => { builder.Property(x => x.DirectorateId).HasColumnName("directorate_id").HasColumnType("binary(16)"); builder.HasIndex(x => new { x.DirectorateId, x.Code }).IsUnique(); builder.HasIndex(x => new { x.DirectorateId, x.IsActive, x.Code }); builder.HasOne(x => x.Directorate).WithMany(x => x.Cities).HasForeignKey(x => x.DirectorateId).OnDelete(DeleteBehavior.Restrict); });
        Configure<Area>(modelBuilder, "areas", builder => { builder.Property(x => x.CityId).HasColumnName("city_id").HasColumnType("binary(16)"); builder.HasIndex(x => new { x.CityId, x.Code }).IsUnique(); builder.HasIndex(x => new { x.CityId, x.IsActive, x.Code }); builder.HasOne(x => x.City).WithMany(x => x.Areas).HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.Restrict); });
        ConfigureBusinessAuditEvents(modelBuilder);
        ConfigureOrg(modelBuilder);
    }

    private static void ConfigureGeoRoot(ModelBuilder modelBuilder)
    {
        var b = modelBuilder.Entity<GeoEntity>();
        b.UseTpcMappingStrategy();
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("id").HasColumnType("binary(16)").ValueGeneratedNever();
        b.Property(x => x.Code).HasColumnName("code").HasMaxLength(64).IsRequired();
        b.Property(x => x.ArabicName).HasColumnName("arabic_name").HasMaxLength(200).IsRequired();
        b.Property(x => x.EnglishName).HasColumnName("english_name").HasMaxLength(200);
        b.Property(x => x.IsActive).HasColumnName("is_active").HasDefaultValue(true).IsRequired();
        b.Property(x => x.Version).HasColumnName("version").HasColumnType("int unsigned").IsConcurrencyToken().IsRequired();
    }

    private static void Configure<TEntity>(ModelBuilder modelBuilder, string table, Action<Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TEntity>> configure) where TEntity : GeoEntity
    {
        var b = modelBuilder.Entity<TEntity>(); b.ToTable(table); b.HasCharSet("utf8mb4");
        if (typeof(TEntity) == typeof(Country)) { b.HasIndex(x => x.Code).IsUnique(); b.HasIndex(x => new { x.IsActive, x.Code }); }
        configure(b);
    }

    private static void ConfigureBusinessAuditEvents(ModelBuilder modelBuilder)
    {
        var b = modelBuilder.Entity<BusinessAuditEvent>();
        b.ToTable("business_audit_events"); b.HasCharSet("utf8mb4"); b.HasKey(x => x.EventId);
        b.Property(x => x.EventId).HasColumnName("event_id").HasColumnType("binary(16)").ValueGeneratedNever();
        b.Property(x => x.ActorId).HasColumnName("actor_id").HasColumnType("binary(16)").IsRequired();
        b.Property(x => x.OccurredAt).HasColumnName("occurred_at").HasColumnType("datetime(6)").HasConversion(value => value.UtcDateTime, value => new DateTimeOffset(DateTime.SpecifyKind(value, DateTimeKind.Utc))).IsRequired();
        b.Property(x => x.CompanyId).HasColumnName("company_id").HasColumnType("binary(16)").IsRequired();
        b.Property(x => x.BranchId).HasColumnName("branch_id").HasColumnType("binary(16)").IsRequired();
        b.Property(x => x.EntityType).HasColumnName("entity_type").HasMaxLength(128).IsRequired();
        b.Property(x => x.RecordId).HasColumnName("record_id").HasColumnType("binary(16)").IsRequired();
        b.Property(x => x.Action).HasColumnName("action").HasMaxLength(64).IsRequired();
        b.Property(x => x.CorrelationId).HasColumnName("correlation_id").HasColumnType("binary(16)").IsRequired();
        b.Property(x => x.Reason).HasColumnName("reason").HasMaxLength(1000);
        b.Property(x => x.BeforeState).HasColumnName("before_state").HasColumnType("json").HasConversion(value => SerializeJson(value), value => DeserializeJson(value));
        b.Property(x => x.AfterState).HasColumnName("after_state").HasColumnType("json").HasConversion(value => SerializeJson(value), value => DeserializeJson(value));
    }

    private static void ConfigureOrg(ModelBuilder modelBuilder)
    {
        ConfigureOrgEntity<Currency>(modelBuilder, "gen_currencies", b =>
        {
            b.Property(x => x.Code).HasColumnName("code").HasMaxLength(3).IsRequired();
            b.Property(x => x.ArabicName).HasColumnName("arabic_name").HasMaxLength(200).IsRequired();
            b.Property(x => x.EnglishName).HasColumnName("english_name").HasMaxLength(200).IsRequired();
            b.Property(x => x.Symbol).HasColumnName("symbol").HasMaxLength(16);
            b.Property(x => x.DecimalPlaces).HasColumnName("decimal_places").HasColumnType("tinyint unsigned");
            b.HasIndex(x => x.Code).IsUnique();
        });
        ConfigureOrgEntity<Company>(modelBuilder, "gen_companies", b =>
        {
            b.Property(x => x.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
            b.Property(x => x.ArabicName).HasColumnName("arabic_name").HasMaxLength(200).IsRequired();
            b.Property(x => x.EnglishName).HasColumnName("english_name").HasMaxLength(200).IsRequired();
            b.Property(x => x.LegalName).HasColumnName("legal_name").HasMaxLength(200).IsRequired();
            b.Property(x => x.TaxNumber).HasColumnName("tax_number").HasMaxLength(100);
            b.Property(x => x.BaseCurrencyId).HasColumnName("base_currency_id").HasColumnType("binary(16)");
            b.Property(x => x.LogoUri).HasColumnName("logo_uri").HasMaxLength(500);
            b.Property(x => x.Notes).HasColumnName("notes").HasMaxLength(2000);
            b.HasIndex(x => x.Code).IsUnique();
            b.HasOne<Currency>().WithMany().HasForeignKey(x => x.BaseCurrencyId).OnDelete(DeleteBehavior.Restrict);
        });
        ConfigureOrgEntity<Branch>(modelBuilder, "gen_branches", b =>
        {
            b.Property(x => x.CompanyId).HasColumnName("company_id").HasColumnType("binary(16)");
            b.Property(x => x.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
            b.Property(x => x.ArabicName).HasColumnName("arabic_name").HasMaxLength(200).IsRequired();
            b.Property(x => x.EnglishName).HasColumnName("english_name").HasMaxLength(200).IsRequired();
            b.Property(x => x.TimeZone).HasColumnName("time_zone").HasMaxLength(64);
            b.Property(x => x.Notes).HasColumnName("notes").HasMaxLength(2000);
            b.HasIndex(x => new { x.CompanyId, x.Code }).IsUnique();
            b.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        });
        ConfigureOrgEntity<ExchangeRate>(modelBuilder, "gen_exchange_rates", b =>
        {
            b.Property(x => x.CompanyId).HasColumnName("company_id").HasColumnType("binary(16)");
            b.Property(x => x.BaseCurrencyId).HasColumnName("base_currency_id").HasColumnType("binary(16)");
            b.Property(x => x.QuoteCurrencyId).HasColumnName("quote_currency_id").HasColumnType("binary(16)");
            b.Property(x => x.Rate).HasColumnName("rate").HasPrecision(20, 10);
            b.Property(x => x.EffectiveFrom).HasColumnName("effective_from").HasColumnType("date");
            b.Property(x => x.EffectiveTo).HasColumnName("effective_to").HasColumnType("date");
            b.Property(x => x.MinimumRate).HasColumnName("minimum_rate").HasPrecision(20, 10);
            b.Property(x => x.MaximumRate).HasColumnName("maximum_rate").HasPrecision(20, 10);
            b.Property(x => x.Source).HasColumnName("source").HasMaxLength(100).IsRequired();
            b.HasIndex(x => new { x.CompanyId, x.BaseCurrencyId, x.QuoteCurrencyId, x.EffectiveFrom });
            b.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
            b.HasOne<Currency>().WithMany().HasForeignKey(x => x.BaseCurrencyId).OnDelete(DeleteBehavior.Restrict);
            b.HasOne<Currency>().WithMany().HasForeignKey(x => x.QuoteCurrencyId).OnDelete(DeleteBehavior.Restrict);
        });
        ConfigureOrgEntity<FiscalYear>(modelBuilder, "gen_fiscal_years", b =>
        {
            b.Property(x => x.CompanyId).HasColumnName("company_id").HasColumnType("binary(16)");
            b.Property(x => x.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
            b.Property(x => x.StartDate).HasColumnName("start_date").HasColumnType("date");
            b.Property(x => x.EndDate).HasColumnName("end_date").HasColumnType("date");
            b.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(16).IsRequired();
            b.HasIndex(x => new { x.CompanyId, x.Code }).IsUnique();
            b.HasIndex(x => new { x.CompanyId, x.StartDate, x.EndDate });
            b.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        });
        ConfigureOrgEntity<NumberSequence>(modelBuilder, "gen_number_sequences", b =>
        {
            b.Property(x => x.Code).HasColumnName("code").HasMaxLength(100).IsRequired();
            b.Property(x => x.ArabicName).HasColumnName("arabic_name").HasMaxLength(200).IsRequired();
            b.Property(x => x.EnglishName).HasColumnName("english_name").HasMaxLength(200).IsRequired();
            b.Property(x => x.ScopeType).HasColumnName("scope_type").HasMaxLength(32).IsRequired();
            b.Property(x => x.DocumentType).HasColumnName("document_type").HasMaxLength(32);
            b.Property(x => x.CompanyId).HasColumnName("company_id").HasColumnType("binary(16)");
            b.Property(x => x.BranchId).HasColumnName("branch_id").HasColumnType("binary(16)");
            b.Property(x => x.FiscalYearId).HasColumnName("fiscal_year_id").HasColumnType("binary(16)");
            b.Property(x => x.Prefix).HasColumnName("prefix").HasMaxLength(32);
            b.Property(x => x.LastNumber).HasColumnName("last_number").HasColumnType("bigint unsigned");
            b.Property(x => x.ResetPolicy).HasColumnName("reset_policy").HasMaxLength(32);
            // MySQL treats NULLs as distinct in a compound unique index. The contract requires
            // a null-safe scope identity, so the database owns a generated key for it.
            b.Property<string>("ScopeKey").HasColumnName("scope_key")
                .HasMaxLength(512)
                .HasComputedColumnSql("CONCAT_WS('|', code, COALESCE(HEX(company_id), '-'), COALESCE(HEX(branch_id), '-'), COALESCE(HEX(fiscal_year_id), '-'), COALESCE(document_type, '-'))", stored: true);
            b.HasIndex("ScopeKey").IsUnique();
        });
        ConfigureNumberReservations(modelBuilder);
        ConfigureOrgEntity<Language>(modelBuilder, "gen_languages", b =>
        {
            b.Property(x => x.LanguageCode).HasColumnName("language_code").HasMaxLength(35).IsRequired();
            b.Property(x => x.ArabicName).HasColumnName("arabic_name").HasMaxLength(200).IsRequired();
            b.Property(x => x.EnglishName).HasColumnName("english_name").HasMaxLength(200).IsRequired();
            b.Property(x => x.Direction).HasColumnName("direction").HasMaxLength(3).IsRequired();
            b.HasIndex(x => x.LanguageCode).IsUnique();
        });
        ConfigureSettings(modelBuilder);
    }

    private static void ConfigureOrgEntity<TEntity>(ModelBuilder modelBuilder, string table, Action<Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TEntity>> configure) where TEntity : OrgEntity
    {
        var b = modelBuilder.Entity<TEntity>();
        b.ToTable(table); b.HasCharSet("utf8mb4"); b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("id").HasColumnType("binary(16)").ValueGeneratedNever();
        b.Property(x => x.IsActive).HasColumnName("is_active").HasDefaultValue(true).IsRequired();
        b.Property(x => x.Version).HasColumnName("version").HasColumnType("int unsigned").IsConcurrencyToken().IsRequired();
        b.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").HasColumnType("datetime(6)").IsRequired();
        b.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc").HasColumnType("datetime(6)").IsRequired();
        configure(b);
    }

    private static void ConfigureNumberReservations(ModelBuilder modelBuilder)
    {
        var b = modelBuilder.Entity<NumberReservation>(); b.ToTable("gen_number_reservations"); b.HasCharSet("utf8mb4"); b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("id").HasColumnType("binary(16)").ValueGeneratedNever();
        b.Property(x => x.SequenceId).HasColumnName("sequence_id").HasColumnType("binary(16)");
        b.Property(x => x.NumberValue).HasColumnName("number_value").HasColumnType("bigint unsigned");
        b.Property(x => x.RenderedNumber).HasColumnName("rendered_number").HasMaxLength(128).IsRequired();
        b.Property(x => x.State).HasColumnName("state").HasConversion<string>().HasMaxLength(16).IsRequired();
        b.Property(x => x.IdempotencyKey).HasColumnName("idempotency_key").HasMaxLength(128).IsRequired();
        b.Property(x => x.Reason).HasColumnName("reason").HasMaxLength(1000);
        b.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").HasColumnType("datetime(6)").IsRequired();
        b.HasIndex(x => new { x.SequenceId, x.NumberValue }).IsUnique(); b.HasIndex(x => new { x.SequenceId, x.IdempotencyKey }).IsUnique();
        b.HasOne<NumberSequence>().WithMany().HasForeignKey(x => x.SequenceId).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureSettings(ModelBuilder modelBuilder)
    {
        var d = modelBuilder.Entity<SettingDefinition>(); d.ToTable("gen_setting_definitions"); d.HasCharSet("utf8mb4"); d.HasKey(x => x.Id);
        d.Property(x => x.Id).HasColumnName("id").HasColumnType("binary(16)").ValueGeneratedNever();
        d.Property(x => x.PropertyCode).HasColumnName("property_code").HasMaxLength(128).IsRequired(); d.HasIndex(x => x.PropertyCode).IsUnique();
        d.Property(x => x.Group).HasColumnName("group_name").HasMaxLength(100).IsRequired(); d.Property(x => x.ValueType).HasColumnName("value_type").HasMaxLength(32).IsRequired();
        d.Property(x => x.BuiltInDefault).HasColumnName("built_in_default").HasMaxLength(4000).IsRequired(); d.Property(x => x.AllowedScopes).HasColumnName("allowed_scopes").HasMaxLength(128).IsRequired(); d.Property(x => x.ResolutionPolicy).HasColumnName("resolution_policy").HasMaxLength(32).IsRequired();
        d.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").HasColumnType("datetime(6)").IsRequired(); d.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc").HasColumnType("datetime(6)").IsRequired();
        ConfigureOrgEntity<SettingOverride>(modelBuilder, "gen_setting_overrides", b => { b.Property(x => x.DefinitionId).HasColumnName("definition_id").HasColumnType("binary(16)"); b.Property(x => x.ScopeType).HasColumnName("scope_type").HasMaxLength(16).IsRequired(); b.Property(x => x.ScopeId).HasColumnName("scope_id").HasColumnType("binary(16)"); b.Property(x => x.TypedValue).HasColumnName("typed_value").HasMaxLength(4000).IsRequired(); b.Property(x => x.EffectiveFrom).HasColumnName("effective_from").HasColumnType("date"); b.Property(x => x.EffectiveTo).HasColumnName("effective_to").HasColumnType("date"); b.HasIndex(x => new { x.DefinitionId, x.ScopeType, x.ScopeId }).IsUnique(); b.HasOne<SettingDefinition>().WithMany().HasForeignKey(x => x.DefinitionId).OnDelete(DeleteBehavior.Restrict); });
    }

    private static string? SerializeJson(JsonElement? value) => value.HasValue ? value.Value.GetRawText() : null;
    private static JsonElement? DeserializeJson(string? value) => string.IsNullOrWhiteSpace(value) ? null : JsonDocument.Parse(value).RootElement.Clone();
    private void EnsureAuditEventsAreAppendOnly()
    {
        if (ChangeTracker.Entries<BusinessAuditEvent>().Any(entry => entry.State is EntityState.Modified or EntityState.Deleted))
        {
            throw new InvalidOperationException("Business audit events are append-only.");
        }
    }
}

public sealed class EfGeoRepository(TransportErpDbContext db) : IGeoRepository
{
    public Task<GeoEntity?> FindAsync(GeoResource r, Guid id, CancellationToken ct) => Query(r).FirstOrDefaultAsync(x => x.Id == id, ct)!;
    public Task AddAsync(GeoResource r, GeoEntity e, CancellationToken ct) { db.Add(e); return Task.CompletedTask; }
    public Task SaveAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
    public Task<bool> ParentExistsAsync(GeoResource r, Guid parentId, CancellationToken ct) => r switch { GeoResource.Governorates => db.Countries.AnyAsync(x => x.Id == parentId, ct), GeoResource.Directorates => db.Governorates.AnyAsync(x => x.Id == parentId, ct), GeoResource.Cities => db.Directorates.AnyAsync(x => x.Id == parentId, ct), GeoResource.Areas => db.Cities.AnyAsync(x => x.Id == parentId, ct), _ => Task.FromResult(true) };
    public Task<bool> CodeExistsAsync(GeoResource r, Guid? parent, string code, Guid? exceptId, CancellationToken ct) => r switch { GeoResource.Countries => db.Countries.AnyAsync(x => x.Code == code && x.Id != exceptId, ct), GeoResource.Governorates => db.Governorates.AnyAsync(x => x.CountryId == parent && x.Code == code && x.Id != exceptId, ct), GeoResource.Directorates => db.Directorates.AnyAsync(x => x.GovernorateId == parent && x.Code == code && x.Id != exceptId, ct), GeoResource.Cities => db.Cities.AnyAsync(x => x.DirectorateId == parent && x.Code == code && x.Id != exceptId, ct), GeoResource.Areas => db.Areas.AnyAsync(x => x.CityId == parent && x.Code == code && x.Id != exceptId, ct), _ => Task.FromResult(false) };
    public async Task<PagedResponse<GeoDto>> ListAsync(GeoResource r, PagedQueryRequest q, CancellationToken ct)
    {
        var rows = await Query(r).Where(x => (!q.IsActive.HasValue || x.IsActive == q.IsActive) && (string.IsNullOrWhiteSpace(q.SearchText) || x.Code.Contains(q.SearchText) || x.ArabicName.Contains(q.SearchText) || (x.EnglishName != null && x.EnglishName.Contains(q.SearchText)))).OrderBy(x => x.Code).ToListAsync(ct);
        if (q.ParentId is { } parent) rows = rows.Where(x => ParentId(x) == parent).ToList();
        var total = rows.Count; return new PagedResponse<GeoDto>(rows.Skip((q.Page - 1) * q.PageSize).Take(q.PageSize).Select(ToDto).ToArray(), q.Page, q.PageSize, total);
    }
    // Resource-specific query paths keep entity details inside this repository.
    private IQueryable<GeoEntity> Query(GeoResource r) => r switch { GeoResource.Countries => db.Countries, GeoResource.Governorates => db.Governorates, GeoResource.Directorates => db.Directorates, GeoResource.Cities => db.Cities, GeoResource.Areas => db.Areas, _ => throw new ArgumentOutOfRangeException(nameof(r)) };
    private static Guid? ParentId(GeoEntity e) => e switch { Governorate x => x.CountryId, Directorate x => x.GovernorateId, City x => x.DirectorateId, Area x => x.CityId, _ => null };
    private static GeoDto ToDto(GeoEntity e) => e switch { Country x => new CountryDto(x.Id,x.Code,x.ArabicName,x.EnglishName,x.NationalityName,x.IsActive,x.Version), Governorate x => new GovernorateDto(x.Id,x.CountryId,x.Code,x.ArabicName,x.EnglishName,x.IsActive,x.Version), Directorate x => new DirectorateDto(x.Id,x.GovernorateId,x.Code,x.ArabicName,x.EnglishName,x.IsActive,x.Version), City x => new CityDto(x.Id,x.DirectorateId,x.Code,x.ArabicName,x.EnglishName,x.IsActive,x.Version), Area x => new AreaDto(x.Id,x.CityId,x.Code,x.ArabicName,x.EnglishName,x.IsActive,x.Version), _ => throw new ArgumentOutOfRangeException(nameof(e)) };
}

public sealed class EfBusinessAuditWriter(TransportErpDbContext db) : IBusinessAuditWriter
{
    public ValueTask AppendAsync(BusinessAuditEvent auditEvent, CancellationToken cancellationToken = default)
    {
        auditEvent.EnsureComplete();
        db.BusinessAuditEvents.Add(auditEvent);
        return ValueTask.CompletedTask;
    }
}
