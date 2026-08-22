using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Wave1;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class Wave1AccountingE2EReconciliationTests
{
    [Fact]
    public async Task ACC049_reconciles_posted_reversal_branch_currency_drilldown_export_print_and_cap()
    {
        await using var db = CreateDb();
        var company = Guid.NewGuid();
        var branch1 = Guid.NewGuid();
        var branch2 = Guid.NewGuid();
        var currency1 = Guid.NewGuid();
        var currency2 = Guid.NewGuid();
        var asset = AddAccount(db, company, "1100", "نقد", "ASSET");
        var revenue = AddAccount(db, company, "4100", "إيراد", "REVENUE");

        var original = AddEntry(db, company, branch1, currency1, new DateTime(2026, 8, 10), "JE-ORIGINAL", "POSTED", null,
            (asset, 100m, 0m), (revenue, 0m, 100m));
        AddEntry(db, company, branch1, currency1, new DateTime(2026, 8, 11), "JE-REVERSAL", "POSTED", original,
            (asset, 0m, 100m), (revenue, 100m, 0m));
        AddEntry(db, company, branch1, currency2, new DateTime(2026, 8, 12), "JE-OTHER-CURRENCY", "POSTED", null,
            (asset, 50m, 0m), (revenue, 0m, 50m));
        AddEntry(db, company, branch2, currency1, new DateTime(2026, 8, 13), "JE-OTHER-BRANCH", "POSTED", null,
            (asset, 70m, 0m), (revenue, 0m, 70m));
        AddEntry(db, company, branch1, currency1, new DateTime(2026, 8, 14), "JE-DRAFT", "DRAFT", null,
            (asset, 999m, 0m), (revenue, 0m, 999m));
        await db.SaveChangesAsync();

        var service = new Wave1BalanceSheetService(db);
        var report = await service.QueryAsync(company, branch1, new BalanceSheetQueryRequest(new DateTime(2026, 8, 31), branch1, currency1));

        Assert.Equal(0m, report.AssetsTotal);
        Assert.Equal(0m, report.CurrentEarnings);
        Assert.Equal(0m, report.EquationDifference);
        Assert.Equal(0m, Assert.Single(report.Assets).Balance);

        var drill = await service.DrillDownAsync(company, branch1,
            new BalanceSheetDrillDownRequest(asset, new DateTime(2026, 8, 31), branch1, currency1, Take: 200));
        Assert.Equal(2, drill.Total);
        Assert.Contains(drill.Items, x => x.DocumentNo == "JE-ORIGINAL" && x.Debit == 100m && x.Credit == 0m);
        Assert.Contains(drill.Items, x => x.DocumentNo == "JE-REVERSAL" && x.Debit == 0m && x.Credit == 100m);
        Assert.DoesNotContain(drill.Items, x => x.DocumentNo is "JE-OTHER-CURRENCY" or "JE-OTHER-BRANCH" or "JE-DRAFT");

        var otherCurrency = await service.QueryAsync(company, branch1, new BalanceSheetQueryRequest(new DateTime(2026, 8, 31), branch1, currency2));
        Assert.Equal(50m, otherCurrency.AssetsTotal);
        Assert.Equal(50m, otherCurrency.CurrentEarnings);
        Assert.Equal(0m, otherCurrency.EquationDifference);

        var export = Wave1BalanceSheetService.Export(report);
        Assert.Equal("balance-sheet.csv", export.FileName);
        Assert.Contains("AccountCode,AccountName,AccountType,Balance", export.Content);
        var print = Wave1BalanceSheetService.Print(report);
        Assert.Equal("text/html; charset=utf-8", print.ContentType);
        Assert.Contains("الميزانية العمومية", print.Content);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DrillDownAsync(company, branch1,
            new BalanceSheetDrillDownRequest(asset, new DateTime(2026, 8, 31), branch1, currency1, Take: 201)));
    }

    [Fact]
    public async Task ACC058_reconciles_posted_reversal_branch_currency_drilldown_export_print_and_cap()
    {
        await using var db = CreateDb();
        var company = Guid.NewGuid();
        var branch1 = Guid.NewGuid();
        var branch2 = Guid.NewGuid();
        var currency1 = Guid.NewGuid();
        var currency2 = Guid.NewGuid();
        var asset = AddAccount(db, company, "1100", "نقد", "ASSET");
        var revenue = AddAccount(db, company, "4100", "إيراد", "REVENUE");

        AddEntry(db, company, branch1, currency1, new DateTime(2026, 7, 20), "JE-OPENING", "POSTED", null,
            (asset, 20m, 0m), (revenue, 0m, 20m));
        var original = AddEntry(db, company, branch1, currency1, new DateTime(2026, 8, 10), "JE-ORIGINAL", "POSTED", null,
            (asset, 100m, 0m), (revenue, 0m, 100m));
        AddEntry(db, company, branch1, currency1, new DateTime(2026, 8, 11), "JE-REVERSAL", "POSTED", original,
            (asset, 0m, 100m), (revenue, 100m, 0m));
        AddEntry(db, company, branch1, currency2, new DateTime(2026, 8, 12), "JE-OTHER-CURRENCY", "POSTED", null,
            (asset, 50m, 0m), (revenue, 0m, 50m));
        AddEntry(db, company, branch2, currency1, new DateTime(2026, 8, 13), "JE-OTHER-BRANCH", "POSTED", null,
            (asset, 70m, 0m), (revenue, 0m, 70m));
        AddEntry(db, company, branch1, currency1, new DateTime(2026, 8, 14), "JE-DRAFT", "DRAFT", null,
            (asset, 999m, 0m), (revenue, 0m, 999m));
        await db.SaveChangesAsync();

        var service = new Wave1DetailedTrialBalanceService(db);
        var report = await service.QueryAsync(company, branch1,
            new DetailedTrialBalanceQueryRequest(new DateTime(2026, 8, 1), new DateTime(2026, 8, 31), branch1, currency1, Take: 200));

        var assetRow = Assert.Single(report.Items.Where(x => x.AccountId == asset));
        Assert.Equal(20m, assetRow.OpeningDebit);
        Assert.Equal(100m, assetRow.PeriodDebit);
        Assert.Equal(100m, assetRow.PeriodCredit);
        Assert.Equal(20m, assetRow.ClosingDebit);
        Assert.Equal(0m, assetRow.ClosingCredit);
        Assert.Equal(report.TotalOpeningDebit, report.TotalOpeningCredit);
        Assert.Equal(report.TotalPeriodDebit, report.TotalPeriodCredit);
        Assert.Equal(report.TotalClosingDebit, report.TotalClosingCredit);

        var drill = await service.DrillDownAsync(company, branch1,
            new DetailedTrialBalanceDrillDownRequest(asset, new DateTime(2026, 8, 1), new DateTime(2026, 8, 31), branch1, currency1, Take: 200));
        Assert.Equal(2, drill.Total);
        Assert.Contains(drill.Items, x => x.DocumentNo == "JE-ORIGINAL" && x.Debit == 100m && x.Credit == 0m);
        Assert.Contains(drill.Items, x => x.DocumentNo == "JE-REVERSAL" && x.Debit == 0m && x.Credit == 100m);
        Assert.DoesNotContain(drill.Items, x => x.DocumentNo is "JE-OTHER-CURRENCY" or "JE-OTHER-BRANCH" or "JE-DRAFT");

        var otherCurrency = await service.QueryAsync(company, branch1,
            new DetailedTrialBalanceQueryRequest(new DateTime(2026, 8, 1), new DateTime(2026, 8, 31), branch1, currency2, Take: 200));
        var otherCurrencyAsset = Assert.Single(otherCurrency.Items.Where(x => x.AccountId == asset));
        Assert.Equal(50m, otherCurrencyAsset.PeriodDebit);
        Assert.Equal(0m, otherCurrencyAsset.PeriodCredit);

        var export = Wave1DetailedTrialBalanceService.Export(report);
        Assert.Equal("detailed-trial-balance.csv", export.FileName);
        Assert.Contains("OpeningDebit,OpeningCredit,PeriodDebit,PeriodCredit", export.Content);
        var print = Wave1DetailedTrialBalanceService.Print(report);
        Assert.Equal("text/html; charset=utf-8", print.ContentType);
        Assert.Contains("ميزان المراجعة التفصيلي", print.Content);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.QueryAsync(company, branch1,
            new DetailedTrialBalanceQueryRequest(new DateTime(2026, 8, 1), new DateTime(2026, 8, 31), branch1, currency1, Take: 201)));
    }

    [Fact]
    public void ACC049_and_ACC058_are_review_candidates_while_unproven_authority_gates_remain_blockers()
    {
        Assert.Equal(Wave1ReadinessState.ImplementedReviewRequired, Wave1ReadinessCatalog.GetRequired("ACC-049").State);
        Assert.Equal(Wave1ReadinessState.ImplementedReviewRequired, Wave1ReadinessCatalog.GetRequired("ACC-058").State);
        Assert.Equal(Wave1ReadinessState.Hold, Wave1ReadinessCatalog.GetRequired("ACC-050").State);
        Assert.True(Wave1ReadinessCatalog.HasMergeBlockers);
    }

    private static TransportErpDbContext CreateDb()
        => new(new DbContextOptionsBuilder<TransportErpDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options);

    private static Guid AddAccount(TransportErpDbContext db, Guid company, string code, string name, string type)
    {
        var id = Guid.NewGuid();
        db.ChartOfAccounts.Add(new ChartOfAccount
        {
            Id = id,
            CompanyId = company,
            Code = code,
            NameAr = name,
            AccountType = type,
            PostingAllowed = true,
            Status = "ACTIVE"
        });
        return id;
    }

    private static Guid AddEntry(
        TransportErpDbContext db,
        Guid company,
        Guid branch,
        Guid currency,
        DateTime date,
        string documentNo,
        string status,
        Guid? reversalOfId,
        params (Guid AccountId, decimal Debit, decimal Credit)[] lines)
    {
        var id = Guid.NewGuid();
        db.JournalEntries.Add(new JournalEntry
        {
            Id = id,
            CompanyId = company,
            BranchId = branch,
            DocumentNo = documentNo,
            FiscalPeriodId = Guid.NewGuid(),
            EntryDate = date,
            Status = status,
            SourceType = reversalOfId.HasValue ? "REVERSAL" : "TEST",
            TotalDebit = lines.Sum(x => x.Debit),
            TotalCredit = lines.Sum(x => x.Credit),
            CurrencyId = currency,
            ExchangeRate = 1m,
            ReversalOfId = reversalOfId
        });

        var lineNo = 1;
        foreach (var line in lines)
        {
            db.JournalEntryLines.Add(new JournalEntryLine
            {
                JournalEntryId = id,
                LineNo = lineNo++,
                AccountId = line.AccountId,
                Description = documentNo,
                Debit = line.Debit,
                Credit = line.Credit,
                ForeignAmount = line.Debit - line.Credit,
                CurrencyId = currency
            });
        }
        return id;
    }
}
