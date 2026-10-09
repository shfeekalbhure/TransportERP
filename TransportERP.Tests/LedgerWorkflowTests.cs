using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Accounting;
using TransportERP.Infrastructure.Persistence;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TransportERP.Tests;

public sealed class LedgerWorkflowTests
{
    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Http_policy_and_review_permissions_are_current_and_automatic_post_cannot_bypass_them()
    {
        var f = ReceiptWorkspaceTests.ReceiptTabFixture("CHEQUE"); await using var db = f.Db;
        if (!db.Database.IsRelational()) throw new InvalidOperationException("HTTP policy verification requires isolated PostgreSQL.");
        const string key = "general-ledger-isolated-test-signing-key-not-for-production-2026";
        using var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:TransportErp", db.Database.GetConnectionString());
            builder.UseSetting("Auth:Issuer", "ledger-test"); builder.UseSetting("Auth:Audience", "ledger-test"); builder.UseSetting("Auth:SigningKey", key);
        });
        using var client = factory.CreateClient();
        const string settingsUrl = "/api/v1/accounting/settings";
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync(settingsUrl)).StatusCode);
        var claims = new[] { new Claim(JwtRegisteredClaimNames.Sub, f.Scope.UserId.ToString()),
            new Claim("company_id", f.Scope.CompanyId.ToString()), new Claim("branch_id", f.Scope.BranchId.ToString()),
            new Claim("permission", GeneralLedgerPolicyService.ConfigurePermission), new Claim("permission", "accounting.receipts.review") };
        var token = new JwtSecurityToken("ledger-test", "ledger-test", claims, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(10),
            new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", new JwtSecurityTokenHandler().WriteToken(token));
        var policy = new LedgerSettingsUpdate(new(true, false, "AUTOMATIC"), null);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.PutAsJsonAsync(settingsUrl, policy)).StatusCode);
        async Task Permission(string code, bool allow)
        {
            var permission = await db.Permissions.SingleAsync(p => p.Code == code);
            var grant = await db.UserPermissionOverrides.SingleOrDefaultAsync(o => o.UserId == f.Scope.UserId && o.PermissionId == permission.Id);
            if (grant == null) db.UserPermissionOverrides.Add(new() { UserId = f.Scope.UserId, PermissionId = permission.Id,
                CompanyId = f.Scope.CompanyId, BranchId = code == GeneralLedgerPolicyService.ConfigurePermission ? null : f.Scope.BranchId, IsAllowed = allow });
            else grant.IsAllowed = allow;
            await db.SaveChangesAsync();
        }
        foreach (var code in new[] { "accounting.general-ledger.view", GeneralLedgerPolicyService.ConfigurePermission,
            "ACC043.View", "ACC043.Create", "accounting.receipts.review" }) await Permission(code, true);
        var user = await db.Users.SingleAsync(u => u.Id == f.Scope.UserId);
        user.BranchId = f.Scope.BranchId; await db.SaveChangesAsync();
        Assert.Equal(HttpStatusCode.Forbidden, (await client.PutAsJsonAsync(settingsUrl, policy)).StatusCode);
        user.BranchId = null; await db.SaveChangesAsync();
        var configPermissionId = (await db.Permissions.SingleAsync(p => p.Code == GeneralLedgerPolicyService.ConfigurePermission)).Id;
        var configGrant = await db.UserPermissionOverrides.SingleAsync(o => o.UserId == f.Scope.UserId && o.PermissionId == configPermissionId);
        configGrant.BranchId = f.Scope.BranchId; await db.SaveChangesAsync();
        Assert.Equal(HttpStatusCode.Forbidden, (await client.PutAsJsonAsync(settingsUrl, policy)).StatusCode);
        configGrant.BranchId = null; await db.SaveChangesAsync();
        var configured = await client.PutAsJsonAsync(settingsUrl, policy);
        Assert.True(configured.IsSuccessStatusCode, await configured.Content.ReadAsStringAsync());
        var current = (await client.GetFromJsonAsync<LedgerSettingsDocument>(settingsUrl))!;
        Assert.Equal(policy.Policy, current.Policy);
        Assert.Equal(HttpStatusCode.Conflict, (await client.PutAsJsonAsync(settingsUrl, policy)).StatusCode);
        await Permission(GeneralLedgerPolicyService.ConfigurePermission, false);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.PutAsJsonAsync(settingsUrl, policy with { ExpectedVersion = current.Version })).StatusCode);
        var response = await client.PutAsJsonAsync("/api/v1/receipts/", f.Draft);
        Assert.True(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        var saved = (await response.Content.ReadFromJsonAsync<ReceiptDocument>())!;
        Assert.Equal("DRAFT", saved.State);
        await Permission("accounting.receipts.review", false);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.PostAsJsonAsync($"/api/v1/receipts/{f.Draft.Id}/review", new { saved.Version })).StatusCode);
        await Permission("accounting.receipts.review", true);
        var reviewedResponse = await client.PostAsJsonAsync($"/api/v1/receipts/{f.Draft.Id}/review", new { saved.Version });
        Assert.True(reviewedResponse.IsSuccessStatusCode, await reviewedResponse.Content.ReadAsStringAsync());
        var reviewed = (await reviewedResponse.Content.ReadFromJsonAsync<ReceiptDocument>())!;
        Assert.Equal("REVIEWED", reviewed.State); Assert.False(string.IsNullOrWhiteSpace(reviewed.WorkflowMessage));
        Assert.Empty(await db.JournalEntries.ToListAsync());
        await Permission("ACC043.Post", true);
        var posted = await client.PostAsJsonAsync($"/api/v1/receipts/{f.Draft.Id}/post", new { reviewed.Version });
        Assert.True(posted.IsSuccessStatusCode, await posted.Content.ReadAsStringAsync());
        Assert.Equal("POSTED", (await posted.Content.ReadFromJsonAsync<ReceiptDocument>())!.State);
        await Permission("ACC043.Post", false);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.PostAsJsonAsync($"/api/v1/receipts/{f.Draft.Id}/post", new { reviewed.Version })).StatusCode);
        Assert.Single(await db.JournalEntries.ToListAsync());
    }

    [Fact]
    public async Task All_policy_combinations_enforce_stages_and_post_once()
    {
        foreach (var review in new[] { false, true })
        foreach (var approval in new[] { false, true })
        foreach (var automatic in new[] { false, true })
        {
            var f = ReceiptWorkspaceTests.ReceiptTabFixture(approval ? "CHEQUE" : "CASH");
            await using var db = f.Db;
            var policy = new LedgerWorkflowPolicy(review, approval, automatic ? "AUTOMATIC" : "MANUAL");
            var settings = new GeneralLedgerPolicyService(db);
            await settings.SaveSettingsAsync(f.Scope, new(policy, null));
            var service = new ReceiptWorkspaceService(db);
            var saved = await service.SaveAsync(f.Scope, f.Draft);
            Assert.Equal("DRAFT", saved.State);
            Assert.Equal(policy, saved.Workflow!.Policy);
            var current = await service.CompleteWorkflowAsync(f.Scope, saved, () => Task.FromResult(true));
            if (review || approval)
            {
                Assert.Equal("DRAFT", current.State);
                await Assert.ThrowsAsync<InvalidOperationException>(() => service.PostAsync(f.Scope, current.Draft.Id, current.Version));
            }
            if (review)
            {
                if (approval)
                    await Assert.ThrowsAsync<InvalidOperationException>(() => service.ApproveAsync(f.Scope, current.Draft.Id, current.Version));
                current = await service.ReviewAsync(f.Scope, current.Draft.Id, current.Version);
                Assert.Equal("REVIEWED", current.State);
                current = await service.CompleteWorkflowAsync(f.Scope, current, () => Task.FromResult(true));
                if (approval)
                {
                    Assert.Equal("REVIEWED", current.State);
                    await Assert.ThrowsAsync<InvalidOperationException>(() => service.PostAsync(f.Scope, current.Draft.Id, current.Version));
                }
            }
            if (approval)
            {
                current = await service.ApproveAsync(f.Scope, current.Draft.Id, current.Version);
                Assert.Equal("APPROVED", current.State);
                current = await service.CompleteWorkflowAsync(f.Scope, current, () => Task.FromResult(true));
            }
            if (automatic) Assert.Equal("POSTED", current.State);
            else
            {
                Assert.Equal(policy.ReadyState, current.State);
                Assert.Empty(await db.JournalEntries.ToListAsync());
                current = await service.PostAsync(f.Scope, current.Draft.Id, current.Version);
            }
            var posted = await service.PostAsync(f.Scope, current.Draft.Id, current.Version);
            var retry = await service.CompleteWorkflowAsync(f.Scope, posted, () => Task.FromResult(true));
            Assert.Equal(posted.JournalId, retry.JournalId);
            Assert.Single(await db.JournalEntries.ToListAsync());
            var lines = await db.JournalEntryLines.ToListAsync();
            Assert.Equal(100m, lines.Sum(l => l.Debit));
            Assert.Equal(100m, lines.Sum(l => l.Credit));
            var loaded = await service.GetAsync(f.Scope, current.Draft.Id);
            Assert.Equal(policy, loaded.Workflow!.Policy);
            Assert.Single(loaded.Audit!.Where(a => a.Action == "PostReceipt"));
            if (review) Assert.Single(loaded.Audit!.Where(a => a.Action == "ReviewReceipt"));
            if (approval) Assert.Single(loaded.Audit!.Where(a => a.Action == "ApproveReceipt"));
        }
    }

    [Fact]
    public async Task Automatic_post_rechecks_permission_and_preserves_saved_stage_on_failure()
    {
        var f = ReceiptWorkspaceTests.ReceiptTabFixture("CASH"); await using var db = f.Db;
        await new GeneralLedgerPolicyService(db).SaveSettingsAsync(f.Scope, new(new(false, false, "AUTOMATIC"), null));
        var service = new ReceiptWorkspaceService(db);
        var saved = await service.SaveAsync(f.Scope, f.Draft);
        var checks = 0;
        var denied = await service.CompleteWorkflowAsync(f.Scope, saved, () => { checks++; return Task.FromResult(false); });
        Assert.Equal(1, checks); Assert.Equal("DRAFT", denied.State);
        Assert.False(string.IsNullOrWhiteSpace(denied.WorkflowMessage));
        Assert.Empty(await db.JournalEntries.ToListAsync());
        var period = await db.FiscalPeriods.SingleAsync(); period.Status = "CLOSED"; await db.SaveChangesAsync();
        foreach (var queryFailure in new Exception[] { new InvalidOperationException("Permission query failed"),
            new Npgsql.PostgresException("Serialization conflict", "ERROR", "ERROR", "40001") })
        {
            var recovered = await service.CompleteWorkflowAsync(f.Scope, saved, () => Task.FromException<bool>(queryFailure));
            Assert.Equal("DRAFT", recovered.State); Assert.Equal(saved.Version, recovered.Version);
            Assert.False(string.IsNullOrWhiteSpace(recovered.WorkflowMessage));
        }
        var failed = await service.CompleteWorkflowAsync(f.Scope, denied, () => { checks++; return Task.FromResult(true); });
        Assert.Equal(2, checks); Assert.Equal("DRAFT", failed.State);
        Assert.False(string.IsNullOrWhiteSpace(failed.WorkflowMessage));
        db.ChangeTracker.Clear();
        Assert.Empty(await db.JournalEntries.ToListAsync());
        Assert.Equal(saved.Version, (await service.GetAsync(f.Scope, saved.Draft.Id)).Version);
        period = await db.FiscalPeriods.SingleAsync(); period.Status = "OPEN"; await db.SaveChangesAsync();
        var posted = await service.CompleteWorkflowAsync(f.Scope, await service.GetAsync(f.Scope, saved.Draft.Id), () => Task.FromResult(true));
        Assert.Equal("POSTED", posted.State);
        await service.CompleteWorkflowAsync(f.Scope, posted, () => Task.FromResult(true));
        Assert.Single(await db.JournalEntries.ToListAsync());
    }

    [Fact]
    public async Task Policy_snapshot_survives_configuration_changes_and_legacy_keeps_approval()
    {
        var f = ReceiptWorkspaceTests.ReceiptTabFixture("CHEQUE"); await using var db = f.Db;
        var service = new ReceiptWorkspaceService(db); var settings = new GeneralLedgerPolicyService(db);
        var legacy = await service.SaveAsync(f.Scope, f.Draft);
        Assert.True(legacy.Workflow!.IsLegacy);
        Assert.Equal(LedgerWorkflowPolicy.Legacy, legacy.Workflow.Policy);
        // Model a receipt saved before workflow snapshots existed.
        db.Remove(await db.Set<AccountingWorkflowSnapshot>().SingleAsync(s => s.DocumentId == legacy.Draft.Id));
        await db.SaveChangesAsync();
        await settings.SaveSettingsAsync(f.Scope, new(new(true, true, "MANUAL"), null));
        var second = await service.SaveAsync(f.Scope, f.Draft with { Id = Guid.NewGuid(), ExpectedVersion = null });
        var before = await settings.GetSettingsAsync(f.Scope, true);
        await settings.SaveSettingsAsync(f.Scope, new(new(false, false, "AUTOMATIC"), before.Version));
        db.ChangeTracker.Clear();
        var loaded = await service.GetAsync(f.Scope, second.Draft.Id);
        Assert.Equal(new LedgerWorkflowPolicy(true, true, "MANUAL"), loaded.Workflow!.Policy);
        Assert.Equal(second.Workflow!.Version, loaded.Workflow.Version);
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.ApproveAsync(f.Scope, loaded.Draft.Id, loaded.Version));
        var legacyLoaded = await service.GetAsync(f.Scope, legacy.Draft.Id);
        Assert.Equal(LedgerWorkflowPolicy.Legacy, legacyLoaded.Workflow!.Policy);
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.PostAsync(f.Scope, legacy.Draft.Id, legacyLoaded.Version));
        var approved = await service.ApproveAsync(f.Scope, legacy.Draft.Id, legacyLoaded.Version);
        var result = await service.CompleteWorkflowAsync(f.Scope, approved, () => Task.FromResult(true));
        Assert.Equal("APPROVED", result.State);
        var newDraft = await service.SaveAsync(f.Scope, f.Draft with { Id = Guid.NewGuid(), ExpectedVersion = null });
        Assert.Equal(new LedgerWorkflowPolicy(false, false, "AUTOMATIC"), newDraft.Workflow!.Policy);
    }

    [Fact]
    public async Task Settings_roundtrip_conflict_audit_and_scope_validation()
    {
        var f = ReceiptWorkspaceTests.ReceiptTabFixture("CASH"); await using var db = f.Db;
        var service = new GeneralLedgerPolicyService(db);
        var empty = await service.GetSettingsAsync(f.Scope, true);
        Assert.Null(empty.Policy); Assert.Null(empty.Version);
        var policy = new LedgerWorkflowPolicy(true, true, "AUTOMATIC");
        await service.SaveSettingsAsync(f.Scope, new(policy, empty.Version));
        db.ChangeTracker.Clear();
        var first = await service.GetSettingsAsync(f.Scope, true);
        Assert.Equal(policy, first.Policy); Assert.NotNull(first.Version);
        Assert.NotEmpty(first.Audit);
        Assert.Contains("RECEIPT_VOUCHER", first.SupportedDocumentTypes);
        await service.SaveSettingsAsync(f.Scope, new(policy with { PostingMode = "MANUAL" }, first.Version));
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => service.SaveSettingsAsync(f.Scope, new(policy, first.Version)));
        var current = await service.GetSettingsAsync(f.Scope, false);
        Assert.False(current.CanConfigure); Assert.Equal("MANUAL", current.Policy!.PostingMode);
        Assert.Equal(2, current.Audit.Count);
        var actor = await db.Users.Where(u => u.Id == f.Scope.UserId).Select(u => u.DisplayName).SingleAsync();
        Assert.All(current.Audit, item => { Assert.Equal("ConfigureGeneralLedger", item.Action); Assert.Equal(actor, item.Actor); Assert.NotEqual(default, item.At); });
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.SaveSettingsAsync(f.Scope, new(policy with { PostingMode = "UNKNOWN" }, current.Version)));
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.GetSettingsAsync(f.Scope with { BranchId = Guid.NewGuid() }, true));
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.GetSettingsAsync(f.Scope with { CompanyId = Guid.NewGuid() }, true));
        Assert.Single(await db.CompanySettings.Where(s => s.Key == GeneralLedgerPolicyService.SettingsKey).ToListAsync());
    }

    [Fact]
    public async Task Legacy_voucher_service_cannot_bypass_managed_receipt_workflow()
    {
        var f = ReceiptWorkspaceTests.ReceiptTabFixture("CASH"); await using var db = f.Db;
        var service = new ReceiptWorkspaceService(db);
        var legacy = new VoucherLifecycleService(db);
        var saved = await service.SaveAsync(f.Scope, f.Draft);
        await Assert.ThrowsAsync<InvalidOperationException>(() => legacy.ApproveReceiptAsync(f.Scope.CompanyId, saved.Draft.Id, f.Scope.UserId));
        await Assert.ThrowsAsync<InvalidOperationException>(() => legacy.CancelReceiptAsync(f.Scope.CompanyId, saved.Draft.Id, "bypass", f.Scope.UserId));
        Assert.Equal("DRAFT", (await service.GetAsync(f.Scope, saved.Draft.Id)).State);
        var approved = await service.ApproveAsync(f.Scope, saved.Draft.Id, saved.Version);
        await Assert.ThrowsAsync<InvalidOperationException>(() => legacy.PostReceiptAsync(f.Scope.CompanyId, saved.Draft.Id, f.Scope.UserId));
        var current = await service.GetAsync(f.Scope, saved.Draft.Id);
        Assert.Equal("APPROVED", current.State); Assert.Equal(approved.Version, current.Version);
        Assert.Empty(await db.JournalEntries.ToListAsync());
    }
}
