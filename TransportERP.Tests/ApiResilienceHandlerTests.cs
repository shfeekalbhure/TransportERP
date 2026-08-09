using System.Net;
using TransportERP.Api.Policies;

namespace TransportERP.Tests;

public sealed class ApiResilienceHandlerTests
{
    [Fact]
    public async Task UnsafeRequest_IsNotRetriedWithoutAnIdempotencyKey()
    {
        var inner = new CountingHandler(_ => new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));
        using var client = new HttpClient(new SafeReadRetryHandler { InnerHandler = inner });

        using var response = await client.PostAsync("https://example.test/records", new StringContent("{}"));

        Assert.Equal(1, inner.Count);
        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
    }

    [Fact]
    public async Task IdempotencyKey_AllowsBoundedRetryForAnUnsafeRequest()
    {
        var inner = new CountingHandler(count => new HttpResponseMessage(
            count == 1 ? HttpStatusCode.ServiceUnavailable : HttpStatusCode.OK));
        using var client = new HttpClient(new SafeReadRetryHandler { InnerHandler = inner });
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test/records")
        {
            Content = new StringContent("{}")
        };
        request.Headers.Add("Idempotency-Key", "test-key");

        using var response = await client.SendAsync(request);

        Assert.Equal(2, inner.Count);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private sealed class CountingHandler(Func<int, HttpResponseMessage> responseFactory) : HttpMessageHandler
    {
        public int Count { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Count++;
            return Task.FromResult(responseFactory(Count));
        }
    }
}
