using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace TransportERP.Offline.Transport;

/// <summary>
/// Supplies the current session bearer from volatile memory. Implementations must not persist it.
/// </summary>
public interface IInMemoryBearerTokenProvider
{
    ValueTask<string> GetBearerTokenAsync(CancellationToken cancellationToken = default);
}

public sealed record DevicePublicP256Jwk(string X, string Y);

/// <summary>
/// An opaque OS-backed device key. Signing is exposed, private key bytes are not.
/// </summary>
public interface IDeviceProofSigningKey
{
    ValueTask<DevicePublicP256Jwk> GetPublicJwkAsync(CancellationToken cancellationToken = default);

    ValueTask<byte[]> SignEs256Async(
        ReadOnlyMemory<byte> signingInput,
        CancellationToken cancellationToken = default);
}

internal sealed class SyncDpopProofFactory(IDeviceProofSigningKey signingKey, TimeProvider timeProvider)
{
    public async ValueTask<string> CreateAsync(
        string canonicalHtu,
        string bearerToken,
        ReadOnlyMemory<byte> body,
        string nonce,
        Guid attemptCorrelationId,
        CancellationToken cancellationToken)
    {
        ValidateAscii(bearerToken, nameof(bearerToken));
        ValidateNonce(nonce);
        if (attemptCorrelationId == Guid.Empty)
            throw new SyncTransportException("ATTEMPT_CORRELATION_REQUIRED", retryable: false);

        var jwk = await signingKey.GetPublicJwkAsync(cancellationToken);
        ValidateCoordinate(jwk.X, nameof(jwk.X));
        ValidateCoordinate(jwk.Y, nameof(jwk.Y));

        var protectedBytes = WriteProtectedHeader(jwk);
        var payloadBytes = WriteClaims(canonicalHtu, bearerToken, body, nonce, attemptCorrelationId);
        var protectedSegment = Base64Url(protectedBytes);
        var payloadSegment = Base64Url(payloadBytes);
        var signingInput = Encoding.ASCII.GetBytes(protectedSegment + "." + payloadSegment);
        byte[] signature;
        try
        {
            signature = await signingKey.SignEs256Async(signingInput, cancellationToken);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(signingInput);
        }

        if (signature.Length != 64)
        {
            CryptographicOperations.ZeroMemory(signature);
            throw new SyncTransportException("DEVICE_PROOF_KEY_INVALID", retryable: false);
        }

        try
        {
            return protectedSegment + "." + payloadSegment + "." + Base64Url(signature);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(signature);
        }
    }

    private byte[] WriteClaims(
        string canonicalHtu,
        string bearerToken,
        ReadOnlyMemory<byte> body,
        string nonce,
        Guid attemptCorrelationId)
    {
        using var output = new MemoryStream();
        using (var writer = new Utf8JsonWriter(output))
        {
            writer.WriteStartObject();
            writer.WriteString("jti", Guid.NewGuid().ToString("D"));
            writer.WriteString("htm", "POST");
            writer.WriteString("htu", canonicalHtu);
            writer.WriteNumber("iat", timeProvider.GetUtcNow().ToUnixTimeSeconds());
            writer.WriteString("ath", Base64Url(SHA256.HashData(Encoding.ASCII.GetBytes(bearerToken))));
            writer.WriteString("tbh", Base64Url(SHA256.HashData(body.Span)));
            writer.WriteString("cid", attemptCorrelationId.ToString("D"));
            writer.WriteString("nonce", nonce);
            writer.WriteEndObject();
        }
        return output.ToArray();
    }

    private static byte[] WriteProtectedHeader(DevicePublicP256Jwk jwk)
    {
        using var output = new MemoryStream();
        using (var writer = new Utf8JsonWriter(output))
        {
            writer.WriteStartObject();
            writer.WriteString("typ", "dpop+jwt");
            writer.WriteString("alg", "ES256");
            writer.WritePropertyName("jwk");
            writer.WriteStartObject();
            writer.WriteString("kty", "EC");
            writer.WriteString("crv", "P-256");
            writer.WriteString("x", jwk.X);
            writer.WriteString("y", jwk.Y);
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
        return output.ToArray();
    }

    private static void ValidateNonce(string nonce)
    {
        try
        {
            if (DecodeBase64Url(nonce).Length != 32)
                throw new FormatException();
        }
        catch (FormatException)
        {
            throw new SyncTransportException("NONCE_CHALLENGE_INVALID", retryable: false);
        }
    }

    private static void ValidateCoordinate(string value, string name)
    {
        try
        {
            if (DecodeBase64Url(value).Length != 32)
                throw new FormatException();
        }
        catch (FormatException)
        {
            throw new SyncTransportException("DEVICE_PROOF_KEY_INVALID", retryable: false, name);
        }
    }

    private static void ValidateAscii(string value, string name)
    {
        if (string.IsNullOrEmpty(value) || value.Any(character => character > 0x7f || char.IsWhiteSpace(character)))
            throw new SyncTransportException("SESSION_TOKEN_INVALID", retryable: false, name);
    }

    internal static string Base64Url(ReadOnlySpan<byte> value) =>
        Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    internal static byte[] DecodeBase64Url(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Contains('=') || value.Any(character =>
                character is not (>= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '0' and <= '9' or '-' or '_')))
            throw new FormatException();
        var padded = value.Replace('-', '+').Replace('_', '/');
        padded += new string('=', (4 - padded.Length % 4) % 4);
        return Convert.FromBase64String(padded);
    }
}

public sealed class SyncTransportException : InvalidOperationException
{
    public SyncTransportException(string code, bool retryable, string? detail = null, Exception? innerException = null)
        : base(detail is null ? code : $"{code}: {detail}", innerException)
    {
        Code = code;
        Retryable = retryable;
    }

    public string Code { get; }
    public bool Retryable { get; }
}
