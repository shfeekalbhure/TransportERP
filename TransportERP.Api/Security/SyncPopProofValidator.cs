using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Security;

public sealed record SyncPopProofValidationInput(
    string CompactProof,
    string RawBearerToken,
    ReadOnlyMemory<byte> RawRequestBody,
    string CanonicalHtu,
    Guid AttemptCorrelationId,
    DateTimeOffset ServerNow);

public sealed class SyncPopProofValidationException : InvalidOperationException
{
    public SyncPopProofValidationException() : base("invalid_dpop_proof") { }
}

public sealed class SyncPopNonceRequiredException : InvalidOperationException
{
    public SyncPopNonceRequiredException() : base("use_dpop_nonce") { }
}

/// <summary>
/// Strict, side-effect-free parser and ES256 validator for TransportERP Sync-PoP v1.
/// It deliberately does not accept token/JWK URLs, private JWK members, alternate
/// algorithms, list-folded proofs, or JSON duplicate names.
/// </summary>
public sealed class SyncPopProofValidator
{
    public const int MaximumCompactProofBytes = 4096;
    public static readonly TimeSpan MaximumPastAge = TimeSpan.FromSeconds(120);
    public static readonly TimeSpan MaximumFutureSkew = TimeSpan.FromSeconds(30);

    public VerifiedSyncProofMaterial Validate(SyncPopProofValidationInput input)
    {
        if (input.AttemptCorrelationId == Guid.Empty ||
            !IsAscii(input.CompactProof) || Encoding.ASCII.GetByteCount(input.CompactProof) > MaximumCompactProofBytes ||
            !IsAscii(input.RawBearerToken) || string.IsNullOrEmpty(input.RawBearerToken) ||
            !IsCanonicalHtu(input.CanonicalHtu))
            throw Invalid();

        var segments = input.CompactProof.Split('.');
        if (segments.Length != 3 || segments.Any(string.IsNullOrEmpty)) throw Invalid();
        var protectedBytes = DecodeBase64Url(segments[0]);
        var payloadBytes = DecodeBase64Url(segments[1]);
        var signature = DecodeBase64Url(segments[2]);
        if (signature.Length != 64) throw Invalid();

        RequireRawProtectedTyp(protectedBytes);
        using var protectedJson = ParseUniqueObject(protectedBytes);
        var header = protectedJson.RootElement;
        RequireExactString(header, "typ", "dpop+jwt");
        RequireExactString(header, "alg", "ES256");
        RejectProperties(header, ["crit", "jku", "x5u", "x5c", "kid"]);
        if (!header.TryGetProperty("jwk", out var jwk) || jwk.ValueKind != JsonValueKind.Object)
            throw Invalid();
        EnsureUniqueObject(jwk);
        var (key, thumbprint) = ReadPublicP256Jwk(jwk);
        using (key)
        {
            var signingInput = Encoding.ASCII.GetBytes(segments[0] + "." + segments[1]);
            if (!key.VerifyData(signingInput, signature, HashAlgorithmName.SHA256,
                    DSASignatureFormat.IeeeP1363FixedFieldConcatenation))
                throw Invalid();
        }

        using var payloadJson = ParseUniqueObject(payloadBytes);
        var claims = payloadJson.RootElement;
        var jti = RequiredString(claims, "jti");
        ValidateJti(jti);
        RequireExactString(claims, "htm", "POST");
        var htu = RequiredString(claims, "htu");
        if (!FixedEqualsAscii(htu, input.CanonicalHtu)) throw Invalid();
        var issuedAt = ReadIssuedAt(claims);
        var now = input.ServerNow.ToUniversalTime();
        if (issuedAt < now - MaximumPastAge || issuedAt > now + MaximumFutureSkew) throw Invalid();
        var expectedAth = Base64Url(SHA256.HashData(Encoding.ASCII.GetBytes(input.RawBearerToken)));
        if (!FixedEqualsAscii(RequiredString(claims, "ath"), expectedAth)) throw Invalid();
        var expectedTbh = Base64Url(SHA256.HashData(input.RawRequestBody.Span));
        if (!FixedEqualsAscii(RequiredString(claims, "tbh"), expectedTbh)) throw Invalid();
        var cid = RequiredString(claims, "cid");
        if (!Guid.TryParseExact(cid, "D", out var claimCorrelation) || claimCorrelation != input.AttemptCorrelationId)
            throw Invalid();
        if (!claims.TryGetProperty("nonce", out var nonceClaim)) throw new SyncPopNonceRequiredException();
        if (nonceClaim.ValueKind != JsonValueKind.String) throw Invalid();
        var nonce = nonceClaim.GetString() ?? throw Invalid();
        if (DecodeBase64Url(nonce).Length != 32) throw Invalid();

        return new VerifiedSyncProofMaterial(jti, nonce, thumbprint, issuedAt,
            input.CanonicalHtu, input.AttemptCorrelationId);
    }

    private static (ECDsa Key, string Thumbprint) ReadPublicP256Jwk(JsonElement jwk)
    {
        RejectProperties(jwk, ["d", "p", "q", "dp", "dq", "qi", "oth", "k", "jku", "x5u", "x5c"]);
        var allowed = new HashSet<string>(["crv", "kty", "x", "y"], StringComparer.Ordinal);
        if (jwk.EnumerateObject().Any(property => !allowed.Contains(property.Name))) throw Invalid();
        RequireExactString(jwk, "kty", "EC");
        RequireExactString(jwk, "crv", "P-256");
        var xText = RequiredString(jwk, "x");
        var yText = RequiredString(jwk, "y");
        var x = DecodeBase64Url(xText);
        var y = DecodeBase64Url(yText);
        if (x.Length != 32 || y.Length != 32) throw Invalid();
        ECDsa key;
        try
        {
            key = ECDsa.Create(new ECParameters
            {
                Curve = ECCurve.NamedCurves.nistP256,
                Q = new ECPoint { X = x, Y = y }
            });
        }
        catch (CryptographicException)
        {
            throw Invalid();
        }
        var canonical = $"{{\"crv\":\"P-256\",\"kty\":\"EC\",\"x\":\"{xText}\",\"y\":\"{yText}\"}}";
        return (key, Base64Url(SHA256.HashData(Encoding.ASCII.GetBytes(canonical))));
    }

    private static DateTimeOffset ReadIssuedAt(JsonElement claims)
    {
        if (!claims.TryGetProperty("iat", out var iat) || iat.ValueKind != JsonValueKind.Number ||
            !iat.TryGetInt64(out var seconds)) throw Invalid();
        try { return DateTimeOffset.FromUnixTimeSeconds(seconds); }
        catch (ArgumentOutOfRangeException) { throw Invalid(); }
    }

    private static void ValidateJti(string value)
    {
        if (value.Length is < 16 or > 128) throw Invalid();
        if (Guid.TryParseExact(value, "D", out _))
        {
            var normalized = value.ToLowerInvariant();
            if (normalized[14] != '4' || normalized[19] is not ('8' or '9' or 'a' or 'b')) throw Invalid();
            return;
        }
        if (DecodeBase64Url(value).Length < 12) throw Invalid();
    }

    private static JsonDocument ParseUniqueObject(byte[] utf8)
    {
        try
        {
            var document = JsonDocument.Parse(utf8, new JsonDocumentOptions
            {
                AllowTrailingCommas = false,
                CommentHandling = JsonCommentHandling.Disallow,
                MaxDepth = 16
            });
            if (document.RootElement.ValueKind != JsonValueKind.Object)
            {
                document.Dispose();
                throw Invalid();
            }
            EnsureUniqueObject(document.RootElement);
            return document;
        }
        catch (JsonException) { throw Invalid(); }
    }

    private static void RequireRawProtectedTyp(ReadOnlySpan<byte> utf8)
    {
        try
        {
            var reader = new Utf8JsonReader(utf8, new JsonReaderOptions
            {
                AllowTrailingCommas = false,
                CommentHandling = JsonCommentHandling.Disallow,
                MaxDepth = 16
            });
            while (reader.Read())
            {
                if (reader.TokenType != JsonTokenType.PropertyName || reader.CurrentDepth != 1 ||
                    reader.HasValueSequence || !reader.ValueSpan.SequenceEqual("typ"u8)) continue;
                if (!reader.Read() || reader.TokenType != JsonTokenType.String || reader.HasValueSequence ||
                    !reader.ValueSpan.SequenceEqual("dpop+jwt"u8))
                    throw Invalid();
                return;
            }
        }
        catch (JsonException)
        {
            throw Invalid();
        }
        throw Invalid();
    }

    private static void EnsureUniqueObject(JsonElement value)
    {
        var names = new HashSet<string>(StringComparer.Ordinal);
        foreach (var property in value.EnumerateObject())
            if (!names.Add(property.Name)) throw Invalid();
    }

    private static string RequiredString(JsonElement value, string name)
    {
        if (!value.TryGetProperty(name, out var property) || property.ValueKind != JsonValueKind.String)
            throw Invalid();
        return property.GetString() ?? throw Invalid();
    }

    private static void RequireExactString(JsonElement value, string name, string expected)
    {
        if (!string.Equals(RequiredString(value, name), expected, StringComparison.Ordinal)) throw Invalid();
    }

    private static void RejectProperties(JsonElement value, IReadOnlyCollection<string> rejected)
    {
        foreach (var name in rejected)
            if (value.TryGetProperty(name, out _)) throw Invalid();
    }

    private static byte[] DecodeBase64Url(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Contains('=') || value.Any(character =>
                character is not (>= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '0' and <= '9' or '-' or '_')))
            throw Invalid();
        try
        {
            var padded = value.Replace('-', '+').Replace('_', '/');
            padded += new string('=', (4 - padded.Length % 4) % 4);
            return Convert.FromBase64String(padded);
        }
        catch (FormatException) { throw Invalid(); }
    }

    private static bool IsCanonicalHtu(string value)
    {
        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri) || uri.Scheme != Uri.UriSchemeHttps ||
            string.IsNullOrEmpty(uri.Host) || !string.IsNullOrEmpty(uri.UserInfo) ||
            !string.IsNullOrEmpty(uri.Query) || !string.IsNullOrEmpty(uri.Fragment))
            return false;
        if (string.Equals(uri.AbsolutePath, "/api/v1/sync/operations:batch", StringComparison.Ordinal))
            return true;

        const string prefix = "/api/v1/sync/conflicts/";
        const string suffix = ":resolve";
        if (!uri.AbsolutePath.StartsWith(prefix, StringComparison.Ordinal) ||
            !uri.AbsolutePath.EndsWith(suffix, StringComparison.Ordinal))
            return false;
        var idText = uri.AbsolutePath[prefix.Length..^suffix.Length];
        return Guid.TryParseExact(idText, "D", out var conflictId) &&
               string.Equals(idText, conflictId.ToString("D"), StringComparison.Ordinal);
    }

    private static bool FixedEqualsAscii(string left, string right)
        => IsAscii(left) && IsAscii(right) && CryptographicOperations.FixedTimeEquals(
            Encoding.ASCII.GetBytes(left), Encoding.ASCII.GetBytes(right));

    private static bool IsAscii(string value) => value.All(character => character <= 0x7f);
    private static string Base64Url(byte[] value)
        => Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    private static SyncPopProofValidationException Invalid() => new();
}
