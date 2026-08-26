using System.Text.Json;
using System.Text.Json.Serialization;

namespace TransportERP.Offline.Transport;

public sealed record SyncV1BatchRequest(
    string DeviceId,
    string ProtocolVersion,
    IReadOnlyList<SyncV1OperationRequest> Operations);

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
    DateTimeOffset ServerTime);

public sealed record SyncV1BatchResponse(
    string ProtocolVersion,
    IReadOnlyList<SyncV1OperationResult> Results,
    DateTimeOffset ServerTime,
    Guid AttemptCorrelationId);

public sealed record SyncV1ErrorResponse(string? ErrorCode, Guid? CorrelationId);

internal static class SyncV1Json
{
    internal static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = false,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        MaxDepth = 64
    };

    internal static byte[] Serialize(SyncV1BatchRequest request) =>
        JsonSerializer.SerializeToUtf8Bytes(request, Options);

    internal static T? Deserialize<T>(ReadOnlySpan<byte> utf8Json) =>
        JsonSerializer.Deserialize<T>(utf8Json, Options);
}
