namespace TransportERP.Infrastructure.Persistence;

public abstract record SyncActionExecutionOutcome
{
    private SyncActionExecutionOutcome() { }

    public sealed record Succeeded(Guid ResultEntityId, long? ResultVersion) : SyncActionExecutionOutcome;
    public sealed record Failed(string ErrorCode) : SyncActionExecutionOutcome;
}

/// <summary>
/// Explicit extension point for the typed business dispatcher. This foundation deliberately does
/// not provide a generic/reflection dispatcher or any business handler.
/// </summary>
public interface ISyncActionExecutor
{
    Task<SyncActionExecutionOutcome> ExecuteAsync(
        SyncOperationExecutionClaim claim,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Executes at most one operation. A crash or cancellation after claim leaves SENDING intact so a
/// later worker can recover it after the lease; neither condition consumes the retry counter.
/// </summary>
public sealed class SyncExecutionProcessor(
    SyncOperationService operations,
    ISyncActionExecutor executor)
{
    public async Task<bool> ExecuteNextAsync(
        TimeSpan leaseDuration,
        DateTimeOffset? now = null,
        CancellationToken cancellationToken = default)
    {
        var claim = await operations.ClaimNextExecutionAsync(
            leaseDuration, now, cancellationToken);
        if (claim is null)
            return false;

        SyncActionExecutionOutcome outcome;
        try
        {
            outcome = await executor.ExecuteAsync(claim, cancellationToken)
                ?? throw new InvalidOperationException("Sync executor returned no outcome.");
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // No completion mutation: restart recovery owns the expired lease.
            throw;
        }
        catch
        {
            // An unclassified executor exception is fail-closed and is never silently retried.
            outcome = new SyncActionExecutionOutcome.Failed("ACTION_EXECUTION_FAILED");
        }

        switch (outcome)
        {
            case SyncActionExecutionOutcome.Succeeded succeeded:
                await operations.CompleteExecutionSuccessAsync(
                    claim.OperationId,
                    claim.ClaimToken,
                    new SyncExecutionSuccess(succeeded.ResultEntityId, succeeded.ResultVersion),
                    cancellationToken: cancellationToken);
                break;
            case SyncActionExecutionOutcome.Failed failed:
                await operations.CompleteExecutionFailureAsync(
                    claim.OperationId,
                    claim.ClaimToken,
                    failed.ErrorCode,
                    cancellationToken: cancellationToken);
                break;
            default:
                throw new InvalidOperationException("Unsupported sync execution outcome.");
        }

        return true;
    }
}
