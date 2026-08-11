using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportERP.Api.Authorization;
using TransportERP.Api.Controllers;
using TransportERP.Application.Geo;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Geo;
using TransportERP.Domain.Geo;
using TransportERP.Infrastructure.Geo;

namespace TransportERP.Tests;

public sealed class GeoAuditAndErrorContractTests
{
    [Fact]
    public void TransportError_NotFoundIsDefined_AndUnknownValuesRemainRejected()
    {
        var error = new TransportError(TransportErrorCode.NotFound, Guid.CreateVersion7(), "error.notFound");
        error.EnsureComplete();
        Assert.Throws<ArgumentException>(() => new TransportError((TransportErrorCode)999, Guid.CreateVersion7(), "error.unknown").EnsureComplete());
    }

    [Fact]
    public async Task MissingAuthorizedGeoRecord_ReturnsStructuredNotFoundWithTheOperationCorrelation()
    {
        var correlation = Guid.CreateVersion7();
        var controller = new CountriesController(new StubGeoService()) { ControllerContext = new ControllerContext { HttpContext = AuthorizedContext("GEN003.View", correlation) } };

        var result = await controller.Get(Guid.CreateVersion7(), CancellationToken.None);

        var response = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        var error = Assert.IsType<TransportError>(response.Value);
        Assert.Equal(TransportErrorCode.NotFound, error.Code);
        Assert.Equal(correlation, error.CorrelationId);
        Assert.Equal("error.notFound", error.MessageKey);
    }

    [Fact]
    public async Task PermissionAndScopeDenialsRemainStructuredForbidden_NotNotFound()
    {
        var service = new StubGeoService();
        var missingPermission = new CountriesController(service) { ControllerContext = new ControllerContext { HttpContext = AuthorizedContext(null, Guid.CreateVersion7()) } };
        var missingScope = new CountriesController(service) { ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = Principal("GEN003.View", includeScope: false) } } };

        var permissionResult = Assert.IsType<ObjectResult>((await missingPermission.Get(Guid.CreateVersion7(), CancellationToken.None)).Result);
        var scopeResult = Assert.IsType<ObjectResult>((await missingScope.Get(Guid.CreateVersion7(), CancellationToken.None)).Result);

        Assert.Equal(StatusCodes.Status403Forbidden, permissionResult.StatusCode);
        Assert.Equal(TransportErrorCode.PermissionDenied, Assert.IsType<TransportError>(permissionResult.Value).Code);
        Assert.Equal(StatusCodes.Status403Forbidden, scopeResult.StatusCode);
        Assert.Equal(TransportErrorCode.ScopeDenied, Assert.IsType<TransportError>(scopeResult.Value).Code);
    }

    [Fact]
    public async Task GeoMutationsAppendTheRequiredEventsBeforeTheSinglePersistenceStep()
    {
        var repository = new RecordingRepository();
        var writer = new RecordingAuditWriter();
        var service = new GeoService(repository, writer);
        var context = NewContext();
        var created = await service.CreateAsync(GeoResource.Countries, new CreateCountryRequest("YE", "اليمن", null, null), context, CancellationToken.None);
        var updated = await service.UpdateAsync(GeoResource.Countries, created.Id, new UpdateCountryRequest("YE", "الجمهورية اليمنية", null, null, created.Version), context, CancellationToken.None);
        var disabled = await service.DisableAsync(GeoResource.Countries, created.Id, new DisableRequest(updated!.Version, "duplicate"), context, CancellationToken.None);

        Assert.Equal(3, repository.SaveCount);
        Assert.Equal(["Create", "Update", "Disable"], writer.Events.Select(x => x.Action).ToArray());
        Assert.Null(writer.Events[0].BeforeState);
        Assert.True(writer.Events[1].BeforeState.HasValue && writer.Events[1].AfterState.HasValue);
        Assert.Equal("duplicate", writer.Events[2].Reason);
        Assert.True(writer.Events[2].BeforeState.HasValue && writer.Events[2].AfterState.HasValue);
        Assert.False(disabled!.IsActive);
    }

    [Fact]
    public async Task AuditFailurePreventsTheGeoBusinessPersistenceStep_AndNoNoOpSinkRemains()
    {
        var repository = new RecordingRepository();
        var service = new GeoService(repository, new ThrowingAuditWriter());

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(GeoResource.Countries, new CreateCountryRequest("YE", "اليمن", null, null), NewContext(), CancellationToken.None));

        Assert.Equal(0, repository.SaveCount);
        Assert.Null(typeof(GeoService).Assembly.GetType("TransportERP.Application.Geo.GeoAuditSink"));
        Assert.Null(typeof(GeoService).Assembly.GetType("TransportERP.Application.Geo.IGeoAuditSink"));
    }

    [Fact]
    public async Task AuditStorageIsAppendOnly()
    {
        var options = new DbContextOptionsBuilder<TransportErpDbContext>()
            .UseMySql("Server=localhost;Database=transporterp_tests;User=root;Password=;", new MySqlServerVersion(new Version(8, 0, 0)))
            .Options;
        await using var db = new TransportErpDbContext(options);
        var auditEvent = new BusinessAuditEvent(Guid.CreateVersion7(), Guid.CreateVersion7(), DateTimeOffset.UtcNow, Guid.CreateVersion7(), Guid.CreateVersion7(), "Country", Guid.CreateVersion7(), "Create", Guid.CreateVersion7(), null, null, null);
        await new EfBusinessAuditWriter(db).AppendAsync(auditEvent);
        db.Entry(auditEvent).State = EntityState.Modified;

        await Assert.ThrowsAsync<InvalidOperationException>(() => db.SaveChangesAsync());
    }

    private static DefaultHttpContext AuthorizedContext(string? permission, Guid correlation)
    {
        var context = new DefaultHttpContext { User = Principal(permission, includeScope: true) };
        context.Request.Headers["X-Correlation-Id"] = correlation.ToString();
        return context;
    }

    private static ClaimsPrincipal Principal(string? permission, bool includeScope)
    {
        var claims = new List<Claim> { new(GeoClaims.UserId, Guid.CreateVersion7().ToString()) };
        if (includeScope)
        {
            claims.Add(new Claim(GeoClaims.CompanyId, Guid.CreateVersion7().ToString()));
            claims.Add(new Claim(GeoClaims.BranchId, Guid.CreateVersion7().ToString()));
        }
        if (permission is not null) claims.Add(new Claim(GeoClaims.Permission, permission));
        return new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));
    }

    private static OperationContext NewContext() => new(Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7());

    private sealed class RecordingRepository : IGeoRepository
    {
        private readonly Dictionary<Guid, GeoEntity> entities = [];
        public int SaveCount { get; private set; }
        public Task<GeoEntity?> FindAsync(GeoResource resource, Guid id, CancellationToken cancellationToken) => Task.FromResult<GeoEntity?>(entities.GetValueOrDefault(id));
        public Task<bool> ParentExistsAsync(GeoResource resource, Guid parentId, CancellationToken cancellationToken) => Task.FromResult(true);
        public Task<bool> CodeExistsAsync(GeoResource resource, Guid? parentId, string code, Guid? exceptId, CancellationToken cancellationToken) => Task.FromResult(false);
        public Task<PagedResponse<GeoDto>> ListAsync(GeoResource resource, PagedQueryRequest query, CancellationToken cancellationToken) => Task.FromResult(new PagedResponse<GeoDto>([], query.Page, query.PageSize, 0));
        public Task AddAsync(GeoResource resource, GeoEntity entity, CancellationToken cancellationToken) { entities.Add(entity.Id, entity); return Task.CompletedTask; }
        public Task SaveAsync(CancellationToken cancellationToken) { SaveCount++; return Task.CompletedTask; }
    }

    private sealed class RecordingAuditWriter : IBusinessAuditWriter
    {
        public List<BusinessAuditEvent> Events { get; } = [];
        public ValueTask AppendAsync(BusinessAuditEvent auditEvent, CancellationToken cancellationToken = default) { Events.Add(auditEvent); return ValueTask.CompletedTask; }
    }

    private sealed class ThrowingAuditWriter : IBusinessAuditWriter
    {
        public ValueTask AppendAsync(BusinessAuditEvent auditEvent, CancellationToken cancellationToken = default) => ValueTask.FromException(new InvalidOperationException("audit persistence failed"));
    }

    private sealed class StubGeoService : IGeoService
    {
        public Task<PagedResponse<GeoDto>> ListAsync(GeoResource resource, PagedQueryRequest query, OperationContext context, CancellationToken cancellationToken) => Task.FromResult(new PagedResponse<GeoDto>([], query.Page, query.PageSize, 0));
        public Task<GeoDto?> GetAsync(GeoResource resource, Guid id, OperationContext context, CancellationToken cancellationToken) => Task.FromResult<GeoDto?>(null);
        public Task<GeoDto> CreateAsync(GeoResource resource, object request, OperationContext context, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task<GeoDto?> UpdateAsync(GeoResource resource, Guid id, object request, OperationContext context, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task<GeoDto?> DisableAsync(GeoResource resource, Guid id, DisableRequest request, OperationContext context, CancellationToken cancellationToken) => throw new NotSupportedException();
    }
}
