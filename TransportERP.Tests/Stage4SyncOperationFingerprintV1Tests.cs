using System.Buffers.Binary;
using System.Globalization;
using System.Text;
using TransportERP.Application.Sync;

namespace TransportERP.Tests;

public sealed class Stage4SyncOperationFingerprintV1Tests
{
    private const string VectorAHash = "d373caf8c441d4380784ca768d6020c1c556e4f7d02bff5f216afb9c813e47dd";
    private const string VectorAPrefix = "5472616e73706f72744552502e53796e634f7065726174696f6e46696e6765727072696e740066702d763100000e0001010000001011111111111111111111111111111111000201000000102222222222222222222222222222222200030100";
    private const string VectorASuffix = "0900000a01000000076f702d30303031000b0100000020000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f000c0100000008000659e7e6860240000d00000e010000001044444444444444444444444444444444";
    private const string VectorBHash = "2143fd79345d63ccc154202cbf56eb317f06ef7054a7d72c5c5f29074e3f9b43";
    private const string VectorBPrefix = "5472616e73706f72744552502e53796e634f7065726174696f6e46696e6765727072696e740066702d763100000e00010100000010aaaaaaaaaaaa4aaa8aaaaaaaaaaaaaaa00020100000010bbbbbbbbbbbb4bbb8bbbbbbbbbbbbbbb00030100";
    private const string VectorBSuffix = "a82dd9a1000b0100000020ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff000c01000000080000000000000000000d0100000008000000000000002a000e0100000010ffffffffffff4fff8fffffffffffffff";

    [Fact]
    public void FpV1_vector_a_encodes_exactly()
        => AssertVector(VectorA(), 281, VectorAHash, VectorAPrefix, VectorASuffix);

    [Fact]
    public void FpV1_vector_b_encodes_exactly()
        => AssertVector(VectorB(), 335, VectorBHash, VectorBPrefix, VectorBSuffix);

    [Fact]
    public void FpV1_header_field_count_ids_and_uuid_order_are_exact()
    {
        var encoded = SyncOperationFingerprintV1.Encode(VectorA());
        var header = Encoding.ASCII.GetBytes("TransportERP.SyncOperationFingerprint\0fp-v1\0");
        Assert.True(encoded.AsSpan(0, header.Length).SequenceEqual(header));
        Assert.Equal((ushort)14, BinaryPrimitives.ReadUInt16BigEndian(encoded.AsSpan(header.Length, 2)));

        var frames = ReadFrames(encoded);
        Assert.Equal(Enumerable.Range(1, 14).Select(x => (ushort)x), frames.Select(x => x.Id));
        Assert.Equal(Convert.FromHexString("11111111111111111111111111111111"), frames[0].Value);
        Assert.Equal(Convert.FromHexString("22222222222222222222222222222222"), frames[1].Value);
    }

    [Fact]
    public void FpV1_null_presence_is_distinct_from_present_zero()
    {
        var nullFrames = ReadFrames(SyncOperationFingerprintV1.Encode(VectorA()));
        AssertFrameIsNull(nullFrames, 0x0004);
        AssertFrameIsNull(nullFrames, 0x0009);
        AssertFrameIsNull(nullFrames, 0x000d);

        var present = VectorA() with
        {
            BranchId = Guid.Empty,
            EntityId = Guid.Empty,
            BaseVersion = 0
        };
        var presentFrames = ReadFrames(SyncOperationFingerprintV1.Encode(present));
        Assert.Equal(new byte[16], Frame(presentFrames, 0x0004).Value);
        Assert.Equal(new byte[16], Frame(presentFrames, 0x0009).Value);
        Assert.Equal(new byte[8], Frame(presentFrames, 0x000d).Value);
        Assert.NotEqual(Hex(SyncOperationFingerprintV1.ComputeHash(VectorA())),
            Hex(SyncOperationFingerprintV1.ComputeHash(present)));
    }

    [Fact]
    public void FpV1_each_of_fourteen_fields_changes_the_hash_or_is_rejected_by_the_canonical_gate()
    {
        var baseline = VectorB();
        var baselineHash = Hex(SyncOperationFingerprintV1.ComputeHash(baseline));
        var mutations = new (string Field, SyncOperationFingerprintV1Input Input)[]
        {
            (nameof(baseline.CompanyId), baseline with { CompanyId = Guid.Parse("11111111-1111-4111-8111-111111111111") }),
            (nameof(baseline.RegisteredDeviceId), baseline with { RegisteredDeviceId = Guid.Parse("22222222-2222-4222-8222-222222222222") }),
            (nameof(baseline.UserId), baseline with { UserId = Guid.Parse("33333333-3333-4333-8333-333333333333") }),
            (nameof(baseline.BranchId), baseline with { BranchId = null }),
            (nameof(baseline.ProtocolVersion), baseline with { ProtocolVersion = "sync-v1" + " " }),
            (nameof(baseline.ActionCode), baseline with { ActionCode = "UpdateWaybillDraftV2" }),
            (nameof(baseline.OperationType), baseline with { OperationType = "COMMAND" }),
            (nameof(baseline.EntityType), baseline with { EntityType = "WaybillV2" }),
            (nameof(baseline.EntityId), baseline with { EntityId = null }),
            (nameof(baseline.ClientOperationId), baseline with { ClientOperationId = "طلب-٢" }),
            (nameof(baseline.PayloadHash), baseline with { PayloadHash = "efffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff" }),
            (nameof(baseline.ClientOccurredAt), baseline with { ClientOccurredAt = "1970-01-01T00:00:00.000001Z" }),
            (nameof(baseline.BaseVersion), baseline with { BaseVersion = 43 }),
            (nameof(baseline.OperationCorrelationId), baseline with { OperationCorrelationId = Guid.Parse("11111111-2222-4333-8444-555555555555") })
        };

        Assert.Equal(14, mutations.Length);
        foreach (var mutation in mutations)
        {
            if (mutation.Field == nameof(baseline.ProtocolVersion))
            {
                Assert.Throws<ArgumentException>(() =>
                    SyncOperationFingerprintV1.ComputeHash(mutation.Input));
                continue;
            }

            Assert.NotEqual(baselineHash, Hex(SyncOperationFingerprintV1.ComputeHash(mutation.Input)));
        }
    }

    [Fact]
    public void FpV1_does_not_trim_case_fold_or_normalize_silently()
    {
        var baseline = VectorA();
        var baselineHash = Hex(SyncOperationFingerprintV1.ComputeHash(baseline));
        Assert.NotEqual(baselineHash, Hex(SyncOperationFingerprintV1.ComputeHash(
            baseline with { ActionCode = " " + baseline.ActionCode })));
        Assert.NotEqual(baselineHash, Hex(SyncOperationFingerprintV1.ComputeHash(
            baseline with { ActionCode = baseline.ActionCode.ToLowerInvariant() })));
        Assert.Throws<ArgumentException>(() => SyncOperationFingerprintV1.Encode(
            baseline with { ActionCode = "Cafe\u0301" }));
    }

    [Fact]
    public void FpV1_rejects_noncanonical_symbols_missing_text_invalid_hash_and_zero_operation_correlation()
    {
        var baseline = VectorA();
        Assert.Throws<ArgumentException>(() => SyncOperationFingerprintV1.Encode(
            baseline with { ProtocolVersion = "sync-v2" }));
        Assert.Throws<ArgumentException>(() => SyncOperationFingerprintV1.Encode(
            baseline with { OperationType = "UPSERT" }));
        Assert.Throws<ArgumentException>(() => SyncOperationFingerprintV1.Encode(
            baseline with { ClientOperationId = "" }));
        Assert.Throws<ArgumentException>(() => SyncOperationFingerprintV1.Encode(
            baseline with { ActionCode = new string('a', 121) }));
        Assert.Throws<ArgumentException>(() => SyncOperationFingerprintV1.Encode(
            baseline with { ClientOperationId = "bad\ud800" }));
        Assert.Throws<ArgumentException>(() => SyncOperationFingerprintV1.Encode(
            baseline with { ClientOperationId = "bad\0value" }));
        SyncOperationFingerprintV1.Encode(
            baseline with { ClientOperationId = string.Concat(Enumerable.Repeat("😀", 120)) });
        Assert.Throws<ArgumentException>(() => SyncOperationFingerprintV1.Encode(
            baseline with { ClientOperationId = string.Concat(Enumerable.Repeat("😀", 121)) }));
        Assert.Throws<ArgumentException>(() => SyncOperationFingerprintV1.Encode(
            baseline with { PayloadHash = new string('0', 63) }));
        Assert.Throws<ArgumentException>(() => SyncOperationFingerprintV1.Encode(
            baseline with { PayloadHash = new string('z', 64) }));
        Assert.Throws<ArgumentException>(() => SyncOperationFingerprintV1.Encode(
            baseline with { OperationCorrelationId = Guid.Empty }));
    }

    [Fact]
    public void FpV1_time_is_utc_microseconds_without_rounding()
    {
        var baseline = VectorA();
        var timeFrame = Frame(ReadFrames(SyncOperationFingerprintV1.Encode(baseline)), 0x000c);
        Assert.Equal(8, timeFrame.Value!.Length);
        Assert.Equal(1_787_702_400_123_456L, BinaryPrimitives.ReadInt64BigEndian(timeFrame.Value));

        Assert.Throws<ArgumentException>(() => SyncOperationFingerprintV1.Encode(
            baseline with { ClientOccurredAt = "2026-08-26T00:00:00.123456+00:00" }));
        Assert.Throws<ArgumentException>(() => SyncOperationFingerprintV1.Encode(
            baseline with { ClientOccurredAt = "2026-08-26T00:00:00.1234560Z" }));
        Assert.Throws<ArgumentException>(() => SyncOperationFingerprintV1.Encode(
            baseline with { ClientOccurredAt = "2026-08-26T00:00:00.Z" }));

        var beforeEpoch = Frame(ReadFrames(SyncOperationFingerprintV1.Encode(
            baseline with { ClientOccurredAt = "1969-12-31T23:59:59.999999Z" })), 0x000c);
        Assert.Equal(-1L, BinaryPrimitives.ReadInt64BigEndian(beforeEpoch.Value));
    }

    [Fact]
    public void FpV1_uses_rfc4122_uuid_network_order_for_nonrepeating_segments()
    {
        var frames = ReadFrames(SyncOperationFingerprintV1.Encode(VectorA() with
        {
            CompanyId = Guid.Parse("00112233-4455-6677-8899-aabbccddeeff")
        }));
        Assert.Equal(Convert.FromHexString("00112233445566778899aabbccddeeff"), frames[0].Value);
    }

    [Fact]
    public void FpV1_is_culture_invariant()
    {
        var input = VectorB();
        var expected = Hex(SyncOperationFingerprintV1.ComputeHash(input));
        var originalCulture = CultureInfo.CurrentCulture;
        var originalUiCulture = CultureInfo.CurrentUICulture;
        try
        {
            foreach (var cultureName in new[] { "ar-SA", "tr-TR", "en-US" })
            {
                CultureInfo.CurrentCulture = new CultureInfo(cultureName);
                CultureInfo.CurrentUICulture = new CultureInfo(cultureName);
                Assert.Equal(expected, Hex(SyncOperationFingerprintV1.ComputeHash(input)));
            }
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUiCulture;
        }
    }

    [Fact]
    public void FpV1_input_surface_contains_exactly_the_fourteen_contract_fields()
    {
        var expected = new[]
        {
            "CompanyId", "RegisteredDeviceId", "UserId", "BranchId", "ProtocolVersion", "ActionCode",
            "OperationType", "EntityType", "EntityId", "ClientOperationId", "PayloadHash", "ClientOccurredAt",
            "BaseVersion", "OperationCorrelationId"
        };
        Assert.Equal(expected, typeof(SyncOperationFingerprintV1Input).GetProperties().Select(x => x.Name));
    }

    private static SyncOperationFingerprintV1Input VectorA()
        => new(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Guid.Parse("33333333-3333-3333-3333-333333333333"),
            null,
            "sync-v1",
            "CreateWaybillDraft",
            "CREATE",
            "Waybill",
            null,
            "op-0001",
            "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f",
            "2026-08-26T00:00:00.123456Z",
            null,
            Guid.Parse("44444444-4444-4444-4444-444444444444"));

    private static SyncOperationFingerprintV1Input VectorB()
        => new(
            Guid.Parse("aaaaaaaa-aaaa-4aaa-8aaa-aaaaaaaaaaaa"),
            Guid.Parse("bbbbbbbb-bbbb-4bbb-8bbb-bbbbbbbbbbbb"),
            Guid.Parse("cccccccc-cccc-4ccc-8ccc-cccccccccccc"),
            Guid.Parse("dddddddd-dddd-4ddd-8ddd-dddddddddddd"),
            "sync-v1",
            "UpdateWaybillDraft",
            "UPDATE",
            "Waybill",
            Guid.Parse("eeeeeeee-eeee-4eee-8eee-eeeeeeeeeeee"),
            "طلب-١",
            new string('f', 64),
            "1970-01-01T00:00:00Z",
            42,
            Guid.Parse("ffffffff-ffff-4fff-8fff-ffffffffffff"));

    private static void AssertVector(
        SyncOperationFingerprintV1Input input,
        int expectedLength,
        string expectedHash,
        string expectedPrefix,
        string expectedSuffix)
    {
        var encoded = SyncOperationFingerprintV1.Encode(input);
        Assert.Equal(expectedLength, encoded.Length);
        Assert.Equal(expectedHash, Hex(SyncOperationFingerprintV1.ComputeHash(input)));
        Assert.Equal(expectedPrefix, Hex(encoded.AsSpan(0, 96)));
        Assert.Equal(expectedSuffix, Hex(encoded.AsSpan(encoded.Length - 96, 96)));
    }

    private static List<FieldFrame> ReadFrames(byte[] encoded)
    {
        var headerLength = Encoding.ASCII.GetByteCount("TransportERP.SyncOperationFingerprint\0fp-v1\0");
        var fieldCount = BinaryPrimitives.ReadUInt16BigEndian(encoded.AsSpan(headerLength, 2));
        var offset = headerLength + 2;
        var result = new List<FieldFrame>(fieldCount);
        for (var i = 0; i < fieldCount; i++)
        {
            var id = BinaryPrimitives.ReadUInt16BigEndian(encoded.AsSpan(offset, 2));
            offset += 2;
            var presence = encoded[offset++];
            if (presence == 0)
            {
                result.Add(new FieldFrame(id, null));
                continue;
            }

            Assert.Equal((byte)1, presence);
            var length = checked((int)BinaryPrimitives.ReadUInt32BigEndian(encoded.AsSpan(offset, 4)));
            offset += 4;
            var value = encoded.AsSpan(offset, length).ToArray();
            offset += length;
            result.Add(new FieldFrame(id, value));
        }

        Assert.Equal(encoded.Length, offset);
        return result;
    }

    private static FieldFrame Frame(IReadOnlyList<FieldFrame> frames, ushort id)
        => Assert.Single(frames, x => x.Id == id);

    private static void AssertFrameIsNull(IReadOnlyList<FieldFrame> frames, ushort id)
        => Assert.Null(Frame(frames, id).Value);

    private static string Hex(ReadOnlySpan<byte> value)
        => Convert.ToHexString(value).ToLowerInvariant();

    private sealed record FieldFrame(ushort Id, byte[]? Value);
}
