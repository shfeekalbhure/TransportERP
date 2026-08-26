using System.Collections.Concurrent;

namespace TransportERP.Offline.Transport;

public interface IOfflineSyncConnectivity
{
    ValueTask<bool> IsOnlineAsync(CancellationToken cancellationToken = default);
    Task WaitUntilOnlineAsync(CancellationToken cancellationToken = default);
}

public sealed class AlwaysOnlineSyncConnectivity : IOfflineSyncConnectivity
{
    public ValueTask<bool> IsOnlineAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return ValueTask.FromResult(true);
    }

    public Task WaitUntilOnlineAsync(CancellationToken cancellationToken = default) =>
        Task.CompletedTask;
}

public sealed record OfflineSyncSupervisorOptions(
    int MaximumBatchOperations = 100,
    TimeSpan? IdleWakeInterval = null,
    TimeSpan? RetentionSweepInterval = null,
    OfflineRetentionPolicy? RetentionPolicy = null,
    TimeSpan? FailureRecoveryBaseDelay = null,
    TimeSpan? FailureRecoveryMaxDelay = null)
{
    public TimeSpan EffectiveIdleWakeInterval => IdleWakeInterval ?? TimeSpan.FromSeconds(30);
    public TimeSpan EffectiveRetentionSweepInterval => RetentionSweepInterval ?? TimeSpan.FromHours(1);
    public TimeSpan EffectiveFailureRecoveryBaseDelay => FailureRecoveryBaseDelay ?? TimeSpan.FromSeconds(1);
    public TimeSpan EffectiveFailureRecoveryMaxDelay => FailureRecoveryMaxDelay ?? TimeSpan.FromSeconds(30);

    internal void Validate()
    {
        if (MaximumBatchOperations is < 1 or > 100)
            throw new ArgumentOutOfRangeException(nameof(MaximumBatchOperations));
        if (EffectiveIdleWakeInterval <= TimeSpan.Zero || EffectiveIdleWakeInterval > TimeSpan.FromMinutes(5))
            throw new ArgumentOutOfRangeException(nameof(IdleWakeInterval));
        if (EffectiveRetentionSweepInterval <= TimeSpan.Zero || EffectiveRetentionSweepInterval > TimeSpan.FromDays(1))
            throw new ArgumentOutOfRangeException(nameof(RetentionSweepInterval));
        if (EffectiveFailureRecoveryBaseDelay <= TimeSpan.Zero ||
            EffectiveFailureRecoveryBaseDelay > TimeSpan.FromMinutes(1))
            throw new ArgumentOutOfRangeException(nameof(FailureRecoveryBaseDelay));
        if (EffectiveFailureRecoveryMaxDelay < EffectiveFailureRecoveryBaseDelay ||
            EffectiveFailureRecoveryMaxDelay > TimeSpan.FromMinutes(5))
            throw new ArgumentOutOfRangeException(nameof(FailureRecoveryMaxDelay));
    }

    internal TimeSpan DelayForFailure(int consecutiveFailureCount)
    {
        var exponent = Math.Clamp(consecutiveFailureCount - 1, 0, 30);
        var multiplier = 1L << exponent;
        var ticks = EffectiveFailureRecoveryBaseDelay.Ticks > long.MaxValue / multiplier
            ? long.MaxValue
            : EffectiveFailureRecoveryBaseDelay.Ticks * multiplier;
        return TimeSpan.FromTicks(Math.Min(ticks, EffectiveFailureRecoveryMaxDelay.Ticks));
    }
}

public sealed record OfflineSyncSupervisorFailure(
    string Code,
    int ConsecutiveFailureCount,
    DateTimeOffset ObservedAt);

/// <summary>
/// Cancellable local drain supervisor. A process-wide path gate prevents two supervisors from
/// draining the same encrypted outbox, while the store lease remains the crash/restart boundary.
/// Queue notifications avoid idle polling latency; connectivity and durable NextRetryAt values
/// determine every other wake-up.
/// </summary>
public sealed class OfflineSyncSupervisor
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> PathGates =
        new(StringComparer.OrdinalIgnoreCase);

    private readonly OfflineOperationStore _store;
    private readonly OfflineSyncTransportClient _transport;
    private readonly IOfflineSyncConnectivity _connectivity;
    private readonly OfflineSyncSupervisorOptions _options;
    private readonly TimeProvider _timeProvider;
    private readonly SemaphoreSlim _wake = new(0, 1);
    private OfflineSyncSupervisorFailure? _lastObservedFailure;

    /// <summary>
    /// Sanitized diagnostic for the most recent recovered loop failure. Exception messages are
    /// deliberately excluded because they may contain local paths or transport data.
    /// </summary>
    public OfflineSyncSupervisorFailure? LastObservedFailure =>
        Volatile.Read(ref _lastObservedFailure);

    public OfflineSyncSupervisor(
        OfflineOperationStore store,
        OfflineSyncTransportClient transport,
        IOfflineSyncConnectivity connectivity,
        OfflineSyncSupervisorOptions? options = null,
        TimeProvider? timeProvider = null)
    {
        _store = store ?? throw new ArgumentNullException(nameof(store));
        _transport = transport ?? throw new ArgumentNullException(nameof(transport));
        _connectivity = connectivity ?? throw new ArgumentNullException(nameof(connectivity));
        _options = options ?? new OfflineSyncSupervisorOptions();
        _options.Validate();
        _timeProvider = timeProvider ?? TimeProvider.System;
    }

    public void NotifyWorkAvailable()
    {
        try { _wake.Release(); }
        catch (SemaphoreFullException) { }
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var path = Path.GetFullPath(_store.DatabasePath);
        var gate = PathGates.GetOrAdd(path, static _ => new SemaphoreSlim(1, 1));
        if (!await gate.WaitAsync(0, cancellationToken))
            throw new OfflineStoreException(
                "LOCAL_SYNC_SUPERVISOR_ALREADY_RUNNING",
                "Only one local sync supervisor may drain an encrypted outbox.");

        try
        {
            var nextRetentionSweepAt = _timeProvider.GetUtcNow();
            var consecutiveFailureCount = 0;
            while (true)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var now = _timeProvider.GetUtcNow();
                    if (now >= nextRetentionSweepAt)
                    {
                        await _store.RedactExpiredPayloadsAsync(_options.RetentionPolicy, cancellationToken);
                        nextRetentionSweepAt = _timeProvider.GetUtcNow() + _options.EffectiveRetentionSweepInterval;
                    }

                    if (!await _connectivity.IsOnlineAsync(cancellationToken))
                    {
                        await WaitForConnectivityOrRetentionAsync(nextRetentionSweepAt, cancellationToken);
                        consecutiveFailureCount = 0;
                        continue;
                    }

                    await _transport.ProcessNextBatchAsync(_options.MaximumBatchOperations, cancellationToken);
                    var nextWorkAt = await _store.GetNextWorkAtAsync(_transport.Scope, cancellationToken);
                    if (nextWorkAt is { } due && due <= _timeProvider.GetUtcNow())
                    {
                        consecutiveFailureCount = 0;
                        continue;
                    }

                    var workDelay = nextWorkAt is null
                        ? _options.EffectiveIdleWakeInterval
                        : Min(nextWorkAt.Value - _timeProvider.GetUtcNow(), _options.EffectiveIdleWakeInterval);
                    var retentionDelay = nextRetentionSweepAt - _timeProvider.GetUtcNow();
                    var delay = Min(workDelay, retentionDelay);
                    consecutiveFailureCount = 0;
                    await WaitForWakeOrDelayAsync(delay, cancellationToken);
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    throw;
                }
                catch (Exception)
                {
                    consecutiveFailureCount++;
                    Volatile.Write(ref _lastObservedFailure, new OfflineSyncSupervisorFailure(
                        "SUPERVISOR_ITERATION_FAILED",
                        consecutiveFailureCount,
                        _timeProvider.GetUtcNow()));
                    await WaitForWakeOrDelayAsync(
                        _options.DelayForFailure(consecutiveFailureCount), cancellationToken);
                }
            }
        }
        finally
        {
            gate.Release();
        }
    }

    private async Task WaitForConnectivityOrRetentionAsync(
        DateTimeOffset nextRetentionSweepAt,
        CancellationToken cancellationToken)
    {
        var delay = nextRetentionSweepAt - _timeProvider.GetUtcNow();
        if (delay <= TimeSpan.Zero) return;
        using var waitCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var onlineTask = _connectivity.WaitUntilOnlineAsync(waitCancellation.Token);
        var retentionTask = Task.Delay(delay, _timeProvider, waitCancellation.Token);
        var completed = await Task.WhenAny(onlineTask, retentionTask);
        await completed;
        await waitCancellation.CancelAsync();
    }

    private async Task WaitForWakeOrDelayAsync(TimeSpan delay, CancellationToken cancellationToken)
    {
        if (delay <= TimeSpan.Zero) return;
        using var waitCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var wakeTask = _wake.WaitAsync(waitCancellation.Token);
        var delayTask = Task.Delay(delay, _timeProvider, waitCancellation.Token);
        var completed = await Task.WhenAny(wakeTask, delayTask);
        await completed;
        await waitCancellation.CancelAsync();
    }

    private static TimeSpan Min(TimeSpan left, TimeSpan right) => left <= right ? left : right;
}
