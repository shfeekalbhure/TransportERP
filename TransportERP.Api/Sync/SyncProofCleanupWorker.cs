using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Sync;

/// <summary>Bounded server-side retention worker; it contains no Offline enable path.</summary>
public sealed class SyncProofCleanupWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<SyncProofCleanupWorker> logger) : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(1);

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
            var cleanup = scope.ServiceProvider.GetRequiredService<SyncProofCleanupService>();
            var result = await cleanup.CleanupExpiredAsync(DateTimeOffset.UtcNow,
                cancellationToken: cancellationToken);
            if (result.DeletedReplays != 0 || result.DeletedNonces != 0)
                logger.LogInformation("Sync proof retention cleanup deleted {ReplayCount} replay rows and {NonceCount} nonce rows.",
                    result.DeletedReplays, result.DeletedNonces);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // Normal host shutdown.
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Sync proof retention cleanup failed.");
        }
    }
}
