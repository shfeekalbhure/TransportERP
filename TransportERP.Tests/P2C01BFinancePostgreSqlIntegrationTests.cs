using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
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

    [Theory]
    [InlineData("40001")]
    [InlineData("40P01")]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Payment_plan_maps_audit_serialization_failure_and_rolls_back_business_and_stream_head(string sqlState)
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        FinanceScope scope;
        await using (var seedDb = CreateP2Db(connection))
            scope = await SeedApprovedWaybillAsync(seedDb, "BSERIAL");

        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());
        var operationId = $"plan-serial-{Guid.NewGuid():N}";
        var suffix = Guid.NewGuid().ToString("N");
        var function = $"fail_payment_plan_serial_{suffix}";
        var trigger = $"trg_fail_payment_plan_serial_{suffix}";
        await using var admin = CreateP2Db(connection);
        await admin.Database.ExecuteSqlRawAsync($$"""
            CREATE FUNCTION transport_erp.{{function}}() RETURNS trigger LANGUAGE plpgsql AS $body$
            BEGIN
              IF NEW."Action" = 'WaybillPaymentPlanSet' THEN
                RAISE EXCEPTION 'forced serialization failure' USING ERRCODE = '{{sqlState}}';
              END IF;
              RETURN NEW;
            END $body$;
            CREATE TRIGGER {{trigger}} BEFORE INSERT ON transport_erp.audit_events
              FOR EACH ROW EXECUTE FUNCTION transport_erp.{{function}}();
            """);
        try
        {
            await using var db = CreateP2Db(connection);
            var ex = await Assert.ThrowsAsync<WaybillPersistenceException>(() =>
                CreateService(db).SetPaymentPlanAsync(context, scope.WaybillId, new SetPaymentPlanRequest(
                    1,
                    [new PaymentPlanLineInput(1, "SENDER", null, "CASH",
                        new MoneyAmount(scope.CurrencyId, 100m), null, "ON_APPROVAL", null)],
                    operationId)));
            Assert.Equal("CONCURRENCY_CONFLICT", ex.Code);
        }
        finally
        {
            await admin.Database.ExecuteSqlRawAsync($$"""
                DROP TRIGGER IF EXISTS {{trigger}} ON transport_erp.audit_events;
                DROP FUNCTION IF EXISTS transport_erp.{{function}}();
                """);
        }

        await using var verify = CreateP2Db(connection);
        var waybill = await verify.Set<WaybillEntity>().AsNoTracking().SingleAsync(x => x.Id == scope.WaybillId);
        Assert.Equal(1, waybill.Version);
        Assert.NotEqual(operationId, waybill.LastClientOperationId);
        Assert.False(await verify.Set<PaymentPlanLineEntity>().AnyAsync(x => x.WaybillId == scope.WaybillId));
        Assert.False(await verify.AuditEvents.AnyAsync(x =>
            x.Action == "WaybillPaymentPlanSet" && x.CorrelationId == context.CorrelationId));
        Assert.False(await verify.AuditStreamHeads.AnyAsync(x =>
            x.StreamKey == AuditEventService.GetStreamKey(scope.CompanyId, scope.BranchId, null)));
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Payment_plan_replay_is_idempotent_only_when_the_complete_plan_matches()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        FinanceScope scope;
        await using (var seedDb = CreateP2Db(connection))
            scope = await SeedApprovedWaybillAsync(seedDb, "BPLANREPLAY");

        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());
        var request = new SetPaymentPlanRequest(1,
            [new PaymentPlanLineInput(1, "SENDER", null, "CASH",
                new MoneyAmount(scope.CurrencyId, 100m), null, "ON_APPROVAL", null)],
            $"plan-replay-{Guid.NewGuid():N}");
        PaymentPlanResponse first;
        await using (var db = CreateP2Db(connection))
            first = await CreateService(db).SetPaymentPlanAsync(context, scope.WaybillId, request);
        await using (var db = CreateP2Db(connection))
        {
            var replay = await CreateService(db).SetPaymentPlanAsync(context, scope.WaybillId, request);
            Assert.Equal(first.Lines[0].Id, replay.Lines[0].Id);
        }
        await using (var db = CreateP2Db(connection))
        {
            var altered = request with
            {
                Lines = [request.Lines[0] with { PaymentMethodCode = "BANK" }]
            };
            var ex = await Assert.ThrowsAsync<WaybillPersistenceException>(() =>
                CreateService(db).SetPaymentPlanAsync(context, scope.WaybillId, altered));
            Assert.Equal("IDEMPOTENCY_CONFLICT", ex.Code);
        }
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
    public async Task Collection_replay_compares_every_caller_controlled_business_field()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        FinanceScope scope;
        await using (var seedDb = CreateP2Db(connection))
            scope = await SeedApprovedWaybillAsync(seedDb, "BFINGERPRINT");

        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());
        var request = NewCollection(scope, 30m, $"fingerprint-{Guid.NewGuid():N}") with
        {
            CollectedAt = DateTimeOffset.UtcNow.AddMinutes(-1)
        };
        await using (var db = CreateP2Db(connection))
            _ = await CreateService(db).RecordCollectionAsync(context, scope.WaybillId, request);

        var mutations = new[]
        {
            request with { PayerRole = "RECEIVER" },
            request with { PaymentMethodCode = "BANK" },
            request with { Amount = new MoneyAmount(Guid.NewGuid(), request.Amount.Amount) },
            request with { Amount = new MoneyAmount(request.Amount.CurrencyId, request.Amount.Amount + 1m) },
            request with { ExchangeRate = request.ExchangeRate + 1m },
            request with { CollectedAt = request.CollectedAt.AddSeconds(1) },
            request with { AccountingReferenceId = Guid.NewGuid(), AccountingDocumentType = "RECEIPT_VOUCHER" }
        };
        foreach (var mutation in mutations)
        {
            await using var db = CreateP2Db(connection);
            var ex = await Assert.ThrowsAsync<WaybillPersistenceException>(() =>
                CreateService(db).RecordCollectionAsync(context, scope.WaybillId, mutation));
            Assert.Equal("IDEMPOTENCY_CONFLICT", ex.Code);
        }
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Finance_application_rejects_cross_company_party_and_unproven_collector_before_store()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        FinanceScope local;
        FinanceScope foreign;
        Guid foreignPartyId;
        Guid foreignBranchPartyId;
        await using (var db = CreateP2Db(connection))
        {
            local = await SeedApprovedWaybillAsync(db, "BLOCAL");
            foreign = await SeedApprovedWaybillAsync(db, "BFOREIGN");
            foreignPartyId = await SeedPartyAsync(db, foreign, "FOREIGN");
            var foreignBranch = await SeedSecondBranchWaybillAsync(db, local, "BFOREIGNBRANCH");
            foreignBranchPartyId = await SeedPartyAsync(db, foreignBranch, "FOREIGNBRANCH");
        }

        var context = new OperationContext(local.UserId, local.CompanyId, local.BranchId, Guid.NewGuid());
        await using (var db = CreateP2Db(connection))
        {
            var ex = await Assert.ThrowsAsync<WaybillPersistenceException>(() =>
                CreateService(db).SetPaymentPlanAsync(context, local.WaybillId, new SetPaymentPlanRequest(
                    1,
                    [new PaymentPlanLineInput(1, "SENDER", foreignPartyId, "CASH",
                        new MoneyAmount(local.CurrencyId, 100m), null, "ON_APPROVAL", null)],
                    $"foreign-plan-{Guid.NewGuid():N}")));
            Assert.Equal("SCOPE_DENIED", ex.Code);
        }

        await using (var db = CreateP2Db(connection))
        {
            var ex = await Assert.ThrowsAsync<WaybillPersistenceException>(() =>
                CreateService(db).SetPaymentPlanAsync(context, local.WaybillId, new SetPaymentPlanRequest(
                    1,
                    [new PaymentPlanLineInput(1, "SENDER", foreignBranchPartyId, "CASH",
                        new MoneyAmount(local.CurrencyId, 100m), null, "ON_APPROVAL", null)],
                    $"foreign-branch-plan-{Guid.NewGuid():N}")));
            Assert.Equal("SCOPE_DENIED", ex.Code);
        }

        await using (var db = CreateP2Db(connection))
        {
            var request = NewCollection(local, 10m, $"spoofed-collector-{Guid.NewGuid():N}") with
            {
                CollectedById = foreign.UserId
            };
            var ex = await Assert.ThrowsAsync<WaybillFinanceApplicationException>(() =>
                CreateService(db).RecordCollectionAsync(context, local.WaybillId, request));
            Assert.Equal("SCOPE_DENIED", ex.Code);
        }
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task PostgreSQL_triggers_reject_cross_company_party_in_all_three_reference_tables()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        FinanceScope local;
        FinanceScope foreign;
        Guid foreignPartyId;
        await using (var db = CreateP2Db(connection))
        {
            local = await SeedApprovedWaybillAsync(db, "BDBLOCAL");
            foreign = await SeedApprovedWaybillAsync(db, "BDBFOREIGN");
            foreignPartyId = await SeedPartyAsync(db, foreign, "DBFOREIGN");
        }

        await AssertDatabaseScopeDenied(connection, db => db.Set<WaybillPartyEntity>().Add(new WaybillPartyEntity
        {
            Id = Guid.NewGuid(), WaybillId = local.WaybillId, Sequence = 1, Role = "SENDER",
            OperationalPartyId = foreignPartyId, NameSnapshot = "masked", MobileSnapshot = "masked"
        }));
        await AssertDatabaseScopeDenied(connection, db => db.Set<PaymentPlanLineEntity>().Add(new PaymentPlanLineEntity
        {
            Id = Guid.NewGuid(), WaybillId = local.WaybillId, LineNo = 1, PayerRole = "SENDER",
            PartyId = foreignPartyId, PaymentMethodCode = "CASH", AmountCurrencyId = local.CurrencyId,
            Amount = 100m, DueTrigger = "ON_APPROVAL", Status = "ACTIVE",
            CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow, Version = 1
        }));
        await AssertDatabaseScopeDenied(connection, db => db.Set<CollectionTransactionEntity>().Add(new CollectionTransactionEntity
        {
            Id = Guid.NewGuid(), WaybillId = local.WaybillId, CompanyId = local.CompanyId, BranchId = local.BranchId,
            PayerRole = "SENDER", PartyId = foreignPartyId, PaymentMethodCode = "CASH",
            CurrencyId = local.CurrencyId, ExchangeRate = 2m, Amount = 10m,
            CollectedByType = "USER", CollectedById = local.UserId, CollectedAt = DateTimeOffset.UtcNow,
            ClientOperationId = $"db-scope-{Guid.NewGuid():N}", Status = "ACCEPTED"
        }));
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
        FinanceScope foreign;
        Guid foreignPartyId;
        await using (var seedDb = CreateP2Db(connection))
        {
            scope = await SeedApprovedWaybillAsync(seedDb, "BHTTP");
            foreign = await SeedApprovedWaybillAsync(seedDb, "BHTTPFOREIGN");
            foreignPartyId = await SeedPartyAsync(seedDb, foreign, "HTTPFOREIGN");
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

        var crossCompanyParty = await client.PutAsJsonAsync($"/api/v1/waybills/{scope.WaybillId}/payment-plan",
            new SetPaymentPlanRequest(2,
                [new PaymentPlanLineInput(1, "SENDER", foreignPartyId, "CASH",
                    new MoneyAmount(scope.CurrencyId, 100m), null, "ON_APPROVAL", null)],
                $"http-cross-company-party-{Guid.NewGuid():N}"));
        Assert.Equal(HttpStatusCode.Forbidden, crossCompanyParty.StatusCode);

        var foreignBranchToken = CreateToken(scope.UserId, scope.CompanyId, Guid.NewGuid(), WaybillFinancePermissionCodes.PaymentPlan);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", foreignBranchToken);
        var scoped = await client.PutAsJsonAsync($"/api/v1/waybills/{scope.WaybillId}/payment-plan", request with
        {
            ClientOperationId = $"http-wrong-{Guid.NewGuid():N}"
        });
        Assert.Equal(HttpStatusCode.NotFound, scoped.StatusCode);
    }

    private static RecordCollectionRequest NewCollection(
        FinanceScope scope, decimal amount, string operationId, Guid? accountingReferenceId = null, string? accountingDocumentType = null)
        => new("SENDER", null, "CASH", new MoneyAmount(scope.CurrencyId, amount), 2m,
            "USER", scope.UserId, DateTimeOffset.UtcNow, operationId, accountingReferenceId, accountingDocumentType);

    private static WaybillFinanceApplicationService CreateService(TransportErpDbContext db)
        => new(
            new EfWaybillFinanceStore(db, new EfWaybillAuditSink(db, new AuditEventService(db))),
            new EfOperationalPartyRepository(db));

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
            builder.UseSetting("Auth:SigningKeyId", "test-current");
            builder.ConfigureServices(services =>
            {
                Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.RemoveAll<TransportERP.Api.Security.ICurrentSecurityContext>(services);
                services.AddSingleton<TransportERP.Api.Security.ICurrentSecurityContext, ClaimTestSecurityContext>();
            });
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
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)) { KeyId = "test-current" }, SecurityAlgorithms.HmacSha256)
        };
        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(descriptor));
    }

    private static async Task<FinanceScope> SeedApprovedWaybillAsync(TransportErpDbContext db, string suffix)
    {
        var now = DateTimeOffset.UtcNow;
        var currency = new Currency
        {
            Id = Guid.NewGuid(), Code = await PostgreSqlTestCurrencyCodeAllocator.NextAsync(db),
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
            DisplayName = "مستخدم اختبار B", PasswordHash = "test-only", SecurityStamp = Guid.NewGuid().ToString("N"), AuthVersion = 1, Status = "ACTIVE",
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

    private static async Task<Guid> SeedPartyAsync(TransportErpDbContext db, FinanceScope scope, string suffix)
    {
        var now = DateTimeOffset.UtcNow;
        var party = new OperationalPartyEntity
        {
            Id = Guid.NewGuid(), CompanyId = scope.CompanyId, BranchId = scope.BranchId,
            PartyNo = $"P-{suffix}-{Guid.NewGuid():N}"[..30], Name = "Party scope test", Mobile = "+967700000000",
            Status = "ACTIVE", ClientOperationId = $"party-{suffix}-{Guid.NewGuid():N}",
            CreatedAt = now, UpdatedAt = now, Version = 1
        };
        db.Set<OperationalPartyEntity>().Add(party);
        await db.SaveChangesAsync();
        return party.Id;
    }

    private static async Task AssertDatabaseScopeDenied(
        string connection,
        Action<TransportErpDbContext> addInvalidReference)
    {
        await using var db = CreateP2Db(connection);
        addInvalidReference(db);
        var ex = await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
        Assert.Contains("operational party reference scope denied", ex.ToString(), StringComparison.OrdinalIgnoreCase);
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
