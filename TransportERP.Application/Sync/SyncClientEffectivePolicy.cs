namespace TransportERP.Application.Sync;

/// <summary>
/// Exact client projection of the server-resolved Global→Company→Branch→Device policy. Clients
/// accept no local defaults once an authenticated Offline activation is requested.
/// </summary>
public sealed record SyncClientEffectivePolicy(
    int MaxBatchOperations,
    int ClientTransportMaxRetryCount,
    int ClientTransportBaseSeconds,
    int ClientTransportMaxDelayMinutes,
    int LocalSuccessHours,
    int LocalRejectedDays,
    int ServerPayloadDays,
    int CacheMaxAgeHours,
    int MaximumRequestBodyBytes,
    int MaximumPayloadBytes,
    string SourceVersion,
    string SourceFingerprint,
    string ActivationImplementationSha)
{
    public bool IsValid =>
        MaxBatchOperations is >= 1 and <= 100 &&
        ClientTransportMaxRetryCount is >= 0 and <= 5 &&
        ClientTransportBaseSeconds is >= 1 and <= 60 &&
        ClientTransportMaxDelayMinutes is >= 1 and <= 30 &&
        TimeSpan.FromMinutes(ClientTransportMaxDelayMinutes) >= TimeSpan.FromSeconds(ClientTransportBaseSeconds) &&
        LocalSuccessHours is >= 1 and <= 24 &&
        LocalRejectedDays is >= 1 and <= 7 &&
        ServerPayloadDays is >= 1 and <= 90 &&
        CacheMaxAgeHours is >= 1 and <= 24 &&
        MaximumRequestBodyBytes is >= 1 and <= 2_097_152 &&
        MaximumPayloadBytes is >= 1 and <= 16_384 &&
        MaximumPayloadBytes <= MaximumRequestBodyBytes &&
        IsSafeSourceVersion(SourceVersion) &&
        IsLowerHex64(SourceFingerprint) &&
        SyncClientDeploymentAuthority.IsAuthorizedImplementation(ActivationImplementationSha);

    public TimeSpan ClientRetryBaseDelay => TimeSpan.FromSeconds(ClientTransportBaseSeconds);
    public TimeSpan ClientRetryMaxDelay => TimeSpan.FromMinutes(ClientTransportMaxDelayMinutes);
    public TimeSpan LocalSuccessRetention => TimeSpan.FromHours(LocalSuccessHours);
    public TimeSpan LocalRejectedRetention => TimeSpan.FromDays(LocalRejectedDays);
    public TimeSpan CacheMaxAge => TimeSpan.FromHours(CacheMaxAgeHours);

    private static bool IsSafeSourceVersion(string? value) => value is { Length: >= 1 and <= 80 } &&
        value.All(character => char.IsAsciiLetterOrDigit(character) || character is '.' or '-' or '_');

    private static bool IsLowerHex64(string? value) => value is { Length: 64 } &&
        value.All(character => character is >= '0' and <= '9' or >= 'a' and <= 'f');
}
