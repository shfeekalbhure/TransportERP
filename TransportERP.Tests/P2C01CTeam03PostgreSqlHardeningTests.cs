using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql;
using TransportERP.Application.Waybills;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Waybills;
using TransportERP.Domain.Waybills;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class P2C01CTeam03PostgreSqlHardeningTests
{
    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Database_append_only_triggers_reject_raw_mutation_and_allow_reversal_inserts()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        ShippingScope scope;
        await using (var seed = CreateDb(connection))
            scope = await SeedApprovedWaybillAsync(seed, "AO", 10m);
        var context = NewContext(scope);

        await using (var db = CreateDb(connection))
            _ = await Service(db).ReleaseItemAsync(context, scope.WaybillId, scope.ItemId,
                new ReleaseItemRequest(10m, DateTimeOffset.UtcNow, $"rel-{Guid.NewGuid():N}"));

        Guid releaseId;
        await using (var db = CreateDb(connection))
            releaseId = await db.Set<ItemReleaseEntity>().AsNoTracking()
                .Where(x => x.CompanyId == scope.CompanyId && x.BranchId == scope.BranchId &&
                            x.WaybillItemId == scope.ItemId && x.Status == "ACTIVE")
                .Select(x => x.Id).SingleAsync();

        var loadedTrip = await CreateTripAsync(connection, context, scope, "LOAD");
        AllocationResponse loadedAllocation;
        await using (var db = CreateDb(connection))
            loadedAllocation = await Service(db).AllocateAsync(context, loadedTrip.Id,
                new AllocateItemRequest(scope.ItemId, releaseId, 5m, $"alloc-load-{Guid.NewGuid():N}"));

        ManifestResponse manifest;
        await using (var db = CreateDb(connection))
            manifest = await Service(db).GenerateManifestAsync(context, loadedTrip.Id,
                new GenerateManifestRequest(null, $"mf-load-{Guid.NewGuid():N}"));
        await using (var db = CreateDb(connection))
            _ = await Service(db).LoadManifestLineAsync(context, manifest.Id, manifest.Lines.Single().Id,
                new LoadManifestLineRequest(1m, DateTimeOffset.UtcNow, true, $"load-{Guid.NewGuid():N}"));

        var reversibleTrip = await CreateTripAsync(connection, context, scope, "REV");
        AllocationResponse reversibleAllocation;
        await using (var db = CreateDb(connection))
            reversibleAllocation = await Service(db).AllocateAsync(context, reversibleTrip.Id,
                new AllocateItemRequest(scope.ItemId, releaseId, 5m, $"alloc-rev-{Guid.NewGuid():N}"));

        Guid movementId;
        await using (var db = CreateDb(connection))
            movementId = await db.Set<MovementEventEntity>().AsNoTracking()
                .Where(x => x.CompanyId == scope.CompanyId && x.BranchId == scope.BranchId &&
                            x.ManifestId == manifest.Id && x.EventType == "LOAD")
                .Select(x => x.Id).SingleAsync();

        await AssertRawAppendOnlyFailure(connection,
            $"""UPDATE transport_erp.item_releases SET "Quantity" = 9 WHERE "Id" = '{releaseId}'""");
        await AssertRawAppendOnlyFailure(connection,
            $"""DELETE FROM transport_erp.trip_allocations WHERE "Id" = '{loadedAllocation.Id}'""");
        await AssertRawAppendOnlyFailure(connection,
            $"""UPDATE transport_erp.movement_events SET "Quantity" = 2 WHERE "Id" = '{movementId}'""");

        await using (var db = CreateDb(connection))
        {
            var reversal = await Service(db).UnallocateAsync(context, reversibleAllocation.Id,
                new UnallocateRequest("test reversal insert", $"unalloc-{Guid.NewGuid():N}"));
            Assert.Equal("REVERSED", reversal.Status);
            Assert.Equal(reversibleAllocation.Id, reversal.ReversalOfId);
        }

        await using (var db = CreateDb(connection))
        {
            db.Set<ItemReleaseEntity>().Add(new ItemReleaseEntity
            {
                Id = Guid.NewGuid(),
                CompanyId = scope.CompanyId,
                BranchId = scope.BranchId,
                WaybillItemId = scope.ItemId,
                Quantity = 1m,
                ReleasedAt = DateTimeOffset.UtcNow,
                ReleasedBy = scope.UserId,
                ClientOperationId = $"release-reversal-{Guid.NewGuid():N}",
                Status = "REVERSED",
                ReversalOfId = releaseId,
                Reason = "test reversal insert"
            });
            await db.SaveChangesAsync();
        }
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Lifecycle_idempotency_outcomes_survive_later_transitions_and_detect_fingerprint_conflicts()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        ShippingScope scope;
        await using (var seed = CreateDb(connection))
            scope = await SeedApprovedWaybillAsync(seed, "IDEM", 6m);
        var context = NewContext(scope);

        var prepared = await PrepareLoadedManifestAsync(connection, context, scope, 6m);

        var finalizeOp = $"same-finalize-{Guid.NewGuid():N}";
        ManifestResponse finalized;
        await using (var db = CreateDb(connection))
            finalized = await Service(db).FinalizeManifestAsync(context, prepared.Manifest.Id,
                new FinalizeManifestRequest(prepared.Manifest.Version, finalizeOp));
        Assert.Equal("FINALIZED", finalized.Status);

        var acceptedAt = DateTimeOffset.UtcNow;
        var handoverOp = $"same-handover-{Guid.NewGuid():N}";
        ManifestResponse accepted;
        await using (var db = CreateDb(connection))
            accepted = await Service(db).HandoverManifestAsync(context, prepared.Manifest.Id,
                new HandoverManifestRequest(scope.UserId, acceptedAt, finalized.Version, handoverOp));
        Assert.Equal("ACCEPTED", accepted.Status);

        var departAt = DateTimeOffset.UtcNow;
        var startOp = $"same-start-{Guid.NewGuid():N}";
        long startExpectedVersion;
        TripResponse departed;
        await using (var db = CreateDb(connection))
        {
            startExpectedVersion = await db.Set<TripEntity>().AsNoTracking()
                .Where(x => x.Id == prepared.Trip.Id &&
                            x.CompanyId == scope.CompanyId && x.BranchId == scope.BranchId)
                .Select(x => x.Version).SingleAsync();
            departed = await Service(db).StartTripAsync(context, prepared.Trip.Id,
                new StartTripRequest(departAt, startExpectedVersion, startOp));
        }
        Assert.Equal("DEPARTED", departed.Status);

        await using (var db = CreateDb(connection))
        {
            var replay = await Service(db).FinalizeManifestAsync(context, prepared.Manifest.Id,
                new FinalizeManifestRequest(prepared.Manifest.Version, finalizeOp));
            Assert.Equal("FINALIZED", replay.Status);
            Assert.Equal(finalized.Version, replay.Version);
        }

        await using (var db = CreateDb(connection))
        {
            var replay = await Service(db).HandoverManifestAsync(context, prepared.Manifest.Id,
                new HandoverManifestRequest(scope.UserId, acceptedAt, finalized.Version, handoverOp));
            Assert.Equal("ACCEPTED", replay.Status);
            Assert.Equal(accepted.Version, replay.Version);
        }

        await using (var db = CreateDb(connection))
        {
            var replay = await Service(db).StartTripAsync(context, prepared.Trip.Id,
                new StartTripRequest(departAt, startExpectedVersion, startOp));
            Assert.Equal("DEPARTED", replay.Status);
            Assert.Equal(departed.Version, replay.Version);
        }

        await using (var db = CreateDb(connection))
        {
            var conflict = await Assert.ThrowsAsync<WaybillPersistenceException>(() =>
                Service(db).FinalizeManifestAsync(context, prepared.Manifest.Id,
                    new FinalizeManifestRequest(prepared.Manifest.Version + 99, finalizeOp)));
            Assert.Equal("IDEMPOTENCY_CONFLICT", conflict.Code);
        }

        await using (var db = CreateDb(connection))
        {
            var count = await db.Database.SqlQueryRaw<int>("""
                SELECT COUNT(*)::int AS "Value"
                FROM transport_erp.shipping_command_outcomes
                WHERE "CompanyId" = {0} AND "BranchId" = {1}
                """, scope.CompanyId, scope.BranchId).SingleAsync();
            Assert.Equal(3, count);
        }
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Same_company_branches_and_cross_company_contexts_are_isolated()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);

        ShippingScope first;
        await using (var seed = CreateDb(connection))
            first = await SeedApprovedWaybillAsync(seed, "SCOPE-A", 4m);

        ShippingScope second;
        await using (var db = CreateDb(connection))
            second = await SeedApprovedWaybillInCompanyAsync(db, first, "SCOPE-B", 4m);

        ShippingScope third;
        await using (var seed = CreateDb(connection))
            third = await SeedApprovedWaybillAsync(seed, "SCOPE-C", 4m);

        var firstContext = NewContext(first);
        var secondContext = NewContext(second);
        var thirdContext = NewContext(third);
        var sharedReleaseOp = $"shared-release-{Guid.NewGuid():N}";

        await using (var db = CreateDb(connection))
            _ = await Service(db).ReleaseItemAsync(firstContext, first.WaybillId, first.ItemId,
                new ReleaseItemRequest(4m, DateTimeOffset.UtcNow, sharedReleaseOp));
        await using (var db = CreateDb(connection))
            _ = await Service(db).ReleaseItemAsync(secondContext, second.WaybillId, second.ItemId,
                new ReleaseItemRequest(4m, DateTimeOffset.UtcNow, sharedReleaseOp));

        var firstPrepared = await PrepareLoadedManifestFromExistingReleaseAsync(
            connection, firstContext, first, 4m, load: false);
        var secondPrepared = await PrepareLoadedManifestFromExistingReleaseAsync(
            connection, secondContext, second, 4m, load: false);

        var sharedLoadOp = $"shared-load-{Guid.NewGuid():N}";
        await using (var db = CreateDb(connection))
            _ = await Service(db).LoadManifestLineAsync(firstContext, firstPrepared.Manifest.Id,
                firstPrepared.Manifest.Lines.Single().Id,
                new LoadManifestLineRequest(4m, DateTimeOffset.UtcNow, true, sharedLoadOp));
        await using (var db = CreateDb(connection))
            _ = await Service(db).LoadManifestLineAsync(secondContext, secondPrepared.Manifest.Id,
                secondPrepared.Manifest.Lines.Single().Id,
                new LoadManifestLineRequest(4m, DateTimeOffset.UtcNow, true, sharedLoadOp));

        var sharedFinalizeOp = $"shared-finalize-{Guid.NewGuid():N}";
        await using (var db = CreateDb(connection))
            _ = await Service(db).FinalizeManifestAsync(firstContext, firstPrepared.Manifest.Id,
                new FinalizeManifestRequest(firstPrepared.Manifest.Version, sharedFinalizeOp));
        await using (var db = CreateDb(connection))
            _ = await Service(db).FinalizeManifestAsync(secondContext, secondPrepared.Manifest.Id,
                new FinalizeManifestRequest(secondPrepared.Manifest.Version, sharedFinalizeOp));

        await using (var db = CreateDb(connection))
        {
            var wrongBranch = await Assert.ThrowsAsync<WaybillPersistenceException>(() =>
                Service(db).ReleaseItemAsync(firstContext, second.WaybillId, second.ItemId,
                    new ReleaseItemRequest(1m, DateTimeOffset.UtcNow,
                        $"wrong-branch-{Guid.NewGuid():N}")));
            Assert.Equal("NOT_FOUND", wrongBranch.Code);
        }

        await using (var db = CreateDb(connection))
        {
            var crossCompany = await Assert.ThrowsAsync<WaybillPersistenceException>(() =>
                Service(db).ReleaseItemAsync(thirdContext, first.WaybillId, first.ItemId,
                    new ReleaseItemRequest(1m, DateTimeOffset.UtcNow,
                        $"wrong-company-{Guid.NewGuid():N}")));
            Assert.Equal("NOT_FOUND", crossCompany.Code);
        }

        await using (var db = CreateDb(connection))
        {
            var movementCount = await db.Set<MovementEventEntity>().AsNoTracking()
                .CountAsync(x => x.CompanyId == first.CompanyId &&
                                 (x.BranchId == first.BranchId || x.BranchId == second.BranchId) &&
                                 x.EventType == "LOAD");
            Assert.Equal(2, movementCount);

            var outcomeCount = await db.Database.SqlQueryRaw<int>("""
                SELECT COUNT(*)::int AS "Value"
                FROM transport_erp.shipping_command_outcomes
                WHERE "CompanyId" = {0}
                  AND "Action" = 'MANIFEST_FINALIZE'
                  AND "ClientOperationId" = {1}
                """, first.CompanyId, sharedFinalizeOp).SingleAsync();
            Assert.Equal(2, outcomeCount);
        }
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Concurrent_manifest_and_lifecycle_commands_preserve_single_persisted_transition()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        ShippingScope scope;
        await using (var seed = CreateDb(connection))
            scope = await SeedApprovedWaybillAsync(seed, "LIFE", 8m);
        var context = NewContext(scope);

        await using (var db = CreateDb(connection))
            _ = await Service(db).ReleaseItemAsync(context, scope.WaybillId, scope.ItemId,
                new ReleaseItemRequest(8m, DateTimeOffset.UtcNow, $"release-{Guid.NewGuid():N}"));

        var trip = await CreateTripAsync(connection, context, scope, "LIFE");
        Guid releaseId;
        await using (var db = CreateDb(connection))
            releaseId = await db.Set<ItemReleaseEntity>().AsNoTracking()
                .Where(x => x.CompanyId == scope.CompanyId && x.BranchId == scope.BranchId &&
                            x.WaybillItemId == scope.ItemId && x.Status == "ACTIVE")
                .Select(x => x.Id).SingleAsync();
        await using (var db = CreateDb(connection))
            _ = await Service(db).AllocateAsync(context, trip.Id,
                new AllocateItemRequest(scope.ItemId, releaseId, 8m, $"alloc-{Guid.NewGuid():N}"));

        var generateGate = NewGate();
        var generateA = RunAtGate(generateGate, async () =>
        {
            await using var db = CreateDb(connection);
            await Service(db).GenerateManifestAsync(context, trip.Id,
                new GenerateManifestRequest(null, $"mf-a-{Guid.NewGuid():N}"));
        });
        var generateB = RunAtGate(generateGate, async () =>
        {
            await using var db = CreateDb(connection);
            await Service(db).GenerateManifestAsync(context, trip.Id,
                new GenerateManifestRequest(null, $"mf-b-{Guid.NewGuid():N}"));
        });
        generateGate.SetResult();
        var generateResults = await Task.WhenAll(generateA, generateB);
        Assert.Equal(1, generateResults.Count(x => x == "OK"));
        Assert.Equal(1, generateResults.Count(x => x is "CONCURRENCY_CONFLICT" or "DUPLICATE_OPERATION"));

        ManifestResponse manifest;
        await using (var db = CreateDb(connection))
        {
            var entity = await db.Set<ManifestEntity>().AsNoTracking()
                .SingleAsync(x => x.TripId == trip.Id &&
                                  x.CompanyId == scope.CompanyId && x.BranchId == scope.BranchId);
            manifest = await LoadManifestResponse(db, context, entity.Id);
        }

        await using (var db = CreateDb(connection))
            _ = await Service(db).LoadManifestLineAsync(context, manifest.Id, manifest.Lines.Single().Id,
                new LoadManifestLineRequest(8m, DateTimeOffset.UtcNow, true, $"load-{Guid.NewGuid():N}"));

        var finalizeGate = NewGate();
        var finalizeA = RunAtGate(finalizeGate, async () =>
        {
            await using var db = CreateDb(connection);
            await Service(db).FinalizeManifestAsync(context, manifest.Id,
                new FinalizeManifestRequest(manifest.Version, $"fin-a-{Guid.NewGuid():N}"));
        });
        var finalizeB = RunAtGate(finalizeGate, async () =>
        {
            await using var db = CreateDb(connection);
            await Service(db).FinalizeManifestAsync(context, manifest.Id,
                new FinalizeManifestRequest(manifest.Version, $"fin-b-{Guid.NewGuid():N}"));
        });
        finalizeGate.SetResult();
        var finalizeResults = await Task.WhenAll(finalizeA, finalizeB);
        Assert.Equal(1, finalizeResults.Count(x => x == "OK"));
        Assert.Equal(1, finalizeResults.Count(x => x is "CONCURRENCY_CONFLICT" or "INVALID_STATE"));

        long manifestVersion;
        await using (var db = CreateDb(connection))
            manifestVersion = await db.Set<ManifestEntity>().AsNoTracking()
                .Where(x => x.Id == manifest.Id &&
                            x.CompanyId == scope.CompanyId && x.BranchId == scope.BranchId)
                .Select(x => x.Version).SingleAsync();

        var handoverGate = NewGate();
        var handoverA = RunAtGate(handoverGate, async () =>
        {
            await using var db = CreateDb(connection);
            await Service(db).HandoverManifestAsync(context, manifest.Id,
                new HandoverManifestRequest(scope.UserId, DateTimeOffset.UtcNow, manifestVersion,
                    $"ho-a-{Guid.NewGuid():N}"));
        });
        var handoverB = RunAtGate(handoverGate, async () =>
        {
            await using var db = CreateDb(connection);
            await Service(db).HandoverManifestAsync(context, manifest.Id,
                new HandoverManifestRequest(scope.UserId, DateTimeOffset.UtcNow, manifestVersion,
                    $"ho-b-{Guid.NewGuid():N}"));
        });
        handoverGate.SetResult();
        var handoverResults = await Task.WhenAll(handoverA, handoverB);
        Assert.Equal(1, handoverResults.Count(x => x == "OK"));
        Assert.Equal(1, handoverResults.Count(x => x is "CONCURRENCY_CONFLICT" or "INVALID_STATE"));

        long tripVersion;
        await using (var db = CreateDb(connection))
            tripVersion = await db.Set<TripEntity>().AsNoTracking()
                .Where(x => x.Id == trip.Id &&
                            x.CompanyId == scope.CompanyId && x.BranchId == scope.BranchId)
                .Select(x => x.Version).SingleAsync();

        var startGate = NewGate();
        var startA = RunAtGate(startGate, async () =>
        {
            await using var db = CreateDb(connection);
            await Service(db).StartTripAsync(context, trip.Id,
                new StartTripRequest(DateTimeOffset.UtcNow, tripVersion, $"start-a-{Guid.NewGuid():N}"));
        });
        var startB = RunAtGate(startGate, async () =>
        {
            await using var db = CreateDb(connection);
            await Service(db).StartTripAsync(context, trip.Id,
                new StartTripRequest(DateTimeOffset.UtcNow, tripVersion, $"start-b-{Guid.NewGuid():N}"));
        });
        startGate.SetResult();
        var startResults = await Task.WhenAll(startA, startB);
        Assert.Equal(1, startResults.Count(x => x == "OK"));
        Assert.Equal(1, startResults.Count(x => x is "CONCURRENCY_CONFLICT" or "INVALID_STATE"));

        await using var verify = CreateDb(connection);
        Assert.Equal("DEPARTED", await verify.Set<TripEntity>().AsNoTracking()
            .Where(x => x.Id == trip.Id &&
                        x.CompanyId == scope.CompanyId && x.BranchId == scope.BranchId)
            .Select(x => x.Status).SingleAsync());
        Assert.Equal(1, await verify.Set<MovementEventEntity>().AsNoTracking()
            .CountAsync(x => x.TripId == trip.Id &&
                             x.CompanyId == scope.CompanyId && x.BranchId == scope.BranchId &&
                             x.EventType == "DEPART"));
    }

    private static async Task AssertRawAppendOnlyFailure(string connection, string sql)
    {
        await using var db = CreateDb(connection);
        var ex = await Assert.ThrowsAsync<PostgresException>(() => db.Database.ExecuteSqlRawAsync(sql));
        Assert.Equal("55000", ex.SqlState);
    }

    private static OperationContext NewContext(ShippingScope scope)
        => new(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());

    private static TaskCompletionSource NewGate()
        => new(TaskCreationOptions.RunContinuationsAsynchronously);

    private static async Task<string> RunAtGate(TaskCompletionSource gate, Func<Task> action)
    {
        await gate.Task;
        try
        {
            await action();
            return "OK";
        }
        catch (WaybillPersistenceException ex)
        {
            return ex.Code;
        }
        catch (ShippingExecutionRuleException ex)
        {
            return ex.Code;
        }
    }

    private static async Task<PreparedManifest> PrepareLoadedManifestAsync(
        string connection, OperationContext context, ShippingScope scope, decimal quantity)
    {
        await using (var db = CreateDb(connection))
            _ = await Service(db).ReleaseItemAsync(context, scope.WaybillId, scope.ItemId,
                new ReleaseItemRequest(quantity, DateTimeOffset.UtcNow, $"release-{Guid.NewGuid():N}"));
        return await PrepareLoadedManifestFromExistingReleaseAsync(
            connection, context, scope, quantity, load: true);
    }

    private static async Task<PreparedManifest> PrepareLoadedManifestFromExistingReleaseAsync(
        string connection, OperationContext context, ShippingScope scope, decimal quantity, bool load)
    {
        Guid releaseId;
        await using (var db = CreateDb(connection))
            releaseId = await db.Set<ItemReleaseEntity>().AsNoTracking()
                .Where(x => x.CompanyId == scope.CompanyId && x.BranchId == scope.BranchId &&
                            x.WaybillItemId == scope.ItemId && x.Status == "ACTIVE")
                .Select(x => x.Id).SingleAsync();

        var trip = await CreateTripAsync(connection, context, scope, "PREP");
        await using (var db = CreateDb(connection))
            _ = await Service(db).AllocateAsync(context, trip.Id,
                new AllocateItemRequest(scope.ItemId, releaseId, quantity, $"alloc-{Guid.NewGuid():N}"));

        ManifestResponse manifest;
        await using (var db = CreateDb(connection))
            manifest = await Service(db).GenerateManifestAsync(context, trip.Id,
                new GenerateManifestRequest(null, $"manifest-{Guid.NewGuid():N}"));

        if (load)
        {
            await using var db = CreateDb(connection);
            _ = await Service(db).LoadManifestLineAsync(context, manifest.Id, manifest.Lines.Single().Id,
                new LoadManifestLineRequest(quantity, DateTimeOffset.UtcNow, true, $"load-{Guid.NewGuid():N}"));
        }
        return new PreparedManifest(trip, manifest);
    }

    private static async Task<TripResponse> CreateTripAsync(
        string connection, OperationContext context, ShippingScope scope, string suffix)
    {
        var tripNo = $"TR-{suffix}-{Guid.NewGuid():N}";
        tripNo = tripNo[..Math.Min(30, tripNo.Length)];
        await using var db = CreateDb(connection);
        return await Service(db).CreateTripAsync(context,
            new CreateTripRequest(tripNo, Guid.NewGuid(), scope.UserId,
                scope.OriginId, scope.DestinationId, DateTimeOffset.UtcNow.AddHours(1), [],
                $"trip-{suffix}-{Guid.NewGuid():N}"));
    }

    private static async Task<ManifestResponse> LoadManifestResponse(
        TransportErpDbContext db, OperationContext context, Guid manifestId)
    {
        var manifest = await db.Set<ManifestEntity>().AsNoTracking().SingleAsync(x =>
            x.Id == manifestId && x.CompanyId == context.CompanyId && x.BranchId == context.BranchId);
        var tripVersion = await db.Set<TripEntity>().AsNoTracking()
            .Where(x => x.Id == manifest.TripId &&
                        x.CompanyId == context.CompanyId && x.BranchId == context.BranchId)
            .Select(x => x.Version).SingleAsync();
        var lines = await db.Set<ManifestLineEntity>().AsNoTracking()
            .Where(x => x.ManifestId == manifestId &&
                        x.Manifest!.CompanyId == context.CompanyId &&
                        x.Manifest.BranchId == context.BranchId)
            .OrderBy(x => x.Id)
            .Select(x => new ManifestLineResponse(
                x.Id, x.AllocationId, x.WaybillId, x.WaybillItemId,
                x.Quantity, x.LoadedQuantity, x.Weight, x.Volume, x.LoadStatus))
            .ToListAsync();
        return new ManifestResponse(
            manifest.Id, manifest.TripId, manifest.ManifestNo, manifest.CreatedAt,
            manifest.HandoverAt, manifest.DriverAcceptedAt, manifest.Status, manifest.Version,
            tripVersion, lines, context.CorrelationId);
    }

    private static ShippingExecutionApplicationService Service(TransportErpDbContext db)
        => new(new EfShippingExecutionStore(db, new EfWaybillAuditSink(db, new AuditEventService(db))));

    private static async Task EnsureMigratedAsync(string connection)
    {
        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
    }

    private static TransportErpDbContext CreateDb(string connection)
        => new(new DbContextOptionsBuilder<TransportErpDbContext>()
            .UseNpgsql(connection, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "transport_erp"))
            .ReplaceService<IModelCustomizer, TransportErpP2CombinedModelCustomizer>()
            .AddInterceptors(new P2FinanceAppendOnlyInterceptor(), new P2ShippingAppendOnlyInterceptor())
            .Options);

    private static string RequireConnection()
        => Environment.GetEnvironmentVariable("TRANSPORTERP_TEST_CONNSTR")
            ?? throw new InvalidOperationException(
                "TRANSPORTERP_TEST_CONNSTR is required for TEAM-03 PostgreSQL gates.");

    private static async Task<ShippingScope> SeedApprovedWaybillAsync(
        TransportErpDbContext db, string suffix, decimal quantity)
    {
        var now = DateTimeOffset.UtcNow;
        var currency = new Currency
        {
            Id = Guid.NewGuid(),
            Code = Guid.NewGuid().ToString("N")[..3].ToUpperInvariant(),
            NameAr = "عملة TEAM-03",
            MinorUnit = 2, IsBase = true, Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        };
        var company = new Company
        {
            Id = Guid.NewGuid(),
            Code = $"T3-{suffix}-{Guid.NewGuid():N}"[..20],
            LegalNameAr = "شركة TEAM-03",
            BaseCurrencyId = currency.Id,
            DefaultCalendarId = Guid.NewGuid(),
            Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        };
        var branch = NewBranch(company.Id, "MAIN", now);
        var user = NewUser(company.Id, branch.Id, suffix, now);
        var seeded = NewWaybill(company.Id, branch.Id, user.Id, suffix, quantity, now);
        seeded.Waybill.CurrencyId = currency.Id;

        db.Currencies.Add(currency);
        db.Companies.Add(company);
        db.Branches.Add(branch);
        db.Users.Add(user);
        db.Set<WaybillEntity>().Add(seeded.Waybill);
        db.Set<WaybillItemEntity>().Add(seeded.Item);
        await db.SaveChangesAsync();
        return seeded.Scope;
    }

    private static async Task<ShippingScope> SeedApprovedWaybillInCompanyAsync(
        TransportErpDbContext db, ShippingScope existing, string suffix, decimal quantity)
    {
        var now = DateTimeOffset.UtcNow;
        var branch = NewBranch(existing.CompanyId, $"B{Guid.NewGuid():N}"[..10], now);
        var user = NewUser(existing.CompanyId, branch.Id, suffix, now);
        var seeded = NewWaybill(existing.CompanyId, branch.Id, user.Id, suffix, quantity, now);
        seeded.Waybill.CurrencyId = await db.Companies.AsNoTracking()
            .Where(x => x.Id == existing.CompanyId)
            .Select(x => x.BaseCurrencyId).SingleAsync();

        db.Branches.Add(branch);
        db.Users.Add(user);
        db.Set<WaybillEntity>().Add(seeded.Waybill);
        db.Set<WaybillItemEntity>().Add(seeded.Item);
        await db.SaveChangesAsync();
        return seeded.Scope;
    }

    private static Branch NewBranch(Guid companyId, string code, DateTimeOffset now)
        => new()
        {
            Id = Guid.NewGuid(), CompanyId = companyId, Code = code,
            NameAr = "فرع TEAM-03", Timezone = "Asia/Aden", Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        };

    private static User NewUser(Guid companyId, Guid branchId, string suffix, DateTimeOffset now)
        => new()
        {
            Id = Guid.NewGuid(),
            UserName = $"team03-{Guid.NewGuid():N}",
            NormalizedUserName = $"TEAM03{suffix}{Guid.NewGuid():N}"[..24],
            DisplayName = "مستخدم TEAM-03",
            PasswordHash = "test-only",
            SecurityStamp = Guid.NewGuid().ToString("N"),
            AuthVersion = 1,
            Status = "ACTIVE",
            CompanyId = companyId, BranchId = branchId,
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        };

    private static SeededWaybill NewWaybill(
        Guid companyId, Guid branchId, Guid userId,
        string suffix, decimal quantity, DateTimeOffset now)
    {
        var origin = Guid.NewGuid();
        var destination = Guid.NewGuid();
        var waybill = new WaybillEntity
        {
            Id = Guid.NewGuid(), CompanyId = companyId, BranchId = branchId,
            DraftNo = $"D-{Guid.NewGuid():N}",
            WaybillNo = $"WB-T3-{Guid.NewGuid():N}"[..24],
            WaybillDateTime = now, ServiceType = "STANDARD", Priority = "NORMAL",
            OriginId = origin, DestinationId = destination,
            CurrencyId = Guid.Empty, ExchangeRate = 1m,
            FreightTotal = 100m, DiscountTotal = 0m,
            Status = "APPROVED", FinancialStatus = "UNPAID",
            CreateClientOperationId = $"seed-create-{Guid.NewGuid():N}",
            LastClientOperationId = $"seed-approve-{Guid.NewGuid():N}",
            Version = 1, CreatedAt = now, UpdatedAt = now
        };

        var item = new WaybillItemEntity
        {
            Id = Guid.NewGuid(), WaybillId = waybill.Id, LineNo = 1,
            ItemType = "GENERAL", Contents = "TEAM-03",
            Quantity = quantity, Pieces = (int)quantity,
            Weight = quantity * 10m, Length = 2m, Width = 3m, Height = 4m,
            RiskFlagsJson = "[]"
        };
        var scope = new ShippingScope(
            companyId, branchId, userId, waybill.Id, item.Id, origin, destination);
        return new SeededWaybill(scope, waybill, item);
    }

    private sealed record PreparedManifest(TripResponse Trip, ManifestResponse Manifest);
    private sealed record SeededWaybill(ShippingScope Scope, WaybillEntity Waybill, WaybillItemEntity Item);
    private sealed record ShippingScope(
        Guid CompanyId,
        Guid BranchId,
        Guid UserId,
        Guid WaybillId,
        Guid ItemId,
        Guid OriginId,
        Guid DestinationId);
}
