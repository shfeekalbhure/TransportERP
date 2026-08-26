using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Sync;

/// <summary>
/// Registration-ready bounded retention worker. Registration is intentionally
/// left to the composition root after review; this worker has no Offline gate.
/// </summary>
public sealed class SyncRetentionCleanupWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<SyncRetentionCleanupWorker> logger) : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromHours(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await RunOnceAsync(stoppingToken);
        using var timer = new PeriodicTimer(Interval);
        while (await timer.WaitForNextTickAsync(stoppingToken))
            await RunOnceAsync(stoppingToken);
    }

    private async Task RunOnceAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var cleanup = scope.ServiceProvider.GetRequiredService<SyncRetentionCleanupService>();
            var result = await cleanup.CleanupBatchAsync(cancellationToken: cancellationToken);
            if (result.RedactedOperations != 0 || result.RedactedConflictCases != 0)
                logger.LogInformation(
                    "Sync retention redacted {OperationCount} operation payloads and {ConflictCount} conflict snapshots.",
                    result.RedactedOperations, result.RedactedConflictCases);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // Normal host shutdown.
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Sync retention cleanup iteration failed.");
        }
    }
}
