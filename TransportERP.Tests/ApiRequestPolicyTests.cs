using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TransportERP.Api.Authorization;
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
    public async Task LookupAuthorizationHandler_AllowsOnlyAnAuthenticatedPrincipalWithTheServerClaim()
    {
        var requirement = new LookupReadRequirement();
        var handler = new LookupReadAuthorizationHandler();
        var allowedContext = new Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext(
            [requirement], CreatePrincipal("north", "north-1", true), null);
        await handler.HandleAsync(allowedContext);
        Assert.True(allowedContext.HasSucceeded);

        var deniedContext = new Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext(
            [requirement], CreatePrincipal("north", "north-1", false), null);
        await handler.HandleAsync(deniedContext);
        Assert.False(deniedContext.HasSucceeded);
    }

    [Fact]
    public void LookupEndpoint_UsesClaimsNotForgedHeaders_AndCapsActualResponseAt50()
    {
        var denied = CreateController();
        denied.HttpContext.Request.Headers["X-TransportERP-Permission"] = "lookup.read";
        denied.HttpContext.Request.Headers["X-TransportERP-Scope"] = "south";
        Assert.IsType<ForbidResult>(denied.Lookup("Reference").Result);

        var allowed = CreateController("north", "north-1", permitted: true);
        allowed.HttpContext.Request.Headers["X-TransportERP-Permission"] = "lookup.read";
        var result = Assert.IsType<OkObjectResult>(allowed.Lookup("Reference").Result);
        var items = Assert.IsAssignableFrom<IReadOnlyList<LookupItem>>(result.Value);
        Assert.InRange(items.Count, 0, RequestLimitPolicy.MaximumLookupResults);
        Assert.All(items, item =>
        {
            Assert.Equal("north", item.Company);
            Assert.Equal("north-1", item.Branch);
        });
        Assert.IsType<BadRequestObjectResult>(allowed.Lookup(null).Result);
    }

    [Fact]
    public void LookupEndpoint_DoesNotReturnMoreThan50WhenTheProviderHasMoreMatches()
    {
        var controller = CreateController("north", "north-1", permitted: true, new LargeLookupProvider());

        var result = Assert.IsType<OkObjectResult>(controller.Lookup("Reference").Result);
        var items = Assert.IsAssignableFrom<IReadOnlyList<LookupItem>>(result.Value);

        Assert.Equal(RequestLimitPolicy.MaximumLookupResults, items.Count);
        Assert.All(items, item =>
        {
            Assert.Equal("north", item.Company);
            Assert.Equal("north-1", item.Branch);
        });
    }

    [Fact]
    public void LookupEndpoint_RejectsCrossCompanyAndCrossBranchBeforeMaterialisingResults()
    {
        var context = new DefaultHttpContext();
        context.User = CreatePrincipal("north", "north-1", true);
        var controller = new ReferenceDataController(new InMemoryReferenceLookupProvider())
        {
            ControllerContext = new ControllerContext { HttpContext = context }
        };
        Assert.IsType<ForbidResult>(controller.Lookup("Reference", company: "south").Result);
        Assert.IsType<ForbidResult>(controller.Lookup("Reference", branch: "north-2").Result);
    }

    private static ReferenceDataController CreateController(
        string? company = null,
        string? branch = null,
        bool permitted = false,
        IReferenceLookupProvider? lookupProvider = null)
    {
        var context = new DefaultHttpContext { User = CreatePrincipal(company, branch, permitted) };
        return new ReferenceDataController(lookupProvider ?? new InMemoryReferenceLookupProvider())
        {
            ControllerContext = new ControllerContext { HttpContext = context }
        };
    }

    private static ClaimsPrincipal CreatePrincipal(string? company = null, string? branch = null, bool permitted = false)
    {
        var claims = new List<Claim>();
        if (!string.IsNullOrWhiteSpace(company)) claims.Add(new Claim(LookupClaims.Company, company));
        if (!string.IsNullOrWhiteSpace(branch)) claims.Add(new Claim(LookupClaims.Branch, branch));
        if (permitted) claims.Add(new Claim(LookupClaims.Permission, LookupClaims.ReadPermission));
        return new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "test"));
    }

    private sealed class LargeLookupProvider : IReferenceLookupProvider
    {
        public IReadOnlyList<LookupItem> Search(string query, LookupAccessContext access) =>
            Enumerable.Range(1, RequestLimitPolicy.MaximumLookupResults + 10)
                .Select(number => new LookupItem(number.ToString(), $"Reference {number}", access.Company, access.Branch))
                .ToArray();
    }
}
