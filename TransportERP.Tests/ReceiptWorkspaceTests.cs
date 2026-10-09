using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TransportERP.Contracts.Accounting;
using TransportERP.Contracts.Core;
using TransportERP.Infrastructure.Persistence;
using System.Text.Json;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TransportERP.Tests;

public sealed partial class ReceiptWorkspaceTests
{
    [Fact]
    public async Task Mixed_currencies_post_in_base_currency_and_rounding_difference_blocks_post()
    {
        await using var db = Database(); var (scope, original, _, _) = Seed(db, "CASH", "DIRECT_BANK");
        var usd = Guid.NewGuid(); var eur = Guid.NewGuid();
        db.Currencies.AddRange(new Currency { Id = usd, Code = "USD", NameAr = "اختبار دولار", MinorUnit = 2 },
            new Currency { Id = eur, Code = "EUR", NameAr = "اختبار يورو", MinorUnit = 2 });
        await db.SaveChangesAsync(); var service = new ReceiptWorkspaceService(db);
        var draft = original with { CurrencyId = usd, ExchangeRate = 3.75m, Lines = [original.Lines[0] with { CurrencyId = eur, Amount = 20m, Rate = 5m }] };
        var saved = await service.SaveAsync(scope, draft); var approved = await service.ApproveAsync(scope, draft.Id, saved.Version);
        await service.PostAsync(scope, draft.Id, approved.Version);
        var journal = await db.JournalEntries.Include(j => j.Lines).SingleAsync();
        Assert.Equal(375m, journal.TotalDebit); Assert.Equal(375m, journal.TotalCredit);
        var credit = journal.Lines.Single(l => l.Credit > 0); Assert.Equal(20m, credit.ForeignAmount); Assert.Equal(eur, credit.CurrencyId);
        var rounding = original with { Id = Guid.NewGuid(), Amount = 0.03m, Lines = [original.Lines[0] with { Amount = 0.015m }, original.Lines[0] with { Amount = 0.015m }] };
        saved = await service.SaveAsync(scope, rounding); approved = await service.ApproveAsync(scope, rounding.Id, saved.Version);
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.PostAsync(scope, rounding.Id, approved.Version));
        Assert.Equal(1, await db.JournalEntries.CountAsync());
    }
    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Http_permissions_are_current_and_scoped()
    {
        await using var db = Database();
        if (!db.Database.IsRelational()) throw new InvalidOperationException("HTTP receipt verification requires the isolated PostgreSQL cluster.");
        var (scope, draft, _, _) = Seed(db, "CHEQUE", "DIRECT_BANK"); await db.SaveChangesAsync();
        const string key = "receipt-isolated-test-signing-key-not-for-production-2026";
        using var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:TransportErp", db.Database.GetConnectionString());
            builder.UseSetting("Auth:Issuer", "receipt-test"); builder.UseSetting("Auth:Audience", "receipt-test"); builder.UseSetting("Auth:SigningKey", key);
        });
        using var client = factory.CreateClient();
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/v1/receipts/bootstrap")).StatusCode);
        var claims = new[] { new Claim(JwtRegisteredClaimNames.Sub, scope.UserId.ToString()), new Claim("company_id", scope.CompanyId.ToString()),
            new Claim("branch_id", scope.BranchId.ToString()), new Claim("permission", "ACC043.View") };
        var token = new JwtSecurityToken("receipt-test", "receipt-test", claims, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(10),
            new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", new JwtSecurityTokenHandler().WriteToken(token));
        Assert.Equal(HttpStatusCode.Forbidden, (await client.GetAsync("/api/v1/receipts/bootstrap")).StatusCode);
        foreach (var code in new[] { "ACC043.View", "ACC043.Create", "ACC043.Post", "accounting.receipts.approve" })
        {
            var permission = await db.Permissions.SingleAsync(p => p.Code == code);
            db.UserPermissionOverrides.Add(new() { UserId = scope.UserId, PermissionId = permission.Id, CompanyId = scope.CompanyId, BranchId = scope.BranchId, IsAllowed = true });
        }
        await db.SaveChangesAsync();
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/api/v1/receipts/bootstrap")).StatusCode);
        var bootstrap = (await client.GetFromJsonAsync<ReceiptBootstrap>("/api/v1/receipts/bootstrap"))!;
        var defaultUpdate = new ReceiptConfigurationUpdate(bootstrap.Configuration with { DefaultCurrencyId = draft.CurrencyId }, bootstrap.ConfigurationVersion);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.PutAsJsonAsync("/api/v1/receipts/configuration", defaultUpdate)).StatusCode);
        var configurePermission = await db.Permissions.SingleAsync(p => p.Code == "accounting.receipts.configure");
        db.UserPermissionOverrides.Add(new() { UserId = scope.UserId, PermissionId = configurePermission.Id, CompanyId = scope.CompanyId, BranchId = scope.BranchId, IsAllowed = true });
        await db.SaveChangesAsync();
        Assert.Equal(HttpStatusCode.OK, (await client.PutAsJsonAsync("/api/v1/receipts/configuration", defaultUpdate)).StatusCode);
        Assert.Equal(draft.CurrencyId, (await client.GetFromJsonAsync<ReceiptBootstrap>("/api/v1/receipts/bootstrap"))!.Configuration.DefaultCurrencyId);
        Assert.Equal(HttpStatusCode.Conflict, (await client.PutAsJsonAsync("/api/v1/receipts/configuration", defaultUpdate)).StatusCode);
        var save = await client.PutAsJsonAsync("/api/v1/receipts/", draft);
        Assert.True(save.IsSuccessStatusCode, await save.Content.ReadAsStringAsync());
        var saved = (await save.Content.ReadFromJsonAsync<ReceiptDocument>())!;
        Assert.Equal(draft.Lines[0].WaybillId, saved.Draft.Lines[0].WaybillId);
        var attachment = new ReceiptAttachmentUpload(Guid.NewGuid(), saved.Version, "http-test.txt", Encoding.UTF8.GetBytes("isolated receipt attachment"));
        Assert.Equal(HttpStatusCode.Forbidden, (await client.PostAsJsonAsync($"/api/v1/receipts/{draft.Id}/attachments", attachment)).StatusCode);
        var editPermission = await db.Permissions.SingleAsync(p => p.Code == "ACC043.Edit");
        db.UserPermissionOverrides.Add(new() { UserId = scope.UserId, PermissionId = editPermission.Id, CompanyId = scope.CompanyId, BranchId = scope.BranchId, IsAllowed = true });
        await db.SaveChangesAsync();
        var attachedResponse = await client.PostAsJsonAsync($"/api/v1/receipts/{draft.Id}/attachments", attachment);
        Assert.True(attachedResponse.IsSuccessStatusCode, await attachedResponse.Content.ReadAsStringAsync());
        saved = (await attachedResponse.Content.ReadFromJsonAsync<ReceiptDocument>())!;
        var downloaded = await client.GetFromJsonAsync<ReceiptAttachmentDownload>($"/api/v1/receipts/{draft.Id}/attachments/{attachment.Id}");
        Assert.Equal(attachment.Content, downloaded!.Content);
        var approvedResponse = await client.PostAsJsonAsync($"/api/v1/receipts/{draft.Id}/approve", new { saved.Version });
        Assert.True(approvedResponse.IsSuccessStatusCode, await approvedResponse.Content.ReadAsStringAsync());
        var approved = (await approvedResponse.Content.ReadFromJsonAsync<ReceiptDocument>())!;
        var posted = await client.PostAsJsonAsync($"/api/v1/receipts/{draft.Id}/post", new { approved.Version });
        Assert.True(posted.IsSuccessStatusCode, await posted.Content.ReadAsStringAsync());
        var permissionToRevoke = await db.Permissions.SingleAsync(p => p.Code == "ACC043.View");
        var grant = await db.UserPermissionOverrides.SingleAsync(o => o.UserId == scope.UserId && o.PermissionId == permissionToRevoke.Id);
        grant.IsAllowed = false; await db.SaveChangesAsync();
        Assert.Equal(HttpStatusCode.Forbidden, (await client.GetAsync($"/api/v1/receipts/{draft.Id}")).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.GetAsync($"/api/v1/receipts/{draft.Id}/attachments/{attachment.Id}")).StatusCode);
        Assert.Equal(1, await db.JournalEntries.CountAsync());
    }
    [Fact]
    public async Task Reversal_is_balanced_audited_and_does_not_change_original_lines()
    {
        await using var db = Database(); var (scope, draft, _, _) = Seed(db, "CHEQUE", "DIRECT_BANK");
        await db.SaveChangesAsync(); var service = new ReceiptWorkspaceService(db);
        var saved = await service.SaveAsync(scope, draft);
        var approved = await service.ApproveAsync(scope, draft.Id, saved.Version);
        var posted = await service.PostAsync(scope, draft.Id, approved.Version);
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CancelAsync(scope, draft.Id, posted.Version, "لا يجوز"));
        var reversed = await service.ReverseAsync(scope, draft.Id, posted.Version, "اختبار العكس", draft.Date);
        Assert.Equal("REVERSED", reversed.State);
        await service.ReverseAsync(scope, draft.Id, posted.Version, "اختبار العكس", draft.Date);
        var journals = await db.JournalEntries.Include(j => j.Lines).ToListAsync();
        Assert.Equal(2, journals.Count);
        var original = journals.Single(j => j.Id == posted.JournalId);
        var reverse = journals.Single(j => j.ReversalOfId == posted.JournalId);
        foreach (var line in original.Lines)
        {
            var inverse = reverse.Lines.Single(l => l.LineNo == line.LineNo);
            Assert.Equal(line.AccountId, inverse.AccountId); Assert.Equal(line.Debit, inverse.Credit); Assert.Equal(line.Credit, inverse.Debit);
        }
        Assert.Equal(4, await db.AuditEvents.CountAsync());
        Assert.Equal(4, (await service.GetAsync(scope, draft.Id)).Audit!.Count);
    }

    [Fact]
    public async Task New_configuration_defaults_to_bank_without_guessing_account_or_rounding()
    {
        await using var db = Database();
        var config = await new ReceiptWorkspaceService(db).SettingsAsync(Guid.NewGuid());
        Assert.Equal("DIRECT_BANK", config.ChequeTreatment);
        Assert.Empty(config.Destinations); Assert.Null(config.Rounding); Assert.Null(config.ChequesReceivableAccountId);
    }
    [Theory]
    [InlineData("CASH", "DIRECT_BANK")]
    [InlineData("CHEQUE", "DIRECT_BANK")]
    [InlineData("CHEQUE", "CHEQUES_RECEIVABLE")]
    public async Task Save_reload_approve_post_is_balanced_and_idempotent(string method, string treatment)
    {
        await using var db = Database();
        var (scope, draft, settings, receivable) = Seed(db, method, treatment);
        await db.SaveChangesAsync();
        var service = new ReceiptWorkspaceService(db);
        var saved = await service.SaveAsync(scope, draft);
        var retry = await service.SaveAsync(scope, draft);
        Assert.Equal(saved.Version, retry.Version);
        db.ChangeTracker.Clear();
        var loaded = await service.GetAsync(scope, draft.Id);
        Assert.Equal(draft.Lines[0].ChequeNumber, loaded.Draft.Lines[0].ChequeNumber);
        Assert.Equal(draft.Lines[0].WaybillId, loaded.Draft.Lines[0].WaybillId);
        Assert.Equal("retained", loaded.Draft.Additional["referenceNumber"]);
        Assert.Equal("extra", loaded.Draft.Lines[0].Additional!["referenceNumber"]);
        Assert.Null(loaded.Draft.Lines[0].Additional!["accountingAmount"]);
        Assert.Equal("100", loaded.Draft.Lines[0].Additional!["foreignAmount"]);
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.PostAsync(scope, draft.Id, saved.Version));
        var approved = await service.ApproveAsync(scope, draft.Id, loaded.Version);
        var posted = await service.PostAsync(scope, draft.Id, approved.Version);
        var again = await service.PostAsync(scope, draft.Id, approved.Version);
        Assert.Equal(posted.JournalId, again.JournalId);
        var journal = await db.JournalEntries.Include(j => j.Lines).SingleAsync();
        Assert.Equal(100m, journal.TotalDebit); Assert.Equal(journal.TotalDebit, journal.TotalCredit);
        Assert.Equal(method == "CHEQUE" && treatment == "CHEQUES_RECEIVABLE" ? receivable : settings.Destinations[0].AccountId,
            journal.Lines.Single(l => l.Debit > 0).AccountId);
        Assert.Equal(1, await db.ReceiptVouchers.CountAsync());
        Assert.Equal(2, journal.Lines.Count);
        Assert.Equal(100m, decimal.Parse((await service.GetAsync(scope, draft.Id)).Draft.Lines[0].Additional!["accountingAmount"]!, System.Globalization.CultureInfo.InvariantCulture));
        var config = await db.CompanySettings.SingleAsync();
        config.ValueJson = JsonSerializer.Serialize(settings with { ChequeTreatment = "CHANGED" });
        await db.SaveChangesAsync();
        Assert.Equal(posted.PostingPolicy, (await service.GetAsync(scope, draft.Id)).PostingPolicy);
    }

    [Fact]
    public async Task Missing_policy_allows_draft_and_blocks_post_without_journal()
    {
        await using var db = Database(); var (scope, draft, _, _) = Seed(db, "CHEQUE", null);
        await db.SaveChangesAsync(); var service = new ReceiptWorkspaceService(db);
        var saved = await service.SaveAsync(scope, draft);
        var approved = await service.ApproveAsync(scope, draft.Id, saved.Version);
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.PostAsync(scope, draft.Id, approved.Version));
        Assert.Empty(db.JournalEntries); Assert.Equal("APPROVED", (await service.GetAsync(scope, draft.Id)).State);
    }

    [Fact]
    public async Task Wrong_scope_stale_version_and_missing_waybill_are_rejected()
    {
        await using var db = Database(); var (scope, draft, _, _) = Seed(db, "CASH", "DIRECT_BANK");
        await db.SaveChangesAsync(); var service = new ReceiptWorkspaceService(db);
        await service.SaveAsync(scope, draft);
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => service.SaveAsync(scope, draft with { Description = "changed" }));
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetAsync(scope with { BranchId = Guid.NewGuid() }, draft.Id));
        var bad = draft with { Id = Guid.NewGuid(), Lines = [draft.Lines[0] with { WaybillId = null }] };
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.SaveAsync(scope, bad));
        bad = bad with { Lines = [draft.Lines[0] with { WaybillId = Guid.NewGuid() }] };
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.SaveAsync(scope, bad));
    }

    private static TransportErpDbContext Database()
    {
        var options = new DbContextOptionsBuilder<TransportErpDbContext>();
        if (Environment.GetEnvironmentVariable("RECEIPT_ISOLATED_POSTGRES") == "54439")
        {
            // Fixed loopback port belongs to the disposable cluster created by ReceiptRuntime/start-test-postgres.ps1.
            // Never read a production connection string or reuse a production database name.
            options.ConfigureTransportErpPostgreSql("Host=127.0.0.1;Port=54439;Username=receipt_test;Pooling=false;Database=receipt_test_" + Guid.NewGuid().ToString("N"));
            var db = new TransportErpDbContext(options.Options); db.Database.Migrate(); return db;
        }
        options.UseInMemoryDatabase("receipt-only-test-" + Guid.NewGuid()).ReplaceService<IModelCustomizer, TransportErpP2CombinedModelCustomizer>();
        return new(options.Options);
    }

    private static (OperationContext, ReceiptDraft, ReceiptConfiguration, Guid) Seed(TransportErpDbContext db, string method, string? treatment)
    {
        var company = Guid.NewGuid(); var branch = Guid.NewGuid(); var user = Guid.NewGuid(); var currency = Guid.NewGuid();
        var destination = Guid.NewGuid(); var type = Guid.NewGuid(); var account = Guid.NewGuid(); var counter = Guid.NewGuid();
        var receivable = Guid.NewGuid(); var sequence = Guid.NewGuid(); var waybill = Guid.NewGuid();
        db.Currencies.Add(new() { Id = currency, Code = "SAR", NameAr = "اختبار", MinorUnit = 2 });
        db.Companies.Add(new() { Id = company, Code = "TEST", LegalNameAr = "شركة اختبار", BaseCurrencyId = currency, DefaultCalendarId = Guid.NewGuid() });
        db.Branches.Add(new() { Id = branch, CompanyId = company, Code = "TEST", NameAr = "فرع اختبار" });
        db.Users.Add(new() { Id = user, CompanyId = company, BranchId = branch, UserName = "receipt-test", NormalizedUserName = "RECEIPT-TEST", DisplayName = "اختبار" });
        foreach (var id in new[] { account, counter, receivable })
            db.ChartOfAccounts.Add(new() { Id = id, Code = id.ToString("N"), NameAr = "حساب اختبار", AccountType = "ASSET", CompanyId = company, PostingAllowed = true, Status = "ACTIVE" });
        db.FiscalPeriods.Add(new() { Id = Guid.NewGuid(), CompanyId = company, StartDate = DateTime.UtcNow.Date.AddDays(-1), EndDate = DateTime.UtcNow.Date.AddDays(1) });
        db.Set<WaybillEntity>().Add(new() { Id = waybill, CompanyId = company, BranchId = branch, CurrencyId = currency, ExchangeRate = 1m, DraftNo = "TEST-WB" });
        db.Set<NumberSequenceEntity>().Add(new() { Id = sequence, CompanyId = company, BranchId = branch, DocumentType = "RECEIPT_VOUCHER", Prefix = "TEST-", NextValue = 1, Status = "ACTIVE" });
        var settings = new ReceiptConfiguration(treatment, receivable, "TO_EVEN", sequence,
            [new(destination, "اختبار", method == "CASH" ? "CASH" : "BANK", account)], [new(type, "قبض بوليصة", true)], [user], []);
        db.CompanySettings.Add(new() { Id = Guid.NewGuid(), CompanyId = company, Key = ReceiptWorkspaceService.SettingsKey, ValueType = "JSON", Version = 1, ValueJson = JsonSerializer.Serialize(settings) });
        var draft = new ReceiptDraft(Guid.NewGuid(), null, DateTime.UtcNow.Date, method, currency, 100m, 1m,
            destination, type, user, "اختبار معزول", [new(counter, currency, 100m, null, "سطر", "TEST-CHQ", DateTime.UtcNow.Date,
                waybill, new() { ["referenceNumber"] = "extra", ["accountingAmount"] = "999", ["foreignAmount"] = "999" })], new() { ["referenceNumber"] = "retained" });
        return (new OperationContext(user, company, branch, Guid.NewGuid()), draft, settings, receivable);
    }
}
