using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Sync;

/// <summary>
/// Registration-ready Stage 4 execution worker. Program registration remains intentionally absent
/// until the typed business executor is implemented and the runtime gate is authorized.
/// </summary>
public sealed class SyncExecutionWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<SyncExecutionWorker> logger) : BackgroundService
{
    private static readonly TimeSpan ClaimLease = TimeSpan.FromMinutes(2);
    private static readonly TimeSpan IdleDelay = TimeSpan.FromSeconds(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var processor = scope.ServiceProvider.GetRequiredService<SyncExecutionProcessor>();
                var executed = await processor.ExecuteNextAsync(ClaimLease,
                    cancellationToken: stoppingToken);
                if (!executed)
                    await Task.Delay(IdleDelay, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Stage 4 sync execution worker iteration failed.");
                await Task.Delay(IdleDelay, stoppingToken);
            }
        }
    }
}
