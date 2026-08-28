using TransportERP.Offline;
using TransportERP.Offline.Transport;

namespace TransportERP.Mobile.Internal;

internal enum MobileOfflineKernelMode
{
    Closed,
    SecurityUnavailable,
    ReadCacheOnly,
    Ready
}

internal sealed record MobileOfflineKernelConfiguration(
    bool OfflineRuntimeAuthorized,
    bool WriteContractAvailable,
    string OutboxDatabasePath,
    string ReadCacheDatabasePath,
    OfflineSyncTransportOptions TransportOptions,
    OfflineRetryPolicy? RetryPolicy = null);

internal sealed record MobileOfflineKernelResult(
    MobileOfflineKernelMode Mode,
    string ReasonCode,
    OfflineReadCacheStore? ReadCache,
    OfflineOperationStore? Outbox,
    OfflineSyncTransportClient? Transport)
{
    internal static MobileOfflineKernelResult Closed(string reasonCode) =>
        new(MobileOfflineKernelMode.Closed, reasonCode, null, null, null);
}

/// <summary>
/// Shared source-linked composition kernel. It contains no platform implementation and never
/// receives private key bytes, a bearer value, a DPoP proof, or a nonce. Those artifacts remain
/// behind the injected opaque interfaces owned by the native host.
/// </summary>
internal static class MobileOfflineCompositionKernel
{
    internal static async Task<MobileOfflineKernelResult> ComposeAsync(
        MobileOfflineKernelConfiguration configuration,
        ILocalEncryptionKeyProvider encryptionKeys,
        IDeviceProofSigningKey signingKey,
        IInMemoryBearerTokenProvider bearerTokens,
        Func<HttpClient> syncHttpClientProvider,
        Func<CancellationToken, ValueTask<bool>> nativeSecureStorageAvailable,
        Func<CancellationToken, ValueTask<bool>> nativeSigningKeyAvailable,
        Func<bool> platformTransportAvailable,
        TimeProvider? timeProvider,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(encryptionKeys);
        ArgumentNullException.ThrowIfNull(signingKey);
        ArgumentNullException.ThrowIfNull(bearerTokens);
        ArgumentNullException.ThrowIfNull(syncHttpClientProvider);
        ArgumentNullException.ThrowIfNull(nativeSecureStorageAvailable);
        ArgumentNullException.ThrowIfNull(nativeSigningKeyAvailable);
        ArgumentNullException.ThrowIfNull(platformTransportAvailable);

        // There is deliberately no implicit enablement based on network or credentials.
        if (!configuration.OfflineRuntimeAuthorized)
        {
            return MobileOfflineKernelResult.Closed("OFFLINE_CLOSED");
        }

        if (!await ProbeAsync(nativeSecureStorageAvailable, cancellationToken))
        {
            return new(MobileOfflineKernelMode.SecurityUnavailable, "NATIVE_SECURE_STORAGE_UNAVAILABLE", null, null, null);
        }

        ValidatePaths(configuration);
        var scope = new OfflineOperationScope(
            configuration.TransportOptions.CompanyId,
            configuration.TransportOptions.BranchId,
            configuration.TransportOptions.UserId,
            configuration.TransportOptions.RegisteredDeviceId);
        var readCache = new OfflineReadCacheStore(
            configuration.ReadCacheDatabasePath,
            encryptionKeys,
            scope,
            timeProvider);

        try
        {
            await readCache.InitializeAsync(cancellationToken);
        }
        catch (OfflineStoreException exception)
        {
            return new(MobileOfflineKernelMode.SecurityUnavailable, exception.Code, null, null, null);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch
        {
            return new(MobileOfflineKernelMode.SecurityUnavailable, "NATIVE_SECURITY_PROVIDER_FAILED", null, null, null);
        }

        if (!configuration.WriteContractAvailable)
        {
            return new(MobileOfflineKernelMode.ReadCacheOnly, "OFFLINE_WRITE_CONTRACT_REQUIRED", readCache, null, null);
        }

        var transportAvailable = Probe(platformTransportAvailable);
        if (!transportAvailable || !await ProbeAsync(nativeSigningKeyAvailable, cancellationToken))
        {
            var reason = !transportAvailable
                ? "PLATFORM_SYNC_TRANSPORT_UNAVAILABLE"
                : "NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE";
            return new(MobileOfflineKernelMode.ReadCacheOnly, reason, readCache, null, null);
        }

        var outbox = new OfflineOperationStore(
            configuration.OutboxDatabasePath,
            encryptionKeys,
            timeProvider,
            configuration.RetryPolicy);

        try
        {
            await outbox.InitializeAsync(cancellationToken);
            var syncHttpClient = syncHttpClientProvider()
                ?? throw new InvalidOperationException("The platform sync HTTP client is unavailable.");
            var transport = new OfflineSyncTransportClient(
                syncHttpClient,
                outbox,
                bearerTokens,
                signingKey,
                configuration.TransportOptions,
                timeProvider);
            return new(MobileOfflineKernelMode.Ready, "READY", readCache, outbox, transport);
        }
        catch (OfflineStoreException exception)
        {
            return new(MobileOfflineKernelMode.SecurityUnavailable, exception.Code, null, null, null);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (ArgumentException)
        {
            return new(MobileOfflineKernelMode.Closed, "SYNC_CONFIGURATION_INVALID", null, null, null);
        }
        catch
        {
            return new(MobileOfflineKernelMode.SecurityUnavailable, "NATIVE_SECURITY_PROVIDER_FAILED", null, null, null);
        }
    }

    private static async ValueTask<bool> ProbeAsync(
        Func<CancellationToken, ValueTask<bool>> probe,
        CancellationToken cancellationToken)
    {
        try
        {
            return await probe(cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch
        {
            return false;
        }
    }

    private static bool Probe(Func<bool> probe)
    {
        try
        {
            return probe();
        }
        catch
        {
            return false;
        }
    }

    private static void ValidatePaths(MobileOfflineKernelConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(configuration.OutboxDatabasePath) ||
            string.IsNullOrWhiteSpace(configuration.ReadCacheDatabasePath))
        {
            throw new ArgumentException("Separate outbox and read-cache database paths are required.");
        }

        var outboxPath = Path.GetFullPath(configuration.OutboxDatabasePath);
        var readCachePath = Path.GetFullPath(configuration.ReadCacheDatabasePath);
        if (string.Equals(outboxPath, readCachePath, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("The write outbox and read cache cannot share a database file.");
        }
    }
}
