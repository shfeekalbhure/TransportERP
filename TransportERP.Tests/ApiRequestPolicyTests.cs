using System.Net;
using TransportERP.Api.Policies;

namespace TransportERP.Tests;

public sealed class ApiRequestPolicyTests
{
    [Fact]
    public void NormalizePageSize_UsesTheApprovedDefault_WhenOmitted()
    {
        Assert.Equal(RequestLimitPolicy.DefaultPageSize, RequestLimitPolicy.NormalizePageSize(null));
    }

    [Fact]
    public void NormalizePageSize_ClampsValuesAboveTheHardMaximum()
    {
        Assert.Equal(RequestLimitPolicy.MaximumPageSize, RequestLimitPolicy.NormalizePageSize(501));
    }

    [Fact]
    public void NormalizePageSize_RejectsNonPositiveValues()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => RequestLimitPolicy.NormalizePageSize(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => RequestLimitPolicy.NormalizePageSize(-1));
    }

    [Fact]
    public void LimitLookup_EnforcesTheServerSideCap()
    {
        var values = Enumerable.Range(1, RequestLimitPolicy.MaximumLookupResults + 1);

        var limited = RequestLimitPolicy.LimitLookup(values);

        Assert.Equal(RequestLimitPolicy.MaximumLookupResults, limited.Count);
        Assert.Equal(RequestLimitPolicy.MaximumLookupResults, limited[^1]);
    }

    [Fact]
    public void RetryPolicy_AllowsAutomaticRetriesOnlyForSafeReadMethods()
    {
        Assert.True(OutgoingRequestResiliencePolicy.IsAutomaticRetryAllowed(HttpMethod.Get));
        Assert.True(OutgoingRequestResiliencePolicy.IsAutomaticRetryAllowed(HttpMethod.Head));
        Assert.False(OutgoingRequestResiliencePolicy.IsAutomaticRetryAllowed(HttpMethod.Post));
        Assert.False(OutgoingRequestResiliencePolicy.IsAutomaticRetryAllowed(HttpMethod.Delete));
    }

    [Fact]
    public void RetryPolicy_DefinesBoundedExponentialBackoffWithJitter()
    {
        Assert.Equal(TimeSpan.FromMilliseconds(250), OutgoingRequestResiliencePolicy.GetBackoff(1, 0));
        Assert.Equal(TimeSpan.FromMilliseconds(600), OutgoingRequestResiliencePolicy.GetBackoff(2, 100));
        Assert.Throws<ArgumentOutOfRangeException>(() => OutgoingRequestResiliencePolicy.GetBackoff(3, 0));
    }

    [Fact]
    public void RetryPolicy_RecognizesOnlyTransientResponses()
    {
        Assert.True(OutgoingRequestResiliencePolicy.IsTransient(HttpStatusCode.RequestTimeout));
        Assert.True(OutgoingRequestResiliencePolicy.IsTransient(HttpStatusCode.ServiceUnavailable));
        Assert.False(OutgoingRequestResiliencePolicy.IsTransient(HttpStatusCode.BadRequest));
    }
}
