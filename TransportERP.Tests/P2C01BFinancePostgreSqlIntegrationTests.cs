using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using TransportERP.Application.Waybills;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Waybills;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class P2C01BFinancePostgreSqlIntegrationTests
{
    private const string Issuer = "TransportERP.P2B.Test.Identity";
    private const string Audience = "TransportERP.P2B.Test.Api";
    private const string SigningKey = "transport-erp-p2b-test-signing-key-2026-minimum-32";

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Payment_plan_collections_reversal_and_financial_status_round_trip_are_audited()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        FinanceScope scope;
        await using (var seedDb = CreateP2Db(connection))
            scope = await SeedApprovedWaybillAsync(seedDb, "B1");

        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());

        PaymentPlanResponse plan;
        await using (var db = CreateP2Db(connection))
        {
            var service = CreateService(db);
            plan = await service.SetPaymentPlanAsync(context, scope.WaybillId, new SetPaymentPlanRequest(
                1,
                [
                    new PaymentPlanLineInput(1, "SENDER", null, "CASH", new MoneyAmount(scope.CurrencyId, 40m), null, "ON_APPROVAL", null),
                    new PaymentPlanLineInput(2, "RECEIVER", null, "BANK", null, 60m, "ON_DELIVERY", null)
                ],
                $"plan-{Guid.NewGuid():N}"));
        }
        Assert.Equal(2, plan.Lines.Count);
        Assert.Equal(2, plan.WaybillVersion);

        CollectionResponse first;
        await using (var db = CreateP2Db(connection))
        {
            first = await CreateService(db).RecordCollectionAsync(context, scope.WaybillId,
                NewCollection(scope, 25m, $"collect-{Guid.NewGuid():N}", scope.ReceiptVoucherId, "RECEIPT_VOUCHER"));
        }
        Assert.Equal("ACCEPTED", first.Status);
        Assert.Equal(scope.ReceiptVoucherId, first.AccountingReferenceId);

        WaybillFinancialStatusResponse partial;
        await using (var db = CreateP2Db(connection))
            partial = await CreateService(db).GetFinancialStatusAsync(context, scope.WaybillId);
        Assert.Equal("PARTIAL", partial.FinancialStatus);
        Assert.Equal(25m, partial.PaidEquivalent.Amount);
        Assert.Equal(75m, partial.RemainingEquivalent.Amount);

        CollectionResponse second;
        await using (var db = CreateP2Db(connection))
        {
            second = await CreateService(db).RecordCollectionAsync(context, scope.WaybillId,
                NewCollection(scope, 75m, $"collect-{Guid.NewGuid():N}"));
        }

        await using (var db = CreateP2Db(connection))
        {
            var paid = await CreateService(db).GetFinancialStatusAsync(context, scope.WaybillId);
            Assert.Equal("PAID", paid.FinancialStatus);
            Assert.Equal(0m, paid.RemainingEquivalent.Amount);
        }

        CollectionResponse reversal;
        await using (var db = CreateP2Db(connection))
        {
            reversal = await CreateService(db).ReverseCollectionAsync(context, second.Id,
                new ReverseCollectionRequest("تصحيح تحصيل اختبار", $"reverse-{Guid.NewGuid():N}"));
        }
        Assert.Equal("REVERSED", reversal.Status);
        Assert.Equal(second.Id, reversal.ReversalOfId);

        await using var verifyDb = CreateP2Db(connection);
        var original = await verifyDb.Set<CollectionTransactionEntity>().AsNoTracking().SingleAsync(x => x.Id == second.Id);
        var finalWaybill = await verifyDb.Set<WaybillEntity>().AsNoTracking().SingleAsync(x => x.Id == scope.WaybillId);
        var planAudit = await verifyDb.AuditEvents.AsNoTracking().SingleAsync(x =>
            x.Action == "WaybillPaymentPlanSet" && x.EntityId == scope.WaybillId);
        var collectionAudit = await verifyDb.AuditEvents.AsNoTracking().SingleAsync(x =>
            x.Action == "WaybillCollectionRecord" && x.EntityId == first.Id);
        var reversalAudit = await verifyDb.AuditEvents.AsNoTracking().SingleAsync(x =>
            x.Action == "WaybillCollectionReverse" && x.EntityId == reversal.Id);

        Assert.Equal("ACCEPTED", original.Status);
        Assert.Equal("PARTIAL", finalWaybill.FinancialStatus);
        Assert.Equal(1, await verifyDb.Set<CollectionTransactionEntity>().CountAsync(x => x.ReversalOfId == second.Id));
        Assert.Equal(1, await verifyDb.Set<FinancialLinkEntity>().CountAsync(x =>
            x.WaybillId == scope.WaybillId && x.DocumentId == scope.ReceiptVoucherId && x.LinkType == "COLLECTION"));
        Assert.Contains("PayerRole", planAudit.AfterJson ?? string.Empty, StringComparison.Ordinal);
        Assert.Contains("PaymentMethodCode", collectionAudit.AfterJson ?? string.Empty, StringComparison.Ordinal);
        Assert.Contains("CollectedById", collectionAudit.AfterJson ?? string.Empty, StringComparison.Ordinal);
        Assert.Contains("ReversalOfId", reversalAudit.AfterJson ?? string.Empty, StringComparison.Ordinal);
        Assert.Equal(context.CorrelationId, collectionAudit.CorrelationId);
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Collection_retry_is_idempotent_and_payload_change_conflicts()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        FinanceScope scope;
        await using (var seedDb = CreateP2Db(connection))
            scope = await SeedApprovedWaybillAsync(seedDb, "B2");

        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());
        var operationId = $"collect-{Guid.NewGuid():N}";
        var request = NewCollection(scope, 30m, operationId);

        CollectionResponse first;
        await using (var db = CreateP2Db(connection))
            first = await CreateService(db).RecordCollectionAsync(context, scope.WaybillId, request);
        CollectionResponse replay;
        await using (var db = CreateP2Db(connection))
            replay = await CreateService(db).RecordCollectionAsync(context, scope.WaybillId, request);

        Assert.Equal(first.Id, replay.Id);

        await using (var db = CreateP2Db(connection))
        {
            var ex = await Assert.ThrowsAsync<WaybillPersistenceException>(() =>
                CreateService(db).RecordCollectionAsync(context, scope.WaybillId,
                    NewCollection(scope, 31m, operationId)));
            Assert.Equal("IDEMPOTENCY_CONFLICT", ex.Code);
        }

        await using var verifyDb = CreateP2Db(connection);
        Assert.Equal(1, await verifyDb.Set<CollectionTransactionEntity>().CountAsync(x =>
            x.CompanyId == scope.CompanyId && x.ClientOperationId == operationId));
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Concurrent_collection_retry_produces_one_accepted_transaction()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        FinanceScope scope;
        await using (var seedDb = CreateP2Db(connection))
            scope = await SeedApprovedWaybillAsync(seedDb, "BCONC");

        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());
        var operationId = $"parallel-{Guid.NewGuid():N}";
        var request = NewCollection(scope, 20m, operationId);

        async Task<CollectionResponse> Execute()
        {
            await using var db = CreateP2Db(connection);
            return await CreateService(db).RecordCollectionAsync(context, scope.WaybillId, request);
        }

        var results = await Task.WhenAll(Task.Run(Execute), Task.Run(Execute));
        Assert.Equal(results[0].Id, results[1].Id);

        await using var verifyDb = CreateP2Db(connection);
        Assert.Equal(1, await verifyDb.Set<CollectionTransactionEntity>().CountAsync(x =>
            x.CompanyId == scope.CompanyId && x.ClientOperationId == operationId));
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Accepted_collection_and_financial_link_are_append_only_and_fake_P1_reference_is_rejected()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        FinanceScope scope;
        await using (var seedDb = CreateP2Db(connection))
            scope = await SeedApprovedWaybillAsync(seedDb, "BIMM");

        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());
        CollectionResponse accepted;
        await using (var db = CreateP2Db(connection))
            accepted = await CreateService(db).RecordCollectionAsync(context, scope.WaybillId,
                NewCollection(scope, 10m, $"immutable-{Guid.NewGuid():N}", scope.ReceiptVoucherId, "RECEIPT_VOUCHER"));

        await using (var db = CreateP2Db(connection))
        {
            var row = await db.Set<CollectionTransactionEntity>().SingleAsync(x => x.Id == accepted.Id);
            row.Amount = 999m;
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => db.SaveChangesAsync());
            Assert.Contains("append-only", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        await using (var db = CreateP2Db(connection))
        {
            var row = await db.Set<CollectionTransactionEntity>().SingleAsync(x => x.Id == accepted.Id);
            db.Remove(row);
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => db.SaveChangesAsync());
            Assert.Contains("append-only", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        await using (var db = CreateP2Db(connection))
        {
            var link = await db.Set<FinancialLinkEntity>().SingleAsync(x => x.DocumentId == scope.ReceiptVoucherId);
            db.Remove(link);
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => db.SaveChangesAsync());
            Assert.Contains("append-only", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        await using (var db = CreateP2Db(connection))
        {
            var ex = await Assert.ThrowsAsync<WaybillPersistenceException>(() =>
                CreateService(db).RecordCollectionAsync(context, scope.WaybillId,
                    NewCollection(scope, 5m, $"fake-ref-{Guid.NewGuid():N}", Guid.NewGuid(), "RECEIPT_VOUCHER")));
            Assert.Equal("ACCOUNTING_REFERENCE_INVALID", ex.Code);
        }
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Client_operation_id_never_replays_collection_across_branches()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        FinanceScope firstScope;
        await using (var seedDb = CreateP2Db(connection))
            firstScope = await SeedApprovedWaybillAsync(seedDb, "BSCOPE");

        FinanceScope secondScope;
        await using (var db = CreateP2Db(connection))
            secondScope = await SeedSecondBranchWaybillAsync(db, firstScope, "BSCOPE2");

        var operationId = $"cross-branch-{Guid.NewGuid():N}";
        var firstContext = new OperationContext(firstScope.UserId, firstScope.CompanyId, firstScope.BranchId, Guid.NewGuid());
        await using (var db = CreateP2Db(connection))
            _ = await CreateService(db).RecordCollectionAsync(firstContext, firstScope.WaybillId,
                NewCollection(firstScope, 15m, operationId));

        var secondContext = new OperationContext(firstScope.UserId, secondScope.CompanyId, secondScope.BranchId, Guid.NewGuid());
        await using (var db = CreateP2Db(connection))
        {
            var ex = await Assert.ThrowsAsync<WaybillPersistenceException>(() =>
                CreateService(db).RecordCollectionAsync(secondContext, secondScope.WaybillId,
                    NewCollection(secondScope, 15m, operationId)));
            Assert.Equal("DUPLICATE_OPERATION", ex.Code);
        }
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Finance_API_enforces_permission_and_company_branch_scope()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        FinanceScope scope;
        await using (var seedDb = CreateP2Db(connection))
        {
            scope = await SeedApprovedWaybillAsync(seedDb, "BHTTP");
            await PersistentRbacTestSeeder.GrantBranchPermissionAsync(
                seedDb, scope.UserId, scope.CompanyId, scope.BranchId,
                WaybillFinancePermissionCodes.PaymentPlan);
        }

        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();
        var request = new SetPaymentPlanRequest(1,
            [new PaymentPlanLineInput(1, "SENDER", null, "CASH", new MoneyAmount(scope.CurrencyId, 100m), null, "ON_APPROVAL", null)],
            $"http-plan-{Guid.NewGuid():N}");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            CreateToken(scope.UserId, scope.CompanyId, scope.BranchId, "waybill.view"));
        var denied = await client.PutAsJsonAsync($"/api/v1/waybills/{scope.WaybillId}/payment-plan", request);
        Assert.Equal(HttpStatusCode.Forbidden, denied.StatusCode);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            CreateToken(scope.UserId, scope.CompanyId, scope.BranchId, WaybillFinancePermissionCodes.PaymentPlan));
        var allowed = await client.PutAsJsonAsync($"/api/v1/waybills/{scope.WaybillId}/payment-plan", request);
        Assert.Equal(HttpStatusCode.OK, allowed.StatusCode);

        var foreignBranchToken = CreateToken(scope.UserId, scope.CompanyId, Guid.NewGuid(), WaybillFinancePermissionCodes.PaymentPlan);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", foreignBranchToken);
        var scoped = await client.PutAsJsonAsync($"/api/v1/waybills/{scope.WaybillId}/payment-plan", request with
        {
            ClientOperationId = $"http-wrong-{Guid.NewGuid():N}"
        });
        Assert.Equal(HttpStatusCode.Forbidden, scoped.StatusCode);
    }

    private static RecordCollectionRequest NewCollection(
        FinanceScope scope, decimal amount, string operationId, Guid? accountingReferenceId = null, string? accountingDocumentType = null)
        => new("SENDER", null, "CASH", new MoneyAmount(scope.CurrencyId, amount), 2m,
            "USER", scope.UserId, DateTimeOffset.UtcNow, operationId, accountingReferenceId, accountingDocumentType);

    private static WaybillFinanceApplicationService CreateService(TransportErpDbContext db)
        => new(new EfWaybillFinanceStore(db, new EfWaybillAuditSink(db, new AuditEventService(db))));

    private static string RequireConnection()
        => Environment.GetEnvironmentVariable("TRANSPORTERP_TEST_CONNSTR")
            ?? throw new InvalidOperationException("TRANSPORTERP_TEST_CONNSTR is required for P2-C01-B PostgreSQL gates.");

    private static async Task EnsureMigratedAsync(string connection)
    {
        await using var db = CreateP2Db(connection);
        await db.Database.MigrateAsync();
    }

    private static TransportErpDbContext CreateP2Db(string connection)
        => new(new DbContextOptionsBuilder<TransportErpDbContext>()
            .UseNpgsql(connection, npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "transport_erp"))
            .ReplaceService<IModelCustomizer, TransportErpP2CombinedModelCustomizer>()
            .AddInterceptors(new P2FinanceAppendOnlyInterceptor())
            .Options);

    private static WebApplicationFactory<Program> CreateFactory(string connection)
        => new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:TransportErp", connection);
            builder.UseSetting("Auth:Issuer", Issuer);
            builder.UseSetting("Auth:Audience", Audience);
            builder.UseSetting("Auth:SigningKey", SigningKey);
        });

    private static string CreateToken(Guid userId, Guid companyId, Guid branchId, string permission)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim("company_id", companyId.ToString()),
            new Claim("branch_id", branchId.ToString()),
            new Claim("permission", permission)
        };
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = Issuer,
            Audience = Audience,
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)), SecurityAlgorithms.HmacSha256)
        };
        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(descriptor));
    }

    private static async Task<FinanceScope> SeedApprovedWaybillAsync(TransportErpDbContext db, string suffix)
    {
        var now = DateTimeOffset.UtcNow;
        var currency = new Currency
        {
            Id = Guid.NewGuid(), Code = Guid.NewGuid().ToString("N")[..3].ToUpperInvariant(),
            NameAr = "عملة اختبار B", MinorUnit = 2, IsBase = true,
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        };
        var company = new Company
        {
            Id = Guid.NewGuid(), Code = $"B-{suffix}-{Guid.NewGuid():N}"[..20], LegalNameAr = "شركة اختبار B",
            BaseCurrencyId = currency.Id, DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        };
        var branch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = "MAIN", NameAr = "الفرع الرئيسي",
            Timezone = "Asia/Aden", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var user = new User
        {
            Id = Guid.NewGuid(), UserName = $"p2b-{Guid.NewGuid():N}", NormalizedUserName = $"P2B{suffix}{Guid.NewGuid():N}"[..24],
            DisplayName = "مستخدم اختبار B", PasswordHash = "test-only", Status = "ACTIVE",
            CompanyId = company.Id, BranchId = branch.Id, CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var waybill = NewApprovedWaybill(company.Id, branch.Id, currency.Id, suffix, now);
        var receipt = new ReceiptVoucher
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, BranchId = branch.Id,
            VoucherNo = $"RV-{suffix}-{Guid.NewGuid():N}"[..30], VoucherDate = DateTime.UtcNow.Date,
            PayerName = "دافع اختبار B", ReferenceType = "WAYBILL", ReferenceId = waybill.Id,
            PaymentMethodCode = "CASH", Amount = 100m, CurrencyId = currency.Id, Status = "APPROVED",
            CollectedBy = user.Id, CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        };

        db.Currencies.Add(currency);
        db.Companies.Add(company);
        db.Branches.Add(branch);
        db.Users.Add(user);
        db.Set<WaybillEntity>().Add(waybill);
        db.ReceiptVouchers.Add(receipt);
        await db.SaveChangesAsync();
        return new FinanceScope(company.Id, branch.Id, user.Id, currency.Id, waybill.Id, receipt.Id);
    }

    private static async Task<FinanceScope> SeedSecondBranchWaybillAsync(
        TransportErpDbContext db, FinanceScope first, string suffix)
    {
        var now = DateTimeOffset.UtcNow;
        var branch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = first.CompanyId, Code = $"B2-{Guid.NewGuid():N}"[..20], NameAr = "فرع اختبار ثان",
            Timezone = "Asia/Aden", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var waybill = NewApprovedWaybill(first.CompanyId, branch.Id, first.CurrencyId, suffix, now);
        db.Branches.Add(branch);
        db.Set<WaybillEntity>().Add(waybill);
        await db.SaveChangesAsync();
        return new FinanceScope(first.CompanyId, branch.Id, first.UserId, first.CurrencyId, waybill.Id, Guid.Empty);
    }

    private static WaybillEntity NewApprovedWaybill(Guid companyId, Guid branchId, Guid currencyId, string suffix, DateTimeOffset now)
        => new()
        {
            Id = Guid.NewGuid(), CompanyId = companyId, BranchId = branchId,
            DraftNo = $"D-{Guid.NewGuid():N}", WaybillNo = $"WB-{suffix}-{Guid.NewGuid():N}"[..30],
            WaybillDateTime = now, ServiceType = "STANDARD", Priority = "NORMAL",
            OriginId = Guid.NewGuid(), DestinationId = Guid.NewGuid(), CurrencyId = currencyId,
            ExchangeRate = 2m, FreightTotal = 100m, DiscountTotal = 0m,
            Status = "APPROVED", FinancialStatus = "UNPAID",
            CreateClientOperationId = $"seed-create-{Guid.NewGuid():N}",
            LastClientOperationId = $"seed-approve-{Guid.NewGuid():N}",
            Version = 1, CreatedAt = now, UpdatedAt = now
        };

    private sealed record FinanceScope(
        Guid CompanyId, Guid BranchId, Guid UserId, Guid CurrencyId, Guid WaybillId, Guid ReceiptVoucherId);
}
