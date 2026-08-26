using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class Stage4SyncConflictResolutionPostgreSqlTests
{
    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Keep_server_atomically_rejects_original_resolves_conflict_and_writes_metadata_only_audit()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        ConflictScope scope;
        await using (var db = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            await db.Database.MigrateAsync();
            scope = await SeedConflictAsync(db, "KEEP");
        }

        await using (var db = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            var result = await Service(db).ResolveAsync(scope.ConflictId,
                KeepRequest("keep reviewed server value"), Context(scope));
            Assert.Equal("RESOLVED", result.ConflictStatus);
            Assert.Equal("REJECTED", result.OriginalOperationStatus);
            Assert.Equal("KEEP_SERVER", result.OriginalOperationErrorCode);
        }

        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        var conflict = await verify.ConflictCases.AsNoTracking().SingleAsync(x => x.Id == scope.ConflictId);
        var operation = await verify.SyncOperations.AsNoTracking().SingleAsync(x => x.Id == scope.OperationId);
        var audit = await verify.AuditEvents.AsNoTracking().SingleAsync(x =>
            x.Action == "SyncConflictResolved" && x.EntityId == scope.ConflictId);
        Assert.Equal(SyncConflictResolutionDecisions.KeepServerAndRejectLocal, conflict.Resolution);
        Assert.Equal(scope.UserId.ToString(), conflict.ResolvedBy);
        Assert.Equal("REJECTED", operation.Status);
        Assert.Equal("KEEP_SERVER", operation.ErrorCode);
        Assert.Equal("keep reviewed server value", audit.Reason);
        Assert.DoesNotContain("PayloadJson", audit.AfterJson ?? string.Empty, StringComparison.Ordinal);
        Assert.DoesNotContain("device-secret", audit.AfterJson ?? string.Empty, StringComparison.Ordinal);
        Assert.Equal(scope.OperationCorrelationId, audit.OperationCorrelationId);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Concurrent_resolvers_produce_one_transition_and_one_audit()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        ConflictScope scope;
        await using (var db = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            await db.Database.MigrateAsync();
            scope = await SeedConflictAsync(db, "RACE");
        }

        async Task<string> Resolve()
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            try
            {
                _ = await Service(db).ResolveAsync(scope.ConflictId, KeepRequest("race decision"), Context(scope));
                return "SUCCESS";
            }
            catch (SyncRuleException exception)
            {
                return exception.Code;
            }
        }

        var outcomes = await Task.WhenAll(Task.Run(Resolve), Task.Run(Resolve));
        Assert.Single(outcomes, x => x == "SUCCESS");
        Assert.Single(outcomes, x => x == "CONFLICT_ALREADY_RESOLVED");

        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        Assert.Equal(1, await verify.AuditEvents.CountAsync(x =>
            x.Action == "SyncConflictResolved" && x.EntityId == scope.ConflictId));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Scope_permission_reason_decision_and_repeat_resolution_fail_closed()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedConflictAsync(db, "DENY");

        var scopeError = await Assert.ThrowsAsync<SyncRuleException>(() => Service(db).ResolveAsync(
            scope.ConflictId, KeepRequest("reason"), Context(scope) with { BranchId = Guid.NewGuid() }));
        Assert.Equal("SCOPE_DENIED", scopeError.Code);

        db.ChangeTracker.Clear();
        var permissionError = await Assert.ThrowsAsync<SyncRuleException>(() => Service(db, allowOriginal: false)
            .ResolveAsync(scope.ConflictId, KeepRequest("reason"), Context(scope)));
        Assert.Equal("PERMISSION_DENIED", permissionError.Code);

        var decisionError = await Assert.ThrowsAsync<SyncRuleException>(() => Service(db).ResolveAsync(
            scope.ConflictId, new ResolveSyncConflictRequest("USE_DEVICE_OVERWRITE", "reason"), Context(scope)));
        Assert.Equal("RESOLUTION_INVALID", decisionError.Code);
        var reasonError = await Assert.ThrowsAsync<SyncRuleException>(() => Service(db).ResolveAsync(
            scope.ConflictId, KeepRequest(" "), Context(scope)));
        Assert.Equal("REASON_REQUIRED", reasonError.Code);

        _ = await Service(db).ResolveAsync(scope.ConflictId, KeepRequest("final reason"), Context(scope));
        db.ChangeTracker.Clear();
        var replay = await Assert.ThrowsAsync<SyncRuleException>(() => Service(db).ResolveAsync(
            scope.ConflictId, KeepRequest("final reason"), Context(scope)));
        Assert.Equal("CONFLICT_ALREADY_RESOLVED", replay.Code);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Reapply_validates_new_identity_and_shape_then_requires_fresh_proof_without_mutation()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedConflictAsync(db, "REAPPLY");
        var reusedIdentity = new ResolveSyncConflictRequest(
            SyncConflictResolutionDecisions.ReapplyAsNew,
            "must use fresh client identity",
            new SyncReapplyAsNewRequest(
                scope.ClientOperationId, scope.OperationCorrelationId, "UpdateWaybillDraft", "UPDATE", "Waybill",
                scope.EntityId, 2, DateTimeOffset.UtcNow, "{\"FreightTotal\":100}"));
        var identityError = await Assert.ThrowsAsync<SyncRuleException>(() =>
            Service(db).ResolveAsync(scope.ConflictId, reusedIdentity, Context(scope)));
        Assert.Equal("REAPPLY_ID_REUSE", identityError.Code);

        var crossEntity = new ResolveSyncConflictRequest(
            SyncConflictResolutionDecisions.ReapplyAsNew,
            "must preserve original entity",
            new SyncReapplyAsNewRequest(
                $"replacement-{Guid.NewGuid():N}", Guid.NewGuid(), "UpdateWaybillDraft", "UPDATE", "Waybill",
                Guid.NewGuid(), 2, DateTimeOffset.UtcNow, "{\"FreightTotal\":100}"));
        var scopeError = await Assert.ThrowsAsync<SyncRuleException>(() =>
            Service(db).ResolveAsync(scope.ConflictId, crossEntity, Context(scope)));
        Assert.Equal("REAPPLY_SCOPE_MISMATCH", scopeError.Code);

        var request = new ResolveSyncConflictRequest(
            SyncConflictResolutionDecisions.ReapplyAsNew,
            "reapply reviewed draft",
            new SyncReapplyAsNewRequest(
                $"replacement-{Guid.NewGuid():N}", Guid.NewGuid(), "UpdateWaybillDraft", "UPDATE", "Waybill",
                scope.EntityId, 2, DateTimeOffset.UtcNow, "{\"FreightTotal\":100}"));

        var error = await Assert.ThrowsAsync<SyncRuleException>(() =>
            Service(db).ResolveAsync(scope.ConflictId, request, Context(scope)));
        Assert.Equal("REAPPLY_PROOF_REQUIRED", error.Code);

        db.ChangeTracker.Clear();
        var conflict = await db.ConflictCases.AsNoTracking().SingleAsync(x => x.Id == scope.ConflictId);
        var operation = await db.SyncOperations.AsNoTracking().SingleAsync(x => x.Id == scope.OperationId);
        Assert.Equal("OPEN", conflict.Status);
        Assert.Equal("CONFLICT", operation.Status);
        Assert.Null(conflict.ReplacedByOperationId);
        Assert.Equal(1, await db.SyncOperations.CountAsync(x => x.CompanyId == scope.CompanyId));
        Assert.False(await db.AuditEvents.AnyAsync(x =>
            x.Action == "SyncConflictResolved" && x.EntityId == scope.ConflictId));
    }

    [Theory]
    [InlineData("PENDING")]
    [InlineData("SUSPENDED")]
    [InlineData("REVOKED")]
    [InlineData("STALE")]
    [InlineData("ASSIGNMENT_REMOVED")]
    [Trait("Category", "PostgreSQL")]
    public async Task Inactive_or_stale_device_binding_fails_closed_before_resolution(string state)
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedConflictAsync(db, $"BIND-{state}");
        var now = DateTimeOffset.UtcNow;

        if (state == "ASSIGNMENT_REMOVED")
        {
            var assignment = await db.RegisteredDeviceAssignments.SingleAsync(x => x.Id == scope.AssignmentId);
            assignment.Status = "REVOKED";
            assignment.RemovedAt = now;
            assignment.RemovedByUserId = scope.UserId;
        }
        else
        {
            var device = await db.RegisteredDevices.SingleAsync(x => x.Id == scope.RegisteredDeviceId);
            switch (state)
            {
                case "PENDING":
                    device.Status = "PENDING";
                    device.ApprovedAt = null;
                    device.ApprovedByUserId = null;
                    break;
                case "SUSPENDED":
                    device.Status = "SUSPENDED";
                    device.SuspendedAt = now;
                    break;
                case "REVOKED":
                    device.Status = "REVOKED";
                    device.RevokedAt = now;
                    break;
                case "STALE":
                    device.ApprovedAt = now.AddDays(-100);
                    device.LastSeenAt = now.AddDays(-91);
                    break;
            }
        }
        await db.SaveChangesAsync();
        db.ChangeTracker.Clear();

        var error = await Assert.ThrowsAsync<SyncRuleException>(() => Service(db).ResolveAsync(
            scope.ConflictId, KeepRequest("must fail before mutation"), Context(scope)));
        Assert.Equal("DEVICE_NOT_REGISTERED", error.Code);

        db.ChangeTracker.Clear();
        Assert.Equal("OPEN", (await db.ConflictCases.AsNoTracking()
            .SingleAsync(x => x.Id == scope.ConflictId)).Status);
        Assert.Equal("CONFLICT", (await db.SyncOperations.AsNoTracking()
            .SingleAsync(x => x.Id == scope.OperationId)).Status);
        Assert.False(await db.AuditEvents.AnyAsync(x =>
            x.Action == "SyncConflictResolved" && x.EntityId == scope.ConflictId));
    }

    private static ResolveSyncConflictRequest KeepRequest(string reason)
        => new(SyncConflictResolutionDecisions.KeepServerAndRejectLocal, reason);

    private static SyncConflictResolutionContext Context(ConflictScope scope)
        => new(scope.UserId, scope.CompanyId, scope.BranchId, scope.RegisteredDeviceId,
            1, scope.DeviceId, Guid.NewGuid());

    private static SyncConflictResolutionService Service(TransportErpDbContext db, bool allowOriginal = true)
        => new(db, new AuditEventService(db), new TestPermissionResolver(allowOriginal));

    private static async Task<ConflictScope> SeedConflictAsync(TransportErpDbContext db, string suffix)
    {
        var now = DateTimeOffset.UtcNow;
        var currency = new Currency
        {
            Id = Guid.NewGuid(), Code = await PostgreSqlTestCurrencyCodeAllocator.NextAsync(db),
            NameAr = "عملة تعارض", MinorUnit = 2, IsBase = true, Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var company = new Company
        {
            Id = Guid.NewGuid(), Code = $"CF-{suffix}-{Guid.NewGuid():N}"[..20], LegalNameAr = "شركة تعارض",
            BaseCurrencyId = currency.Id, DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var branch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = $"B-{Guid.NewGuid():N}"[..12], NameAr = "فرع تعارض",
            Timezone = "Asia/Riyadh", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var userName = $"conflict-{Guid.NewGuid():N}";
        var user = new User
        {
            Id = Guid.NewGuid(), UserName = userName, NormalizedUserName = userName.ToUpperInvariant(),
            DisplayName = "Conflict resolver", PasswordHash = "test", SecurityStamp = Guid.NewGuid().ToString("N"),
            AuthVersion = 1, Status = "ACTIVE", CompanyId = company.Id, BranchId = branch.Id,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.AddRange(currency, company, branch, user);
        await db.SaveChangesAsync();

        var deviceId = $"conflict-device-{Guid.NewGuid():N}";
        var device = new RegisteredDevice
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, DeviceId = deviceId, DisplayName = "Conflict device",
            Platform = "TEST", AppVersion = "1", RegistrationRequestId = $"req-{Guid.NewGuid():N}",
            CredentialHash = new string('d', 64), CredentialVersion = 1, Status = "ACTIVE",
            RegisteredByUserId = user.Id, ApprovedByUserId = user.Id, ApprovedAt = now, LastSeenAt = now,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var assignment = new RegisteredDeviceAssignment
        {
            Id = Guid.NewGuid(), RegisteredDeviceId = device.Id, UserId = user.Id, CompanyId = company.Id,
            BranchId = branch.Id, Status = "ACTIVE", AssignedByUserId = user.Id, AssignedAt = now,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.AddRange(device, assignment);
        await db.SaveChangesAsync();

        var nonce = new SyncProofNonce
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, RegisteredDeviceId = device.Id, DeviceId = deviceId,
            ProofKeyVersion = 1, NonceHash = RandomNumberGenerator.GetBytes(32), IssuedAt = now,
            ExpiresAt = now.AddMinutes(5)
        };
        db.SyncProofNonces.Add(nonce);
        await db.SaveChangesAsync();
        var proof = new SyncProofReplay
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, RegisteredDeviceId = device.Id, DeviceId = deviceId,
            DeviceAssignmentId = assignment.Id, UserId = user.Id, BranchId = branch.Id, ProofKeyVersion = 1,
            ProofKeyThumbprint = new string('t', 43), JtiHash = RandomNumberGenerator.GetBytes(32),
            HtuHash = RandomNumberGenerator.GetBytes(32), HttpMethod = "POST", NonceRecordId = nonce.Id,
            IssuedAt = now, FirstSeenAt = now, ExpiresAt = now.AddMinutes(4), AttemptCorrelationId = Guid.NewGuid()
        };
        db.SyncProofReplays.Add(proof);
        await db.SaveChangesAsync();

        var entityId = Guid.NewGuid();
        var operationCorrelationId = Guid.NewGuid();
        var operation = new SyncOperation
        {
            Id = Guid.NewGuid(), DeviceId = deviceId, UserId = user.Id, CompanyId = company.Id, BranchId = branch.Id,
            OperationType = "UPDATE", EntityType = "Waybill", EntityId = entityId,
            ClientOperationId = $"conflict-{Guid.NewGuid():N}", PayloadJson = "{\"device-secret\":true}",
            PayloadHash = new string('a', 64), ClientOccurredAt = now, ServerReceivedAt = now, BaseVersion = 1,
            Status = "CONFLICT", RetryCount = 0, RegisteredDeviceId = device.Id,
            RegisteredDeviceCredentialVersion = 1, ActionCode = "UpdateWaybillDraft", ProtocolVersion = "sync-v1",
            OperationCorrelationId = operationCorrelationId, RequestFingerprintVersion = "fp-v1",
            RequestFingerprintHash = RandomNumberGenerator.GetBytes(32), ProofKeyVersion = 1,
            ProofKeyThumbprint = new string('t', 43), AcceptedProofReplayId = proof.Id,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.SyncOperations.Add(operation);
        await db.SaveChangesAsync();
        var conflict = new ConflictCase
        {
            Id = Guid.NewGuid(), SyncOperationId = operation.Id, CompanyId = company.Id, BranchId = branch.Id,
            BaseVersion = 1, DeviceSnapshot = "{\"redacted\":true}", ServerSnapshot = "{\"version\":2}",
            ConflictReason = "BASE_VERSION_CONFLICT", Status = "OPEN", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.ConflictCases.Add(conflict);
        await db.SaveChangesAsync();
        return new(company.Id, branch.Id, user.Id, device.Id, assignment.Id, deviceId, operation.Id, conflict.Id,
            entityId, operation.ClientOperationId, operationCorrelationId);
    }

    private sealed class TestPermissionResolver(bool allowOriginal) : IEffectivePermissionResolver
    {
        public Task<bool> HasPermissionAsync(Guid userId, Guid companyId, Guid? branchId, string permissionCode,
            CancellationToken cancellationToken = default)
            => Task.FromResult(permissionCode == SyncConflictPermissionCodes.Resolve ||
                (allowOriginal && permissionCode == "waybill.edit"));
    }

    private sealed record ConflictScope(
        Guid CompanyId,
        Guid BranchId,
        Guid UserId,
        Guid RegisteredDeviceId,
        Guid AssignmentId,
        string DeviceId,
        Guid OperationId,
        Guid ConflictId,
        Guid EntityId,
        string ClientOperationId,
        Guid OperationCorrelationId);
}
