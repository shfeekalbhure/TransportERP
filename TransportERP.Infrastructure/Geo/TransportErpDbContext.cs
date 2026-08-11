using Microsoft.EntityFrameworkCore;
using TransportERP.Application.Geo;
using TransportERP.Contracts.Geo;
using TransportERP.Domain.Geo;

namespace TransportERP.Infrastructure.Geo;

public sealed class TransportErpDbContext(DbContextOptions<TransportErpDbContext> options) : DbContext(options)
{
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Governorate> Governorates => Set<Governorate>();
    public DbSet<Directorate> Directorates => Set<Directorate>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Area> Areas => Set<Area>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Each approved geography entity owns its explicitly named table; no hierarchy table or
        // discriminator is permitted by the GEO execution contract.
        modelBuilder.Entity<GeoEntity>().UseTpcMappingStrategy();
        Configure<Country>(modelBuilder, "countries", builder => { builder.Property(x => x.NationalityName).HasColumnName("nationality_name").HasMaxLength(200); });
        Configure<Governorate>(modelBuilder, "governorates", builder => { builder.Property(x => x.CountryId).HasColumnName("country_id").HasColumnType("binary(16)"); builder.HasIndex(x => new { x.CountryId, x.Code }).IsUnique(); builder.HasIndex(x => new { x.CountryId, x.IsActive, x.Code }); builder.HasOne(x => x.Country).WithMany(x => x.Governorates).HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Restrict); });
        Configure<Directorate>(modelBuilder, "directorates", builder => { builder.Property(x => x.GovernorateId).HasColumnName("governorate_id").HasColumnType("binary(16)"); builder.HasIndex(x => new { x.GovernorateId, x.Code }).IsUnique(); builder.HasIndex(x => new { x.GovernorateId, x.IsActive, x.Code }); builder.HasOne(x => x.Governorate).WithMany(x => x.Directorates).HasForeignKey(x => x.GovernorateId).OnDelete(DeleteBehavior.Restrict); });
        Configure<City>(modelBuilder, "cities", builder => { builder.Property(x => x.DirectorateId).HasColumnName("directorate_id").HasColumnType("binary(16)"); builder.HasIndex(x => new { x.DirectorateId, x.Code }).IsUnique(); builder.HasIndex(x => new { x.DirectorateId, x.IsActive, x.Code }); builder.HasOne(x => x.Directorate).WithMany(x => x.Cities).HasForeignKey(x => x.DirectorateId).OnDelete(DeleteBehavior.Restrict); });
        Configure<Area>(modelBuilder, "areas", builder => { builder.Property(x => x.CityId).HasColumnName("city_id").HasColumnType("binary(16)"); builder.HasIndex(x => new { x.CityId, x.Code }).IsUnique(); builder.HasIndex(x => new { x.CityId, x.IsActive, x.Code }); builder.HasOne(x => x.City).WithMany(x => x.Areas).HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.Restrict); });
    }

    private static void Configure<TEntity>(ModelBuilder modelBuilder, string table, Action<Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TEntity>> configure) where TEntity : GeoEntity
    {
        var b = modelBuilder.Entity<TEntity>(); b.ToTable(table, x => x.HasCharSet("utf8mb4")); b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("id").HasColumnType("binary(16)").ValueGeneratedNever(); b.Property(x => x.Code).HasColumnName("code").HasMaxLength(64).IsRequired(); b.Property(x => x.ArabicName).HasColumnName("arabic_name").HasMaxLength(200).IsRequired(); b.Property(x => x.EnglishName).HasColumnName("english_name").HasMaxLength(200); b.Property(x => x.IsActive).HasColumnName("is_active").HasDefaultValue(true).IsRequired(); b.Property(x => x.Version).HasColumnName("version").HasColumnType("int unsigned").IsConcurrencyToken().IsRequired();
        if (typeof(TEntity) == typeof(Country)) { b.HasIndex(x => x.Code).IsUnique(); b.HasIndex(x => new { x.IsActive, x.Code }); }
        configure(b);
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
