using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using TransportERP.Contracts.Authentication;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Authentication;

public sealed record LoginTicket(Guid UserId, string PasswordHash);

public static partial class DesktopLoginModule
{
    // A pre-scope ticket is opaque and never accepted as an API bearer token.
    public static void AddDesktopLogin(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<PasswordHasher<User>>();
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = 429;
            options.AddFixedWindowLimiter("desktop-login", limiter =>
            {
                limiter.PermitLimit = 20;
                limiter.Window = TimeSpan.FromMinutes(1);
                limiter.QueueLimit = 0;
            });
        });
    }

    public static void MapDesktopLogin(this WebApplication app)
    {
        app.MapPost("/api/v1/auth/login", Login).RequireRateLimiting("desktop-login");
        app.MapPost("/api/v1/auth/select-scope", SelectScope).RequireRateLimiting("desktop-login");
        app.MapGet("/api/v1/auth/receipt-scopes", ReceiptScopes).RequireAuthorization();
        app.MapPost("/api/v1/auth/receipt-scope", SelectReceiptScope).RequireAuthorization();
    }

    private static readonly object TicketLock = new();

    private static bool LocalIssuerConfigured(IConfiguration config) =>
        string.IsNullOrWhiteSpace(config["Auth:Authority"] ?? Environment.GetEnvironmentVariable("TRANSPORTERP_JWT_AUTHORITY"));

    private static async Task<IResult> Login(LoginRequest request, TransportErpDbContext db,
        PasswordHasher<User> hasher, IMemoryCache cache, IConfiguration config, CancellationToken ct)
    {
        if (!LocalIssuerConfigured(config))
            return Results.Problem("External identity provider is configured; local password login is disabled.", statusCode: 503);
        if (string.IsNullOrWhiteSpace(request.UserName) || request.UserName.Length > 100 ||
            string.IsNullOrEmpty(request.Password) || request.Password.Length > 1024)
            return Results.BadRequest();
        var name = request.UserName.Trim().ToUpperInvariant();
        var matches = await db.Users.Where(u => u.NormalizedUserName == name && u.Status == "ACTIVE")
            .Take(2).ToListAsync(ct);
        // Existing schema allows duplicate usernames across companies; do not choose one by password.
        if (matches.Count != 1) return Results.Unauthorized();
        var user = matches[0];
        PasswordVerificationResult result;
        try { result = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password); }
        catch (FormatException) { return Results.Unauthorized(); }
        if (result == PasswordVerificationResult.Failed) return Results.Unauthorized();
        if (result == PasswordVerificationResult.SuccessRehashNeeded)
            user.PasswordHash = hasher.HashPassword(user, request.Password);
        user.LastLoginAt = DateTimeOffset.UtcNow;
        user.UpdatedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(ct);
        var scopes = await AllowedScopes(db, user, ct);
        if (scopes.Count == 0) return Results.Json(new { ErrorCode = "NO_ALLOWED_SCOPE" }, statusCode: 403);
        var ticket = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        cache.Set("login:" + ticket, new LoginTicket(user.Id, user.PasswordHash), TimeSpan.FromMinutes(3));
        return Results.Ok(new LoginResponse(ticket, user.DisplayName, scopes));
    }

    public static async Task<List<ScopeOption>> AllowedScopes(TransportErpDbContext db, User user, CancellationToken ct)
    {
        var rows = await (from ur in db.UserRoles
            join role in db.Roles on ur.RoleId equals role.Id
            where ur.UserId == user.Id && role.Status == "ACTIVE"
            select new { ur.CompanyId, ur.BranchId, RoleCompanyId = role.CompanyId }).ToListAsync(ct);
        var branches = await (from b in db.Branches join c in db.Companies on b.CompanyId equals c.Id
            where b.Status == "ACTIVE" && c.Status == "ACTIVE"
            select new { b.Id, b.CompanyId, b.NameAr, c.LegalNameAr }).ToListAsync(ct);
        var periods = await db.FiscalPeriods.AsNoTracking().Where(p => p.Status == "OPEN").ToListAsync(ct);
        var allowed = new List<ScopeOption>();
        foreach (var b in branches)
        {
            if (user.CompanyId.HasValue && user.CompanyId != b.CompanyId) continue;
            if (user.BranchId.HasValue && user.BranchId != b.Id) continue;
            // Null scopes do not silently grant access to every company.
            if (!rows.Any(r => (r.CompanyId ?? r.RoleCompanyId ?? user.CompanyId) == b.CompanyId &&
                (!r.RoleCompanyId.HasValue || r.RoleCompanyId == b.CompanyId) &&
                (!r.CompanyId.HasValue || r.CompanyId == b.CompanyId) &&
                (!r.BranchId.HasValue || r.BranchId == b.Id))) continue;
            foreach (var period in periods.Where(p => p.CompanyId == b.CompanyId))
                allowed.Add(new(b.CompanyId, b.LegalNameAr, b.Id, b.NameAr, period.Id,
                    $"{period.Code} ({period.StartDate:yyyy-MM-dd} — {period.EndDate:yyyy-MM-dd})"));
        }
        return allowed;
    }

    private static async Task<IResult> SelectScope(SelectScopeRequest request, TransportErpDbContext db,
        IMemoryCache cache, IConfiguration config, CancellationToken ct)
    {
        if (!LocalIssuerConfigured(config)) return Results.StatusCode(503);
        if (string.IsNullOrEmpty(request.LoginTicket) || request.LoginTicket.Length != 64)
            return Results.Unauthorized();
        LoginTicket? ticket;
        lock (TicketLock)
        {
            if (!cache.TryGetValue<LoginTicket>("login:" + request.LoginTicket, out ticket) || ticket is null)
                return Results.Unauthorized();
            cache.Remove("login:" + request.LoginTicket);
        }
        var user = await db.Users.SingleOrDefaultAsync(u => u.Id == ticket.UserId && u.Status == "ACTIVE", ct);
        if (user is null || user.PasswordHash != ticket.PasswordHash) return Results.Unauthorized();
        var scopes = await AllowedScopes(db, user, ct);
        var scope = scopes.SingleOrDefault(s => s.CompanyId == request.CompanyId && s.BranchId == request.BranchId &&
            s.FiscalPeriodId == request.FiscalPeriodId);
        if (scope is null) return Results.StatusCode(403);
        return await IssueSession(user, scope, DateTimeOffset.UtcNow.AddMinutes(15), db, config, ct);
    }

    private static async Task<IResult> IssueSession(User user, ScopeOption scope, DateTimeOffset expiry,
        TransportErpDbContext db, IConfiguration config, CancellationToken ct)
    {
        var permissions = await (from ur in db.UserRoles
            join r in db.Roles on ur.RoleId equals r.Id
            join rp in db.RolePermissions on r.Id equals rp.RoleId
            join p in db.Permissions on rp.PermissionId equals p.Id
            where ur.UserId == user.Id && r.Status == "ACTIVE" && p.Status == "ACTIVE" &&
                (ur.CompanyId ?? r.CompanyId ?? user.CompanyId) == scope.CompanyId &&
                (!ur.CompanyId.HasValue || ur.CompanyId == scope.CompanyId) &&
                (!r.CompanyId.HasValue || r.CompanyId == scope.CompanyId) &&
                (!ur.BranchId.HasValue || ur.BranchId == scope.BranchId) &&
                (!rp.CompanyId.HasValue || rp.CompanyId == scope.CompanyId) &&
                (!rp.BranchId.HasValue || rp.BranchId == scope.BranchId) &&
                rp.ScopeType != "PLATFORM" && p.ScopeType != "PLATFORM"
            select p.Code).ToListAsync(ct);
        var overrides = await (from o in db.UserPermissionOverrides join p in db.Permissions on o.PermissionId equals p.Id
            where o.UserId == user.Id && p.Status == "ACTIVE" && p.ScopeType != "PLATFORM" &&
                (!o.CompanyId.HasValue || o.CompanyId == scope.CompanyId) &&
                (!o.BranchId.HasValue || o.BranchId == scope.BranchId)
            select new { p.Code, o.IsAllowed }).ToListAsync(ct);
        var effective = permissions.ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var o in overrides.Where(o => o.IsAllowed)) effective.Add(o.Code);
        foreach (var o in overrides.Where(o => !o.IsAllowed)) effective.Remove(o.Code);
        var now = DateTimeOffset.UtcNow;
        if (expiry <= now) return Results.Unauthorized();
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("company_id", scope.CompanyId.ToString()), new("branch_id", scope.BranchId.ToString()),
            new("fiscal_period_id", scope.FiscalPeriodId.ToString()), new("scope", "branch")
        };
        claims.AddRange(effective.Select(p => new Claim("permission", p)));
        var key = config["Auth:SigningKey"] ?? Environment.GetEnvironmentVariable("TRANSPORTERP_JWT_SIGNING_KEY");
        var issuer = config["Auth:Issuer"] ?? Environment.GetEnvironmentVariable("TRANSPORTERP_JWT_ISSUER");
        var audience = config["Auth:Audience"] ?? Environment.GetEnvironmentVariable("TRANSPORTERP_JWT_AUDIENCE");
        if (string.IsNullOrWhiteSpace(key) || Encoding.UTF8.GetByteCount(key) < 32 ||
            string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience)) return Results.StatusCode(503);
        var jwt = new JwtSecurityToken(issuer, audience, claims, now.UtcDateTime, expiry.UtcDateTime,
            new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256));
        return Results.Ok(new SessionResponse(new JwtSecurityTokenHandler().WriteToken(jwt), expiry, user.DisplayName, scope));
    }
}
