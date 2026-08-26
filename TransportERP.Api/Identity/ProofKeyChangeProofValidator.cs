using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace TransportERP.Api.Identity;

public sealed record PublicProofKeyMaterial(string CanonicalJson, string Thumbprint);

public sealed record ProofKeyChangeValidationInput(
    string CompactProof,
    string RawBearerToken,
    ReadOnlyMemory<byte> RawRequestBody,
    string CanonicalHtu,
    Guid ChallengeId,
    Guid ChangeRequestId,
    Guid RegisteredDeviceId,
    string ChangeType,
    string NewProofKeyThumbprint,
    DateTimeOffset ServerNow);

public sealed record VerifiedProofKeyChangeMaterial(
    string Jti,
    string RawChallenge,
    string PublicKeyCanonicalJson,
    string PublicKeyThumbprint,
    DateTimeOffset IssuedAt,
    string Ath,
    string Tbh,
    string Htu);

/// <summary>Strict, side-effect-free verifier for the proof-key lifecycle compact JWS.</summary>
public sealed class ProofKeyChangeProofValidator
{
    public const int MaximumCompactProofBytes = 4096;
    public static readonly TimeSpan MaximumPastAge = TimeSpan.FromSeconds(120);
    public static readonly TimeSpan MaximumFutureSkew = TimeSpan.FromSeconds(30);

    private static readonly HashSet<string> HeaderMembers =
        new(["typ", "alg", "jwk"], StringComparer.Ordinal);
    private static readonly HashSet<string> PayloadMembers = new(
        ["cid", "rid", "did", "ct", "iat", "jti", "chl", "nkt", "htm", "htu", "ath", "tbh"],
        StringComparer.Ordinal);
    private static readonly HashSet<string> JwkMembers =
        new(["crv", "kty", "x", "y"], StringComparer.Ordinal);

    public PublicProofKeyMaterial ReadPublicKey(JsonElement publicJwk)
    {
        EnsureObjectWithExactMembers(publicJwk, JwkMembers);
        RequireExactString(publicJwk, "kty", "EC");
        RequireExactString(publicJwk, "crv", "P-256");
        var x = RequiredString(publicJwk, "x");
        var y = RequiredString(publicJwk, "y");
        if (DecodeBase64Url(x).Length != 32 || DecodeBase64Url(y).Length != 32) throw Invalid();
        using var _ = CreateVerifier(publicJwk); // Reject coordinates that are not a valid P-256 point.
        var canonical = $"{{\"crv\":\"P-256\",\"kty\":\"EC\",\"x\":\"{x}\",\"y\":\"{y}\"}}";
        return new(canonical, Base64Url(SHA256.HashData(Encoding.ASCII.GetBytes(canonical))));
    }

    public VerifiedProofKeyChangeMaterial Validate(ProofKeyChangeValidationInput input)
    {
        if (!IsAscii(input.CompactProof) || Encoding.ASCII.GetByteCount(input.CompactProof) > MaximumCompactProofBytes ||
            !IsAscii(input.RawBearerToken) || string.IsNullOrEmpty(input.RawBearerToken) ||
            input.ChallengeId == Guid.Empty || input.ChangeRequestId == Guid.Empty ||
            input.RegisteredDeviceId == Guid.Empty || !IsCanonicalHtu(input.CanonicalHtu, input.RegisteredDeviceId, input.ChangeType))
            throw Invalid();

        var parts = input.CompactProof.Split('.');
        if (parts.Length != 3 || parts.Any(string.IsNullOrEmpty)) throw Invalid();
        var headerBytes = DecodeBase64Url(parts[0]);
        var payloadBytes = DecodeBase64Url(parts[1]);
        var signature = DecodeBase64Url(parts[2]);
        if (signature.Length != 64) throw Invalid();

        using var headerDocument = ParseUniqueObject(headerBytes);
        var header = headerDocument.RootElement;
        EnsureObjectWithExactMembers(header, HeaderMembers);
        RequireExactString(header, "typ", "transporterp-key-change+jwt");
        RequireExactString(header, "alg", "ES256");
        var jwk = header.GetProperty("jwk");
        var publicKey = ReadPublicKey(jwk);
        using (var verifier = CreateVerifier(jwk))
        {
            var signingInput = Encoding.ASCII.GetBytes(parts[0] + "." + parts[1]);
            if (!verifier.VerifyData(signingInput, signature, HashAlgorithmName.SHA256,
                    DSASignatureFormat.IeeeP1363FixedFieldConcatenation)) throw Invalid();
        }

        using var payloadDocument = ParseUniqueObject(payloadBytes);
        var claims = payloadDocument.RootElement;
        EnsureObjectWithExactMembers(claims, PayloadMembers);
        RequireGuid(claims, "cid", input.ChallengeId);
        RequireGuid(claims, "rid", input.ChangeRequestId);
        RequireGuid(claims, "did", input.RegisteredDeviceId);
        RequireExactString(claims, "ct", input.ChangeType);
        RequireExactString(claims, "htm", "POST");
        var htu = RequiredString(claims, "htu");
        if (!FixedEqualsAscii(htu, input.CanonicalHtu)) throw Invalid();
        var nkt = RequiredString(claims, "nkt");
        if (!FixedEqualsAscii(nkt, input.NewProofKeyThumbprint)) throw Invalid();

        var issuedAt = ReadIssuedAt(claims);
        var now = input.ServerNow.ToUniversalTime();
        if (issuedAt < now - MaximumPastAge || issuedAt > now + MaximumFutureSkew) throw Invalid();
        var jti = RequiredString(claims, "jti");
        ValidateJti(jti);
        var challenge = RequiredString(claims, "chl");
        if (DecodeBase64Url(challenge).Length != 32) throw Invalid();

        var expectedAth = Base64Url(SHA256.HashData(Encoding.ASCII.GetBytes(input.RawBearerToken)));
        var ath = RequiredString(claims, "ath");
        if (!FixedEqualsAscii(ath, expectedAth)) throw Invalid();
        var expectedTbh = Base64Url(SHA256.HashData(input.RawRequestBody.Span));
        var tbh = RequiredString(claims, "tbh");
        if (!FixedEqualsAscii(tbh, expectedTbh)) throw Invalid();

        return new(jti, challenge, publicKey.CanonicalJson, publicKey.Thumbprint, issuedAt, ath, tbh, htu);
    }

    public static void RequireMatchingPayloads(
        VerifiedProofKeyChangeMaterial current,
        VerifiedProofKeyChangeMaterial next)
    {
        if (string.Equals(current.Jti, next.Jti, StringComparison.Ordinal) ||
            current.IssuedAt != next.IssuedAt ||
            !FixedEqualsAscii(current.RawChallenge, next.RawChallenge) ||
            !FixedEqualsAscii(current.Ath, next.Ath) ||
            !FixedEqualsAscii(current.Tbh, next.Tbh) ||
            !FixedEqualsAscii(current.Htu, next.Htu))
            throw Invalid();
    }

    public static byte[] DecodeChallenge(string rawChallenge) => DecodeBase64Url(rawChallenge);

    private static ECDsa CreateVerifier(JsonElement jwk)
    {
        try
        {
            return ECDsa.Create(new ECParameters
            {
                Curve = ECCurve.NamedCurves.nistP256,
                Q = new ECPoint
                {
                    X = DecodeBase64Url(RequiredString(jwk, "x")),
                    Y = DecodeBase64Url(RequiredString(jwk, "y"))
                }
            });
        }
        catch (Exception exception) when (exception is CryptographicException or ArgumentException or PlatformNotSupportedException)
        {
            throw Invalid();
        }
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

    private static void EnsureObjectWithExactMembers(JsonElement value, IReadOnlySet<string> expected)
    {
        if (value.ValueKind != JsonValueKind.Object) throw Invalid();
        EnsureUniqueObject(value);
        var actual = value.EnumerateObject().Select(x => x.Name).ToHashSet(StringComparer.Ordinal);
        if (!actual.SetEquals(expected)) throw Invalid();
    }

    private static void EnsureUniqueObject(JsonElement value)
    {
        var names = new HashSet<string>(StringComparer.Ordinal);
        foreach (var property in value.EnumerateObject())
            if (!names.Add(property.Name)) throw Invalid();
    }

    private static void RequireGuid(JsonElement value, string name, Guid expected)
    {
        var text = RequiredString(value, name);
        if (!Guid.TryParseExact(text, "D", out var actual) || actual != expected) throw Invalid();
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

    private static byte[] DecodeBase64Url(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Contains('=') || value.Any(c =>
                c is not (>= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '0' and <= '9' or '-' or '_')))
            throw Invalid();
        try
        {
            var padded = value.Replace('-', '+').Replace('_', '/');
            padded += new string('=', (4 - padded.Length % 4) % 4);
            return Convert.FromBase64String(padded);
        }
        catch (FormatException) { throw Invalid(); }
    }

    private static bool IsCanonicalHtu(string value, Guid deviceId, string changeType)
    {
        var suffix = changeType switch
        {
            "BIND" => "bind-proof-key",
            "ROTATE" => "rotate-proof-key",
            "RECOVER" => "recover-proof-key",
            _ => null
        };
        return suffix is not null && Uri.TryCreate(value, UriKind.Absolute, out var uri) &&
               uri.Scheme == Uri.UriSchemeHttps && string.IsNullOrEmpty(uri.Query) &&
               string.IsNullOrEmpty(uri.Fragment) &&
               string.Equals(uri.AbsolutePath, $"/api/v1/devices/{deviceId:D}:{suffix}", StringComparison.Ordinal);
    }

    private static bool FixedEqualsAscii(string left, string right)
        => IsAscii(left) && IsAscii(right) && CryptographicOperations.FixedTimeEquals(
            Encoding.ASCII.GetBytes(left), Encoding.ASCII.GetBytes(right));

    private static bool IsAscii(string value) => value.All(c => c <= 0x7f);
    private static string Base64Url(byte[] value)
        => Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    private static ProofKeyLifecycleException Invalid() => new("PROOF_KEY_PROOF_INVALID");
}
