using System.Data;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using TransportERP.Api.Security;
using TransportERP.Contracts.Identity;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Identity;

public sealed class RegisteredDeviceException(string code) : InvalidOperationException(code)
{
    public string Code { get; } = code;
}

public sealed record TrustedDeviceBinding(Guid RegisteredDeviceId, int CredentialVersion)
{
    internal bool LastSeenAuditPending { get; init; }
}

internal sealed record LoginDeviceBindingDecision(bool IsRegistered, TrustedDeviceBinding? Binding);

public sealed class RegisteredDeviceService(TransportErpDbContext db, AuditEventService audit)
{
    public static readonly TimeSpan InactivityLimit = TimeSpan.FromDays(90);
    public static readonly TimeSpan LastSeenWriteInterval = TimeSpan.FromMinutes(15);

    public async Task<RegisteredDeviceResponse> RegisterAsync(CurrentSecurityContext current,
        RegisterDeviceRequest request, Guid correlationId, CancellationToken ct)
    {
        var deviceId = Normalize(request.DeviceId, 120, "DEVICE_ID_INVALID");
        var displayName = Normalize(request.DisplayName, 200, "DISPLAY_NAME_INVALID");
        var platform = Normalize(request.Platform, 40, "PLATFORM_INVALID");
        var appVersion = Normalize(request.AppVersion, 40, "APP_VERSION_INVALID");
        var deviceModel = Optional(request.DeviceModel, 120, "DEVICE_MODEL_INVALID");
        var osVersion = Optional(request.OsVersion, 80, "OS_VERSION_INVALID");
        var requestId = Normalize(request.RegistrationRequestId, 120, "REGISTRATION_REQUEST_ID_INVALID");
        var hash = HashCredential(request.Credential);
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
        try
        {
            if (db.Database.IsNpgsql())
            {
                var lockKeys = new[] { $"request|{current.CompanyId}|{requestId}", $"device|{current.CompanyId}|{deviceId}" }
                    .OrderBy(x => x, StringComparer.Ordinal).ToArray();
                foreach (var lockKey in lockKeys)
                    await db.Database.ExecuteSqlInterpolatedAsync(
                        $"SELECT pg_advisory_xact_lock(hashtextextended({lockKey}, 0))", ct);
            }
            var candidates = await db.RegisteredDevices.Where(x => x.CompanyId == current.CompanyId &&
                (x.RegistrationRequestId == requestId || x.DeviceId == deviceId)).ToListAsync(ct);
            if (candidates.Count > 0)
            {
                var replay = candidates.SingleOrDefault(x => x.RegistrationRequestId == requestId &&
                    x.DeviceId == deviceId && FixedEquals(x.CredentialHash, hash) &&
                    x.DisplayName == displayName && x.Platform == platform && x.AppVersion == appVersion &&
                    x.DeviceModel == deviceModel && x.OsVersion == osVersion);
                if (replay is null || candidates.Count != 1)
                    throw new RegisteredDeviceException("DEVICE_REGISTRATION_CONFLICT");
                var replayRevokedUnboundSessions = await RevokeUnboundSessionsForRegistrationAsync(
                    current.CompanyId, deviceId, DateTimeOffset.UtcNow, ct);
                await db.SaveChangesAsync(ct);
                await AppendRegistrationSessionRevocationAuditAsync(
                    replay, current, correlationId, replayRevokedUnboundSessions, ct);
                await transaction.CommitAsync(ct);
                return ToResponse(replay);
            }

            var now = DateTimeOffset.UtcNow;
            var device = NewEntity(new RegisteredDevice
            {
                CompanyId = current.CompanyId, DeviceId = deviceId, DisplayName = displayName,
                Platform = platform, AppVersion = appVersion, DeviceModel = deviceModel, OsVersion = osVersion,
                RegistrationRequestId = requestId, CredentialHash = hash, CredentialVersion = 1,
                Status = "PENDING", RegisteredByUserId = current.UserId
            }, now);
            db.RegisteredDevices.Add(device);
            // The device advisory lock is already held. Revoke every still-unbound session for
            // this exact company/device before publishing the registration, so an online-only
            // session cannot be downgraded into access to the newly trusted device identity.
            var revokedUnboundSessions = await RevokeUnboundSessionsForRegistrationAsync(
                current.CompanyId, deviceId, now, ct);
            await db.SaveChangesAsync(ct);
            await AppendAuditAsync("RegisteredDeviceCreated", device, current, correlationId, "PENDING", ct);
            await AppendRegistrationSessionRevocationAuditAsync(
                device, current, correlationId, revokedUnboundSessions, ct);
            await transaction.CommitAsync(ct);
            return ToResponse(device);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            db.ChangeTracker.Clear();
            throw;
        }
    }

    public async Task<IReadOnlyList<RegisteredDeviceResponse>> ListAsync(CurrentSecurityContext current,
        CancellationToken ct) => (await db.RegisteredDevices.AsNoTracking()
        .Where(x => x.CompanyId == current.CompanyId).OrderBy(x => x.DeviceId).ToListAsync(ct))
        .Select(ToResponse).ToArray();

    public async Task<RegisteredDeviceResponse?> CurrentAsync(CurrentSecurityContext current, CancellationToken ct)
    {
        if (!current.IsLocalSession || current.BranchId is null ||
            current.RegisteredDeviceId is null || current.DeviceCredentialVersion is null) return null;
        var now = DateTimeOffset.UtcNow;
        var device = await (from candidate in db.RegisteredDevices.AsNoTracking()
            join assignment in db.RegisteredDeviceAssignments.AsNoTracking()
                on candidate.Id equals assignment.RegisteredDeviceId
            where candidate.Id == current.RegisteredDeviceId && candidate.CompanyId == current.CompanyId &&
                  candidate.DeviceId == current.DeviceId && candidate.Status == "ACTIVE" &&
                  candidate.CredentialVersion == current.DeviceCredentialVersion &&
                  (candidate.ExpiresAt == null || candidate.ExpiresAt > now) &&
                  (candidate.LastSeenAt ?? candidate.ApprovedAt ?? candidate.CreatedAt) > now - InactivityLimit &&
                  assignment.CompanyId == current.CompanyId && assignment.UserId == current.UserId &&
                  assignment.BranchId == current.BranchId && assignment.Status == "ACTIVE"
            select candidate).SingleOrDefaultAsync(ct);
        return device is null ? null : ToResponse(device);
    }

    public Task<RegisteredDeviceResponse> ApproveAsync(Guid id, CurrentSecurityContext current,
        Guid correlationId, CancellationToken ct) => ChangeStatusAsync(id, current, "ACTIVE",
        ["PENDING"], "RegisteredDeviceApproved", correlationId, ct);

    public Task<RegisteredDeviceResponse> SuspendAsync(Guid id, CurrentSecurityContext current,
        Guid correlationId, CancellationToken ct) => ChangeStatusAsync(id, current, "SUSPENDED",
        ["ACTIVE"], "RegisteredDeviceSuspended", correlationId, ct);

    public Task<RegisteredDeviceResponse> ReactivateAsync(Guid id, CurrentSecurityContext current,
        Guid correlationId, CancellationToken ct) => ChangeStatusAsync(id, current, "ACTIVE",
        ["SUSPENDED", "EXPIRED"], "RegisteredDeviceReactivated", correlationId, ct);

    public Task<RegisteredDeviceResponse> RevokeAsync(Guid id, CurrentSecurityContext current,
        Guid correlationId, CancellationToken ct) => ChangeStatusAsync(id, current, "REVOKED",
        ["PENDING", "ACTIVE", "SUSPENDED", "EXPIRED"], "RegisteredDeviceRevoked", correlationId, ct);

    public async Task<RegisteredDeviceAssignmentResponse> AddAssignmentAsync(Guid deviceId,
        AddDeviceAssignmentRequest request, CurrentSecurityContext current, Guid correlationId, CancellationToken ct)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
        try
        {
            var device = await LockDeviceAsync(deviceId, current.CompanyId, ct);
            if (device.Status == "REVOKED") throw new RegisteredDeviceException("DEVICE_REVOKED");
            var user = await db.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == request.UserId &&
                x.CompanyId == current.CompanyId && x.Status == "ACTIVE", ct);
            if (user is null || (user.BranchId.HasValue && user.BranchId != request.BranchId))
                throw new RegisteredDeviceException("ASSIGNMENT_SCOPE_INVALID");
            if (!await db.Branches.AsNoTracking().AnyAsync(x => x.Id == request.BranchId &&
                    x.CompanyId == current.CompanyId && x.Status == "ACTIVE", ct))
                throw new RegisteredDeviceException("ASSIGNMENT_SCOPE_INVALID");
            var existing = await db.RegisteredDeviceAssignments.SingleOrDefaultAsync(x =>
                x.RegisteredDeviceId == device.Id && x.UserId == request.UserId &&
                x.BranchId == request.BranchId && x.Status == "ACTIVE", ct);
            if (existing is not null)
            {
                await transaction.CommitAsync(ct);
                return ToResponse(existing);
            }
            var now = DateTimeOffset.UtcNow;
            var assignment = NewEntity(new RegisteredDeviceAssignment
            {
                RegisteredDeviceId = device.Id, UserId = request.UserId, CompanyId = current.CompanyId,
                BranchId = request.BranchId, Status = "ACTIVE", AssignedByUserId = current.UserId,
                AssignedAt = now
            }, now);
            db.RegisteredDeviceAssignments.Add(assignment);
            await db.SaveChangesAsync(ct);
            await AppendAuditAsync("RegisteredDeviceAssignmentAdded", device, current, correlationId,
                $"AssignmentId={assignment.Id}", ct);
            await transaction.CommitAsync(ct);
            return ToResponse(assignment);
        }
        catch
        {
            await transaction.RollbackAsync(ct); db.ChangeTracker.Clear(); throw;
        }
    }

    public async Task RemoveAssignmentAsync(Guid deviceId, Guid assignmentId, CurrentSecurityContext current,
        Guid correlationId, CancellationToken ct)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
        try
        {
            var device = await LockDeviceAsync(deviceId, current.CompanyId, ct);
            var assignment = await db.RegisteredDeviceAssignments.SingleOrDefaultAsync(x => x.Id == assignmentId &&
                x.RegisteredDeviceId == device.Id && x.CompanyId == current.CompanyId, ct)
                ?? throw new RegisteredDeviceException("ASSIGNMENT_NOT_FOUND");
            if (assignment.Status != "REVOKED")
            {
                var now = DateTimeOffset.UtcNow;
                assignment.Status = "REVOKED"; assignment.RemovedAt = now;
                assignment.RemovedByUserId = current.UserId; Touch(assignment, now);
                await RevokeSessionsAsync(device.Id, "DEVICE_ASSIGNMENT_REMOVED", now, ct, assignment.UserId, assignment.BranchId);
                await db.SaveChangesAsync(ct);
                await AppendAuditAsync("RegisteredDeviceAssignmentRemoved", device, current, correlationId,
                    $"AssignmentId={assignment.Id}", ct);
            }
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct); db.ChangeTracker.Clear(); throw;
        }
    }

    public async Task<RegisteredDeviceResponse> RotateCredentialAsync(Guid id, RotateDeviceCredentialRequest request,
        CurrentSecurityContext current, Guid correlationId, CancellationToken ct)
    {
        var newHash = HashCredential(request.Credential);
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
        try
        {
            var device = await LockDeviceAsync(id, current.CompanyId, ct);
            if (device.Status == "REVOKED") throw new RegisteredDeviceException("DEVICE_REVOKED");
            if (device.CredentialVersion != request.ExpectedCredentialVersion)
                throw new RegisteredDeviceException("DEVICE_VERSION_CONFLICT");
            var now = DateTimeOffset.UtcNow;
            device.CredentialHash = newHash; device.CredentialVersion++;
            Touch(device, now);
            await RevokeSessionsAsync(device.Id, "DEVICE_CREDENTIAL_ROTATED", now, ct);
            await db.SaveChangesAsync(ct);
            await AppendAuditAsync("RegisteredDeviceCredentialRotated", device, current, correlationId,
                $"CredentialVersion={device.CredentialVersion}", ct);
            await transaction.CommitAsync(ct);
            return ToResponse(device);
        }
        catch
        {
            await transaction.RollbackAsync(ct); db.ChangeTracker.Clear(); throw;
        }
    }

    public Task<TrustedDeviceBinding?> ValidateBindingAsync(Guid userId, Guid companyId, Guid? branchId,
        string deviceId, string? credential, bool updateLastSeen, Guid correlationId, CancellationToken ct)
        => ValidateBindingCoreAsync(userId, companyId, branchId, deviceId, credential, updateLastSeen,
            correlationId, ct, deferLastSeenAudit: false);

    internal async Task<LoginDeviceBindingDecision> ResolveLoginBindingAsync(Guid userId, Guid companyId,
        Guid? branchId, string deviceId, string? credential, Guid correlationId, CancellationToken ct)
    {
        var normalizedDevice = IdentitySessionService.NormalizeDevice(deviceId);
        if (normalizedDevice is null) return new(false, null);
        if (db.Database.CurrentTransaction is null)
            throw new InvalidOperationException("Login device binding requires a caller-owned transaction.");

        // Registration takes the same key. This closes the absent-row race where a device could be
        // registered between an unbound login's lookup and session creation.
        if (db.Database.IsNpgsql())
        {
            var lockKey = $"device|{companyId}|{normalizedDevice}";
            await db.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT pg_advisory_xact_lock(hashtextextended({lockKey}, 0))", ct);
        }

        var isRegistered = await db.RegisteredDevices.AsNoTracking().AnyAsync(x =>
            x.CompanyId == companyId && x.DeviceId == normalizedDevice, ct);
        if (!isRegistered) return new(false, null);

        var binding = await ValidateBindingCoreAsync(userId, companyId, branchId, normalizedDevice,
            credential, updateLastSeen: true, correlationId, ct, deferLastSeenAudit: false);
        return new(true, binding);
    }

    internal Task<TrustedDeviceBinding?> ValidateBindingForRefreshAsync(Guid userId, Guid companyId, Guid? branchId,
        string deviceId, string? credential, Guid correlationId, CancellationToken ct)
        => ValidateBindingCoreAsync(userId, companyId, branchId, deviceId, credential, updateLastSeen: true,
            correlationId, ct, deferLastSeenAudit: true);

    private async Task<TrustedDeviceBinding?> ValidateBindingCoreAsync(Guid userId, Guid companyId, Guid? branchId,
        string deviceId, string? credential, bool updateLastSeen, Guid correlationId, CancellationToken ct,
        bool deferLastSeenAudit)
    {
        if (!branchId.HasValue || !TryHashCredential(credential, out var hash)) return null;
        var normalizedDevice = IdentitySessionService.NormalizeDevice(deviceId);
        if (normalizedDevice is null) return null;
        var ownsTransaction = db.Database.CurrentTransaction is null;
        if (deferLastSeenAudit && ownsTransaction)
            throw new InvalidOperationException("Deferred LastSeen audit requires a caller-owned transaction.");
        await using var transaction = ownsTransaction
            ? await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct) : null;
        try
        {
        var tracked = db.ChangeTracker.Entries<RegisteredDevice>()
            .SingleOrDefault(x => x.Entity.CompanyId == companyId && x.Entity.DeviceId == normalizedDevice);
        if (tracked is not null) tracked.State = EntityState.Detached;
        var device = await db.RegisteredDevices.FromSqlInterpolated(
            $"SELECT * FROM transport_erp.registered_devices WHERE \"CompanyId\"={companyId} AND \"DeviceId\"={normalizedDevice} FOR UPDATE")
            .SingleOrDefaultAsync(ct);
        var now = DateTimeOffset.UtcNow;
        if (device is null || device.Status != "ACTIVE" || !FixedEquals(device.CredentialHash, hash))
        {
            if (transaction is not null) await transaction.CommitAsync(ct);
            return null;
        }
        if (IsInactive(device, now))
        {
            device.Status = "EXPIRED"; device.ExpiresAt ??= now; Touch(device, now);
            await RevokeSessionsAsync(device.Id, "DEVICE_EXPIRED", now, ct);
            await db.SaveChangesAsync(ct);
            await audit.AppendAuditEventAsync(new AuditEventDraft("RegisteredDeviceExpired", "SUCCESS",
                nameof(RegisteredDevice), device.Id, userId, companyId, branchId, correlationId,
                device.DeviceId, Reason: "INACTIVITY_LIMIT"), ct);
            if (transaction is not null) await transaction.CommitAsync(ct);
            return null;
        }
        if (!await db.RegisteredDeviceAssignments.AsNoTracking().AnyAsync(x =>
                x.RegisteredDeviceId == device.Id && x.CompanyId == companyId && x.UserId == userId &&
                x.BranchId == branchId && x.Status == "ACTIVE", ct))
        {
            if (transaction is not null) await transaction.CommitAsync(ct);
            return null;
        }
        var lastSeenAuditPending = false;
        if (updateLastSeen && (!device.LastSeenAt.HasValue || now - device.LastSeenAt.Value >= LastSeenWriteInterval))
        {
            device.LastSeenAt = now; Touch(device, now);
            await db.SaveChangesAsync(ct);
            lastSeenAuditPending = deferLastSeenAudit;
            if (!deferLastSeenAudit)
                await AppendSeenAuditAsync(device.Id, device.CredentialVersion, userId, companyId, branchId,
                    correlationId, device.DeviceId, ct);
        }
        if (transaction is not null) await transaction.CommitAsync(ct);
        return new(device.Id, device.CredentialVersion) { LastSeenAuditPending = lastSeenAuditPending };
        }
        catch
        {
            if (transaction is not null)
            {
                await transaction.RollbackAsync(ct);
                db.ChangeTracker.Clear();
            }
            throw;
        }
    }

    internal async Task AppendDeferredLastSeenAuditAsync(TrustedDeviceBinding binding, Guid userId, Guid companyId,
        Guid? branchId, Guid correlationId, string deviceId, CancellationToken ct)
    {
        if (!binding.LastSeenAuditPending) return;
        if (db.Database.CurrentTransaction is null)
            throw new InvalidOperationException("Deferred LastSeen audit must remain in the identity transaction.");
        await AppendSeenAuditAsync(binding.RegisteredDeviceId, binding.CredentialVersion, userId, companyId,
            branchId, correlationId, deviceId, ct);
    }

    private async Task<RegisteredDeviceResponse> ChangeStatusAsync(Guid id, CurrentSecurityContext current,
        string newStatus, string[] allowed, string action, Guid correlationId, CancellationToken ct)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
        try
        {
            var device = await LockDeviceAsync(id, current.CompanyId, ct);
            if (device.Status == "REVOKED") throw new RegisteredDeviceException("DEVICE_REVOKED");
            if (device.Status == newStatus) { await transaction.CommitAsync(ct); return ToResponse(device); }
            if (!allowed.Contains(device.Status, StringComparer.Ordinal))
                throw new RegisteredDeviceException("DEVICE_STATE_INVALID");
            var now = DateTimeOffset.UtcNow;
            device.Status = newStatus;
            if (newStatus == "ACTIVE")
            {
                device.ApprovedAt ??= now; device.ApprovedByUserId ??= current.UserId;
                device.SuspendedAt = null; device.ExpiresAt = null; device.LastSeenAt = now;
            }
            if (newStatus == "SUSPENDED") device.SuspendedAt = now;
            if (newStatus == "REVOKED") device.RevokedAt = now;
            Touch(device, now);
            if (newStatus is "SUSPENDED" or "REVOKED")
                await RevokeSessionsAsync(device.Id, $"DEVICE_{newStatus}", now, ct);
            await db.SaveChangesAsync(ct);
            await AppendAuditAsync(action, device, current, correlationId, newStatus, ct);
            await transaction.CommitAsync(ct);
            return ToResponse(device);
        }
        catch
        {
            await transaction.RollbackAsync(ct); db.ChangeTracker.Clear(); throw;
        }
    }

    private async Task<RegisteredDevice> LockDeviceAsync(Guid id, Guid companyId, CancellationToken ct)
    {
        var tracked = db.ChangeTracker.Entries<RegisteredDevice>().SingleOrDefault(x => x.Entity.Id == id);
        if (tracked is not null) tracked.State = EntityState.Detached;
        return await db.RegisteredDevices.FromSqlInterpolated(
            $"SELECT * FROM transport_erp.registered_devices WHERE \"Id\"={id} AND \"CompanyId\"={companyId} FOR UPDATE")
            .SingleOrDefaultAsync(ct) ?? throw new RegisteredDeviceException("DEVICE_NOT_FOUND");
    }

    private async Task RevokeSessionsAsync(Guid deviceId, string reason, DateTimeOffset now, CancellationToken ct,
        Guid? userId = null, Guid? branchId = null)
    {
        // Every caller owns the RegisteredDevice row. Explicit row locks make multi-session
        // revocation deterministic before the caller appends to AuditStreamHead.
        List<AuthSession> sessions;
        if (userId.HasValue && branchId.HasValue)
            sessions = await db.AuthSessions.FromSqlInterpolated($$"""
                SELECT * FROM transport_erp.auth_sessions
                WHERE "RegisteredDeviceId"={{deviceId}} AND "RevokedAt" IS NULL
                  AND "UserId"={{userId.Value}} AND "BranchId"={{branchId.Value}}
                ORDER BY "Id" FOR UPDATE
                """).ToListAsync(ct);
        else if (userId.HasValue)
            sessions = await db.AuthSessions.FromSqlInterpolated($$"""
                SELECT * FROM transport_erp.auth_sessions
                WHERE "RegisteredDeviceId"={{deviceId}} AND "RevokedAt" IS NULL
                  AND "UserId"={{userId.Value}}
                ORDER BY "Id" FOR UPDATE
                """).ToListAsync(ct);
        else if (branchId.HasValue)
            sessions = await db.AuthSessions.FromSqlInterpolated($$"""
                SELECT * FROM transport_erp.auth_sessions
                WHERE "RegisteredDeviceId"={{deviceId}} AND "RevokedAt" IS NULL
                  AND "BranchId"={{branchId.Value}}
                ORDER BY "Id" FOR UPDATE
                """).ToListAsync(ct);
        else
            sessions = await db.AuthSessions.FromSqlInterpolated($$"""
                SELECT * FROM transport_erp.auth_sessions
                WHERE "RegisteredDeviceId"={{deviceId}} AND "RevokedAt" IS NULL
                ORDER BY "Id" FOR UPDATE
                """).ToListAsync(ct);

        foreach (var session in sessions)
        {
            session.RevokedAt = now; session.RevokeReason = reason;
            session.UpdatedAt = now; session.RowVersion = RandomNumberGenerator.GetBytes(16);
        }
    }

    private async Task<int> RevokeUnboundSessionsForRegistrationAsync(
        Guid companyId,
        string deviceId,
        DateTimeOffset now,
        CancellationToken ct)
    {
        var sessions = db.Database.IsNpgsql()
            ? await db.AuthSessions.FromSqlInterpolated($$"""
                SELECT * FROM transport_erp.auth_sessions
                WHERE "CompanyId"={{companyId}} AND "DeviceId"={{deviceId}}
                  AND "RegisteredDeviceId" IS NULL AND "RevokedAt" IS NULL
                ORDER BY "Id" FOR UPDATE
                """).ToListAsync(ct)
            : await db.AuthSessions.Where(session =>
                session.CompanyId == companyId && session.DeviceId == deviceId &&
                session.RegisteredDeviceId == null && session.RevokedAt == null)
                .OrderBy(session => session.Id)
                .ToListAsync(ct);

        foreach (var session in sessions)
        {
            session.RevokedAt = now;
            session.RevokeReason = "DEVICE_REGISTERED_REAUTHENTICATION_REQUIRED";
            session.UpdatedAt = now;
            session.RowVersion = RandomNumberGenerator.GetBytes(16);
        }
        return sessions.Count;
    }

    private Task AppendRegistrationSessionRevocationAuditAsync(
        RegisteredDevice device,
        CurrentSecurityContext actor,
        Guid correlationId,
        int revokedCount,
        CancellationToken ct)
        => AppendAuditAsync(
            "RegisteredDeviceUnboundSessionsRevoked",
            device,
            actor,
            correlationId,
            $"Count={revokedCount};Reason=DEVICE_REGISTERED_REAUTHENTICATION_REQUIRED",
            ct);

    private Task AppendSeenAuditAsync(Guid deviceId, int credentialVersion, Guid userId, Guid companyId,
        Guid? branchId, Guid correlationId, string textualDeviceId, CancellationToken ct)
        => audit.AppendAuditEventAsync(new AuditEventDraft("RegisteredDeviceSeen", "SUCCESS",
            nameof(RegisteredDevice), deviceId, userId, companyId, branchId, correlationId,
            textualDeviceId, Reason: $"CredentialVersion={credentialVersion}"), ct);

    private Task AppendAuditAsync(string action, RegisteredDevice device, CurrentSecurityContext actor,
        Guid correlationId, string reason, CancellationToken ct)
        => audit.AppendAuditEventAsync(new AuditEventDraft(action, "SUCCESS", nameof(RegisteredDevice),
            device.Id, actor.UserId, device.CompanyId, actor.BranchId, correlationId, device.DeviceId,
            Reason: reason), ct);

    public static string HashCredential(string? credential)
        => TryHashCredential(credential, out var hash) ? hash : throw new RegisteredDeviceException("DEVICE_CREDENTIAL_INVALID");

    public static bool TryHashCredential(string? credential, out string hash)
    {
        hash = string.Empty;
        if (string.IsNullOrWhiteSpace(credential) || credential.Length > 128) return false;
        try
        {
            var bytes = Convert.FromBase64String(credential.Trim());
            if (bytes.Length != 32) return false;
            hash = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
            return true;
        }
        catch (FormatException) { return false; }
    }

    private static bool IsInactive(RegisteredDevice device, DateTimeOffset now)
        => (device.ExpiresAt.HasValue && device.ExpiresAt.Value <= now) ||
           now - (device.LastSeenAt ?? device.ApprovedAt ?? device.CreatedAt) >= InactivityLimit;
    private static bool FixedEquals(string left, string right)
        => CryptographicOperations.FixedTimeEquals(System.Text.Encoding.ASCII.GetBytes(left), System.Text.Encoding.ASCII.GetBytes(right));
    private static string Normalize(string? value, int max, string code)
    {
        var normalized = value?.Trim();
        if (string.IsNullOrWhiteSpace(normalized) || normalized.Length > max) throw new RegisteredDeviceException(code);
        return normalized;
    }
    private static string? Optional(string? value, int max, string code)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        var normalized = value.Trim();
        if (normalized.Length > max) throw new RegisteredDeviceException(code);
        return normalized;
    }
    private static T NewEntity<T>(T entity, DateTimeOffset now) where T : P1Entity
    { entity.Id = Guid.NewGuid(); entity.CreatedAt = now; entity.UpdatedAt = now; entity.RowVersion = RandomNumberGenerator.GetBytes(16); return entity; }
    private static void Touch(P1Entity entity, DateTimeOffset now)
    { entity.UpdatedAt = now; entity.RowVersion = RandomNumberGenerator.GetBytes(16); }
    private static RegisteredDeviceResponse ToResponse(RegisteredDevice x) => new(x.Id, x.CompanyId, x.DeviceId,
        x.DisplayName, x.Platform, x.AppVersion, x.DeviceModel, x.OsVersion, x.CredentialVersion,
        x.Status, x.LastSeenAt, x.ExpiresAt, x.CreatedAt, x.UpdatedAt);
    private static RegisteredDeviceAssignmentResponse ToResponse(RegisteredDeviceAssignment x) => new(x.Id,
        x.RegisteredDeviceId, x.UserId, x.CompanyId, x.BranchId, x.Status, x.AssignedAt, x.RemovedAt);
}

public sealed class OfflineSyncPolicyService
{
    public Task<bool> IsEnabledAsync(Guid companyId, CancellationToken ct)
        => Task.FromResult(false); // Stage 3 stays hard-disabled until request freshness/PoP exists in Stage 4.
}
