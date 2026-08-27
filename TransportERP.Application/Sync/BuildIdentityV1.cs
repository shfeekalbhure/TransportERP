using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace TransportERP.Application.Sync;

/// <summary>
/// A measured deployment identity. It prevents an implementation-SHA label from authorizing a
/// different artifact; it is not a substitute for platform attestation against a hostile client.
/// </summary>
public sealed record BuildIdentityV1(
    string Platform,
    string ArtifactSha256,
    string? SignerCertificateSha256 = null)
{
    public BuildIdentityV1() : this(string.Empty, string.Empty, null) { }

    public const string DesktopWindowsPlatform = "desktop-windows";
    public const string AndroidPlatform = "android";
    public const string PlatformHeader = "X-TransportERP-Build-Platform";
    public const string ArtifactSha256Header = "X-TransportERP-Build-Artifact-SHA256";
    public const string SignerCertificateSha256Header = "X-TransportERP-Build-Signer-SHA256";

    [JsonIgnore]
    public bool IsValid => Platform switch
    {
        DesktopWindowsPlatform => IsLowerHexSha256(ArtifactSha256) &&
            (SignerCertificateSha256 is null || IsLowerHexSha256(SignerCertificateSha256)),
        AndroidPlatform => IsLowerHexSha256(ArtifactSha256) &&
            IsLowerHexSha256(SignerCertificateSha256),
        _ => false
    };

    public bool FixedTimeEquals(BuildIdentityV1? other) =>
        other is not null && IsValid && other.IsValid &&
        string.Equals(Platform, other.Platform, StringComparison.Ordinal) &&
        FixedLowerHexEquals(ArtifactSha256, other.ArtifactSha256) &&
        ((SignerCertificateSha256 is null && other.SignerCertificateSha256 is null) ||
         (SignerCertificateSha256 is not null && other.SignerCertificateSha256 is not null &&
          FixedLowerHexEquals(SignerCertificateSha256, other.SignerCertificateSha256)));

    public static string Sha256LowerHex(ReadOnlySpan<byte> value) =>
        Convert.ToHexString(SHA256.HashData(value)).ToLowerInvariant();

    public static string Sha256LowerHex(Stream value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return Convert.ToHexString(SHA256.HashData(value)).ToLowerInvariant();
    }

    public static bool IsLowerHexSha256(string? value) => value is { Length: 64 } &&
        value.All(character => character is >= '0' and <= '9' or >= 'a' and <= 'f');

    private static bool FixedLowerHexEquals(string left, string right)
    {
        if (!IsLowerHexSha256(left) || !IsLowerHexSha256(right)) return false;
        var leftBytes = Encoding.ASCII.GetBytes(left);
        var rightBytes = Encoding.ASCII.GetBytes(right);
        try { return CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes); }
        finally
        {
            CryptographicOperations.ZeroMemory(leftBytes);
            CryptographicOperations.ZeroMemory(rightBytes);
        }
    }
}
