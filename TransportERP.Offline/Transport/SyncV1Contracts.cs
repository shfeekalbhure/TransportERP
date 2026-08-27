using System.Text.Json;
using System.Text.Json.Serialization;
using TransportERP.Application.Sync;

namespace TransportERP.Offline.Transport;

public sealed record SyncV1BatchRequest(
    string DeviceId,
    string ProtocolVersion,
    IReadOnlyList<SyncV1OperationRequest> Operations,
    BuildIdentityV1 BuildIdentity);

public sealed record SyncV1OperationRequest(
    string ActionCode,
    string OperationType,
    string EntityType,
    Guid? EntityId,
    string ClientOperationId,
    string PayloadJson,
    string PayloadHash,
    string ClientOccurredAt,
    Guid OperationCorrelationId,
    long? BaseVersion = null);

public sealed record SyncV1OperationResult(
    string? ClientOperationId,
    Guid? OperationCorrelationId,
    Guid? ServerOperationId,
    string? ActionCode,
    Guid? ResultEntityId,
    string Status,
    long? ResultVersion,
    string? ErrorCode,
    Guid? ConflictCaseId,
    DateTimeOffset ServerTime,
    SyncV1ConflictReview? ConflictReview = null);

public sealed record SyncV1ConflictLocalSnapshot(
    string? ActionCode,
    string? EntityType,
    Guid? EntityId,
    long? RequestedBaseVersion);

public sealed record SyncV1ConflictServerSnapshot(
    string? EntityType,
    Guid? EntityId,
    bool? Exists,
    long? CurrentVersion);

public sealed record SyncV1ConflictReview(
    long? BaseVersion,
    string? ConflictReason,
    SyncV1ConflictLocalSnapshot? LocalSnapshot,
    SyncV1ConflictServerSnapshot? ServerSnapshot,
    string? Status,
    string? Resolution,
    bool ResolvedByAuthorizedUser,
    DateTimeOffset? ResolvedAt,
    Guid? ReplacedByOperationId);

public sealed record SyncV1BatchResponse(
    string ProtocolVersion,
    IReadOnlyList<SyncV1OperationResult> Results,
    DateTimeOffset ServerTime,
    Guid AttemptCorrelationId);

public sealed record SyncV1ErrorResponse(string? ErrorCode, Guid? CorrelationId);

public sealed record SyncV1ConflictReapplyRequest(
    string ClientOperationId,
    Guid OperationCorrelationId,
    string ActionCode,
    string OperationType,
    string EntityType,
    Guid? EntityId,
    long? BaseVersion,
    DateTimeOffset ClientOccurredAt,
    string PayloadJson,
    string PayloadHash);

public sealed record SyncV1ConflictResolutionRequest(
    string Decision,
    string Reason,
    SyncV1ConflictReapplyRequest? Reapply,
    BuildIdentityV1 BuildIdentity);

public sealed record SyncV1ConflictResolutionResponse(
    Guid ConflictCaseId,
    Guid OriginalOperationId,
    string Decision,
    string ConflictStatus,
    string OriginalOperationStatus,
    string? OriginalOperationErrorCode,
    Guid? ReplacedByOperationId,
    DateTimeOffset ResolvedAt,
    Guid CorrelationId);

internal static class SyncV1Json
{
    private const int MaximumErrorResponseBytes = 65_536;

    internal static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = false,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        MaxDepth = 64
    };

    internal static byte[] Serialize(SyncV1BatchRequest request) =>
        JsonSerializer.SerializeToUtf8Bytes(request, Options);

    internal static byte[] Serialize<T>(T request) =>
        JsonSerializer.SerializeToUtf8Bytes(request, Options);

    internal static T? Deserialize<T>(ReadOnlySpan<byte> utf8Json) =>
        JsonSerializer.Deserialize<T>(utf8Json, Options);

    internal static async Task<byte[]> ReadBoundedErrorBytesAsync(
        HttpContent content,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (content.Headers.ContentLength is > MaximumErrorResponseBytes)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new JsonException("SYNC_ERROR_RESPONSE_TOO_LARGE");
        }

        await using var stream = await content.ReadAsStreamAsync(cancellationToken);
        using var bounded = new MemoryStream();
        var buffer = new byte[4096];
        while (true)
        {
            var read = await stream.ReadAsync(buffer, cancellationToken);
            if (read == 0)
                break;
            if (bounded.Length + read > MaximumErrorResponseBytes)
            {
                cancellationToken.ThrowIfCancellationRequested();
                throw new JsonException("SYNC_ERROR_RESPONSE_TOO_LARGE");
            }
            await bounded.WriteAsync(buffer.AsMemory(0, read), cancellationToken);
        }
        return bounded.ToArray();
    }
}
