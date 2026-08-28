using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using TransportERP.Api.Security;
using TransportERP.Application.Waybills;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Waybills;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class P2C01CShippingApiContractTests
{
    private const string Issuer = "TransportERP.P2C.ApiContractTests";
    private const string Audience = "TransportERP.P2C.Api";
    private const string SigningKey = "transport-erp-p2c-api-contract-signing-key-2026";

    [Fact]
    public async Task All_C_routes_are_POST_permission_protected_and_forward_authenticated_scope()
    {
        var userId = Guid.NewGuid();
        var companyId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var store = new RecordingShippingExecutionStore();

        using var factory = CreateFactory(store);
        using var client = factory.CreateClient();

        foreach (var route in BuildRoutes())
        {
            using (var deniedRequest = route.CreateRequest())
            {
                deniedRequest.Headers.Authorization = new AuthenticationHeaderValue(
                    "Bearer", CreateToken(userId, companyId, branchId, "other.permission"));
                var denied = await client.SendAsync(deniedRequest);
                Assert.Equal(HttpStatusCode.Forbidden, denied.StatusCode);
                var error = await denied.Content.ReadFromJsonAsync<ApiError>();
                Assert.Equal("SCOPE_DENIED", error?.ErrorCode);
            }

            store.Reset();
            var correlationId = Guid.NewGuid();
            using (var allowedRequest = route.CreateRequest())
            {
                allowedRequest.Headers.Authorization = new AuthenticationHeaderValue(
                    "Bearer", CreateToken(userId, companyId, branchId, route.Permission));
                allowedRequest.Headers.TryAddWithoutValidation("X-Correlation-Id", correlationId.ToString());

                var allowed = await client.SendAsync(allowedRequest);
                Assert.Equal(HttpStatusCode.OK, allowed.StatusCode);
                Assert.Equal(route.CallName, store.LastCall);
                Assert.NotNull(store.LastContext);
                Assert.Equal(userId, store.LastContext!.UserId);
                Assert.Equal(companyId, store.LastContext.CompanyId);
                Assert.Equal(branchId, store.LastContext.BranchId);
                Assert.Equal(correlationId, store.LastContext.CorrelationId);
            }

            using (var wrongMethod = new HttpRequestMessage(HttpMethod.Get, route.Path))
            {
                wrongMethod.Headers.Authorization = new AuthenticationHeaderValue(
                    "Bearer", CreateToken(userId, companyId, branchId, route.Permission));
                var response = await client.SendAsync(wrongMethod);
                Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
            }
        }
    }

    [Fact]
    public async Task C_group_requires_authentication_and_complete_company_branch_context()
    {
        var store = new RecordingShippingExecutionStore();
        using var factory = CreateFactory(store);
        using var client = factory.CreateClient();
        var route = BuildRoutes().Single(x => x.CallName == "CreateTrip");

        using (var unauthenticated = route.CreateRequest())
        {
            var response = await client.SendAsync(unauthenticated);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        using (var missingBranch = route.CreateRequest())
        {
            missingBranch.Headers.Authorization = new AuthenticationHeaderValue(
                "Bearer", CreateToken(Guid.NewGuid(), Guid.NewGuid(), null, ShippingExecutionPermissionCodes.TripCreate));
            var response = await client.SendAsync(missingBranch);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }

    [Theory]
    [InlineData("NOT_FOUND", 404)]
    [InlineData("SCOPE_DENIED", 403)]
    [InlineData("CONCURRENCY_CONFLICT", 409)]
    [InlineData("IDEMPOTENCY_CONFLICT", 409)]
    [InlineData("DUPLICATE_OPERATION", 409)]
    [InlineData("DUPLICATE_TRIP_NO", 409)]
    [InlineData("INVALID_STATE", 409)]
    [InlineData("HOLD_BLOCKED", 409)]
    [InlineData("ALREADY_LOADED", 409)]
    [InlineData("QUANTITY_EXCEEDS_REMAINING", 409)]
    [InlineData("QUANTITY_EXCEEDS_RELEASED", 409)]
    [InlineData("QUANTITY_EXCEEDS_ALLOCATION", 409)]
    [InlineData("ROUTE_INCOMPATIBLE", 409)]
    [InlineData("NO_ALLOCATIONS", 409)]
    [InlineData("RESOURCE_CONSTRAINT", 409)]
    [InlineData("MANIFEST_LINE_INVALID", 409)]
    [InlineData("MANIFEST_NOT_ACCEPTED", 409)]
    [InlineData("DRIVER_MISMATCH", 409)]
    [InlineData("VALIDATION_ERROR", 400)]
    [InlineData("REASON_REQUIRED", 400)]
    [InlineData("CLIENT_OPERATION_REQUIRED", 400)]
    public async Task C_error_codes_have_stable_HTTP_mapping(string code, int expectedStatus)
    {
        var userId = Guid.NewGuid();
        var companyId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var store = new RecordingShippingExecutionStore { ErrorCode = code };
        using var factory = CreateFactory(store);
        using var client = factory.CreateClient();
        var route = BuildRoutes().Single(x => x.CallName == "CreateTrip");

        using var request = route.CreateRequest();
        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Bearer", CreateToken(userId, companyId, branchId, ShippingExecutionPermissionCodes.TripCreate));
        var response = await client.SendAsync(request);

        Assert.Equal(expectedStatus, (int)response.StatusCode);
        var error = await response.Content.ReadFromJsonAsync<ApiError>();
        Assert.Equal(code == "SCOPE_DENIED" ? "SCOPE_DENIED" : code, error?.ErrorCode);
        Assert.NotEqual(Guid.Empty, error?.CorrelationId ?? Guid.Empty);
    }

    [Fact]
    public async Task C_does_not_expose_next_phase_runtime_endpoints()
    {
        var store = new RecordingShippingExecutionStore();
        using var factory = CreateFactory(store);
        using var client = factory.CreateClient();
        var token = CreateToken(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "phase.next");
        var id = Guid.NewGuid();

        var forbiddenLaterRoutes = new[]
        {
            $"/api/v1/arrivals/{id}:finalize",
            $"/api/v1/trips/{id}:close",
            $"/api/v1/waybills/{id}/notifications",
            $"/api/v1/exceptions/{id}:resolve",
            $"/api/v1/trips/{id}/accruals:approve",
            $"/api/v1/waybills/{id}/unload",
            $"/api/v1/warehouses/{id}/receipts",
            $"/api/v1/deliveries/{id}:confirm"
        };

        foreach (var path in forbiddenLaterRoutes)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, path)
            {
                Content = JsonContent.Create(new { })
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    private static IReadOnlyList<RouteCase> BuildRoutes()
    {
        var now = DateTimeOffset.UtcNow;
        var waybillId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var tripId = Guid.NewGuid();
        var allocationId = Guid.NewGuid();
        var releaseId = Guid.NewGuid();
        var manifestId = Guid.NewGuid();
        var lineId = Guid.NewGuid();
        var vehicleId = Guid.NewGuid();
        var driverId = Guid.NewGuid();
        var originId = Guid.NewGuid();
        var destinationId = Guid.NewGuid();

        return new[]
        {
            new RouteCase(
                "ReleaseItem", ShippingExecutionPermissionCodes.Release,
                $"/api/v1/waybills/{waybillId}/items/{itemId}/releases",
                () => Post($"/api/v1/waybills/{waybillId}/items/{itemId}/releases",
                    new ReleaseItemRequest(1m, now, "api-release"))),
            new RouteCase(
                "CreateTrip", ShippingExecutionPermissionCodes.TripCreate,
                "/api/v1/trips",
                () => Post("/api/v1/trips",
                    new CreateTripRequest("TR-API-1", vehicleId, driverId, originId, destinationId,
                        now.AddHours(1), [], "api-trip"))),
            new RouteCase(
                "Allocate", ShippingExecutionPermissionCodes.Allocate,
                $"/api/v1/trips/{tripId}/allocations",
                () => Post($"/api/v1/trips/{tripId}/allocations",
                    new AllocateItemRequest(itemId, releaseId, 1m, "api-allocate"))),
            new RouteCase(
                "Unallocate", ShippingExecutionPermissionCodes.Unallocate,
                $"/api/v1/allocations/{allocationId}:reverse",
                () => Post($"/api/v1/allocations/{allocationId}:reverse",
                    new UnallocateRequest("contract test", "api-unallocate"))),
            new RouteCase(
                "GenerateManifest", ShippingExecutionPermissionCodes.ManifestCreate,
                $"/api/v1/trips/{tripId}/manifests",
                () => Post($"/api/v1/trips/{tripId}/manifests",
                    new GenerateManifestRequest("MF-API-1", "api-manifest"))),
            new RouteCase(
                "LoadManifestLine", ShippingExecutionPermissionCodes.ManifestLoad,
                $"/api/v1/manifests/{manifestId}/lines/{lineId}:load",
                () => Post($"/api/v1/manifests/{manifestId}/lines/{lineId}:load",
                    new LoadManifestLineRequest(1m, now, true, "api-load"))),
            new RouteCase(
                "FinalizeManifest", ShippingExecutionPermissionCodes.ManifestFinalize,
                $"/api/v1/manifests/{manifestId}:finalize",
                () => Post($"/api/v1/manifests/{manifestId}:finalize",
                    new FinalizeManifestRequest(1, "api-finalize"))),
            new RouteCase(
                "HandoverManifest", ShippingExecutionPermissionCodes.ManifestHandover,
                $"/api/v1/manifests/{manifestId}:handover",
                () => Post($"/api/v1/manifests/{manifestId}:handover",
                    new HandoverManifestRequest(driverId, now, 1, "api-handover"))),
            new RouteCase(
                "StartTrip", ShippingExecutionPermissionCodes.TripStart,
                $"/api/v1/trips/{tripId}:start",
                () => Post($"/api/v1/trips/{tripId}:start",
                    new StartTripRequest(now, 1, "api-start")))
        };
    }

    private static HttpRequestMessage Post<T>(string path, T body)
        => new(HttpMethod.Post, path) { Content = JsonContent.Create(body) };

    private static WebApplicationFactory<Program> CreateFactory(IShippingExecutionStore store)
        => new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:TransportErp",
                "Host=localhost;Port=5432;Database=unused;Username=unused;Password=unused");
            builder.UseSetting("Auth:Issuer", Issuer);
            builder.UseSetting("Auth:Audience", Audience);
            builder.UseSetting("Auth:SigningKey", SigningKey);
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IShippingExecutionStore>();
                services.RemoveAll<ICurrentRequestSecurityResolver>();
                services.AddSingleton(store);
                services.AddSingleton<ICurrentRequestSecurityResolver, ContractRequestSecurityResolver>();
            });
        });

    private static string CreateToken(
        Guid userId, Guid companyId, Guid? branchId, string permission)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new("company_id", companyId.ToString()),
            new("permission", permission)
        };
        if (branchId.HasValue)
            claims.Add(new Claim("branch_id", branchId.Value.ToString()));

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = Issuer,
            Audience = Audience,
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)),
                SecurityAlgorithms.HmacSha256)
        };
        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(handler.CreateToken(descriptor));
    }

    private sealed record RouteCase(
        string CallName,
        string Permission,
        string Path,
        Func<HttpRequestMessage> CreateRequest);

    private sealed record ApiError(string ErrorCode, Guid CorrelationId);

    private sealed class ContractRequestSecurityResolver : ICurrentRequestSecurityResolver
    {
        public Task<RequestSecurityResolution> ResolveAsync(
            HttpContext http,
            string permissionCode,
            CancellationToken cancellationToken = default)
        {
            if (http.User.Identity?.IsAuthenticated != true)
                return Task.FromResult(new RequestSecurityResolution(null, Results.Unauthorized()));

            if (!Guid.TryParse(http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? http.User.FindFirst("sub")?.Value, out var userId) ||
                !Guid.TryParse(http.User.FindFirst("company_id")?.Value, out var companyId) ||
                !Guid.TryParse(http.User.FindFirst("branch_id")?.Value, out var branchId))
                return Task.FromResult(new RequestSecurityResolution(null, Results.Unauthorized()));

            var correlationId = Guid.TryParse(
                http.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var parsed)
                    ? parsed
                    : Guid.NewGuid();
            var context = new OperationContext(userId, companyId, branchId, correlationId);
            var allowed = http.User.Claims.Any(x => x.Type is "permission" or ClaimTypes.Role &&
                string.Equals(x.Value, permissionCode, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(allowed
                ? new RequestSecurityResolution(context, null)
                : new RequestSecurityResolution(null, Results.Json(
                    new { ErrorCode = "SCOPE_DENIED", correlationId },
                    statusCode: StatusCodes.Status403Forbidden)));
        }
    }

    private sealed class RecordingShippingExecutionStore : IShippingExecutionStore
    {
        public string? LastCall { get; private set; }
        public OperationContext? LastContext { get; private set; }
        public string? ErrorCode { get; set; }

        public void Reset()
        {
            LastCall = null;
            LastContext = null;
            ErrorCode = null;
        }

        private OperationContext Capture(string call, OperationContext context)
        {
            if (ErrorCode is not null)
                throw new WaybillPersistenceException(ErrorCode);
            LastCall = call;
            LastContext = context;
            return context;
        }

        public Task<ItemQuantityStateResponse> ReleaseItemAsync(
            OperationContext context, Guid waybillId, Guid itemId, ReleaseItemRequest request,
            CancellationToken cancellationToken)
        {
            Capture("ReleaseItem", context);
            return Task.FromResult(new ItemQuantityStateResponse(
                waybillId, itemId, 10m, request.Quantity, 10m - request.Quantity, context.CorrelationId));
        }

        public Task<TripResponse> CreateTripAsync(
            OperationContext context, CreateTripRequest request, CancellationToken cancellationToken)
        {
            Capture("CreateTrip", context);
            return Task.FromResult(new TripResponse(
                Guid.NewGuid(), context.CompanyId, context.BranchId, request.TripNo,
                request.VehicleId, request.DriverId, request.OriginId, request.DestinationId,
                request.PlannedDepartAt, null, "DRAFT", 1, [], context.CorrelationId));
        }

        public Task<AllocationResponse> AllocateAsync(
            OperationContext context, Guid tripId, AllocateItemRequest request, CancellationToken cancellationToken)
        {
            Capture("Allocate", context);
            return Task.FromResult(new AllocationResponse(
                Guid.NewGuid(), request.WaybillItemId, request.ReleaseId, tripId,
                request.Quantity, "ALLOCATED", null, context.CorrelationId));
        }

        public Task<AllocationResponse> UnallocateAsync(
            OperationContext context, Guid allocationId, UnallocateRequest request, CancellationToken cancellationToken)
        {
            Capture("Unallocate", context);
            return Task.FromResult(new AllocationResponse(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                1m, "REVERSED", allocationId, context.CorrelationId));
        }

        public Task<ManifestResponse> GenerateManifestAsync(
            OperationContext context, Guid tripId, GenerateManifestRequest request, CancellationToken cancellationToken)
        {
            Capture("GenerateManifest", context);
            return Task.FromResult(new ManifestResponse(
                Guid.NewGuid(), tripId, request.ManifestNo ?? "MF-API", DateTimeOffset.UtcNow,
                null, null, "DRAFT", 1, 1, [], context.CorrelationId));
        }

        public Task<ManifestLineResponse> LoadManifestLineAsync(
            OperationContext context, Guid manifestId, Guid lineId, LoadManifestLineRequest request,
            CancellationToken cancellationToken)
        {
            Capture("LoadManifestLine", context);
            return Task.FromResult(new ManifestLineResponse(
                lineId, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), request.Quantity,
                request.Quantity, 0m, 0m, "LOADED"));
        }

        public Task<ManifestResponse> FinalizeManifestAsync(
            OperationContext context, Guid manifestId, FinalizeManifestRequest request,
            CancellationToken cancellationToken)
        {
            Capture("FinalizeManifest", context);
            return Task.FromResult(new ManifestResponse(
                manifestId, Guid.NewGuid(), "MF-API", DateTimeOffset.UtcNow,
                null, null, "FINALIZED", request.ExpectedVersion + 1, 2, [], context.CorrelationId));
        }

        public Task<ManifestResponse> HandoverManifestAsync(
            OperationContext context, Guid manifestId, HandoverManifestRequest request,
            CancellationToken cancellationToken)
        {
            Capture("HandoverManifest", context);
            return Task.FromResult(new ManifestResponse(
                manifestId, Guid.NewGuid(), "MF-API", DateTimeOffset.UtcNow,
                request.AcceptedAt, request.AcceptedAt, "ACCEPTED", request.ExpectedVersion + 1,
                2, [], context.CorrelationId));
        }

        public Task<TripResponse> StartTripAsync(
            OperationContext context, Guid tripId, StartTripRequest request, CancellationToken cancellationToken)
        {
            Capture("StartTrip", context);
            return Task.FromResult(new TripResponse(
                tripId, context.CompanyId, context.BranchId, "TR-API", Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow.AddHours(-1), request.ActualDepartAt,
                "DEPARTED", request.ExpectedVersion + 1, [], context.CorrelationId));
        }
    }
}
