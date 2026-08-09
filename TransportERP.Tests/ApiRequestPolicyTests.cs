using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransportERP.Api.Controllers;
using TransportERP.Api.Policies;
using TransportERP.Api.ReferenceData;

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
        Assert.Equal(RequestLimitPolicy.MaximumPageSize, RequestLimitPolicy.NormalizePageSize(201));
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
        Assert.Equal(TimeSpan.FromSeconds(2), OutgoingRequestResiliencePolicy.GetBackoff(1, 0));
        Assert.Equal(TimeSpan.FromMilliseconds(4250), OutgoingRequestResiliencePolicy.GetBackoff(2, 250));
        Assert.Throws<ArgumentOutOfRangeException>(() => OutgoingRequestResiliencePolicy.GetBackoff(3, 0));
    }

    [Fact]
    public void RetryPolicy_RecognizesOnlyTransientResponses()
    {
        Assert.True(OutgoingRequestResiliencePolicy.IsTransient(HttpStatusCode.RequestTimeout));
        Assert.True(OutgoingRequestResiliencePolicy.IsTransient(HttpStatusCode.ServiceUnavailable));
        Assert.False(OutgoingRequestResiliencePolicy.IsTransient(HttpStatusCode.BadRequest));
    }

    [Fact]
    public void RetryPolicy_ExposesTheApprovedTimeoutAndUnsafeMethodContract()
    {
        Assert.Equal(TimeSpan.FromSeconds(30), OutgoingRequestResiliencePolicy.TotalRequestTimeout);
        Assert.Equal(TimeSpan.FromSeconds(10), OutgoingRequestResiliencePolicy.AttemptTimeout);
        Assert.Equal(3, OutgoingRequestResiliencePolicy.MaximumAttempts);
        Assert.False(OutgoingRequestResiliencePolicy.IsAutomaticRetryAllowed(HttpMethod.Post));
        Assert.True(OutgoingRequestResiliencePolicy.IsAutomaticRetryAllowed(HttpMethod.Post, hasIdempotencyKey: true));
    }

    [Fact]
    public void RetryPolicy_UsesRetryAfterWhenDownstreamProvidesIt()
    {
        using var response = new HttpResponseMessage((HttpStatusCode)429);
        response.Headers.RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(TimeSpan.FromSeconds(7));
        Assert.Equal(TimeSpan.FromSeconds(7), SafeReadRetryHandler.GetRetryAfter(response));
    }

    [Fact]
    public void RecordsEndpoint_ClampsTheActualApiResponseTo200()
    {
        var controller = CreateController();
        var result = Assert.IsType<OkObjectResult>(controller.GetRecords(999).Result);
        var records = Assert.IsAssignableFrom<IReadOnlyList<int>>(result.Value);
        Assert.Equal(200, records.Count);
    }

    [Fact]
    public void LookupEndpoint_RequiresPermissionScopeAndQuery_AndCapsActualResponseAt50()
    {
        var denied = CreateController();
        Assert.IsType<ForbidResult>(denied.Lookup("Reference").Result);

        var allowed = CreateController("north", "lookup.read");
        var result = Assert.IsType<OkObjectResult>(allowed.Lookup("Reference").Result);
        var items = Assert.IsAssignableFrom<IReadOnlyList<LookupItem>>(result.Value);
        Assert.Equal(50, items.Count);
        Assert.All(items, item => Assert.Equal("north", item.Scope));
        Assert.IsType<BadRequestObjectResult>(allowed.Lookup(null).Result);
    }

    private static ReferenceDataController CreateController(string? scope = null, string? permission = null)
    {
        var context = new DefaultHttpContext();
        if (scope is not null) context.Request.Headers["X-TransportERP-Scope"] = scope;
        if (permission is not null) context.Request.Headers["X-TransportERP-Permission"] = permission;
        return new ReferenceDataController(new InMemoryReferenceLookupProvider())
        {
            ControllerContext = new ControllerContext { HttpContext = context }
        };
    }
}
