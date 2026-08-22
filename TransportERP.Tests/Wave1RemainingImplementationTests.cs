using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Wave1;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class Wave1RemainingImplementationTests
{
    [Fact]
    public async Task Language_and_account_classification_masters_are_concurrency_safe_and_audited()
    {
        await using var db = CreateReferenceDb();
        var service = new Wave1ReferenceService(db);
        var ctx = Context();

        var language = await service.CreateLanguageAsync(ctx, new CreateLanguageRequest("ar", "العربية", "Arabic", true));
        Assert.Equal("AR", language.Code);
        var translation = await service.UpsertTranslationAsync(ctx, language.Id, new UpsertTranslationRequest("Common.Save", "حفظ"));
        Assert.Equal("حفظ", translation.Text);

        var classification = await service.CreateClassificationAsync(ctx,
            new CreateAccountClassificationRequest("CASH", "نقدية", "Cash", "asset"));
        Assert.Equal("ASSET", classification.AccountType);

        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => service.UpdateClassificationAsync(
            ctx, classification.Id,
            new UpdateAccountClassificationRequest("CASH", "نقدية", "Cash", "ASSET", classification.Version + 10)));

        Assert.Equal(3, await db.AuditEvents.CountAsync());
        Assert.All(await db.AuditEvents.OrderBy(x => x.OccurredAt).ToListAsync(), x => Assert.False(string.IsNullOrWhiteSpace(x.Hash)));
    }

    [Fact]
    public async Task Aging_separates_receivables_and_payables_and_buckets_outstanding_amounts()
    {
        await using var accounting = CreateAccountingDb();
        await using var reference = CreateReferenceDb();
        var company = Guid.NewGuid(); var branch = Guid.NewGuid(); var party = Guid.NewGuid(); var currency = Guid.NewGuid();
        var asOf = new DateTime(2026, 8, 22);

        reference.OpenItems.AddRange(
            Item(company, branch, party, currency, "RECEIVABLE", "AR-CURRENT", asOf.AddDays(-5), asOf.AddDays(5), 100m, 10m),
            Item(company, branch, party, currency, "RECEIVABLE", "AR-30", asOf.AddDays(-40), asOf.AddDays(-10), 200m, 50m),
            Item(company, branch, party, currency, "RECEIVABLE", "AR-90", asOf.AddDays(-100), asOf.AddDays(-95), 300m, 0m),
            Item(company, branch, party, currency, "PAYABLE", "AP-30", asOf.AddDays(-20), asOf.AddDays(-15), 400m, 100m));
        await reference.SaveChangesAsync();

        var service = new Wave1FinancialReportService(accounting, reference);
        var receivable = await service.QueryCustomerAgingAsync(company, branch, new AgingQueryRequest(asOf, Take: 50));
        var r = Assert.Single(receivable.Items);
        Assert.Equal(90m, r.Current);
        Assert.Equal(150m, r.Days1To30);
        Assert.Equal(300m, r.Over90);
        Assert.Equal(540m, r.TotalOutstanding);

        var payable = await service.QuerySupplierAgingAsync(company, branch, new AgingQueryRequest(asOf, Take: 50));
        Assert.Equal(300m, Assert.Single(payable.Items).TotalOutstanding);
    }

    [Fact]
    public async Task Balance_sheet_uses_only_posted_ledger_and_balances_with_current_earnings()
    {
        await using var accounting = CreateAccountingDb();
        await using var reference = CreateReferenceDb();
        var company = Guid.NewGuid(); var branch = Guid.NewGuid(); var currency = Guid.NewGuid();
        var asset = AddAccount(accounting, company, "1100", "أصل", "ASSET");
        var liability = AddAccount(accounting, company, "2100", "التزام", "LIABILITY");
        var equity = AddAccount(accounting, company, "3100", "حقوق", "EQUITY");
        var revenue = AddAccount(accounting, company, "4100", "إيراد", "REVENUE");
        var expense = AddAccount(accounting, company, "5100", "مصروف", "EXPENSE");

        AddEntry(accounting, company, branch, currency, new DateTime(2026, 8, 1), "POSTED-1", "POSTED",
            (asset,160m,0m),(expense,10m,0m),(liability,0m,40m),(equity,0m,100m),(revenue,0m,30m));
        AddEntry(accounting, company, branch, currency, new DateTime(2026, 8, 2), "DRAFT-X", "DRAFT",
            (asset,999m,0m),(revenue,0m,999m));
        await accounting.SaveChangesAsync();

        var service = new Wave1FinancialReportService(accounting, reference);
        var result = await service.QueryBalanceSheetAsync(company, branch, new BalanceSheetQueryRequest(new DateTime(2026, 8, 22)));
        Assert.Equal(160m, result.AssetsTotal);
        Assert.Equal(40m, result.LiabilitiesTotal);
        Assert.Equal(100m, result.EquityTotal);
        Assert.Equal(20m, result.CurrentEarnings);
        Assert.Equal(0m, result.EquationDifference);
    }

    [Fact]
    public async Task Cash_flow_uses_posted_receipts_and_payments_and_preserves_unclassified_activity()
    {
        await using var accounting = CreateAccountingDb();
        await using var reference = CreateReferenceDb();
        var company = Guid.NewGuid(); var branch = Guid.NewGuid(); var currency = Guid.NewGuid();
        accounting.ReceiptVouchers.Add(new ReceiptVoucher
        {
            Id = Guid.NewGuid(), CompanyId = company, BranchId = branch, VoucherNo = "RV-1", VoucherDate = new DateTime(2026,8,5),
            Amount = 100m, CurrencyId = currency, PaymentMethodCode = "CASH", ReferenceType = "CUSTOMER_COLLECTION", Status = "POSTED"
        });
        accounting.PaymentVouchers.Add(new PaymentVoucher
        {
            Id = Guid.NewGuid(), CompanyId = company, BranchId = branch, VoucherNo = "PV-1", VoucherDate = new DateTime(2026,8,6),
            Amount = 40m, CurrencyId = currency, PaymentMethodCode = "BANK", ReferenceType = "EXPENSE", Status = "POSTED"
        });
        accounting.PaymentVouchers.Add(new PaymentVoucher
        {
            Id = Guid.NewGuid(), CompanyId = company, BranchId = branch, VoucherNo = "PV-DRAFT", VoucherDate = new DateTime(2026,8,6),
            Amount = 999m, CurrencyId = currency, PaymentMethodCode = "BANK", ReferenceType = "EXPENSE", Status = "DRAFT"
        });
        await accounting.SaveChangesAsync();

        var service = new Wave1FinancialReportService(accounting, reference);
        var result = await service.QueryCashFlowAsync(company, branch, new CashFlowQueryRequest(new DateTime(2026,8,1), new DateTime(2026,8,31)));
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(60m, result.OperatingNet);
        Assert.Equal(60m, result.NetCashMovement);
        Assert.Contains("Activity,SourceType", Wave1FinancialReportService.ExportCashFlow(result).Content);
    }

    private static Wave1AccountingOpenItemEntity Item(Guid company, Guid branch, Guid party, Guid currency, string side, string doc, DateTime documentDate, DateTime dueDate, decimal original, decimal settled)
        => new()
        {
            Id = Guid.NewGuid(), CompanyId = company, BranchId = branch, PartyId = party, PartyCode = "P-001", PartyName = "طرف اختبار",
            Side = side, SourceType = "TEST", SourceId = Guid.NewGuid(), DocumentNo = doc, DocumentDate = documentDate, DueDate = dueDate,
            CurrencyId = currency, OriginalAmount = original, SettledAmount = settled, Status = "OPEN", Version = 1,
            CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow
        };

    private static Guid AddAccount(TransportErpDbContext db, Guid company, string code, string name, string type)
    {
        var id = Guid.NewGuid();
        db.ChartOfAccounts.Add(new ChartOfAccount { Id=id, CompanyId=company, Code=code, NameAr=name, AccountType=type, PostingAllowed=true, Status="ACTIVE" });
        return id;
    }

    private static void AddEntry(TransportErpDbContext db, Guid company, Guid branch, Guid currency, DateTime date, string no, string status, params (Guid AccountId, decimal Debit, decimal Credit)[] lines)
    {
        var id = Guid.NewGuid();
        db.JournalEntries.Add(new JournalEntry
        {
            Id=id, CompanyId=company, BranchId=branch, DocumentNo=no, FiscalPeriodId=Guid.NewGuid(), EntryDate=date, Status=status,
            SourceType="TEST", TotalDebit=lines.Sum(x=>x.Debit), TotalCredit=lines.Sum(x=>x.Credit), CurrencyId=currency, ExchangeRate=1m
        });
        var n=1;
        foreach (var line in lines)
            db.JournalEntryLines.Add(new JournalEntryLine { JournalEntryId=id, LineNo=n++, AccountId=line.AccountId, Description=no, Debit=line.Debit, Credit=line.Credit, ForeignAmount=line.Debit-line.Credit, CurrencyId=currency });
    }

    private static OperationContext Context() => new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
    private static TransportErpDbContext CreateAccountingDb() => new(new DbContextOptionsBuilder<TransportErpDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options);
    private static Wave1ReferenceDbContext CreateReferenceDb() => new(new DbContextOptionsBuilder<Wave1ReferenceDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options);
}
