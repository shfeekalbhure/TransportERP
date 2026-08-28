using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace TransportERP.Api.Sync;

/// <summary>
/// Reads the implementation SHA embedded in the running API assembly by the governed build.
/// Runtime configuration can authorize only that exact server binary.
/// </summary>
public static class SyncServerDeploymentAuthority
{
    private const string MetadataKey = "TransportERPImplementationSha";

    public static string? ImplementationSha
    {
        get
        {
            var values = typeof(SyncServerDeploymentAuthority).Assembly
                .GetCustomAttributes<AssemblyMetadataAttribute>()
                .Where(attribute => string.Equals(attribute.Key, MetadataKey, StringComparison.Ordinal))
                .Select(attribute => attribute.Value)
                .ToArray();
            return values.Length == 1 && IsExactSha(values[0]) ? values[0] : null;
        }
    }

    public static bool IsAuthorizedImplementation(string? embeddedSha, string? authorizedSha)
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

    private static bool IsExactSha(string? value) =>
        value is { Length: 40 } &&
        value.All(character => character is >= '0' and <= '9' or >= 'a' and <= 'f');
}
