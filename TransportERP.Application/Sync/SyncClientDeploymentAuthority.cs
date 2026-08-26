using System.Reflection;

namespace TransportERP.Application.Sync;

/// <summary>
/// Single build-pinned trust anchor shared by Desktop and Mobile. The value is assembly metadata
/// produced by the governed build and cannot be replaced by runtime configuration or user input.
/// </summary>
public static class SyncClientDeploymentAuthority
{
    private const string MetadataKey = "TransportERPClientPublicOrigin";

    public static Uri Origin { get; } = LoadOrigin();

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
}
