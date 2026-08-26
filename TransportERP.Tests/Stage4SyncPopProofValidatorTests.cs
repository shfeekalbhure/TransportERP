using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using TransportERP.Api.Security;

namespace TransportERP.Tests;

public sealed class Stage4SyncPopProofValidatorTests
{
    private const string Htu = "https://sync.example.test/api/v1/sync/operations:batch";
    private const string Bearer = "stage4-test-bearer-token";
    private static readonly DateTimeOffset Now = DateTimeOffset.FromUnixTimeSeconds(1_788_220_800);

    [Theory]
    [InlineData(-120)]
    [InlineData(30)]
    public void Freshness_inclusive_boundaries_are_accepted(int offsetSeconds)
    {
        var body = Encoding.UTF8.GetBytes("{\"DeviceId\":\"device-a\",\"Operations\":[]}");
        using var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var correlation = Guid.NewGuid();
        var proof = CreateProof(key, body, correlation, Now.AddSeconds(offsetSeconds));

        var verified = new SyncPopProofValidator().Validate(new SyncPopProofValidationInput(
            proof, Bearer, body, Htu, correlation, Now));

        Assert.Equal(correlation, verified.AttemptCorrelationId);
        Assert.Equal(Now.AddSeconds(offsetSeconds), verified.IssuedAt);
        Assert.Equal(43, verified.ProofKeyThumbprint.Length);
    }

    [Theory]
    [InlineData(-121)]
    [InlineData(31)]
    public void Freshness_outside_boundaries_is_rejected(int offsetSeconds)
    {
        var body = Encoding.UTF8.GetBytes("{}");
        using var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var correlation = Guid.NewGuid();
        var proof = CreateProof(key, body, correlation, Now.AddSeconds(offsetSeconds));

        Assert.Throws<SyncPopProofValidationException>(() => new SyncPopProofValidator().Validate(
            new SyncPopProofValidationInput(proof, Bearer, body, Htu, correlation, Now)));
    }

    [Fact]
    public void Body_token_uri_and_correlation_are_bound_to_the_signature()
    {
        var body = Encoding.UTF8.GetBytes("{\"a\":1}");
        using var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var correlation = Guid.NewGuid();
        var proof = CreateProof(key, body, correlation, Now);
        var validator = new SyncPopProofValidator();

        Assert.Throws<SyncPopProofValidationException>(() => validator.Validate(new(
            proof, Bearer, Encoding.UTF8.GetBytes("{\"a\":2}"), Htu, correlation, Now)));
        Assert.Throws<SyncPopProofValidationException>(() => validator.Validate(new(
            proof, Bearer + "-changed", body, Htu, correlation, Now)));
        Assert.Throws<SyncPopProofValidationException>(() => validator.Validate(new(
            proof, Bearer, body, Htu + "/", correlation, Now)));
        Assert.Throws<SyncPopProofValidationException>(() => validator.Validate(new(
            proof, Bearer, body, Htu, Guid.NewGuid(), Now)));
    }

    [Fact]
    public void Duplicate_claims_and_private_jwk_members_are_rejected()
    {
        var body = Encoding.UTF8.GetBytes("{}");
        using var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var correlation = Guid.NewGuid();
        var duplicatePayload = BasePayload(body, correlation, Now).TrimEnd('}') + ",\"jti\":\"duplicate-value-1234\"}";
        var duplicateProof = CreateProofFromJson(key, PublicHeader(key), duplicatePayload);
        Assert.Throws<SyncPopProofValidationException>(() => new SyncPopProofValidator().Validate(new(
            duplicateProof, Bearer, body, Htu, correlation, Now)));

        var publicHeader = PublicHeader(key);
        var privateHeader = publicHeader[..^2] + ",\"d\":\"not-a-private-key\"}}";
        var privateProof = CreateProofFromJson(key, privateHeader, BasePayload(body, correlation, Now));
        Assert.Throws<SyncPopProofValidationException>(() => new SyncPopProofValidator().Validate(new(
            privateProof, Bearer, body, Htu, correlation, Now)));
    }

    [Fact]
    public void Malformed_oversized_missing_nonce_and_invalid_signature_are_fail_closed()
    {
        var body = Encoding.UTF8.GetBytes("{}");
        using var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var correlation = Guid.NewGuid();
        var validator = new SyncPopProofValidator();

        Assert.Throws<SyncPopProofValidationException>(() => validator.Validate(new(
            "not-a-compact-proof", Bearer, body, Htu, correlation, Now)));
        Assert.Throws<SyncPopProofValidationException>(() => validator.Validate(new(
            new string('a', SyncPopProofValidator.MaximumCompactProofBytes + 1),
            Bearer, body, Htu, correlation, Now)));

        var missingNoncePayload = JsonSerializer.Serialize(new
        {
            jti = Guid.NewGuid().ToString("D"), htm = "POST", htu = Htu,
            iat = Now.ToUnixTimeSeconds(),
            ath = Base64Url(SHA256.HashData(Encoding.ASCII.GetBytes(Bearer))),
            tbh = Base64Url(SHA256.HashData(body)), cid = correlation.ToString("D")
        });
        var missingNonce = CreateProofFromJson(key, PublicHeader(key), missingNoncePayload);
        Assert.Throws<SyncPopNonceRequiredException>(() => validator.Validate(new(
            missingNonce, Bearer, body, Htu, correlation, Now)));

        var valid = CreateProof(key, body, correlation, Now);
        var segments = valid.Split('.');
        segments[2] = (segments[2][0] == 'A' ? "B" : "A") + segments[2][1..];
        Assert.Throws<SyncPopProofValidationException>(() => validator.Validate(new(
            string.Join('.', segments), Bearer, body, Htu, correlation, Now)));
    }

    [Theory]
    [InlineData("typ", "not-dpop")]
    [InlineData("alg", "ES384")]
    [InlineData("crv", "P-384")]
    public void Protected_header_profile_is_exact(string field, string replacement)
    {
        var body = Encoding.UTF8.GetBytes("{}");
        using var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var correlation = Guid.NewGuid();
        var header = PublicHeader(key);
        header = field switch
        {
            "typ" => header.Replace("dpop+jwt", replacement, StringComparison.Ordinal),
            "alg" => header.Replace("ES256", replacement, StringComparison.Ordinal),
            _ => header.Replace("P-256", replacement, StringComparison.Ordinal)
        };
        var proof = CreateProofFromJson(key, header, BasePayload(body, correlation, Now));

        Assert.Throws<SyncPopProofValidationException>(() => new SyncPopProofValidator().Validate(new(
            proof, Bearer, body, Htu, correlation, Now)));
    }

    [Fact]
    public void Protected_typ_must_be_literal_and_not_a_normalized_json_escape()
    {
        var body = Encoding.UTF8.GetBytes("{}");
        using var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var correlation = Guid.NewGuid();
        var escapedHeader = PublicHeader(key).Replace("dpop+jwt", "dpop\\u002Bjwt", StringComparison.Ordinal);
        var proof = CreateProofFromJson(key, escapedHeader, BasePayload(body, correlation, Now));

        Assert.Throws<SyncPopProofValidationException>(() => new SyncPopProofValidator().Validate(new(
            proof, Bearer, body, Htu, correlation, Now)));
    }

    [Theory]
    [InlineData("crit", "[\"exp\"]")]
    [InlineData("jku", "\"https://attacker.invalid/jwks.json\"")]
    [InlineData("x5u", "\"https://attacker.invalid/cert.pem\"")]
    [InlineData("x5c", "[\"certificate\"]")]
    [InlineData("kid", "\"attacker-key\"")]
    public void Protected_header_rejects_critical_remote_and_key_selection_members(
        string field,
        string jsonValue)
    {
        var body = Encoding.UTF8.GetBytes("{}");
        using var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var correlation = Guid.NewGuid();
        var header = JsonNode.Parse(PublicHeader(key))!.AsObject();
        header[field] = JsonNode.Parse(jsonValue);
        var proof = CreateProofFromJson(key, header.ToJsonString(), BasePayload(body, correlation, Now));

        Assert.Throws<SyncPopProofValidationException>(() => new SyncPopProofValidator().Validate(new(
            proof, Bearer, body, Htu, correlation, Now)));
    }

    [Theory]
    [InlineData("typ")]
    [InlineData("alg")]
    [InlineData("jwk")]
    [InlineData("kty")]
    [InlineData("crv")]
    [InlineData("x")]
    [InlineData("y")]
    public void Protected_header_requires_every_profile_member(string field)
    {
        var body = Encoding.UTF8.GetBytes("{}");
        using var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var correlation = Guid.NewGuid();
        var header = JsonNode.Parse(PublicHeader(key))!.AsObject();
        if (field is "typ" or "alg" or "jwk")
            _ = header.Remove(field);
        else
            _ = header["jwk"]!.AsObject().Remove(field);
        var proof = CreateProofFromJson(key, header.ToJsonString(), BasePayload(body, correlation, Now));

        Assert.Throws<SyncPopProofValidationException>(() => new SyncPopProofValidator().Validate(new(
            proof, Bearer, body, Htu, correlation, Now)));
    }

    [Fact]
    public void Protected_header_duplicate_member_is_rejected()
    {
        var body = Encoding.UTF8.GetBytes("{}");
        using var key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var correlation = Guid.NewGuid();
        var validHeader = PublicHeader(key);
        var header = validHeader[..^1] + ",\"typ\":\"dpop+jwt\"}";
        var proof = CreateProofFromJson(key, header, BasePayload(body, correlation, Now));

        Assert.Throws<SyncPopProofValidationException>(() => new SyncPopProofValidator().Validate(new(
            proof, Bearer, body, Htu, correlation, Now)));
    }

    private static string CreateProof(ECDsa key, byte[] body, Guid correlation, DateTimeOffset issuedAt)
        => CreateProofFromJson(key, PublicHeader(key), BasePayload(body, correlation, issuedAt));

    private static string CreateProofFromJson(ECDsa key, string headerJson, string payloadJson)
    {
        var header = Base64Url(Encoding.UTF8.GetBytes(headerJson));
        var payload = Base64Url(Encoding.UTF8.GetBytes(payloadJson));
        var signingInput = Encoding.ASCII.GetBytes(header + "." + payload);
        var signature = key.SignData(signingInput, HashAlgorithmName.SHA256,
            DSASignatureFormat.IeeeP1363FixedFieldConcatenation);
        return header + "." + payload + "." + Base64Url(signature);
    }

    private static string PublicHeader(ECDsa key)
    {
        var parameters = key.ExportParameters(false);
        return JsonSerializer.Serialize(new
        {
            typ = "dpop+jwt", alg = "ES256",
            jwk = new { kty = "EC", crv = "P-256", x = Base64Url(parameters.Q.X!), y = Base64Url(parameters.Q.Y!) }
        }, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
    }

    private static string BasePayload(byte[] body, Guid correlation, DateTimeOffset issuedAt)
        => JsonSerializer.Serialize(new
        {
            jti = Guid.NewGuid().ToString("D"),
            htm = "POST",
            htu = Htu,
            iat = issuedAt.ToUnixTimeSeconds(),
            ath = Base64Url(SHA256.HashData(Encoding.ASCII.GetBytes(Bearer))),
            nonce = Base64Url(RandomNumberGenerator.GetBytes(32)),
            tbh = Base64Url(SHA256.HashData(body)),
            cid = correlation.ToString("D")
        });

    private static string Base64Url(byte[] value)
        => Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');
}
