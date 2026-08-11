using System.Text.Json;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Geo;
using TransportERP.Domain.Geo;

namespace TransportERP.Application.Geo;

public enum GeoResource { Countries, Governorates, Directorates, Cities, Areas }

public interface IGeoRepository
{
    Task<GeoEntity?> FindAsync(GeoResource resource, Guid id, CancellationToken cancellationToken);
    Task<bool> ParentExistsAsync(GeoResource resource, Guid parentId, CancellationToken cancellationToken);
    Task<bool> CodeExistsAsync(GeoResource resource, Guid? parentId, string code, Guid? exceptId, CancellationToken cancellationToken);
    Task<PagedResponse<GeoDto>> ListAsync(GeoResource resource, PagedQueryRequest query, CancellationToken cancellationToken);
    Task AddAsync(GeoResource resource, GeoEntity entity, CancellationToken cancellationToken);
    Task SaveAsync(CancellationToken cancellationToken);
}

public interface IGeoAuditSink { Task WriteAsync(BusinessAuditEvent auditEvent, CancellationToken cancellationToken); }
public sealed class GeoAuditSink : IGeoAuditSink
{
    // The shared BusinessAuditEvent is the only audit shape; this sink intentionally does not add a parallel model.
    public Task WriteAsync(BusinessAuditEvent auditEvent, CancellationToken cancellationToken) { auditEvent.EnsureComplete(); return Task.CompletedTask; }
}

public interface IGeoService
{
    Task<PagedResponse<GeoDto>> ListAsync(GeoResource resource, PagedQueryRequest query, OperationContext context, CancellationToken cancellationToken);
    Task<GeoDto?> GetAsync(GeoResource resource, Guid id, OperationContext context, CancellationToken cancellationToken);
    Task<GeoDto> CreateAsync(GeoResource resource, object request, OperationContext context, CancellationToken cancellationToken);
    Task<GeoDto?> UpdateAsync(GeoResource resource, Guid id, object request, OperationContext context, CancellationToken cancellationToken);
    Task<GeoDto?> DisableAsync(GeoResource resource, Guid id, DisableRequest request, OperationContext context, CancellationToken cancellationToken);
}

public sealed class GeoService(IGeoRepository repository, IGeoAuditSink auditSink) : IGeoService
{
    public Task<PagedResponse<GeoDto>> ListAsync(GeoResource resource, PagedQueryRequest query, OperationContext context, CancellationToken cancellationToken)
    {
        context.EnsureComplete();
        if (query.Page < 1 || query.PageSize is < 1 or > 200) throw new ArgumentOutOfRangeException(nameof(query));
        return repository.ListAsync(resource, query, cancellationToken);
    }

    public async Task<GeoDto?> GetAsync(GeoResource resource, Guid id, OperationContext context, CancellationToken cancellationToken)
    {
        context.EnsureComplete();
        return (await repository.FindAsync(resource, id, cancellationToken)) is { } entity ? ToDto(entity) : null;
    }

    public async Task<GeoDto> CreateAsync(GeoResource resource, object request, OperationContext context, CancellationToken cancellationToken)
    {
        context.EnsureComplete();
        var entity = FromCreate(resource, request);
        Validate(entity);
        await EnsureParentAndCodeAsync(resource, entity, null, cancellationToken);
        entity.Id = Guid.CreateVersion7();
        await repository.AddAsync(resource, entity, cancellationToken);
        await repository.SaveAsync(cancellationToken);
        await AuditAsync(entity, "Create", null, context, null, cancellationToken);
        return ToDto(entity);
    }

    public async Task<GeoDto?> UpdateAsync(GeoResource resource, Guid id, object request, OperationContext context, CancellationToken cancellationToken)
    {
        context.EnsureComplete();
        var entity = await repository.FindAsync(resource, id, cancellationToken);
        if (entity is null) return null;
        var expected = ExpectedVersion(request);
        if (entity.Version != expected) throw new InvalidOperationException("CONCURRENCY_CONFLICT");
        var before = ToJson(entity);
        ApplyUpdate(resource, entity, request); Validate(entity);
        await EnsureParentAndCodeAsync(resource, entity, id, cancellationToken);
        entity.Version++;
        await repository.SaveAsync(cancellationToken);
        await AuditAsync(entity, "Update", before, context, null, cancellationToken);
        return ToDto(entity);
    }

    public async Task<GeoDto?> DisableAsync(GeoResource resource, Guid id, DisableRequest request, OperationContext context, CancellationToken cancellationToken)
    {
        context.EnsureComplete();
        if (string.IsNullOrWhiteSpace(request.Reason)) throw new ArgumentException("A disable reason is required.", nameof(request));
        var entity = await repository.FindAsync(resource, id, cancellationToken);
        if (entity is null) return null;
        if (entity.Version != request.ExpectedVersion) throw new InvalidOperationException("CONCURRENCY_CONFLICT");
        var before = ToJson(entity); entity.IsActive = false; entity.Version++;
        await repository.SaveAsync(cancellationToken);
        await AuditAsync(entity, "Disable", before, context, request.Reason, cancellationToken);
        return ToDto(entity);
    }

    private async Task EnsureParentAndCodeAsync(GeoResource resource, GeoEntity entity, Guid? exceptId, CancellationToken ct)
    {
        var parent = ParentId(entity);
        if (parent is { } id && !await repository.ParentExistsAsync(resource, id, ct)) throw new ArgumentException("The selected parent does not exist.");
        if (await repository.CodeExistsAsync(resource, parent, entity.Code, exceptId, ct)) throw new ArgumentException("Code must be unique at this geography level.");
    }
    private static void Validate(GeoEntity entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Code) || entity.Code.Length > 64) throw new ArgumentException("Code is required and cannot exceed 64 characters.");
        if (string.IsNullOrWhiteSpace(entity.ArabicName) || entity.ArabicName.Length > 200 || entity.EnglishName?.Length > 200) throw new ArgumentException("ArabicName is required and names cannot exceed 200 characters.");
        if (entity is Country { NationalityName.Length: > 200 }) throw new ArgumentException("NationalityName cannot exceed 200 characters.");
    }
    private static Guid? ParentId(GeoEntity e) => e switch { Governorate x => x.CountryId, Directorate x => x.GovernorateId, City x => x.DirectorateId, Area x => x.CityId, _ => null };
    private static int ExpectedVersion(object r) => r switch { UpdateCountryRequest x => x.ExpectedVersion, UpdateGovernorateRequest x => x.ExpectedVersion, UpdateDirectorateRequest x => x.ExpectedVersion, UpdateCityRequest x => x.ExpectedVersion, UpdateAreaRequest x => x.ExpectedVersion, _ => throw new ArgumentException("Unsupported request.") };
    private static GeoEntity FromCreate(GeoResource r, object x) => (r, x) switch
    {
        (GeoResource.Countries, CreateCountryRequest v) => new Country { Code=v.Code, ArabicName=v.ArabicName, EnglishName=v.EnglishName, NationalityName=v.NationalityName },
        (GeoResource.Governorates, CreateGovernorateRequest v) => new Governorate { CountryId=v.CountryId, Code=v.Code, ArabicName=v.ArabicName, EnglishName=v.EnglishName },
        (GeoResource.Directorates, CreateDirectorateRequest v) => new Directorate { GovernorateId=v.GovernorateId, Code=v.Code, ArabicName=v.ArabicName, EnglishName=v.EnglishName },
        (GeoResource.Cities, CreateCityRequest v) => new City { DirectorateId=v.DirectorateId, Code=v.Code, ArabicName=v.ArabicName, EnglishName=v.EnglishName },
        (GeoResource.Areas, CreateAreaRequest v) => new Area { CityId=v.CityId, Code=v.Code, ArabicName=v.ArabicName, EnglishName=v.EnglishName }, _ => throw new ArgumentException("Request does not match the resource.") };
    private static void ApplyUpdate(GeoResource r, GeoEntity e, object x)
    {
        switch (r, e, x) { case (GeoResource.Countries, Country a, UpdateCountryRequest b): a.Code=b.Code;a.ArabicName=b.ArabicName;a.EnglishName=b.EnglishName;a.NationalityName=b.NationalityName;break; case (GeoResource.Governorates, Governorate a, UpdateGovernorateRequest b): a.CountryId=b.CountryId;a.Code=b.Code;a.ArabicName=b.ArabicName;a.EnglishName=b.EnglishName;break; case (GeoResource.Directorates, Directorate a, UpdateDirectorateRequest b): a.GovernorateId=b.GovernorateId;a.Code=b.Code;a.ArabicName=b.ArabicName;a.EnglishName=b.EnglishName;break; case (GeoResource.Cities, City a, UpdateCityRequest b): a.DirectorateId=b.DirectorateId;a.Code=b.Code;a.ArabicName=b.ArabicName;a.EnglishName=b.EnglishName;break; case (GeoResource.Areas, Area a, UpdateAreaRequest b): a.CityId=b.CityId;a.Code=b.Code;a.ArabicName=b.ArabicName;a.EnglishName=b.EnglishName;break; default: throw new ArgumentException("Request does not match the resource."); }
    }
    private static GeoDto ToDto(GeoEntity e) => e switch { Country x => new CountryDto(x.Id,x.Code,x.ArabicName,x.EnglishName,x.NationalityName,x.IsActive,x.Version), Governorate x => new GovernorateDto(x.Id,x.CountryId,x.Code,x.ArabicName,x.EnglishName,x.IsActive,x.Version), Directorate x => new DirectorateDto(x.Id,x.GovernorateId,x.Code,x.ArabicName,x.EnglishName,x.IsActive,x.Version), City x => new CityDto(x.Id,x.DirectorateId,x.Code,x.ArabicName,x.EnglishName,x.IsActive,x.Version), Area x => new AreaDto(x.Id,x.CityId,x.Code,x.ArabicName,x.EnglishName,x.IsActive,x.Version), _ => throw new ArgumentOutOfRangeException(nameof(e)) };
    private async Task AuditAsync(GeoEntity entity, string action, JsonElement? before, OperationContext context, string? reason, CancellationToken ct) => await auditSink.WriteAsync(new BusinessAuditEvent(Guid.CreateVersion7(), context.UserId, DateTimeOffset.UtcNow, context.CompanyId, context.BranchId, entity.GetType().Name, entity.Id, action, context.CorrelationId, reason, before, ToJson(entity)), ct);
    private static JsonElement ToJson(GeoEntity entity) => JsonSerializer.SerializeToElement(ToDto(entity));
}
