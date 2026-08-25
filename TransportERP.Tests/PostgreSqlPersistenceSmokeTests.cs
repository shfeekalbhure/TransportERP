using Microsoft.EntityFrameworkCore;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class PostgreSqlPersistenceSmokeTests
{
    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Migration_and_receipt_round_trip_work_on_postgresql()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();

        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();

        var currencyCode = await NextCurrencyCodeAsync(db);
        var currency = new Currency
        {
            Id = Guid.NewGuid(), Code = currencyCode, NameAr = "عملة اختبار", MinorUnit = 2, IsBase = true,
            CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow, RowVersion = Guid.NewGuid().ToByteArray()
        };
        var company = new Company
        {
            Id = Guid.NewGuid(), Code = $"P1T-{Guid.NewGuid():N}"[..12], LegalNameAr = "شركة اختبار P1",
            BaseCurrencyId = currency.Id, DefaultCalendarId = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow, RowVersion = Guid.NewGuid().ToByteArray()
        };
        var branch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = "MAIN", NameAr = "الفرع الرئيسي",
            Timezone = "Asia/Aden", CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var user = new User
        {
            Id = Guid.NewGuid(), UserName = $"p1-{Guid.NewGuid():N}"[..14], NormalizedUserName = "P1TEST",
            DisplayName = "مستخدم اختبار", PasswordHash = "test-only", SecurityStamp = Guid.NewGuid().ToString("N"), AuthVersion = 1,
            CompanyId = company.Id, BranchId = branch.Id,
            CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow, RowVersion = Guid.NewGuid().ToByteArray()
        };
        db.Currencies.Add(currency);
        db.Companies.Add(company);
        db.Branches.Add(branch);
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var service = new VoucherLifecycleService(db);
        var voucher = await service.CreateReceiptAsync(new CreateReceiptVoucherCommand(
            company.Id, branch.Id, $"RV-{Guid.NewGuid():N}"[..15], DateTime.UtcNow, "عميل PostgreSQL",
            "TEST", null, "CASH", 125m, currency.Id, user.Id, null, "smoke", $"SMOKE-{Guid.NewGuid():N}"));

        Assert.Equal("DRAFT", voucher.Status);
        Assert.Equal(125m, await db.ReceiptVouchers.Where(x => x.Id == voucher.Id).Select(x => x.Amount).SingleAsync());
    }

    private static async Task<string> NextCurrencyCodeAsync(TransportErpDbContext db)
    {
        for (var attempt = 0; attempt < 16; attempt++)
        {
            var code = Guid.NewGuid().ToString("N")[..3].ToUpperInvariant();
            if (!await db.Currencies.AnyAsync(x => x.Code == code))
                return code;
        }

        throw new InvalidOperationException("Unable to allocate a unique three-character currency code for the PostgreSQL smoke test.");
    }
}
