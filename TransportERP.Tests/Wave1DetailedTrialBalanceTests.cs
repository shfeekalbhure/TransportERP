using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Wave1;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class Wave1DetailedTrialBalanceTests
{
    [Fact]
    public async Task Query_calculates_opening_period_and_closing_and_respects_branch_scope()
    {
        await using var db = CreateDb();
        var companyId = Guid.NewGuid();
        var branch1 = Guid.NewGuid();
        var branch2 = Guid.NewGuid();
        var currencyId = Guid.NewGuid();
        var asset = Guid.NewGuid();
        var revenue = Guid.NewGuid();

        db.ChartOfAccounts.AddRange(
            new ChartOfAccount { Id = asset, CompanyId = companyId, Code = "1100", NameAr = "نقد", AccountType = "ASSET", PostingAllowed = true, Status = "ACTIVE" },
            new ChartOfAccount { Id = revenue, CompanyId = companyId, Code = "4100", NameAr = "إيراد", AccountType = "REVENUE", PostingAllowed = true, Status = "ACTIVE" });

        AddEntry(db, companyId, branch1, currencyId, new DateTime(2026, 1, 10), "OPEN-1", "POSTED",
            (asset, 100m, 0m), (revenue, 0m, 100m));
        AddEntry(db, companyId, branch1, currencyId, new DateTime(2026, 2, 10), "PERIOD-1", "POSTED",
            (asset, 50m, 0m), (revenue, 0m, 50m));
        AddEntry(db, companyId, branch2, currencyId, new DateTime(2026, 2, 11), "OTHER-BRANCH", "POSTED",
            (asset, 999m, 0m), (revenue, 0m, 999m));
        AddEntry(db, companyId, branch1, currencyId, new DateTime(2026, 2, 12), "DRAFT-IGNORED", "DRAFT",
            (asset, 777m, 0m), (revenue, 0m, 777m));
        await db.SaveChangesAsync();

        var service = new Wave1DetailedTrialBalanceService(db);
        var result = await service.QueryAsync(companyId, branch1,
            new DetailedTrialBalanceQueryRequest(new DateTime(2026, 2, 1), new DateTime(2026, 2, 28), Take: 50));

        Assert.Equal(2, result.Total);
        var assetRow = Assert.Single(result.Items.Where(x => x.AccountId == asset));
        Assert.Equal(100m, assetRow.OpeningDebit);
        Assert.Equal(0m, assetRow.OpeningCredit);
        Assert.Equal(50m, assetRow.PeriodDebit);
        Assert.Equal(0m, assetRow.PeriodCredit);
        Assert.Equal(150m, assetRow.ClosingDebit);
        Assert.Equal(0m, assetRow.ClosingCredit);

        var revenueRow = Assert.Single(result.Items.Where(x => x.AccountId == revenue));
        Assert.Equal(0m, revenueRow.OpeningDebit);
        Assert.Equal(100m, revenueRow.OpeningCredit);
        Assert.Equal(0m, revenueRow.PeriodDebit);
        Assert.Equal(50m, revenueRow.PeriodCredit);
        Assert.Equal(0m, revenueRow.ClosingDebit);
        Assert.Equal(150m, revenueRow.ClosingCredit);

        Assert.Equal(result.TotalOpeningDebit, result.TotalOpeningCredit);
        Assert.Equal(result.TotalPeriodDebit, result.TotalPeriodCredit);
        Assert.Equal(result.TotalClosingDebit, result.TotalClosingCredit);
    }

    [Fact]
    public async Task Drill_down_returns_only_posted_period_lines_for_requested_account_and_branch()
    {
        await using var db = CreateDb();
        var companyId = Guid.NewGuid();
        var branch1 = Guid.NewGuid();
        var branch2 = Guid.NewGuid();
        var currencyId = Guid.NewGuid();
        var accountId = Guid.NewGuid();

        db.ChartOfAccounts.Add(new ChartOfAccount { Id = accountId, CompanyId = companyId, Code = "1100", NameAr = "نقد", AccountType = "ASSET", PostingAllowed = true, Status = "ACTIVE" });
        AddEntry(db, companyId, branch1, currencyId, new DateTime(2026, 2, 2), "P-1", "POSTED", (accountId, 10m, 0m));
        AddEntry(db, companyId, branch2, currencyId, new DateTime(2026, 2, 3), "P-2", "POSTED", (accountId, 20m, 0m));
        AddEntry(db, companyId, branch1, currencyId, new DateTime(2026, 2, 4), "D-1", "DRAFT", (accountId, 30m, 0m));
        await db.SaveChangesAsync();

        var service = new Wave1DetailedTrialBalanceService(db);
        var result = await service.DrillDownAsync(companyId, branch1,
            new DetailedTrialBalanceDrillDownRequest(accountId, new DateTime(2026, 2, 1), new DateTime(2026, 2, 28)));

        var row = Assert.Single(result.Items);
        Assert.Equal("P-1", row.DocumentNo);
        Assert.Equal(10m, row.Debit);
        Assert.Equal(0m, row.Credit);
    }

    [Fact]
    public async Task Query_rejects_invalid_date_range()
    {
        await using var db = CreateDb();
        var service = new Wave1DetailedTrialBalanceService(db);
        await Assert.ThrowsAsync<ArgumentException>(() => service.QueryAsync(
            Guid.NewGuid(), null,
            new DetailedTrialBalanceQueryRequest(new DateTime(2026, 3, 1), new DateTime(2026, 2, 1))));
    }

    private static TransportErpDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<TransportErpDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;
        return new TransportErpDbContext(options);
    }

    private static void AddEntry(
        TransportErpDbContext db,
        Guid companyId,
        Guid branchId,
        Guid currencyId,
        DateTime date,
        string documentNo,
        string status,
        params (Guid AccountId, decimal Debit, decimal Credit)[] lines)
    {
        var id = Guid.NewGuid();
        db.JournalEntries.Add(new JournalEntry
        {
            Id = id,
            CompanyId = companyId,
            BranchId = branchId,
            DocumentNo = documentNo,
            FiscalPeriodId = Guid.NewGuid(),
            EntryDate = date,
            Status = status,
            SourceType = "TEST",
            TotalDebit = lines.Sum(x => x.Debit),
            TotalCredit = lines.Sum(x => x.Credit),
            CurrencyId = currencyId,
            ExchangeRate = 1m
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
                CurrencyId = currencyId
            });
        }
    }
}
