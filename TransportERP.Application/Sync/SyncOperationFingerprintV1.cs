using System.Buffers.Binary;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace TransportERP.Application.Sync;

internal sealed record SyncOperationFingerprintV1Input(
    Guid CompanyId,
    Guid RegisteredDeviceId,
    Guid UserId,
    Guid? BranchId,
    string ProtocolVersion,
    string ActionCode,
    string OperationType,
    string EntityType,
    Guid? EntityId,
    string ClientOperationId,
    string PayloadHash,
    string ClientOccurredAt,
    long? BaseVersion,
    Guid OperationCorrelationId);

internal static class SyncOperationFingerprintV1
{
    private const string DomainSeparator = "TransportERP.SyncOperationFingerprint";
    private const string Version = "fp-v1";
    private const ushort FieldCount = 14;
    private const int MaxContractTextLength = 120;
    private static readonly UTF8Encoding StrictUtf8 = new(false, true);
    private static readonly Regex CanonicalUtcTimestamp = new(
        "^[0-9]{4}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}:[0-9]{2}(?:\\.[0-9]{1,6})?Z$",
        RegexOptions.CultureInvariant | RegexOptions.NonBacktracking);
    private static readonly string[] CanonicalUtcFormats =
    [
        "yyyy-MM-dd'T'HH:mm:ss'Z'",
        "yyyy-MM-dd'T'HH:mm:ss.FFFFFF'Z'"
    ];

    internal static byte[] ComputeHash(SyncOperationFingerprintV1Input input)
        => SHA256.HashData(Encode(input));

    internal static byte[] Encode(SyncOperationFingerprintV1Input input)
    {
        ArgumentNullException.ThrowIfNull(input);
        if (input.OperationCorrelationId == Guid.Empty)
            throw new ArgumentException("OperationCorrelationId must be non-zero.", nameof(input));
        if (!string.Equals(input.ProtocolVersion, "sync-v1", StringComparison.Ordinal))
            throw new ArgumentException("ProtocolVersion must be sync-v1.", nameof(input));
        if (input.OperationType is not ("CREATE" or "UPDATE" or "DELETE" or "COMMAND"))
            throw new ArgumentException("OperationType is not canonical.", nameof(input));

        using var output = new MemoryStream(capacity: 384);
        output.Write(Encoding.ASCII.GetBytes(DomainSeparator));
        output.WriteByte(0);
        output.Write(Encoding.ASCII.GetBytes(Version));
        output.WriteByte(0);
        WriteUInt16(output, FieldCount);

        WriteRequired(output, 0x0001, EncodeGuid(input.CompanyId));
        WriteRequired(output, 0x0002, EncodeGuid(input.RegisteredDeviceId));
        WriteRequired(output, 0x0003, EncodeGuid(input.UserId));
        WriteOptional(output, 0x0004, input.BranchId.HasValue ? EncodeGuid(input.BranchId.Value) : null);
        WriteRequired(output, 0x0005, EncodeText(input.ProtocolVersion, nameof(input.ProtocolVersion), 20));
        WriteRequired(output, 0x0006, EncodeText(input.ActionCode, nameof(input.ActionCode), MaxContractTextLength));
        WriteRequired(output, 0x0007, EncodeText(input.OperationType, nameof(input.OperationType), 20));
        WriteRequired(output, 0x0008, EncodeText(input.EntityType, nameof(input.EntityType), MaxContractTextLength));
        WriteOptional(output, 0x0009, input.EntityId.HasValue ? EncodeGuid(input.EntityId.Value) : null);
        WriteRequired(output, 0x000a, EncodeText(input.ClientOperationId, nameof(input.ClientOperationId), MaxContractTextLength));
        WriteRequired(output, 0x000b, DecodePayloadHash(input.PayloadHash));
        WriteRequired(output, 0x000c, EncodeClientOccurredAt(input.ClientOccurredAt));
        WriteOptional(output, 0x000d, input.BaseVersion.HasValue ? EncodeInt64(input.BaseVersion.Value) : null);
        WriteRequired(output, 0x000e, EncodeGuid(input.OperationCorrelationId));

        return output.ToArray();
    }

    private static byte[] EncodeText(string? value, string parameterName, int maxCharacterLength)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("A required fingerprint text field is missing.", parameterName);
        if (value.Contains('\0'))
            throw new ArgumentException("Fingerprint text cannot contain U+0000.", parameterName);
        byte[] encoded;
        try
        {
            encoded = StrictUtf8.GetBytes(value);
        }
        catch (EncoderFallbackException exception)
        {
            throw new ArgumentException("Fingerprint text must be valid Unicode.", parameterName, exception);
        }
        var characterCount = 0;
        foreach (var _ in value.EnumerateRunes())
            characterCount++;
        if (characterCount > maxCharacterLength)
            throw new ArgumentException($"Fingerprint text exceeds {maxCharacterLength} characters.", parameterName);
        if (!value.IsNormalized(NormalizationForm.FormC))
            throw new ArgumentException("Fingerprint text fields must use Unicode NFC.", parameterName);
        return encoded;
    }

    private static byte[] DecodePayloadHash(string? value)
    {
        if (value is null || value.Length != 64)
            throw new ArgumentException("PayloadHash must contain exactly 64 hexadecimal characters.", nameof(value));
        try
        {
            var bytes = Convert.FromHexString(value);
            if (bytes.Length != 32)
                throw new ArgumentException("PayloadHash must decode to 32 bytes.", nameof(value));
            return bytes;
        }
        catch (FormatException exception)
        {
            throw new ArgumentException("PayloadHash must contain only hexadecimal characters.", nameof(value), exception);
        }
    }

    private static byte[] EncodeClientOccurredAt(string? value)
    {
        if (value is null || !CanonicalUtcTimestamp.IsMatch(value) ||
            !DateTimeOffset.TryParseExact(value, CanonicalUtcFormats, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var timestamp))
            throw new ArgumentException(
                "ClientOccurredAt must be an exact UTC Z timestamp with zero to six fractional digits.",
                nameof(value));
        var ticksFromEpoch = timestamp.UtcDateTime.Ticks - DateTime.UnixEpoch.Ticks;
        return EncodeInt64(ticksFromEpoch / 10);
    }

    private static byte[] EncodeGuid(Guid value)
    {
        var bytes = new byte[16];
        if (!value.TryWriteBytes(bytes, bigEndian: true, out var bytesWritten) || bytesWritten != bytes.Length)
            throw new InvalidOperationException("Unable to encode UUID in RFC 4122 network order.");
        return bytes;
    }

    private static byte[] EncodeInt64(long value)
    {
        var bytes = new byte[sizeof(long)];
        BinaryPrimitives.WriteInt64BigEndian(bytes, value);
        return bytes;
    }

    private static void WriteRequired(Stream output, ushort fieldId, byte[] value)
        => WriteField(output, fieldId, value);

    private static void WriteOptional(Stream output, ushort fieldId, byte[]? value)
        => WriteField(output, fieldId, value);

    private static void WriteField(Stream output, ushort fieldId, byte[]? value)
    {
        WriteUInt16(output, fieldId);
        if (value is null)
        {
            output.WriteByte(0);
            return;
        }

        output.WriteByte(1);
        WriteUInt32(output, checked((uint)value.Length));
        output.Write(value);
    }

    private static void WriteUInt16(Stream output, ushort value)
    {
        Span<byte> bytes = stackalloc byte[sizeof(ushort)];
        BinaryPrimitives.WriteUInt16BigEndian(bytes, value);
        output.Write(bytes);
    }

    private static void WriteUInt32(Stream output, uint value)
    {
        Span<byte> bytes = stackalloc byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32BigEndian(bytes, value);
        output.Write(bytes);
    }
}
