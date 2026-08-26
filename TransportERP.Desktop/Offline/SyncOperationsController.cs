using TransportERP.Offline;

namespace TransportERP.Desktop.Offline;

public sealed class SyncOperationsController
{
    private readonly ISyncOperationsQuery _query;
    private readonly ISyncManualRetryService _retry;
    private readonly ISyncConflictActionService _conflicts;
    private readonly ISyncOperationsPermissionPolicy _permissions;
    private IReadOnlyDictionary<Guid, AllowedUiActions> _allowedActions = new Dictionary<Guid, AllowedUiActions>();

    public SyncOperationsController(
        ISyncOperationsQuery query,
        ISyncManualRetryService retry,
        ISyncConflictActionService conflicts,
        ISyncOperationsPermissionPolicy permissions)
    {
        _query = query ?? throw new ArgumentNullException(nameof(query));
        _retry = retry ?? throw new ArgumentNullException(nameof(retry));
        _conflicts = conflicts ?? throw new ArgumentNullException(nameof(conflicts));
        _permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
    }

    public async Task<IReadOnlyList<SyncOperationDisplayRow>> RefreshAsync(CancellationToken cancellationToken = default)
    {
        var operations = await _query.ListAsync(cancellationToken);
        _allowedActions = operations.ToDictionary(
            x => x.LocalOperationId,
            x => new AllowedUiActions(
                x.Status == OfflineOperationStatus.Failed && _permissions.CanRetry(x),
                x.Status == OfflineOperationStatus.Conflict && _permissions.CanResolveConflict(x, SyncConflictDecision.KeepServer),
                x.Status == OfflineOperationStatus.Conflict && _permissions.CanResolveConflict(x, SyncConflictDecision.Reapply)));

        return operations
            .OrderByDescending(x => x.UpdatedAt)
            .Select(SyncOperationDisplayRow.From)
            .ToArray();
    }

    public async Task<SyncUiActionResult> RetryAsync(Guid localOperationId, CancellationToken cancellationToken = default)
    {
        var operation = await FindCurrentAsync(localOperationId, cancellationToken);
        if (operation is null || operation.Status != OfflineOperationStatus.Failed)
            return SyncUiActionResult.InvalidState();
        if (!_permissions.CanRetry(operation))
            return SyncUiActionResult.Denied();

        await _retry.RetryAsync(operation.LocalOperationId, cancellationToken);
        return SyncUiActionResult.Success("أُعيدت العملية إلى قائمة الانتظار.");
    }

    public Task<SyncUiActionResult> KeepServerAsync(Guid localOperationId, string reason,
        CancellationToken cancellationToken = default) =>
        ResolveAsync(localOperationId, SyncConflictDecision.KeepServer, reason, cancellationToken);

    public Task<SyncUiActionResult> ReapplyAsync(Guid localOperationId, string reason,
        CancellationToken cancellationToken = default) =>
        ResolveAsync(localOperationId, SyncConflictDecision.Reapply, reason, cancellationToken);

    public bool CanRetry(SyncOperationDisplayRow? row) =>
        row is not null && _allowedActions.TryGetValue(row.LocalOperationId, out var allowed) && allowed.Retry;

    public bool CanResolve(SyncOperationDisplayRow? row, SyncConflictDecision decision)
    {
        if (row is null || !_allowedActions.TryGetValue(row.LocalOperationId, out var allowed))
            return false;
        return decision == SyncConflictDecision.KeepServer ? allowed.KeepServer : allowed.Reapply;
    }

    private async Task<SyncUiActionResult> ResolveAsync(
        Guid localOperationId,
        SyncConflictDecision decision,
        string reason,
        CancellationToken cancellationToken)
    {
        var operation = await FindCurrentAsync(localOperationId, cancellationToken);
        if (operation is null || operation.Status != OfflineOperationStatus.Conflict)
            return SyncUiActionResult.InvalidState();
        if (!_permissions.CanResolveConflict(operation, decision))
            return SyncUiActionResult.Denied();

        if (string.IsNullOrWhiteSpace(reason))
            return new SyncUiActionResult(false, "CONFLICT_REASON_REQUIRED", "أدخل سببًا واضحًا ومراجعًا للقرار.");

        await _conflicts.ResolveAsync(operation.LocalOperationId, decision, reason, cancellationToken);
        return SyncUiActionResult.Success(
            decision == SyncConflictDecision.KeepServer
                ? "حُفظت نسخة الخادم وأُغلقت العملية المتعارضة."
                : "أُنشئت عملية بديلة لإعادة التطبيق.");
    }

    private async Task<OfflineOperation?> FindCurrentAsync(Guid localOperationId, CancellationToken cancellationToken) =>
        (await _query.ListAsync(cancellationToken)).SingleOrDefault(x => x.LocalOperationId == localOperationId);

    private sealed record AllowedUiActions(bool Retry, bool KeepServer, bool Reapply);
}
