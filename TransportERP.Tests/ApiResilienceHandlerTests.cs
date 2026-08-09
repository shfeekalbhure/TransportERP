using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using TransportERP.Api.Clients;
using TransportERP.Api.Policies;
using TransportERP.Api.Services;

namespace TransportERP.Tests;

public sealed class ApiResilienceHandlerTests
{
    [Theory]
    [InlineData(408)]
    [InlineData(429)]
    [InlineData(500)]
    [InlineData(503)]
    public async Task TransientResponses_AreRetriedUpToTheApprovedAttemptLimit(int status)
    {
        var inner = new ScriptedHandler(_ => new HttpResponseMessage((HttpStatusCode)status));
        var delay = new RecordingDelay();
        using var client = CreateClient(inner, delay);

        using var response = await client.GetAsync("https://example.test/read");

        Assert.Equal(OutgoingRequestResiliencePolicy.MaximumAttempts, inner.Count);
        Assert.Equal(OutgoingRequestResiliencePolicy.MaximumRetries, delay.Delays.Count);
    }

    [Theory]
    [InlineData(400)]
    [InlineData(401)]
    [InlineData(403)]
    [InlineData(409)]
    [InlineData(422)]
    public async Task PermanentResponses_AreNotRetried(int status)
    {
        var inner = new ScriptedHandler(_ => new HttpResponseMessage((HttpStatusCode)status));
        using var client = CreateClient(inner, new RecordingDelay());

        using var response = await client.GetAsync("https://example.test/read");

        Assert.Equal(1, inner.Count);
        Assert.Equal((HttpStatusCode)status, response.StatusCode);
    }

    [Fact]
    public async Task RetryAfter_IsHonouredWhenLongerThanExponentialBackoff()
    {
        var inner = new ScriptedHandler(count => new HttpResponseMessage(count == 1 ? (HttpStatusCode)429 : HttpStatusCode.OK)
        {
            Headers = { RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(7)) }
        });
        var delay = new RecordingDelay();
        using var client = CreateClient(inner, delay);

        using var response = await client.GetAsync("https://example.test/read");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Single(delay.Delays);
        Assert.True(delay.Delays[0] >= TimeSpan.FromSeconds(7));
    }

    [Fact]
    public async Task Backoff_IsExponentialWithConfiguredJitterRange()
    {
        var inner = new ScriptedHandler(count => new HttpResponseMessage(count < 3 ? HttpStatusCode.ServiceUnavailable : HttpStatusCode.OK));
        var delay = new RecordingDelay();
        using var client = CreateClient(inner, delay);

        using var response = await client.GetAsync("https://example.test/read");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, delay.Delays.Count);
        Assert.InRange(delay.Delays[0], TimeSpan.FromSeconds(2), TimeSpan.FromMilliseconds(2250));
        Assert.InRange(delay.Delays[1], TimeSpan.FromSeconds(4), TimeSpan.FromMilliseconds(4250));
    }

    [Fact]
    public async Task UnsafeRequest_IsNotRetriedWithoutAnIdempotencyKey()
    {
        var inner = new ScriptedHandler(_ => new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));
        using var client = CreateClient(inner, new RecordingDelay());

        using var response = await client.PostAsync("https://example.test/records", new StringContent("{}"));

        Assert.Equal(1, inner.Count);
        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
    }

    [Fact]
    public async Task IdempotencyKey_AllowsBoundedRetryForAnUnsafeRequest()
    {
        var inner = new ScriptedHandler(count => new HttpResponseMessage(count == 1 ? HttpStatusCode.ServiceUnavailable : HttpStatusCode.OK));
        using var client = CreateClient(inner, new RecordingDelay());
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test/records") { Content = new StringContent("{}") };
        request.Headers.Add("Idempotency-Key", "test-key");

        using var response = await client.SendAsync(request);

        Assert.Equal(2, inner.Count);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AttemptTimeout_CancelsAnUnresponsiveAttemptAndBoundedRetriesOccur()
    {
        var inner = new CancellableHangingHandler();
        var options = new ResilienceExecutionOptions(TimeSpan.FromSeconds(2), TimeSpan.FromMilliseconds(20), 3);
        using var client = CreateClient(inner, new RecordingDelay(), options);

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => client.GetAsync("https://example.test/read"));
        Assert.Equal(3, inner.Count);
        Assert.True(inner.CancellationObserved);
    }

    [Fact]
    public async Task TotalTimeout_CancelsTheOperationWithoutAdditionalRetries()
    {
        var inner = new CancellableHangingHandler();
        var options = new ResilienceExecutionOptions(TimeSpan.FromMilliseconds(25), TimeSpan.FromSeconds(1), 3);
        using var client = CreateClient(inner, new RecordingDelay(), options);
        var stopwatch = Stopwatch.StartNew();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => client.GetAsync("https://example.test/read"));
        Assert.True(stopwatch.Elapsed < TimeSpan.FromSeconds(1));
        Assert.Equal(1, inner.Count);
    }

    [Fact]
    public async Task CallerCancellation_StopsWithoutAnAdditionalRetry()
    {
        var inner = new CancellableHangingHandler();
        using var client = CreateClient(inner, new RecordingDelay());
        using var cancellation = new CancellationTokenSource();
        cancellation.CancelAfter(TimeSpan.FromMilliseconds(20));

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => client.GetAsync("https://example.test/read", cancellation.Token));
        Assert.Equal(1, inner.Count);
    }

    [Fact]
    public async Task OperationalConsumer_UsesIApiClientWithServerConfiguredTarget()
    {
        var client = new RecordingApiClient();
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Downstream:StatusUrl"] = "https://downstream.example.test/status"
        }).Build();
        var service = new DownstreamStatusService(client, configuration);

        using var response = await service.ProbeAsync();

        Assert.Equal(HttpMethod.Get, client.Request!.Method);
        Assert.Equal("https://downstream.example.test/status", client.Request.RequestUri!.ToString());
    }

    private static HttpClient CreateClient(HttpMessageHandler inner, IResilienceDelay delay, ResilienceExecutionOptions? options = null) =>
        new(new SafeReadRetryHandler(delay, options) { InnerHandler = inner });

    private sealed class RecordingDelay : IResilienceDelay
    {
        public List<TimeSpan> Delays { get; } = [];
        public Task DelayAsync(TimeSpan delay, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Delays.Add(delay);
            return Task.CompletedTask;
        }
    }

    private sealed class ScriptedHandler(Func<int, HttpResponseMessage> factory) : HttpMessageHandler
    {
        public int Count { get; private set; }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            Task.FromResult(factory(++Count));
    }

    private sealed class CancellableHangingHandler : HttpMessageHandler
    {
        public int Count { get; private set; }
        public bool CancellationObserved { get; private set; }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Count++;
            try { await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken); }
            catch (OperationCanceledException) { CancellationObserved = true; throw; }
            throw new UnreachableException();
        }
    }

    private sealed class RecordingApiClient : IApiClient
    {
        public HttpRequestMessage? Request { get; private set; }
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            Request = request;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }
    }
}
