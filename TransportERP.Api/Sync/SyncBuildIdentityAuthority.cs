using Microsoft.Extensions.Options;
using TransportERP.Application.Sync;

namespace TransportERP.Api.Sync;

internal static class SyncBuildIdentityAuthority
{
    internal static BuildIdentityV1? ReadMeasured(HttpRequest request)
    {
        if (!TrySingle(request, BuildIdentityV1.PlatformHeader, out var platform) ||
            !TrySingle(request, BuildIdentityV1.ArtifactSha256Header, out var artifact))
            return null;
        var signerValues = request.Headers[BuildIdentityV1.SignerCertificateSha256Header];
        if (signerValues.Count > 1) return null;
        var signer = signerValues.Count == 1 ? signerValues[0] : null;
        if (signerValues.Count == 1 && string.IsNullOrEmpty(signer)) return null;
        var identity = new BuildIdentityV1(platform!, artifact!, signer);
        return identity.IsValid ? identity : null;
    }

    internal static BuildIdentityV1? Authorized(
        BuildIdentityV1? measured,
        IOptions<SyncRuntimePolicyOptions> policy)
    {
        if (measured is not { IsValid: true }) return null;
        var candidates = (policy.Value.OfflineAuthorizedBuilds ?? []).Where(identity =>
                string.Equals(identity.Platform, measured.Platform, StringComparison.Ordinal))
            .Take(2)
            .ToArray();
        if (candidates.Length != 1) return null;
        var candidate = candidates[0];
        return candidate is { IsValid: true } && candidate.FixedTimeEquals(measured) ? candidate : null;
    }

    internal static bool MatchesAuthorized(BuildIdentityV1? measured, IOptions<SyncRuntimePolicyOptions> policy)
        => Authorized(measured, policy) is not null;

    private static bool TrySingle(HttpRequest request, string name, out string? value)
    {
        var values = request.Headers[name];
        value = values.Count == 1 ? values[0] : null;
        return !string.IsNullOrEmpty(value);
    }
}
