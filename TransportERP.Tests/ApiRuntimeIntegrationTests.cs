using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using TransportERP.Api;
using TransportERP.Api.Authorization;
using TransportERP.Api.Clients;
using TransportERP.Api.Policies;
using TransportERP.Api.ReferenceData;

namespace TransportERP.Tests;

/// <summary>
/// HTTP-boundary evidence for the two remaining W2 gaps. These tests deliberately enter through
/// the hosted API rather than invoking controllers or policy helpers directly.
/// </summary>
public sealed class ApiRuntimeIntegrationTests
{
    [Fact]
    public async Task Lookup_UsesJwtAuthenticationAndAuthorization_RejectsForgedHeaders_AndCapsAt50()
    {
        using var factory = new TransportApiFactory();
        using var authorizedClient = factory.CreateClient();
        authorizedClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", factory.CreateToken(permission: true, company: "north", branch: "north-1"));

        using var allowed = await authorizedClient.GetAsync("/api/reference-data/lookup?query=Reference");
        Assert.Equal(HttpStatusCode.OK, allowed.StatusCode);
        var results = await allowed.Content.ReadFromJsonAsync<List<LookupItem>>();
        Assert.NotNull(results);
        Assert.Equal(RequestLimitPolicy.MaximumLookupResults, results.Count);
        Assert.All(results, result =>
        {
            Assert.Equal("north", result.Company);
            Assert.Equal("north-1", result.Branch);
        });

        using var crossCompany = await authorizedClient.GetAsync(
            "/api/reference-data/lookup?query=Reference&company=south");
        using var crossBranch = await authorizedClient.GetAsync(
            "/api/reference-data/lookup?query=Reference&branch=north-2");
        Assert.Equal(HttpStatusCode.Forbidden, crossCompany.StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, crossBranch.StatusCode);

        using var forgedHeaderClient = factory.CreateClient();
        forgedHeaderClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", factory.CreateToken(permission: false, company: "north", branch: "north-1"));
        forgedHeaderClient.DefaultRequestHeaders.Add("X-TransportERP-Permission", LookupClaims.ReadPermission);
        forgedHeaderClient.DefaultRequestHeaders.Add("X-TransportERP-Company", "south");
        forgedHeaderClient.DefaultRequestHeaders.Add("X-TransportERP-Branch", "south-1");

        using var forbidden = await forgedHeaderClient.GetAsync("/api/reference-data/lookup?query=Reference");
        Assert.Equal(HttpStatusCode.Forbidden, forbidden.StatusCode);
    }

    [Fact]
    public async Task DownstreamStatus_UsesFullContainerPathAndHonoursRetryAfter()
    {
        using var factory = new TransportApiFactory();
        factory.Downstream.Enqueue((HttpStatusCode)429, TimeSpan.FromSeconds(7));
        factory.Downstream.Enqueue(HttpStatusCode.OK);

        using var client = factory.CreateClient();
        using var response = await client.GetAsync("/api/operations/downstream-status");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, factory.Downstream.RequestCount);
        Assert.Single(factory.Delay.Delays);
        Assert.True(factory.Delay.Delays[0] >= TimeSpan.FromSeconds(7));
    }

    [Fact]
    public async Task ContainerResolvedApiClient_ProtectsUnsafeWritesUnlessAnIdempotencyKeyIsPresent()
    {
        using var factory = new TransportApiFactory();
        factory.Downstream.Enqueue(HttpStatusCode.ServiceUnavailable);
        factory.Downstream.Enqueue(HttpStatusCode.OK);

        using var scope = factory.Services.CreateScope();
        var apiClient = scope.ServiceProvider.GetRequiredService<IApiClient>();
        using var unsafeRequest = new HttpRequestMessage(HttpMethod.Post, "https://downstream.test/status")
        {
            Content = new StringContent("{}")
        };
        using var unsafeResponse = await apiClient.SendAsync(unsafeRequest);
        Assert.Equal(HttpStatusCode.ServiceUnavailable, unsafeResponse.StatusCode);
        Assert.Equal(1, factory.Downstream.RequestCount);

        factory.Downstream.Enqueue(HttpStatusCode.ServiceUnavailable);
        factory.Downstream.Enqueue(HttpStatusCode.OK);
        using var idempotentRequest = new HttpRequestMessage(HttpMethod.Post, "https://downstream.test/status")
        {
            Content = new StringContent("{}")
        };
        idempotentRequest.Headers.Add("Idempotency-Key", "integration-proof-key");
        using var idempotentResponse = await apiClient.SendAsync(idempotentRequest);
        Assert.Equal(HttpStatusCode.OK, idempotentResponse.StatusCode);
        Assert.Equal(3, factory.Downstream.RequestCount);
    }

    private sealed class TransportApiFactory : WebApplicationFactory<Program>
    {
        private const string Issuer = "TransportERP.IntegrationTests";
        private const string Audience = "TransportERP.Api.IntegrationTests";
        private const string SigningKey = "integration-test-signing-key-must-be-at-least-32-characters";

        public ScriptedDownstreamHandler Downstream { get; } = new();
        public RecordingDelay Delay { get; } = new();

        public string CreateToken(bool permission, string company, string branch)
        {
            var claims = new List<Claim>
            {
                new(LookupClaims.Company, company),
                new(LookupClaims.Branch, branch)
            };
            if (permission)
            {
                claims.Add(new Claim(LookupClaims.Permission, LookupClaims.ReadPermission));
            }

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)), SecurityAlgorithms.HmacSha256);
            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                Issuer, Audience, claims, expires: DateTime.UtcNow.AddMinutes(5), signingCredentials: credentials));
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((_, configuration) => configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Authentication:JwtBearer:Issuer"] = Issuer,
                ["Authentication:JwtBearer:Audience"] = Audience,
                ["Authentication:JwtBearer:SigningKey"] = SigningKey,
                ["Downstream:StatusUrl"] = "https://downstream.test/status"
            }));
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IReferenceLookupProvider>();
                services.AddSingleton<IReferenceLookupProvider, LargeScopedLookupProvider>();
                services.RemoveAll<IResilienceDelay>();
                services.AddSingleton<IResilienceDelay>(Delay);

                // This is the same concrete client and retry handler that Program registers.
                // The test transport replaces only the network edge, allowing the complete DI
                // chain Controller -> Service -> IApiClient -> SafeReadRetryHandler to be observed.
                services.RemoveAll<IApiClient>();
                services.AddScoped<IApiClient>(serviceProvider => new ApiClient(new HttpClient(
                    new SafeReadRetryHandler(serviceProvider.GetRequiredService<IResilienceDelay>())
                    {
                        InnerHandler = Downstream
                    }, disposeHandler: false)
                {
                    Timeout = OutgoingRequestResiliencePolicy.TotalRequestTimeout
                }));
            });
        }
    }

    private sealed class LargeScopedLookupProvider : IReferenceLookupProvider
    {
        public IReadOnlyList<LookupItem> Search(string query, LookupAccessContext access) =>
            Enumerable.Range(1, RequestLimitPolicy.MaximumLookupResults + 25)
                .Select(number => new LookupItem(number.ToString(), $"Reference {number}", access.Company, access.Branch))
                .ToArray();
    }

    public sealed class RecordingDelay : IResilienceDelay
    {
        public List<TimeSpan> Delays { get; } = [];
        public Task DelayAsync(TimeSpan delay, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Delays.Add(delay);
            return Task.CompletedTask;
        }
    }

    public sealed class ScriptedDownstreamHandler : HttpMessageHandler
    {
        private readonly Queue<(HttpStatusCode Status, TimeSpan? RetryAfter)> _responses = new();
        public int RequestCount { get; private set; }

        public void Enqueue(HttpStatusCode status, TimeSpan? retryAfter = null) => _responses.Enqueue((status, retryAfter));

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestCount++;
            (HttpStatusCode Status, TimeSpan? RetryAfter) step = _responses.Count > 0
                ? _responses.Dequeue()
                : (HttpStatusCode.OK, null);
            var (status, retryAfter) = step;
            var response = new HttpResponseMessage(status);
            if (retryAfter is { } delay)
            {
                response.Headers.RetryAfter = new RetryConditionHeaderValue(delay);
            }
            return Task.FromResult(response);
        }
    }
}
