using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TransportERP.Contracts.Authentication;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class ReceiptBranchScopeTests
{
    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Receipt_branch_tokens_are_permitted_company_fixed_current_and_expiry_bounded()
    {
        var f = ReceiptWorkspaceTests.ReceiptTabFixture("CASH"); await using var db = f.Db;
        if (!db.Database.IsRelational()) throw new InvalidOperationException("Branch scope HTTP verification requires isolated PostgreSQL.");
        var user = await db.Users.SingleAsync(u => u.Id == f.Scope.UserId); user.BranchId = null; user.CompanyId = null;
        var period = await db.FiscalPeriods.SingleAsync();
        var second = Guid.NewGuid(); var denied = Guid.NewGuid(); var foreign = Guid.NewGuid(); var foreignCompany = Guid.NewGuid();
        db.Companies.Add(new() { Id = foreignCompany, Code = "FOREIGN", LegalNameAr = "شركة أخرى", BaseCurrencyId = f.Draft.CurrencyId, DefaultCalendarId = Guid.NewGuid() });
        db.Branches.AddRange(new Branch { Id = second, CompanyId = f.Scope.CompanyId, Code = "B2", NameAr = "فرع ثان" },
            new Branch { Id = denied, CompanyId = f.Scope.CompanyId, Code = "DENIED", NameAr = "فرع غير ممنوح" },
            new Branch { Id = foreign, CompanyId = foreignCompany, Code = "FOREIGN", NameAr = "فرع شركة أخرى" });
        db.FiscalPeriods.Add(new() { Id = Guid.NewGuid(), CompanyId = foreignCompany, Code = "FOREIGN", StartDate = period.StartDate, EndDate = period.EndDate });
        var view = await db.Permissions.SingleAsync(p => p.Code == "ACC043.View");
        db.UserPermissionOverrides.Add(new() { UserId = user.Id, PermissionId = view.Id, CompanyId = f.Scope.CompanyId, IsAllowed = true });
        await db.SaveChangesAsync();
        const string key = "receipt-branch-isolated-signing-key-not-for-production-2026";
        using var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:TransportErp", db.Database.GetConnectionString());
            builder.UseSetting("Auth:Issuer", "branch-test"); builder.UseSetting("Auth:Audience", "branch-test"); builder.UseSetting("Auth:SigningKey", key);
        });
        using var client = factory.CreateClient();
        const string listUrl = "/api/v1/auth/receipt-scopes"; const string selectUrl = "/api/v1/auth/receipt-scope";
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync(listUrl)).StatusCode);
        var expiry = DateTimeOffset.FromUnixTimeSeconds(DateTimeOffset.UtcNow.AddMinutes(4).ToUnixTimeSeconds());
        string Token(DateTimeOffset until) => new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken("branch-test", "branch-test",
            [new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), new Claim("company_id", f.Scope.CompanyId.ToString()),
                new Claim("branch_id", f.Scope.BranchId.ToString()), new Claim("fiscal_period_id", period.Id.ToString())],
            DateTime.UtcNow.AddMinutes(-2), until.UtcDateTime,
            new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256)));
        var originalToken = Token(expiry);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", originalToken);
        Assert.Empty((await client.GetFromJsonAsync<List<ScopeOption>>(listUrl))!); // overrides alone never grant branch scope
        async Task<Guid> AddRole(Guid company, Guid branch)
        {
            var role = new Role { Id = Guid.NewGuid(), CompanyId = company, Code = branch.ToString("N"), NameAr = "دور اختبار" };
            db.Roles.Add(role);
            db.UserRoles.Add(new() { UserId = user.Id, RoleId = role.Id, CompanyId = company, BranchId = branch });
            db.RolePermissions.Add(new() { RoleId = role.Id, PermissionId = view.Id, CompanyId = company, BranchId = branch, ScopeType = "BRANCH" });
            await db.SaveChangesAsync(); return role.Id;
        }
        await AddRole(f.Scope.CompanyId, f.Scope.BranchId);
        var secondRole = await AddRole(f.Scope.CompanyId, second);
        await AddRole(foreignCompany, foreign);
        var choices = (await client.GetFromJsonAsync<List<ScopeOption>>(listUrl))!;
        Assert.Equal(2, choices.Count);
        Assert.All(choices, s => { Assert.Equal(f.Scope.CompanyId, s.CompanyId); Assert.Equal(period.Id, s.FiscalPeriodId); });
        Assert.DoesNotContain(choices, s => s.BranchId == denied || s.BranchId == foreign);
        foreach (var branch in new[] { denied, foreign, Guid.NewGuid() })
            Assert.Equal(HttpStatusCode.Forbidden, (await client.PostAsJsonAsync(selectUrl, new SelectReceiptScopeRequest(branch))).StatusCode);
        var response = await client.PostAsJsonAsync(selectUrl, new SelectReceiptScopeRequest(second));
        Assert.True(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        var session = (await response.Content.ReadFromJsonAsync<SessionResponse>())!;
        Assert.Equal(second, session.Scope.BranchId); Assert.Equal(f.Scope.CompanyId, session.Scope.CompanyId);
        Assert.Equal(expiry, session.ExpiresAt);
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(session.AccessToken);
        Assert.Equal(second.ToString(), jwt.Claims.Single(c => c.Type == "branch_id").Value);
        Assert.Equal(period.Id.ToString(), jwt.Claims.Single(c => c.Type == "fiscal_period_id").Value);
        Assert.True(jwt.ValidTo <= expiry.UtcDateTime);
        Assert.Equal(originalToken, client.DefaultRequestHeaders.Authorization!.Parameter);
        var grant = await db.UserPermissionOverrides.SingleAsync(o => o.UserId == user.Id && o.PermissionId == view.Id);
        grant.IsAllowed = false; await db.SaveChangesAsync();
        Assert.Empty((await client.GetFromJsonAsync<List<ScopeOption>>(listUrl))!);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.PostAsJsonAsync(selectUrl, new SelectReceiptScopeRequest(second))).StatusCode);
        grant.IsAllowed = true;
        (await db.Roles.SingleAsync(r => r.Id == secondRole)).Status = "INACTIVE"; await db.SaveChangesAsync();
        Assert.Single((await client.GetFromJsonAsync<List<ScopeOption>>(listUrl))!);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.PostAsJsonAsync(selectUrl, new SelectReceiptScopeRequest(second))).StatusCode);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token(DateTimeOffset.UtcNow.AddSeconds(-1)));
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.PostAsJsonAsync(selectUrl, new SelectReceiptScopeRequest(f.Scope.BranchId))).StatusCode);
    }
}
