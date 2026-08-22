using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using TransportERP.Application.Waybills;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Waybills;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class P2C01DArrivalApiContractTests
{
    private const string Issuer = "TransportERP.P2D.ApiContractTests";
    private const string Audience = "TransportERP.P2D.Api";
    private const string SigningKey = "transport-erp-p2d-api-contract-signing-key-2026";

    [Fact]
    public async Task D_routes_enforce_permission_and_forward_authenticated_scope()
    {
        var userId = Guid.NewGuid(); var companyId = Guid.NewGuid(); var branchId = Guid.NewGuid();
        var store = new RecordingArrivalStore();
        using var factory = CreateFactory(store); using var client = factory.CreateClient();

        foreach (var route in BuildRoutes())
        {
            using (var denied = route.CreateRequest())
            {
                denied.Headers.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(userId, companyId, branchId, "other.permission"));
                Assert.Equal(HttpStatusCode.Forbidden, (await client.SendAsync(denied)).StatusCode);
            }

            store.Reset(); var correlation = Guid.NewGuid();
            using var allowed = route.CreateRequest();
            allowed.Headers.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(userId, companyId, branchId, route.Permission));
            allowed.Headers.TryAddWithoutValidation("X-Correlation-Id", correlation.ToString());
            var response = await client.SendAsync(allowed);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(route.CallName, store.LastCall);
            Assert.Equal(userId, store.LastContext?.UserId);
            Assert.Equal(companyId, store.LastContext?.CompanyId);
            Assert.Equal(branchId, store.LastContext?.BranchId);
            Assert.Equal(correlation, store.LastContext?.CorrelationId);
        }
    }

    [Fact]
    public async Task D_requires_authentication_and_complete_branch_context()
    {
        var store = new RecordingArrivalStore();
        using var factory = CreateFactory(store); using var client = factory.CreateClient();
        var route = BuildRoutes().First();
        using var anonymous = route.CreateRequest();
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.SendAsync(anonymous)).StatusCode);
        using var missingBranch = route.CreateRequest();
        missingBranch.Headers.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(Guid.NewGuid(), Guid.NewGuid(), null, route.Permission));
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.SendAsync(missingBranch)).StatusCode);
    }

    [Theory]
    [InlineData("NOT_FOUND", 404)]
    [InlineData("SCOPE_DENIED", 403)]
    [InlineData("INVALID_STATE", 409)]
    [InlineData("LOCATION_INVALID", 409)]
    [InlineData("DUPLICATE_OPERATION", 409)]
    [InlineData("IDEMPOTENCY_CONFLICT", 409)]
    [InlineData("CONCURRENCY_CONFLICT", 409)]
    [InlineData("QUANTITY_EXCEEDS_IN_TRANSIT", 409)]
    [InlineData("QUANTITY_EXCEEDS_AVAILABLE", 409)]
    [InlineData("ROUTE_INCOMPATIBLE", 409)]
    [InlineData("HOLD_BLOCKED", 409)]
    [InlineData("UNVALIDATED_LINES", 409)]
    [InlineData("DIFFERENCE_REQUIRES_EVIDENCE", 409)]
    [InlineData("CARGO_UNACCOUNTED", 409)]
    [InlineData("CUSTODY_OPEN", 409)]
    [InlineData("EXCEPTION_BLOCKED", 409)]
    [InlineData("VALIDATION_ERROR", 400)]
    [InlineData("INVALID_FILTER", 400)]
    public async Task D_error_mapping_is_stable(string code, int status)
    {
        var store = new RecordingArrivalStore { ErrorCode = code };
        using var factory = CreateFactory(store); using var client = factory.CreateClient();
        var route = BuildRoutes().First();
        using var request = route.CreateRequest();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), route.Permission));
        var response = await client.SendAsync(request);
        Assert.Equal(status, (int)response.StatusCode);
        var error = await response.Content.ReadFromJsonAsync<ApiError>();
        Assert.Equal(code, error?.ErrorCode);
    }

    [Fact]
    public async Task D_does_not_expose_delivery_or_later_phase_runtime()
    {
        var store = new RecordingArrivalStore();
        using var factory = CreateFactory(store); using var client = factory.CreateClient();
        var token = CreateToken(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "later.phase"); var id = Guid.NewGuid();
        foreach (var path in new[]
        {
            $"/api/v1/waybills/{id}/deliveries", $"/api/v1/deliveries/{id}/proof",
            $"/api/v1/waybills/{id}/notifications", $"/api/v1/exceptions/{id}:resolve",
            $"/api/v1/trips/{id}:settle", $"/api/v1/waybills/{id}:financial-close"
        })
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, path) { Content = JsonContent.Create(new { }) };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Assert.Equal(HttpStatusCode.NotFound, (await client.SendAsync(request)).StatusCode);
        }
    }

    private static IReadOnlyList<RouteCase> BuildRoutes()
    {
        var now = DateTimeOffset.UtcNow; var trip = Guid.NewGuid(); var manifest = Guid.NewGuid(); var location = Guid.NewGuid();
        var arrival = Guid.NewGuid(); var line = Guid.NewGuid(); var holding = Guid.NewGuid(); var nextTrip = Guid.NewGuid();
        var waybill = Guid.NewGuid(); var item = Guid.NewGuid();
        return new[]
        {
            new RouteCase("RecordArrival", ArrivalExecutionPermissionCodes.RecordArrival, () => Post($"/api/v1/trips/{trip}/arrivals", new RecordArrivalRequest(manifest, location, now, "d-arrival"))),
            new RouteCase("RecordUnload", ArrivalExecutionPermissionCodes.RecordUnload, () => Post($"/api/v1/arrivals/{arrival}/lines:unload", new RecordUnloadRequest([new ArrivalUnloadLineInput(line, 1m, 0m, null, null, null)], now, "d-unload"))),
            new RouteCase("Reallocate", ArrivalExecutionPermissionCodes.Reallocate, () => Post($"/api/v1/holdings/{holding}:allocate", new ReallocateTransitRequest(nextTrip, 1m, "d-reallocate"))),
            new RouteCase("FinalizeArrival", ArrivalExecutionPermissionCodes.FinalizeArrival, () => Post($"/api/v1/arrivals/{arrival}:finalize", new FinalizeArrivalRequest(1, "d-finalize"))),
            new RouteCase("CloseTrip", ArrivalExecutionPermissionCodes.TripClose, () => Post($"/api/v1/trips/{trip}:close", new CloseTripRequest(now, 1, "d-close"))),
            new RouteCase("WaybillMovement", ArrivalExecutionPermissionCodes.WaybillMovementView, () => new HttpRequestMessage(HttpMethod.Get, $"/api/v1/waybills/{waybill}/movement")),
            new RouteCase("ItemMovement", ArrivalExecutionPermissionCodes.ItemMovementView, () => new HttpRequestMessage(HttpMethod.Get, $"/api/v1/waybills/{waybill}/items/{item}/movement"))
        };
    }

    private static HttpRequestMessage Post<T>(string path, T body) => new(HttpMethod.Post, path) { Content = JsonContent.Create(body) };

    private static WebApplicationFactory<Program> CreateFactory(IArrivalExecutionStore store)
        => new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:TransportErp", "Host=localhost;Port=5432;Database=unused;Username=unused;Password=unused");
            builder.UseSetting("Auth:Issuer", Issuer); builder.UseSetting("Auth:Audience", Audience); builder.UseSetting("Auth:SigningKey", SigningKey);
            builder.ConfigureServices(services => { services.RemoveAll<IArrivalExecutionStore>(); services.AddSingleton(store); });
        });

    private static string CreateToken(Guid userId, Guid companyId, Guid? branchId, string permission)
    {
        var claims = new List<Claim> { new(JwtRegisteredClaimNames.Sub, userId.ToString()), new("company_id", companyId.ToString()), new("permission", permission) };
        if (branchId.HasValue) claims.Add(new Claim("branch_id", branchId.Value.ToString()));
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims), Issuer = Issuer, Audience = Audience, Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)), SecurityAlgorithms.HmacSha256)
        };
        var handler = new JwtSecurityTokenHandler(); return handler.WriteToken(handler.CreateToken(descriptor));
    }

    private sealed record RouteCase(string CallName, string Permission, Func<HttpRequestMessage> CreateRequest);
    private sealed record ApiError(string ErrorCode, Guid CorrelationId);

    private sealed class RecordingArrivalStore : IArrivalExecutionStore
    {
        public string? LastCall { get; private set; } public OperationContext? LastContext { get; private set; } public string? ErrorCode { get; set; }
        public void Reset() { LastCall = null; LastContext = null; ErrorCode = null; }
        private void Capture(string name, OperationContext context)
        {
            if (ErrorCode is not null) throw new WaybillPersistenceException(ErrorCode);
            LastCall = name; LastContext = context;
        }
        private static ArrivalReceiptResponse Receipt(OperationContext c, Guid id = default) => new(id == default ? Guid.NewGuid() : id, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), c.BranchId, DateTimeOffset.UtcNow, c.UserId, "DRAFT", 1, [], c.CorrelationId);
        private static TripResponse Trip(OperationContext c, Guid id) => new(id, c.CompanyId, c.BranchId, "TR-D", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, "ARRIVED", 1, [], c.CorrelationId);
        public Task<ArrivalReceiptResponse> RecordArrivalAsync(OperationContext c, Guid tripId, RecordArrivalRequest r, CancellationToken ct) { Capture("RecordArrival", c); return Task.FromResult(Receipt(c)); }
        public Task<ArrivalReceiptResponse> RecordUnloadAsync(OperationContext c, Guid arrivalId, RecordUnloadRequest r, CancellationToken ct) { Capture("RecordUnload", c); return Task.FromResult(Receipt(c, arrivalId)); }
        public Task<AllocationResponse> ReallocateTransitAsync(OperationContext c, Guid holdingId, ReallocateTransitRequest r, CancellationToken ct) { Capture("Reallocate", c); return Task.FromResult(new AllocationResponse(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), r.NextTripId, r.Quantity, "ALLOCATED", null, c.CorrelationId)); }
        public Task<ArrivalReceiptResponse> FinalizeArrivalAsync(OperationContext c, Guid arrivalId, FinalizeArrivalRequest r, CancellationToken ct) { Capture("FinalizeArrival", c); return Task.FromResult(Receipt(c, arrivalId) with { Status = "FINALIZED", Version = 2 }); }
        public Task<TripResponse> CloseTripAsync(OperationContext c, Guid tripId, CloseTripRequest r, CancellationToken ct) { Capture("CloseTrip", c); return Task.FromResult(Trip(c, tripId) with { Status = "CLOSED", Version = 2 }); }
        public Task<WaybillMovementResponse> GetWaybillMovementAsync(OperationContext c, Guid waybillId, MovementQueryRequest r, CancellationToken ct) { Capture("WaybillMovement", c); return Task.FromResult(new WaybillMovementResponse(waybillId, [], c.CorrelationId)); }
        public Task<ItemMovementResponse> GetItemMovementAsync(OperationContext c, Guid waybillId, Guid itemId, MovementQueryRequest r, CancellationToken ct) { Capture("ItemMovement", c); return Task.FromResult(new ItemMovementResponse(waybillId, itemId, 1m, 1m, 1m, 0m, 1m, 0m, 0m, [], c.CorrelationId)); }
    }
}
