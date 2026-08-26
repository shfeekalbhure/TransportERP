using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace TransportERP.Offline;

public enum OfflineOperationStatus
{
    Queued,
    Sending,
    Succeeded,
    Failed,
    Conflict,
    Rejected,
    Resolved
}

public sealed record OfflineOperationEnqueueRequest(
    Guid LocalIntentId,
    Guid CompanyId,
    Guid BranchId,
    Guid UserId,
    Guid RegisteredDeviceId,
    string ActionCode,
    string OperationType,
    string EntityType,
    Guid? EntityId,
    long? BaseVersion,
    DateTimeOffset ClientOccurredAt,
    string PayloadJson);

public sealed record OfflineOperation(
    Guid LocalOperationId,
    Guid LocalIntentId,
    Guid CompanyId,
    Guid BranchId,
    Guid UserId,
    Guid RegisteredDeviceId,
    string ClientOperationId,
    Guid OperationCorrelationId,
    Guid? AttemptCorrelationId,
    string ProtocolVersion,
    string ActionCode,
    string OperationType,
    string EntityType,
    Guid? EntityId,
    long? BaseVersion,
    DateTimeOffset ClientOccurredAt,
    string? PayloadJson,
    string PayloadHash,
    string RequestFingerprint,
    OfflineOperationStatus Status,
    int ClientTransportRetryCount,
    DateTimeOffset? NextRetryAt,
    string? LeaseOwner,
    DateTimeOffset? LeaseExpiresAt,
    string? ResultCode,
    Guid? ConflictCaseId,
    Guid? ResultEntityId,
    long? ResultVersion,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    DateTimeOffset? AcknowledgedAt,
    DateTimeOffset? RedactedAt);

public sealed record OfflineEnqueueResult(OfflineOperation Operation, bool Created);

public sealed record OfflineRetryPolicy(
    int MaxRetryCount = 5,
    TimeSpan? BaseDelay = null,
    TimeSpan? MaxDelay = null)
{
    public TimeSpan EffectiveBaseDelay => BaseDelay ?? TimeSpan.FromSeconds(5);
    public TimeSpan EffectiveMaxDelay => MaxDelay ?? TimeSpan.FromMinutes(30);

    internal void Validate()
    {
        if (MaxRetryCount < 0 || MaxRetryCount > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(MaxRetryCount));
        }

        if (EffectiveBaseDelay <= TimeSpan.Zero || EffectiveMaxDelay < EffectiveBaseDelay)
        {
            throw new ArgumentOutOfRangeException(nameof(BaseDelay));
        }
    }

    public TimeSpan DelayForRetry(int retryNumber)
    {
        if (retryNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(retryNumber));
        }

        var exponent = Math.Min(retryNumber - 1, 30);
        var ticks = EffectiveBaseDelay.Ticks * Math.Pow(2, exponent);
        return TimeSpan.FromTicks((long)Math.Min(ticks, EffectiveMaxDelay.Ticks));
    }
}

public sealed record OfflineRetentionPolicy(
    TimeSpan? SucceededOrResolved = null,
    TimeSpan? Rejected = null)
{
    public TimeSpan EffectiveSucceededOrResolved => SucceededOrResolved ?? TimeSpan.FromHours(24);
    public TimeSpan EffectiveRejected => Rejected ?? TimeSpan.FromDays(7);
}

public sealed class OfflineStoreException : Exception
{
    public OfflineStoreException(string code, string message, Exception? innerException = null)
        : base(message, innerException) => Code = code;

    public string Code { get; }
}

internal static class OfflineOperationIntegrity
{
    private static readonly HashSet<string> CurrentRuntimeActions = new(StringComparer.Ordinal)
    {
        "CreateWaybillDraft",
        "UpdateWaybillDraft",
        "CreateOperationalParty",
        "RecordCollection",
        "LoadAllocatedQuantity"
    };

    private static readonly HashSet<string> ForbiddenPropertyNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "authorization", "token", "rawToken", "accessToken", "refreshToken", "idToken", "sessionToken",
        "bearerToken", "apiKey", "clientSecret", "password", "secret", "deviceCredential", "credential",
        "dpop", "dpopProof", "rawProof", "proof", "rawNonce", "nonce", "jti", "privateKey"
    };

    public static (string PayloadHash, string Fingerprint) ValidateAndHash(OfflineOperationEnqueueRequest request)
    {
        if (request.LocalIntentId == Guid.Empty || request.CompanyId == Guid.Empty ||
            request.BranchId == Guid.Empty || request.UserId == Guid.Empty ||
            request.RegisteredDeviceId == Guid.Empty)
        {
            throw new OfflineStoreException("LOCAL_OPERATION_INVALID", "Required operation identities must be non-empty.");
        }

        if (string.IsNullOrWhiteSpace(request.ActionCode) || string.IsNullOrWhiteSpace(request.OperationType) ||
            string.IsNullOrWhiteSpace(request.EntityType) || string.IsNullOrWhiteSpace(request.PayloadJson))
        {
            throw new OfflineStoreException("LOCAL_OPERATION_INVALID", "Required operation fields cannot be empty.");
        }

        if (!CurrentRuntimeActions.Contains(request.ActionCode) || string.Equals(request.OperationType, "DELETE", StringComparison.Ordinal))
        {
            throw new OfflineStoreException(
                "ACTION_RUNTIME_UNAVAILABLE",
                "The action is not available to the current offline client runtime.");
        }

        ValidatePayload(request.PayloadJson);

        var payloadHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(request.PayloadJson))).ToLowerInvariant();
        var fingerprintSource = string.Join('\n',
            request.CompanyId.ToString("D"),
            request.BranchId.ToString("D"),
            request.UserId.ToString("D"),
            request.RegisteredDeviceId.ToString("D"),
            request.ActionCode,
            request.OperationType,
            request.EntityType,
            request.EntityId?.ToString("D") ?? string.Empty,
            request.BaseVersion?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty,
            request.ClientOccurredAt.ToUniversalTime().ToString("O"),
            payloadHash);

        var fingerprint = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(fingerprintSource))).ToLowerInvariant();
        return (payloadHash, fingerprint);
    }

    public static void ValidatePayload(string payloadJson)
    {
        try
        {
            using var document = JsonDocument.Parse(payloadJson);
            RejectTransportSecrets(document.RootElement);
        }
        catch (JsonException exception)
        {
            throw new OfflineStoreException("LOCAL_PAYLOAD_INVALID", "Payload must be valid JSON.", exception);
        }
    }

    private static void RejectTransportSecrets(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in element.EnumerateObject())
            {
                if (ForbiddenPropertyNames.Contains(property.Name))
                {
                    throw new OfflineStoreException(
                        "TRANSPORT_SECRET_PERSISTENCE_DENIED",
                        "Transport authentication material cannot be persisted in an offline operation.");
                }

                RejectTransportSecrets(property.Value);
            }
        }
        else if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in element.EnumerateArray())
            {
                RejectTransportSecrets(item);
            }
        }
    }
}
