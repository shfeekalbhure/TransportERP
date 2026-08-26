using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TransportERP.Api.Identity;

namespace TransportERP.Tests;

public sealed class Stage4ProofKeyChangeProofValidatorTests
{
    [Fact]
    public void New_key_proof_validates_exact_body_token_endpoint_and_p256_key()
    {
        using var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var validator = new ProofKeyChangeProofValidator();
        var jwk = PublicJwk(key);
        var publicKey = validator.ReadPublicKey(jwk);
        var fixture = Fixture("BIND", publicKey.Thumbprint);
        var compact = Sign(key, jwk, fixture.Claims);

        var verified = validator.Validate(fixture.Input with { CompactProof = compact });

        Assert.Equal(publicKey.CanonicalJson, verified.PublicKeyCanonicalJson);
        Assert.Equal(publicKey.Thumbprint, verified.PublicKeyThumbprint);
        Assert.Equal(fixture.Challenge, verified.RawChallenge);
    }

    [Fact]
    public void Rotate_requires_distinct_jti_but_other_payload_members_are_identical()
    {
        using var oldKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        using var newKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var validator = new ProofKeyChangeProofValidator();
        var oldJwk = PublicJwk(oldKey);
        var newJwk = PublicJwk(newKey);
        var newMaterial = validator.ReadPublicKey(newJwk);
        var fixture = Fixture("ROTATE", newMaterial.Thumbprint);
        var oldClaims = new Dictionary<string, object?>(fixture.Claims) { ["jti"] = Guid.NewGuid().ToString("D") };
        var newClaims = new Dictionary<string, object?>(fixture.Claims) { ["jti"] = Guid.NewGuid().ToString("D") };

        var current = validator.Validate(fixture.Input with { CompactProof = Sign(oldKey, oldJwk, oldClaims) });
        var next = validator.Validate(fixture.Input with { CompactProof = Sign(newKey, newJwk, newClaims) });

        ProofKeyChangeProofValidator.RequireMatchingPayloads(current, next);
        Assert.Throws<ProofKeyLifecycleException>(() =>
            ProofKeyChangeProofValidator.RequireMatchingPayloads(current, next with { Tbh = Base64Url(RandomNumberGenerator.GetBytes(32)) }));
    }

    [Theory]
    [InlineData(-120, true)]
    [InlineData(-121, false)]
    [InlineData(30, true)]
    [InlineData(31, false)]
    public void Iat_window_is_fail_closed_at_governed_boundaries(int seconds, bool accepted)
    {
        using var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var validator = new ProofKeyChangeProofValidator();
        var jwk = PublicJwk(key);
        var publicKey = validator.ReadPublicKey(jwk);
        var fixture = Fixture("BIND", publicKey.Thumbprint);
        fixture.Claims["iat"] = fixture.Now.AddSeconds(seconds).ToUnixTimeSeconds();
        var input = fixture.Input with { CompactProof = Sign(key, jwk, fixture.Claims) };

        if (accepted) validator.Validate(input);
        else Assert.Throws<ProofKeyLifecycleException>(() => validator.Validate(input));
    }

    [Fact]
    public void Rejects_private_jwk_and_does_not_echo_raw_artifacts()
    {
        using var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var validator = new ProofKeyChangeProofValidator();
        var publicJwk = PublicJwk(key);
        var material = validator.ReadPublicKey(publicJwk);
        var fixture = Fixture("BIND", material.Thumbprint);
        var privateParameters = key.ExportParameters(true);
        var privateJwk = new Dictionary<string, object?>
        {
            ["kty"] = "EC", ["crv"] = "P-256", ["x"] = Base64Url(privateParameters.Q.X!),
            ["y"] = Base64Url(privateParameters.Q.Y!), ["d"] = Base64Url(privateParameters.D!)
        };
        var compact = Sign(key, JsonSerializer.SerializeToElement(privateJwk), fixture.Claims);

        var exception = Assert.Throws<ProofKeyLifecycleException>(() =>
            validator.Validate(fixture.Input with { CompactProof = compact }));

        Assert.Equal("PROOF_KEY_PROOF_INVALID", exception.Code);
        Assert.DoesNotContain(fixture.Challenge, exception.ToString(), StringComparison.Ordinal);
        Assert.DoesNotContain(fixture.Token, exception.ToString(), StringComparison.Ordinal);
        Assert.DoesNotContain("\"d\"", exception.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public void Rejects_body_hash_or_access_token_hash_mismatch()
    {
        using var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var validator = new ProofKeyChangeProofValidator();
        var jwk = PublicJwk(key);
        var publicKey = validator.ReadPublicKey(jwk);
        var fixture = Fixture("BIND", publicKey.Thumbprint);
        var compact = Sign(key, jwk, fixture.Claims);

        Assert.Throws<ProofKeyLifecycleException>(() => validator.Validate(fixture.Input with
        {
            CompactProof = compact,
            RawRequestBody = Encoding.UTF8.GetBytes("{\"changed\":true}")
        }));
        Assert.Throws<ProofKeyLifecycleException>(() => validator.Validate(fixture.Input with
        {
            CompactProof = compact,
            RawBearerToken = "different-token"
        }));
    }

    [Fact]
    public void Public_key_reader_rejects_an_invalid_p256_point_before_challenge_creation()
    {
        var invalid = JsonSerializer.SerializeToElement(new Dictionary<string, object?>
        {
            ["kty"] = "EC", ["crv"] = "P-256",
            ["x"] = Base64Url(new byte[32]), ["y"] = Base64Url(new byte[32])
        });

        var exception = Assert.Throws<ProofKeyLifecycleException>(() =>
            new ProofKeyChangeProofValidator().ReadPublicKey(invalid));

        Assert.Equal("PROOF_KEY_PROOF_INVALID", exception.Code);
    }

    [Theory]
    [InlineData("application/json", null, null)]
    [InlineData("application/json; charset=utf-8", "identity", null)]
    [InlineData("text/json", null, "UNSUPPORTED_MEDIA_TYPE")]
    [InlineData("application/json", "gzip", "UNSUPPORTED_CONTENT_ENCODING")]
    [InlineData("application/json", "identity,gzip", "UNSUPPORTED_CONTENT_ENCODING")]
    public void Change_request_metadata_rejects_non_json_and_non_identity_encoding(
        string contentType, string? contentEncoding, string? expected)
    {
        var context = new Microsoft.AspNetCore.Http.DefaultHttpContext();
        context.Request.ContentType = contentType;
        if (contentEncoding is not null) context.Request.Headers["Content-Encoding"] = contentEncoding;

        Assert.Equal(expected, ProofKeyLifecycleApiModule.ValidateChangeRequestMetadata(context.Request));
    }

    private static ProofFixture Fixture(string changeType, string newThumbprint)
    {
        var now = new DateTimeOffset(2026, 8, 26, 4, 30, 0, TimeSpan.Zero);
        var challengeId = Guid.NewGuid();
        var requestId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var token = "header.payload.signature";
        var body = Encoding.UTF8.GetBytes("{\"changeType\":\"" + changeType + "\"}");
        var challenge = Base64Url(RandomNumberGenerator.GetBytes(32));
        var htu = $"https://erp.example/api/v1/devices/{deviceId:D}:{changeType.ToLowerInvariant()}-proof-key";
        var claims = new Dictionary<string, object?>
        {
            ["cid"] = challengeId.ToString("D"),
            ["rid"] = requestId.ToString("D"),
            ["did"] = deviceId.ToString("D"),
            ["ct"] = changeType,
            ["iat"] = now.ToUnixTimeSeconds(),
            ["jti"] = Guid.NewGuid().ToString("D"),
            ["chl"] = challenge,
            ["nkt"] = newThumbprint,
            ["htm"] = "POST",
            ["htu"] = htu,
            ["ath"] = Base64Url(SHA256.HashData(Encoding.ASCII.GetBytes(token))),
            ["tbh"] = Base64Url(SHA256.HashData(body))
        };
        return new(new ProofKeyChangeValidationInput("placeholder", token, body, htu, challengeId,
            requestId, deviceId, changeType, newThumbprint, now), claims, challenge, token, now);
    }

    private static JsonElement PublicJwk(ECDsa key)
    {
        var parameters = key.ExportParameters(false);
        return JsonSerializer.SerializeToElement(new Dictionary<string, object?>
        {
            ["kty"] = "EC", ["crv"] = "P-256",
            ["x"] = Base64Url(parameters.Q.X!), ["y"] = Base64Url(parameters.Q.Y!)
        });
    }

    private static string Sign(ECDsa key, JsonElement jwk, IReadOnlyDictionary<string, object?> claims)
    {
        var header = JsonSerializer.SerializeToUtf8Bytes(new Dictionary<string, object?>
        {
            ["typ"] = "transporterp-key-change+jwt", ["alg"] = "ES256", ["jwk"] = jwk
        });
        var payload = JsonSerializer.SerializeToUtf8Bytes(claims);
        var first = Base64Url(header);
        var second = Base64Url(payload);
        var signature = key.SignData(Encoding.ASCII.GetBytes(first + "." + second),
            HashAlgorithmName.SHA256, DSASignatureFormat.IeeeP1363FixedFieldConcatenation);
        return first + "." + second + "." + Base64Url(signature);
    }

    private static string Base64Url(byte[] value)
        => Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    private sealed record ProofFixture(
        ProofKeyChangeValidationInput Input,
        Dictionary<string, object?> Claims,
        string Challenge,
        string Token,
        DateTimeOffset Now);
}
