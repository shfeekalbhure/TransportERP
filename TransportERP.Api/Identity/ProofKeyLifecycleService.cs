using System.Data;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TransportERP.Api.Security;
using TransportERP.Contracts.Identity;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Identity;

public sealed class ProofKeyLifecycleException(string code) : InvalidOperationException(code)
{
    public string Code { get; } = code;
}

public sealed class ProofKeyLifecycleService(
    TransportErpDbContext db,
    AuditEventService audit,
    ProofKeyChangeProofValidator proofValidator)
{
    public static readonly TimeSpan ChallengeLifetime = TimeSpan.FromMinutes(5);

    public async Task<ProofKeyChallengeResponse> CreateChallengeAsync(
        Guid deviceId,
        CurrentSecurityContext current,
        CreateProofKeyChallengeRequest request,
        Guid correlationId,
        CancellationToken ct)
    {
        RequireLocalAuthority(current);
        ValidateIdentity(request.ChangeRequestId, "CHANGE_REQUEST_ID_INVALID");
        var changeType = ValidateChangeType(request.ChangeType);
        var publicKey = proofValidator.ReadPublicKey(request.NewPublicJwk);

        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
        try
        {
            var device = await LockDeviceAsync(deviceId, current.CompanyId, ct);
            await LockActorScopeAsync(current.UserId, ct);

            var replay = await db.RegisteredDeviceProofKeyChallenges.SingleOrDefaultAsync(x =>
                x.RegisteredDeviceId == device.Id && x.ChangeRequestId == request.ChangeRequestId, ct);
            if (replay is not null)
            {
                var response = MatchChallengeReplay(replay, changeType, request.ExpectedProofKeyVersion,
                    publicKey.Thumbprint);
                await transaction.CommitAsync(ct);
                return response;
            }
            ValidateDeviceState(device, changeType, request.ExpectedProofKeyVersion);

            var rawChallenge = RandomNumberGenerator.GetBytes(32);
            var now = DateTimeOffset.UtcNow;
            var challenge = new RegisteredDeviceProofKeyChallenge
            {
                Id = Guid.NewGuid(),
                CompanyId = device.CompanyId,
                RegisteredDeviceId = device.Id,
                DeviceId = device.DeviceId,
                ChangeRequestId = request.ChangeRequestId,
                ChangeType = changeType,
                ExpectedProofKeyVersion = request.ExpectedProofKeyVersion,
                NewProofKeyThumbprint = publicKey.Thumbprint,
                ChallengeHash = SHA256.HashData(rawChallenge),
                IssuedAt = now,
                ExpiresAt = now + ChallengeLifetime,
                CreatedByUserId = current.UserId
            };
            db.RegisteredDeviceProofKeyChallenges.Add(challenge);
            await db.SaveChangesAsync(ct);
            await AppendAuditAsync("RegisteredDeviceProofKeyChallengeCreated", device, current, correlationId,
                changeType, publicKey.Thumbprint, null, ct);
            await transaction.CommitAsync(ct);
            return ToChallengeResponse(challenge, Base64Url(rawChallenge));
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex, "ux_device_key_challenge_request"))
        {
            await transaction.RollbackAsync(ct);
            db.ChangeTracker.Clear();
            var replay = await db.RegisteredDeviceProofKeyChallenges.AsNoTracking().SingleOrDefaultAsync(x =>
                x.RegisteredDeviceId == deviceId && x.CompanyId == current.CompanyId &&
                x.ChangeRequestId == request.ChangeRequestId, ct)
                ?? throw new ProofKeyLifecycleException("PROOF_KEY_CHALLENGE_CONFLICT");
            return MatchChallengeReplay(replay, changeType, request.ExpectedProofKeyVersion, publicKey.Thumbprint);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            db.ChangeTracker.Clear();
            throw;
        }
    }

    public async Task<ProofKeyChangeResponse> ChangeAsync(
        Guid deviceId,
        string routeChangeType,
        CurrentSecurityContext current,
        ChangeProofKeyRequest request,
        string? currentCompactProof,
        string newCompactProof,
        string rawBearerToken,
        ReadOnlyMemory<byte> rawRequestBody,
        string canonicalHtu,
        Guid correlationId,
        CancellationToken ct)
    {
        RequireLocalAuthority(current);
        ValidateIdentity(request.ChallengeId, "CHALLENGE_ID_INVALID");
        ValidateIdentity(request.ChangeRequestId, "CHANGE_REQUEST_ID_INVALID");
        var changeType = ValidateChangeType(request.ChangeType);
        if (!string.Equals(changeType, routeChangeType, StringComparison.Ordinal))
            throw new ProofKeyLifecycleException("PROOF_KEY_CHANGE_TYPE_MISMATCH");
        var reason = ValidateReason(changeType, request.Reason);
        var newPublicKey = proofValidator.ReadPublicKey(request.NewPublicJwk);
        ValidateProofHeaderShape(changeType, currentCompactProof, newCompactProof);

        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
        try
        {
            var device = await LockDeviceAsync(deviceId, current.CompanyId, ct);
            await LockActorScopeAsync(current.UserId, ct);

            // Idempotency is checked only after the governed device->actor lock order.
            var existing = await db.RegisteredDeviceProofKeyChanges.AsNoTracking().SingleOrDefaultAsync(x =>
                x.RegisteredDeviceId == device.Id && x.ChangeRequestId == request.ChangeRequestId, ct);
            if (existing is not null)
            {
                var response = MatchChangeReplay(existing, request, changeType, newPublicKey.Thumbprint, reason);
                await transaction.CommitAsync(ct);
                return response;
            }

            ValidateDeviceState(device, changeType, request.ExpectedProofKeyVersion);
            var challenge = await LockChallengeAsync(request.ChallengeId, device.Id, current.CompanyId, ct);
            ValidateChallenge(challenge, request, changeType, newPublicKey.Thumbprint);

            var validationInput = new ProofKeyChangeValidationInput(
                newCompactProof, rawBearerToken, rawRequestBody, canonicalHtu, challenge.Id,
                request.ChangeRequestId, device.Id, changeType, newPublicKey.Thumbprint, DateTimeOffset.UtcNow);
            var nextProof = proofValidator.Validate(validationInput);
            if (!FixedEquals(nextProof.PublicKeyCanonicalJson, newPublicKey.CanonicalJson) ||
                !FixedEquals(nextProof.PublicKeyThumbprint, newPublicKey.Thumbprint))
                throw new ProofKeyLifecycleException("PROOF_KEY_PROOF_INVALID");
            if (!CryptographicOperations.FixedTimeEquals(
                    SHA256.HashData(ProofKeyChangeProofValidator.DecodeChallenge(nextProof.RawChallenge)),
                    challenge.ChallengeHash))
                throw new ProofKeyLifecycleException("PROOF_KEY_PROOF_INVALID");

            if (changeType == "ROTATE")
            {
                var currentProof = proofValidator.Validate(validationInput with { CompactProof = currentCompactProof! });
                if (device.ProofPublicJwkCanonicalJson is null || device.ProofKeyThumbprint is null ||
                    !FixedEquals(currentProof.PublicKeyCanonicalJson, device.ProofPublicJwkCanonicalJson) ||
                    !FixedEquals(currentProof.PublicKeyThumbprint, device.ProofKeyThumbprint))
                    throw new ProofKeyLifecycleException("PROOF_KEY_PROOF_INVALID");
                ProofKeyChangeProofValidator.RequireMatchingPayloads(currentProof, nextProof);
            }

            var now = DateTimeOffset.UtcNow;
            var previousThumbprint = device.ProofKeyThumbprint;
            var resultVersion = changeType == "BIND" ? 1 : request.ExpectedProofKeyVersion!.Value + 1;
            var change = new RegisteredDeviceProofKeyChange
            {
                Id = Guid.NewGuid(),
                CompanyId = device.CompanyId,
                RegisteredDeviceId = device.Id,
                DeviceId = device.DeviceId,
                ChangeRequestId = request.ChangeRequestId,
                ChallengeId = challenge.Id,
                ChangeType = changeType,
                ExpectedProofKeyVersion = request.ExpectedProofKeyVersion,
                PreviousProofKeyThumbprint = previousThumbprint,
                NewProofKeyThumbprint = newPublicKey.Thumbprint,
                ResultProofKeyVersion = resultVersion,
                ChangedByUserId = current.UserId,
                Reason = reason,
                ChangedAt = now
            };
            db.RegisteredDeviceProofKeyChanges.Add(change);
            await db.SaveChangesAsync(ct); // Trigger checks the still-unconsumed challenge and old device state.

            challenge.ConsumedAt = now;
            device.ProofPublicJwkCanonicalJson = newPublicKey.CanonicalJson;
            device.ProofKeyThumbprint = newPublicKey.Thumbprint;
            device.ProofKeyVersion = resultVersion;
            device.ProofKeyChangedAt = now;
            device.ProofKeyChangedByUserId = current.UserId;
            device.UpdatedAt = now;
            device.RowVersion = RandomNumberGenerator.GetBytes(16);
            if (changeType == "RECOVER")
                await RevokeDeviceSessionsAsync(device.Id, now, ct);
            await db.SaveChangesAsync(ct);
            await AppendAuditAsync($"RegisteredDeviceProofKey{changeType}", device, current, correlationId,
                changeType, newPublicKey.Thumbprint, reason, ct);
            await transaction.CommitAsync(ct);
            return ToChangeResponse(change);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex, "ux_registered_device_proof_thumbprint"))
        {
            await transaction.RollbackAsync(ct);
            db.ChangeTracker.Clear();
            throw new ProofKeyLifecycleException("DEVICE_PROOF_KEY_CONFLICT");
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex, "ux_device_key_change_request"))
        {
            await transaction.RollbackAsync(ct);
            db.ChangeTracker.Clear();
            var existing = await db.RegisteredDeviceProofKeyChanges.AsNoTracking().SingleOrDefaultAsync(x =>
                x.RegisteredDeviceId == deviceId && x.CompanyId == current.CompanyId &&
                x.ChangeRequestId == request.ChangeRequestId, ct)
                ?? throw new ProofKeyLifecycleException("PROOF_KEY_CHANGE_CONFLICT");
            return MatchChangeReplay(existing, request, changeType, newPublicKey.Thumbprint, reason);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            db.ChangeTracker.Clear();
            throw;
        }
    }

    private async Task<RegisteredDevice> LockDeviceAsync(Guid id, Guid companyId, CancellationToken ct)
    {
        var query = db.RegisteredDevices.Where(x => x.Id == id && x.CompanyId == companyId);
        if (db.Database.IsNpgsql())
            query = db.RegisteredDevices.FromSqlInterpolated(
                $"SELECT * FROM transport_erp.registered_devices WHERE \"Id\"={id} AND \"CompanyId\"={companyId} FOR UPDATE");
        return await query.SingleOrDefaultAsync(ct)
            ?? throw new ProofKeyLifecycleException("DEVICE_NOT_FOUND");
    }

    private async Task<RegisteredDeviceProofKeyChallenge> LockChallengeAsync(
        Guid challengeId, Guid deviceId, Guid companyId, CancellationToken ct)
    {
        IQueryable<RegisteredDeviceProofKeyChallenge> query = db.RegisteredDeviceProofKeyChallenges.Where(x =>
            x.Id == challengeId && x.RegisteredDeviceId == deviceId && x.CompanyId == companyId);
        if (db.Database.IsNpgsql())
            query = db.RegisteredDeviceProofKeyChallenges.FromSqlInterpolated(
                $"SELECT * FROM transport_erp.registered_device_proof_key_challenges WHERE \"Id\"={challengeId} AND \"RegisteredDeviceId\"={deviceId} AND \"CompanyId\"={companyId} FOR UPDATE");
        return await query.SingleOrDefaultAsync(ct)
            ?? throw new ProofKeyLifecycleException("PROOF_KEY_CHALLENGE_INVALID");
    }

    private async Task LockActorScopeAsync(Guid userId, CancellationToken ct)
    {
        if (db.Database.IsNpgsql())
        {
            var lockKey = "user-scope|" + userId;
            await db.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT pg_advisory_xact_lock(hashtextextended({lockKey}, 0))", ct);
        }
    }

    private async Task RevokeDeviceSessionsAsync(Guid deviceId, DateTimeOffset now, CancellationToken ct)
    {
        var sessions = await db.AuthSessions.Where(x => x.RegisteredDeviceId == deviceId && x.RevokedAt == null)
            .OrderBy(x => x.Id).ToListAsync(ct);
        foreach (var session in sessions)
        {
            session.RevokedAt = now;
            session.RevokeReason = "DEVICE_PROOF_KEY_RECOVERED";
            session.UpdatedAt = now;
            session.RowVersion = RandomNumberGenerator.GetBytes(16);
        }
    }

    private Task AppendAuditAsync(
        string action,
        RegisteredDevice device,
        CurrentSecurityContext current,
        Guid correlationId,
        string changeType,
        string thumbprint,
        string? reason,
        CancellationToken ct)
        => audit.AppendAuditEventAsync(new AuditEventDraft(
            action, "SUCCESS", "RegisteredDevice", device.Id, current.UserId, device.CompanyId,
            current.BranchId, correlationId, device.DeviceId,
            AfterJson: JsonSerializer.Serialize(new
            {
                ChangeType = changeType,
                ProofKeyThumbprint = thumbprint,
                ProofKeyVersion = device.ProofKeyVersion
            }),
            Reason: reason), ct);

    private static void RequireLocalAuthority(CurrentSecurityContext current)
    {
        if (!current.IsLocalSession || current.SessionId is null)
            throw new ProofKeyLifecycleException("LOCAL_SESSION_REQUIRED");
    }

    private static void ValidateDeviceState(RegisteredDevice device, string changeType, int? expectedVersion)
    {
        var valid = changeType switch
        {
            "BIND" => device.Status is "PENDING" or "ACTIVE" && device.ProofKeyVersion is null &&
                      expectedVersion is null,
            "ROTATE" => device.Status == "ACTIVE" && expectedVersion is >= 1 &&
                        device.ProofKeyVersion == expectedVersion,
            "RECOVER" => device.Status is "ACTIVE" or "SUSPENDED" or "EXPIRED" && expectedVersion is >= 1 &&
                         device.ProofKeyVersion == expectedVersion,
            _ => false
        };
        if (!valid) throw new ProofKeyLifecycleException(
            device.Status == "REVOKED" ? "DEVICE_REVOKED" : "DEVICE_PROOF_KEY_STATE_INVALID");
    }

    private static void ValidateChallenge(
        RegisteredDeviceProofKeyChallenge challenge,
        ChangeProofKeyRequest request,
        string changeType,
        string newThumbprint)
    {
        if (challenge.ConsumedAt is not null || challenge.ExpiresAt <= DateTimeOffset.UtcNow ||
            challenge.ChangeRequestId != request.ChangeRequestId || challenge.ChangeType != changeType ||
            challenge.ExpectedProofKeyVersion != request.ExpectedProofKeyVersion ||
            !FixedEquals(challenge.NewProofKeyThumbprint, newThumbprint))
            throw new ProofKeyLifecycleException("PROOF_KEY_CHALLENGE_INVALID");
    }

    private static ProofKeyChallengeResponse MatchChallengeReplay(
        RegisteredDeviceProofKeyChallenge existing,
        string changeType,
        int? expectedVersion,
        string newThumbprint)
    {
        if (existing.ChangeType != changeType || existing.ExpectedProofKeyVersion != expectedVersion ||
            !FixedEquals(existing.NewProofKeyThumbprint, newThumbprint))
            throw new ProofKeyLifecycleException("PROOF_KEY_CHALLENGE_MISMATCH");
        return ToChallengeResponse(existing, null);
    }

    private static ProofKeyChangeResponse MatchChangeReplay(
        RegisteredDeviceProofKeyChange existing,
        ChangeProofKeyRequest request,
        string changeType,
        string newThumbprint,
        string? reason)
    {
        if (existing.ChallengeId != request.ChallengeId || existing.ChangeType != changeType ||
            existing.ExpectedProofKeyVersion != request.ExpectedProofKeyVersion ||
            !FixedEquals(existing.NewProofKeyThumbprint, newThumbprint) ||
            !string.Equals(existing.Reason, reason, StringComparison.Ordinal))
            throw new ProofKeyLifecycleException("PROOF_KEY_CHANGE_MISMATCH");
        return ToChangeResponse(existing);
    }

    private static string ValidateChangeType(string? value)
    {
        if (value is not ("BIND" or "ROTATE" or "RECOVER"))
            throw new ProofKeyLifecycleException("PROOF_KEY_CHANGE_TYPE_INVALID");
        return value;
    }

    private static string? ValidateReason(string changeType, string? reason)
    {
        if (changeType != "RECOVER")
        {
            if (reason is not null) throw new ProofKeyLifecycleException("RECOVERY_REASON_INVALID");
            return null;
        }
        var normalized = reason?.Trim();
        if (string.IsNullOrEmpty(normalized) || normalized.Length > 500)
            throw new ProofKeyLifecycleException("RECOVERY_REASON_INVALID");
        return normalized;
    }

    private static void ValidateProofHeaderShape(string changeType, string? currentProof, string newProof)
    {
        if (string.IsNullOrEmpty(newProof) ||
            (changeType == "ROTATE" ? string.IsNullOrEmpty(currentProof) : currentProof is not null))
            throw new ProofKeyLifecycleException("PROOF_KEY_PROOF_INVALID");
    }

    private static void ValidateIdentity(Guid id, string code)
    {
        if (id == Guid.Empty) throw new ProofKeyLifecycleException(code);
    }

    private static ProofKeyChallengeResponse ToChallengeResponse(
        RegisteredDeviceProofKeyChallenge value,
        string? rawChallenge)
        => new(value.Id, value.ChangeRequestId, value.ChangeType, value.ExpectedProofKeyVersion,
            value.NewProofKeyThumbprint, value.IssuedAt, value.ExpiresAt, rawChallenge);

    private static ProofKeyChangeResponse ToChangeResponse(RegisteredDeviceProofKeyChange value)
        => new(value.RegisteredDeviceId, value.ChangeRequestId, value.ChangeType, value.NewProofKeyThumbprint,
            value.ResultProofKeyVersion, value.ChangedAt);

    private static bool IsUniqueViolation(DbUpdateException exception, string constraint)
        => exception.GetBaseException() is PostgresException
        {
            SqlState: PostgresErrorCodes.UniqueViolation
        } postgres && string.Equals(postgres.ConstraintName, constraint, StringComparison.Ordinal);

    private static bool FixedEquals(string left, string right)
        => CryptographicOperations.FixedTimeEquals(System.Text.Encoding.UTF8.GetBytes(left),
            System.Text.Encoding.UTF8.GetBytes(right));

    private static string Base64Url(byte[] value)
        => Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');
}
