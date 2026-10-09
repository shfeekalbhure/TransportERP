using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Accounting;
using TransportERP.Contracts.Core;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed partial class ReceiptWorkspaceTests
{
    [Fact]
    public async Task Posting_dimension_is_configurable_and_frozen()
    {
        foreach (var key in new string?[] { null, "costCenter", "project", "activity" })
        {
            var f = ReceiptTabFixture("CASH"); await using var db = f.Db;
            var config = f.Bootstrap.Configuration with { PostingDimension = key };
            var setting = await db.CompanySettings.SingleAsync(); setting.ValueJson = JsonSerializer.Serialize(config); await db.SaveChangesAsync();
            var extra = new Dictionary<string, string?> { ["costCenter"] = f.Bootstrap.Dimensions![0].Id.ToString(),
                ["project"] = f.Bootstrap.Dimensions[1].Id.ToString(), ["activity"] = f.Bootstrap.Dimensions[2].Id.ToString() };
            var service = new ReceiptWorkspaceService(db); var saved = await service.SaveAsync(f.Scope, f.Draft with { Additional = extra });
            var approved = await service.ApproveAsync(f.Scope, saved.Draft.Id, saved.Version);
            var posted = await service.PostAsync(f.Scope, saved.Draft.Id, approved.Version);
            var lines = await db.JournalEntryLines.ToListAsync();
            Assert.All(lines, l => Assert.Equal(key == null ? null : Guid.Parse(extra[key]!), l.FinancialDimensionId));
            using var policy = JsonDocument.Parse(posted.PostingPolicy!);
            Assert.Equal(key, policy.RootElement.GetProperty("PostingDimension").GetString());
            setting.ValueJson = JsonSerializer.Serialize(config with { PostingDimension = "changed" }); await db.SaveChangesAsync();
            Assert.Equal(posted.PostingPolicy, (await service.GetAsync(f.Scope, saved.Draft.Id)).PostingPolicy);
        }
    }
    public static (TransportErpDbContext Db, OperationContext Scope, ReceiptDraft Draft, ReceiptBootstrap Bootstrap) ReceiptTabFixture(string method)
    {
        var db = Database(); var (scope, draft, config, _) = Seed(db, method, "DIRECT_BANK");
        var dimensions = new List<ReceiptDimension>();
        foreach (var code in new[] { "CC", "PROJECT", "ACTIVITY" })
        {
            var id = Guid.NewGuid();
            db.FinancialDimensions.Add(new() { Id = id, CompanyId = scope.CompanyId, DimensionCode = code, NameAr = code,
                ValueCode = "TEST-01", ValueNameAr = "مرجع اختبار " + code, ValidFrom = draft.Date.AddDays(-1) });
            dimensions.Add(new(id, code, "TEST-01 — " + code, draft.Date.AddDays(-1), null));
        }
        config = config with { DefaultCurrencyId = draft.CurrencyId, DefaultCostCenterId = dimensions[0].Id,
            CostCenterDimensionCode = "CC", ProjectDimensionCode = "PROJECT", ActivityDimensionCode = "ACTIVITY" };
        db.CompanySettings.Local.Single().ValueJson = JsonSerializer.Serialize(config);
        db.SaveChanges();
        var bootstrap = new ReceiptBootstrap(config,
            db.ChartOfAccounts.Select(a => new ReceiptChoice(a.Id, a.NameAr)).ToList(), [new(draft.CurrencyId, "SAR")],
            [new(scope.UserId, "محصل اختبار")], [new(draft.Lines[0].WaybillId!.Value, "TEST-WB")], [],
            ["ACC043.View", "ACC043.Create", "ACC043.Edit", "ACC043.Post", "accounting.receipts.approve", "accounting.receipts.configure"],
            "فرع اختبار", null, dimensions, [new("WAYBILL", draft.Lines[0].WaybillId!.Value, "TEST-WB")], draft.CurrencyId);
        return (db, scope, draft, bootstrap);
    }

    [Fact]
    public async Task Receipt_tabs_persist_references_attachments_and_actual_journal_rows()
    {
        var f = ReceiptTabFixture("CHEQUE"); await using var db = f.Db;
        var service = new ReceiptWorkspaceService(db);
        var extra = new Dictionary<string, string?>(f.Draft.Additional) { ["costCenter"] = f.Bootstrap.Dimensions![0].Id.ToString(),
            ["project"] = f.Bootstrap.Dimensions[1].Id.ToString(), ["activity"] = f.Bootstrap.Dimensions[2].Id.ToString(),
            ["linkedDocumentKind"] = "WAYBILL", ["linkedDocumentId"] = f.Draft.Lines[0].WaybillId.ToString() };
        var draft = f.Draft with { Additional = extra };
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.SaveAsync(f.Scope, draft with { TypeId = Guid.Empty }));
        var saved = await service.SaveAsync(f.Scope, draft);
        var upload = new ReceiptAttachmentUpload(Guid.NewGuid(), saved.Version, "receipt-test.txt", Encoding.UTF8.GetBytes("بيانات اختبار محلية"));
        var attached = await service.AddAttachmentAsync(f.Scope, draft.Id, upload);
        var retry = await service.AddAttachmentAsync(f.Scope, draft.Id, upload);
        Assert.Equal(attached.Version, retry.Version); Assert.Single(retry.Attachments!);
        var file = await service.DownloadAttachmentAsync(f.Scope, draft.Id, upload.Id);
        Assert.Equal(upload.Content, file.Content);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DownloadAttachmentAsync(f.Scope with { BranchId = Guid.NewGuid() }, draft.Id, upload.Id));
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddAttachmentAsync(f.Scope, draft.Id, upload with { Id = Guid.NewGuid(), Version = attached.Version, FileName = "bad.exe" }));
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddAttachmentAsync(f.Scope, draft.Id, upload with { Id = Guid.NewGuid(), Version = attached.Version, FileName = "../bad.txt" }));
        db.ChangeTracker.Clear(); var loaded = await service.GetAsync(f.Scope, draft.Id);
        Assert.Equal(extra, loaded.Draft.Additional.Where(p => extra.ContainsKey(p.Key)).ToDictionary(p => p.Key, p => p.Value));
        Assert.Single(loaded.Attachments!); Assert.Empty(loaded.AccountRows!);
        var bad = loaded.Draft with { Id = Guid.NewGuid(), ExpectedVersion = null, Additional = new(extra) { ["project"] = Guid.NewGuid().ToString() } };
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.SaveAsync(f.Scope, bad));
        bad = bad with { Additional = new(extra) { ["linkedDocumentId"] = Guid.NewGuid().ToString() } };
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.SaveAsync(f.Scope, bad));
        var approved = await service.ApproveAsync(f.Scope, draft.Id, loaded.Version);
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddAttachmentAsync(f.Scope, draft.Id, upload with { Id = Guid.NewGuid(), Version = approved.Version }));
        await service.PostAsync(f.Scope, draft.Id, approved.Version);
        loaded = await service.GetAsync(f.Scope, draft.Id);
        Assert.Equal(2, loaded.AccountRows!.Count); Assert.Equal(100m, loaded.AccountRows.Sum(r => r.Debit)); Assert.Equal(100m, loaded.AccountRows.Sum(r => r.Credit));
        Assert.Contains(loaded.Audit!, a => a.Action == "AttachReceipt");
    }
}
