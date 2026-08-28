using System.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace TransportERP.Infrastructure.Persistence;

public sealed record SyncProofSecurityContext(
    Guid UserId,
    Guid CompanyId,
    Guid BranchId,
    Guid RegisteredDeviceId,
    string DeviceId);

public sealed record IssuedSyncNonce(string Value, DateTimeOffset ExpiresAt);

public sealed record VerifiedSyncProofMaterial(
    string Jti,
    string Nonce,
    string ProofKeyThumbprint,
    DateTimeOffset IssuedAt,
    string CanonicalHtu,
    Guid AttemptCorrelationId);

public sealed record AcceptedSyncProofContext(
    Guid ReplayId,
    Guid UserId,
    Guid CompanyId,
    Guid BranchId,
    Guid RegisteredDeviceId,
    string DeviceId,
    int DeviceCredentialVersion,
    int ProofKeyVersion,
    string ProofKeyThumbprint,
    Guid AttemptCorrelationId);

public sealed class SyncProofRuntimeException(string code) : InvalidOperationException(code)
{
    public string Code { get; } = code;
}

public interface ISyncProofRuntime
{
    Task<IssuedSyncNonce> IssueNonceAsync(
        SyncProofSecurityContext security,
        CancellationToken cancellationToken = default);

    Task<AcceptedSyncProofContext> ClaimAsync(
        SyncProofSecurityContext security,
        VerifiedSyncProofMaterial proof,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Shared PostgreSQL nonce and proof-replay boundary for TransportERP Sync-PoP v1.
/// Raw nonces and jti values are never persisted. The endpoint's closed gate must be
/// evaluated before invoking any method on this service.
/// </summary>
public sealed class SyncProofRuntimeService(TransportErpDbContext db, AuditEventService audit) : ISyncProofRuntime
{
    public static readonly TimeSpan NonceLifetime = TimeSpan.FromMinutes(5);
    public static readonly TimeSpan ReplayRetention = TimeSpan.FromMinutes(10);
    private const int NonceGenerationAttempts = 3;

    public async Task<IssuedSyncNonce> IssueNonceAsync(
        SyncProofSecurityContext security,
        CancellationToken cancellationToken = default)
    {
        for (var attempt = 1; attempt <= NonceGenerationAttempts; attempt++)
        {
            var rawNonce = RandomNumberGenerator.GetBytes(32);
            var nonceHash = SHA256.HashData(rawNonce);
            var now = NormalizeTimestamp(DateTimeOffset.UtcNow);
            var expiresAt = NormalizeTimestamp(now.Add(NonceLifetime));
            await using var transaction = await db.Database.BeginTransactionAsync(
                IsolationLevel.ReadCommitted, cancellationToken);
            try
            {
                var binding = await LockAndValidateBindingAsync(security, cancellationToken);
                await AcquireUserScopeLockAsync(security.UserId, cancellationToken);
                if (!await db.RegisteredDeviceAssignments.AsNoTracking().AnyAsync(x =>
                        x.RegisteredDeviceId == security.RegisteredDeviceId &&
                        x.CompanyId == security.CompanyId && x.UserId == security.UserId &&
                        x.BranchId == security.BranchId && x.Status == "ACTIVE", cancellationToken))
                    throw new SyncProofRuntimeException("DEVICE_NOT_REGISTERED");
                db.SyncProofNonces.Add(new SyncProofNonce
                {
                    Id = Guid.NewGuid(),
                    CompanyId = security.CompanyId,
                    RegisteredDeviceId = security.RegisteredDeviceId,
                    DeviceId = security.DeviceId,
                    ProofKeyVersion = binding.ProofKeyVersion!.Value,
                    NonceHash = nonceHash,
                    IssuedAt = now,
                    ExpiresAt = expiresAt
                });
                await db.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return new IssuedSyncNonce(Base64Url(rawNonce), expiresAt);
            }
            catch (DbUpdateException exception) when (
                exception.GetBaseException() is PostgresException
                {
                    SqlState: PostgresErrorCodes.UniqueViolation,
                    ConstraintName: "ux_sync_nonce_hash"
                })
            {
                await transaction.RollbackAsync(cancellationToken);
                db.ChangeTracker.Clear();
                if (attempt == NonceGenerationAttempts) break;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                db.ChangeTracker.Clear();
                throw;
            }
        }

        throw new SyncProofRuntimeException("NONCE_GENERATION_FAILED");
    }

    public async Task<AcceptedSyncProofContext> ClaimAsync(
        SyncProofSecurityContext security,
        VerifiedSyncProofMaterial proof,
        CancellationToken cancellationToken = default)
    {
        ValidateProofMaterial(proof);
        var nonceBytes = DecodeBase64Url(proof.Nonce, expectedLength: 32, "invalid_dpop_proof");
        var nonceHash = SHA256.HashData(nonceBytes);
        var jtiHash = SHA256.HashData(Encoding.UTF8.GetBytes(proof.Jti));
        var htuHash = SHA256.HashData(Encoding.ASCII.GetBytes(proof.CanonicalHtu));
        var now = NormalizeTimestamp(DateTimeOffset.UtcNow);
        var issuedAt = NormalizeTimestamp(proof.IssuedAt);
        if (issuedAt < now.AddSeconds(-120) || issuedAt > now.AddSeconds(30))
            throw new SyncProofRuntimeException("invalid_dpop_proof");

        await using var transaction = await db.Database.BeginTransactionAsync(
            IsolationLevel.ReadCommitted, cancellationToken);
        try
        {
            var binding = await LockAndValidateBindingAsync(security, cancellationToken);
            if (!FixedEquals(binding.ProofKeyThumbprint!, proof.ProofKeyThumbprint))
                throw new SyncProofRuntimeException("invalid_dpop_proof");

            await AcquireUserScopeLockAsync(security.UserId, cancellationToken);
            var assignment = await db.RegisteredDeviceAssignments.FromSqlInterpolated($"""
                SELECT * FROM transport_erp.registered_device_assignments
                WHERE "RegisteredDeviceId"={security.RegisteredDeviceId}
                  AND "CompanyId"={security.CompanyId}
                  AND "UserId"={security.UserId}
                  AND "BranchId"={security.BranchId}
                  AND "Status"='ACTIVE'
                ORDER BY "Id" FOR UPDATE
                """).SingleOrDefaultAsync(cancellationToken)
                ?? throw new SyncProofRuntimeException("DEVICE_NOT_REGISTERED");

            var nonce = await db.SyncProofNonces.FromSqlInterpolated($"""
                SELECT * FROM transport_erp.sync_proof_nonces
                WHERE "NonceHash"={nonceHash}
                ORDER BY "Id" FOR UPDATE
                """).SingleOrDefaultAsync(cancellationToken);
            if (nonce is null || nonce.CompanyId != security.CompanyId ||
                nonce.RegisteredDeviceId != security.RegisteredDeviceId ||
                !string.Equals(nonce.DeviceId, security.DeviceId, StringComparison.Ordinal) ||
                nonce.ProofKeyVersion != binding.ProofKeyVersion || nonce.ExpiresAt <= now)
                throw new SyncProofRuntimeException("use_dpop_nonce");

            var replay = new SyncProofReplay
            {
                Id = Guid.NewGuid(),
                CompanyId = security.CompanyId,
                RegisteredDeviceId = security.RegisteredDeviceId,
                DeviceId = security.DeviceId,
                DeviceAssignmentId = assignment.Id,
                UserId = security.UserId,
                BranchId = security.BranchId,
                ProofKeyVersion = binding.ProofKeyVersion!.Value,
                ProofKeyThumbprint = binding.ProofKeyThumbprint!,
                JtiHash = jtiHash,
                HtuHash = htuHash,
                HttpMethod = "POST",
                NonceRecordId = nonce.Id,
                IssuedAt = issuedAt,
                FirstSeenAt = now,
                ExpiresAt = NormalizeTimestamp(now.Add(ReplayRetention)),
                AttemptCorrelationId = proof.AttemptCorrelationId
            };
            db.SyncProofReplays.Add(replay);
            await db.SaveChangesAsync(cancellationToken);
            await audit.AppendAuditEventAsync(new AuditEventDraft(
                "SyncProofAccepted", "SUCCESS", nameof(SyncProofReplay), replay.Id,
                security.UserId, security.CompanyId, security.BranchId,
                proof.AttemptCorrelationId, security.DeviceId,
                Reason: $"ProofKeyVersion={replay.ProofKeyVersion}"), cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return new AcceptedSyncProofContext(
                replay.Id, security.UserId, security.CompanyId, security.BranchId,
                security.RegisteredDeviceId, security.DeviceId, binding.CredentialVersion, replay.ProofKeyVersion,
                replay.ProofKeyThumbprint, replay.AttemptCorrelationId);
        }
        catch (DbUpdateException exception) when (
            exception.GetBaseException() is PostgresException
            {
                SqlState: PostgresErrorCodes.UniqueViolation,
                ConstraintName: "ux_sync_replay_device_key_jti"
            })
        {
            await transaction.RollbackAsync(cancellationToken);
            db.ChangeTracker.Clear();
            throw new SyncProofRuntimeException("invalid_dpop_proof");
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            db.ChangeTracker.Clear();
            throw;
        }
    }

    private async Task<RegisteredDevice> LockAndValidateBindingAsync(
        SyncProofSecurityContext security,
        CancellationToken cancellationToken)
    {
        var device = await db.RegisteredDevices.FromSqlInterpolated($"""
            SELECT * FROM transport_erp.registered_devices
            WHERE "Id"={security.RegisteredDeviceId}
              AND "CompanyId"={security.CompanyId}
              AND "DeviceId"={security.DeviceId}
            FOR UPDATE
            """).SingleOrDefaultAsync(cancellationToken)
            ?? throw new SyncProofRuntimeException("DEVICE_NOT_REGISTERED");
        var now = DateTimeOffset.UtcNow;
        if (device.Status != "ACTIVE" || device.ProofKeyVersion is null ||
            string.IsNullOrEmpty(device.ProofKeyThumbprint) ||
            string.IsNullOrEmpty(device.ProofPublicJwkCanonicalJson) ||
            (device.ExpiresAt.HasValue && device.ExpiresAt <= now) ||
            now - (device.LastSeenAt ?? device.ApprovedAt ?? device.CreatedAt) >= TimeSpan.FromDays(90))
            throw new SyncProofRuntimeException(device.ProofKeyVersion is null
                ? "DEVICE_PROOF_KEY_REQUIRED"
                : "DEVICE_NOT_REGISTERED");
        return device;
    }

    private async Task AcquireUserScopeLockAsync(Guid userId, CancellationToken cancellationToken)
    {
        var lockKey = "user-scope|" + userId;
        await db.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT pg_advisory_xact_lock(hashtextextended({lockKey}, 0))",
            cancellationToken);
    }

    private static void ValidateProofMaterial(VerifiedSyncProofMaterial proof)
    {
        if (proof.AttemptCorrelationId == Guid.Empty || proof.Jti.Length is < 16 or > 128 ||
            proof.ProofKeyThumbprint.Length != 43 ||
            !proof.CanonicalHtu.StartsWith("https://", StringComparison.Ordinal) ||
            !IsBase64Url(proof.Jti))
            throw new SyncProofRuntimeException("invalid_dpop_proof");
    }

    private static byte[] DecodeBase64Url(string value, int expectedLength, string code)
    {
        if (!IsBase64Url(value)) throw new SyncProofRuntimeException(code);
        try
        {
            var padded = value.Replace('-', '+').Replace('_', '/');
            padded += new string('=', (4 - padded.Length % 4) % 4);
            var decoded = Convert.FromBase64String(padded);
            if (decoded.Length != expectedLength) throw new SyncProofRuntimeException(code);
            return decoded;
        }
        catch (FormatException)
        {
            throw new SyncProofRuntimeException(code);
        }
    }

    private static bool IsBase64Url(string value)
        => !string.IsNullOrEmpty(value) && value.All(character =>
            character is >= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '0' and <= '9' or '-' or '_');

    private static bool FixedEquals(string left, string right)
        => CryptographicOperations.FixedTimeEquals(
            Encoding.ASCII.GetBytes(left), Encoding.ASCII.GetBytes(right));

    private static string Base64Url(byte[] value)
        => Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    private static DateTimeOffset NormalizeTimestamp(DateTimeOffset value)
    {
        var ticks = value.UtcDateTime.Ticks;
        return new DateTimeOffset(new DateTime(
            ticks - ticks % TimeSpan.TicksPerMicrosecond, DateTimeKind.Utc));
    }
}
