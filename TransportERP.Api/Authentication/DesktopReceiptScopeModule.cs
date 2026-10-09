using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TransportERP.Api.Accounting;
using TransportERP.Contracts.Authentication;
using TransportERP.Contracts.Core;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Authentication;

public static partial class DesktopLoginModule
{
    private static async Task<(User User, Guid CompanyId, Guid PeriodId, DateTimeOffset Expiry)?> ReceiptIdentity(
        HttpContext http, TransportErpDbContext db, CancellationToken ct)
    {
        if (!Guid.TryParse(http.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? http.User.FindFirstValue("sub"), out var userId) ||
            !Guid.TryParse(http.User.FindFirstValue("company_id"), out var companyId) ||
            !Guid.TryParse(http.User.FindFirstValue("fiscal_period_id"), out var periodId) ||
            !long.TryParse(http.User.FindFirstValue(JwtRegisteredClaimNames.Exp), out var seconds)) return null;
        DateTimeOffset expiry;
        try { expiry = DateTimeOffset.FromUnixTimeSeconds(seconds); }
        catch (ArgumentOutOfRangeException) { return null; }
        if (expiry <= DateTimeOffset.UtcNow) return null;
        var user = await db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == userId && u.Status == "ACTIVE" && u.DeletedAt == null, ct);
        return user == null ? null : (user, companyId, periodId, expiry);
    }

    private static async Task<List<ScopeOption>> ReceiptScopeOptions(User user, Guid company, Guid period,
        TransportErpDbContext db, CancellationToken ct)
    {
        var result = new List<ScopeOption>();
        foreach (var scope in (await AllowedScopes(db, user, ct)).Where(s => s.CompanyId == company && s.FiscalPeriodId == period).DistinctBy(s => s.BranchId))
            if (await ReceiptApiModule.Allowed(db, new OperationContext(user.Id, company, scope.BranchId, Guid.NewGuid()), "ACC043.View", ct))
                result.Add(scope);
        return result;
    }

    private static async Task<IResult> ReceiptScopes(HttpContext http, TransportErpDbContext db, IConfiguration config, CancellationToken ct)
    {
        if (!LocalIssuerConfigured(config)) return Results.StatusCode(503);
        var identity = await ReceiptIdentity(http, db, ct);
        if (identity is not { } current) return Results.Unauthorized();
        return Results.Ok(await ReceiptScopeOptions(current.User, current.CompanyId, current.PeriodId, db, ct));
    }

    private static async Task<IResult> SelectReceiptScope(SelectReceiptScopeRequest request, HttpContext http,
        TransportErpDbContext db, IConfiguration config, CancellationToken ct)
    {
        if (!LocalIssuerConfigured(config)) return Results.StatusCode(503);
        var identity = await ReceiptIdentity(http, db, ct);
        if (identity is not { } current) return Results.Unauthorized();
        var scope = (await ReceiptScopeOptions(current.User, current.CompanyId, current.PeriodId, db, ct))
            .SingleOrDefault(s => s.BranchId == request.BranchId);
        if (scope == null) return Results.StatusCode(403);
        return await IssueSession(current.User, scope, current.Expiry, db, config, ct);
    }
}
