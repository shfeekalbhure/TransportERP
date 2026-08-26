using System.Text;
using System.Text.Json;
using TransportERP.Api.Sync;

namespace TransportERP.Tests;

public sealed class Stage4SyncApiContractTests
{
    [Fact]
    public void Envelope_codec_accepts_exact_web_camel_case_and_rejects_pascal_or_unknown_properties()
    {
        const string camel = """
            {"deviceId":"device-1","protocolVersion":"sync-v1","operations":[]}
            """;
        var parsed = SyncBatchJsonContract.Deserialize(Encoding.UTF8.GetBytes(camel));

        Assert.NotNull(parsed);
        Assert.Equal("device-1", parsed!.DeviceId);
        Assert.True(SyncBatchJsonContract.TryReadDeviceId(Encoding.UTF8.GetBytes(camel), out var deviceId));
        Assert.Equal(parsed.DeviceId, deviceId);

        const string pascal = """
            {"DeviceId":"device-1","ProtocolVersion":"sync-v1","Operations":[]}
            """;
        Assert.Throws<JsonException>(() => SyncBatchJsonContract.Deserialize(Encoding.UTF8.GetBytes(pascal)));
        Assert.False(SyncBatchJsonContract.TryReadDeviceId(Encoding.UTF8.GetBytes(pascal), out _));

        const string unknown = """
            {"deviceId":"device-1","protocolVersion":"sync-v1","operations":[],"extra":true}
            """;
        Assert.Throws<JsonException>(() => SyncBatchJsonContract.Deserialize(Encoding.UTF8.GetBytes(unknown)));
    }

    [Fact]
    public void Success_contract_serializes_attempt_correlation_id_with_its_governed_name()
    {
        var attemptCorrelationId = Guid.NewGuid();
        var response = new SyncBatchResponse("sync-v1", [], DateTimeOffset.UtcNow, attemptCorrelationId);

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        using var document = JsonDocument.Parse(json);

        Assert.Equal(attemptCorrelationId,
            document.RootElement.GetProperty("attemptCorrelationId").GetGuid());
        Assert.False(document.RootElement.TryGetProperty("correlationId", out _));
    }
}
