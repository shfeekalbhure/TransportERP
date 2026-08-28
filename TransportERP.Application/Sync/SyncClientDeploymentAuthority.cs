using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace TransportERP.Application.Sync;

/// <summary>
/// Single build-pinned trust anchor shared by Desktop and Mobile. The value is assembly metadata
/// produced by the governed build and cannot be replaced by runtime configuration or user input.
/// </summary>
public static class SyncClientDeploymentAuthority
{
    private const string MetadataKey = "TransportERPClientPublicOrigin";
    private const string ImplementationShaMetadataKey = "TransportERPImplementationSha";

    public static Uri Origin { get; } = LoadOrigin();
    public static string? ImplementationSha
    {
        get
        {
            var value = MetadataValue(ImplementationShaMetadataKey);
            return IsExactSha(value) ? value : null;
        }
    }

    /// <summary>
    /// Verifies that the exact implementation SHA authorized by the server is embedded by the
    /// governed build in the shared client assembly. Runtime arguments, files and environment
    /// variables are deliberately outside this trust boundary.
    /// </summary>
    public static bool IsAuthorizedImplementation(string? authorizedSha)
    {
        return FixedShaEquals(ImplementationSha, authorizedSha);
    }

    internal static bool FixedShaEquals(string? embeddedSha, string? authorizedSha)
    {
        if (!IsExactSha(embeddedSha) || !IsExactSha(authorizedSha)) return false;
        var embedded = Encoding.ASCII.GetBytes(embeddedSha!);
        var authorized = Encoding.ASCII.GetBytes(authorizedSha!);
        try { return CryptographicOperations.FixedTimeEquals(embedded, authorized); }
        finally
        {
            CryptographicOperations.ZeroMemory(embedded);
            CryptographicOperations.ZeroMemory(authorized);
        }
    }

    private static Uri LoadOrigin()
    {
        var values = typeof(SyncClientDeploymentAuthority).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .Where(attribute => string.Equals(attribute.Key, MetadataKey, StringComparison.Ordinal))
            .Select(attribute => attribute.Value)
            .ToArray();
        if (values.Length != 1 || !Uri.TryCreate(values[0], UriKind.Absolute, out var origin) ||
            origin.Scheme != Uri.UriSchemeHttps || !string.IsNullOrEmpty(origin.UserInfo) ||
            !string.IsNullOrEmpty(origin.Query) || !string.IsNullOrEmpty(origin.Fragment) ||
            origin.AbsolutePath != "/" || string.IsNullOrEmpty(origin.IdnHost))
            throw new InvalidOperationException("SYNC_CLIENT_DEPLOYMENT_AUTHORITY_INVALID");
        return new Uri(origin.GetLeftPart(UriPartial.Authority) + "/", UriKind.Absolute);
    }

    private static string? MetadataValue(string key)
    {
        var values = typeof(SyncClientDeploymentAuthority).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .Where(attribute => string.Equals(attribute.Key, key, StringComparison.Ordinal))
            .Select(attribute => attribute.Value)
            .ToArray();
        return values.Length == 1 ? values[0] : null;
    }

    private static bool IsExactSha(string? value) =>
        value is { Length: 40 } && value.All(character => character is >= '0' and <= '9' or >= 'a' and <= 'f');
}
