using System.Buffers;
using System.Buffers.Binary;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace TransportERP.Infrastructure.Persistence;

public sealed record AuditV2CanonicalInput(
    short HashVersion,
    short CanonicalizerVersion,
    string StreamKey,
    long StreamSequence,
    byte[]? PreviousHash,
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid? ActorUserId,
    Guid CompanyId,
    Guid? BranchId,
    string Action,
    string Outcome,
    string EntityType,
    Guid? EntityId,
    Guid CorrelationId,
    Guid OperationId,
    string? DeviceId,
    string? BeforeJson,
    string? AfterJson,
    string? Reason,
    string? Ip,
    string RetentionClass,
    byte[] PayloadDigest);

public static class AuditV2Canonicalizer
{
    private static readonly byte[] Prefix = Encoding.ASCII.GetBytes("TransportERP-Audit-V2\0");

    public static byte[] ComputeHash(AuditV2CanonicalInput input)
        => SHA256.HashData(Encode(input));

    public static byte[] Encode(AuditV2CanonicalInput input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input.StreamKey);
        if (input.StreamSequence < 1) throw new ArgumentOutOfRangeException(nameof(input.StreamSequence));
        if (input.PreviousHash is { Length: not 32 }) throw new ArgumentException("PreviousHash must be 32 bytes.");
        if (input.PayloadDigest.Length != 32) throw new ArgumentException("PayloadDigest must be 32 bytes.");
        if (input.HashVersion != 2 || input.CanonicalizerVersion != 1)
            throw new ArgumentException("Unsupported Audit V2 canonicalizer version.");

        using var stream = new MemoryStream();
        stream.Write(Prefix);
        WriteInt16(stream, input.HashVersion);
        WriteInt16(stream, input.CanonicalizerVersion);
        WriteText(stream, input.StreamKey);
        WriteInt64(stream, input.StreamSequence);
        WriteBytes(stream, input.PreviousHash);
        WriteGuid(stream, input.EventId);
        WriteTimestamp(stream, input.OccurredAt);
        WriteGuid(stream, input.ActorUserId);
        WriteGuid(stream, input.CompanyId);
        WriteGuid(stream, input.BranchId);
        WriteText(stream, input.Action);
        WriteText(stream, input.Outcome);
        WriteText(stream, input.EntityType);
        WriteGuid(stream, input.EntityId);
        WriteGuid(stream, input.CorrelationId);
        WriteGuid(stream, input.OperationId);
        WriteText(stream, input.DeviceId);
        WriteJson(stream, input.BeforeJson);
        WriteJson(stream, input.AfterJson);
        WriteText(stream, input.Reason);
        WriteText(stream, input.Ip);
        WriteText(stream, input.RetentionClass);
        WriteBytes(stream, input.PayloadDigest);
        return stream.ToArray();
    }

    public static byte[] CanonicalizeJsonToUtf8(string json)
    {
        ArgumentNullException.ThrowIfNull(json);
        try
        {
            using var document = JsonDocument.Parse(json, new JsonDocumentOptions
            {
                AllowTrailingCommas = false,
                CommentHandling = JsonCommentHandling.Disallow,
                MaxDepth = 128
            });
            var buffer = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(buffer, new JsonWriterOptions { Indented = false }))
            {
                WriteJsonElement(writer, document.RootElement);
            }
            return buffer.WrittenSpan.ToArray();
        }
        catch (JsonException ex)
        {
            throw new FormatException("Audit JSON is invalid.", ex);
        }
    }

    private static void WriteJsonElement(Utf8JsonWriter writer, JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                writer.WriteStartObject();
                foreach (var property in element.EnumerateObject().OrderBy(x => Encoding.UTF8.GetBytes(x.Name.Normalize(NormalizationForm.FormC)), Utf8BytesComparer.Instance))
                {
                    writer.WritePropertyName(property.Name.Normalize(NormalizationForm.FormC));
                    WriteJsonElement(writer, property.Value);
                }
                writer.WriteEndObject();
                break;
            case JsonValueKind.Array:
                writer.WriteStartArray();
                foreach (var item in element.EnumerateArray()) WriteJsonElement(writer, item);
                writer.WriteEndArray();
                break;
            case JsonValueKind.String:
                writer.WriteStringValue((element.GetString() ?? string.Empty).Normalize(NormalizationForm.FormC));
                break;
            case JsonValueKind.Number:
                writer.WriteRawValue(NormalizeJsonNumber(element.GetRawText()), skipInputValidation: true);
                break;
            case JsonValueKind.True:
                writer.WriteBooleanValue(true);
                break;
            case JsonValueKind.False:
                writer.WriteBooleanValue(false);
                break;
            case JsonValueKind.Null:
                writer.WriteNullValue();
                break;
            default:
                throw new FormatException("Unsupported JSON value kind.");
        }
    }

    private static string NormalizeJsonNumber(string raw)
    {
        var negative = raw.StartsWith('-', StringComparison.Ordinal);
        if (negative) raw = raw[1..];
        var exponent = 0;
        var e = raw.IndexOfAny(['e', 'E']);
        if (e >= 0)
        {
            exponent = int.Parse(raw[(e + 1)..], NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
            raw = raw[..e];
        }
        var dot = raw.IndexOf('.');
        var fractionDigits = dot < 0 ? 0 : raw.Length - dot - 1;
        var digits = dot < 0 ? raw : raw.Remove(dot, 1);
        digits = digits.TrimStart('0');
        if (digits.Length == 0) return "0";

        var decimalPosition = digits.Length - fractionDigits + exponent;
        string result;
        if (decimalPosition <= 0)
            result = "0." + new string('0', -decimalPosition) + digits;
        else if (decimalPosition >= digits.Length)
            result = digits + new string('0', decimalPosition - digits.Length);
        else
            result = digits[..decimalPosition] + "." + digits[decimalPosition..];

        if (result.Contains('.', StringComparison.Ordinal))
        {
            result = result.TrimEnd('0').TrimEnd('.');
            if (result.StartsWith('.', StringComparison.Ordinal)) result = "0" + result;
        }
        return negative && result != "0" ? "-" + result : result;
    }

    private static void WriteJson(Stream stream, string? json)
    {
        if (json is null) { WriteNull(stream); return; }
        WriteBytes(stream, CanonicalizeJsonToUtf8(json));
    }

    private static void WriteText(Stream stream, string? value)
    {
        if (value is null) { WriteNull(stream); return; }
        WriteBytes(stream, Encoding.UTF8.GetBytes(value.Normalize(NormalizationForm.FormC)));
    }

    private static void WriteGuid(Stream stream, Guid? value)
    {
        if (!value.HasValue) { WriteNull(stream); return; }
        WriteBytes(stream, Convert.FromHexString(value.Value.ToString("N", CultureInfo.InvariantCulture)));
    }

    private static void WriteTimestamp(Stream stream, DateTimeOffset value)
    {
        var micros = (value.ToUniversalTime().Ticks - DateTimeOffset.UnixEpoch.Ticks) / TimeSpan.TicksPerMicrosecond;
        Span<byte> bytes = stackalloc byte[8];
        BinaryPrimitives.WriteInt64BigEndian(bytes, micros);
        WriteBytes(stream, bytes);
    }

    private static void WriteInt16(Stream stream, short value)
    {
        Span<byte> bytes = stackalloc byte[2];
        BinaryPrimitives.WriteInt16BigEndian(bytes, value);
        WriteBytes(stream, bytes);
    }

    private static void WriteInt64(Stream stream, long value)
    {
        Span<byte> bytes = stackalloc byte[8];
        BinaryPrimitives.WriteInt64BigEndian(bytes, value);
        WriteBytes(stream, bytes);
    }

    private static void WriteNull(Stream stream)
    {
        Span<byte> length = stackalloc byte[4];
        BinaryPrimitives.WriteInt32BigEndian(length, -1);
        stream.Write(length);
    }

    private static void WriteBytes(Stream stream, ReadOnlySpan<byte> bytes)
    {
        Span<byte> length = stackalloc byte[4];
        BinaryPrimitives.WriteInt32BigEndian(length, bytes.Length);
        stream.Write(length);
        stream.Write(bytes);
    }

    private sealed class Utf8BytesComparer : IComparer<byte[]>
    {
        public static readonly Utf8BytesComparer Instance = new();
        public int Compare(byte[]? x, byte[]? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return -1;
            if (y is null) return 1;
            var length = Math.Min(x.Length, y.Length);
            for (var i = 0; i < length; i++)
            {
                var comparison = x[i].CompareTo(y[i]);
                if (comparison != 0) return comparison;
            }
            return x.Length.CompareTo(y.Length);
        }
    }
}
