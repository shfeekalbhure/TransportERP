using System.ComponentModel;
using TransportERP.Offline;

namespace TransportERP.Desktop.Offline;

public sealed record SyncOperationDisplayRow(
    [property: Browsable(false)] Guid LocalOperationId,
    [property: DisplayName("الإجراء")] string Action,
    [property: DisplayName("نوع السجل")] string EntityType,
    [property: Browsable(false)] OfflineOperationStatus Status,
    [property: DisplayName("الحالة")] string StatusText,
    [property: DisplayName("عدد المحاولات")] int RetryCount,
    [property: DisplayName("النتيجة")] string Result,
    [property: DisplayName("آخر تحديث")] DateTimeOffset UpdatedAt)
{
    public static SyncOperationDisplayRow From(OfflineOperation operation) => new(
        operation.LocalOperationId,
        operation.ActionCode,
        operation.EntityType,
        operation.Status,
        SyncOperationStatusText.Arabic(operation.Status),
        operation.ClientTransportRetryCount,
        string.IsNullOrWhiteSpace(operation.ResultCode) ? "—" : operation.ResultCode,
        operation.UpdatedAt);
}

public static class SyncOperationStatusText
{
    public static string Arabic(OfflineOperationStatus status) => status switch
    {
        OfflineOperationStatus.Queued => "بانتظار الإرسال",
        OfflineOperationStatus.Sending => "جارٍ الإرسال",
        OfflineOperationStatus.Succeeded => "نجحت",
        OfflineOperationStatus.Failed => "فشلت مؤقتًا",
        OfflineOperationStatus.Conflict => "تعارض",
        OfflineOperationStatus.Rejected => "مرفوضة",
        OfflineOperationStatus.Resolved => "تم الحل",
        _ => "حالة غير معروفة"
    };
}

public enum SyncConflictDecision
{
    KeepServer,
    Reapply
}

public interface ISyncOperationsQuery
{
    Task<IReadOnlyList<OfflineOperation>> ListAsync(CancellationToken cancellationToken = default);
}

public interface ISyncManualRetryService
{
    Task RetryAsync(Guid localOperationId, CancellationToken cancellationToken = default);
}

public interface ISyncConflictActionService
{
    Task ResolveAsync(
        Guid localOperationId,
        SyncConflictDecision decision,
        string reason,
        CancellationToken cancellationToken = default);
}

public interface ISyncOperationsPermissionPolicy
{
    bool CanRetry(OfflineOperation operation);
    bool CanResolveConflict(OfflineOperation operation, SyncConflictDecision decision);
}

public sealed record SyncUiActionResult(bool Succeeded, string Code, string Message)
{
    public static SyncUiActionResult Success(string message) => new(true, "OK", message);
    public static SyncUiActionResult Denied() => new(false, "PERMISSION_DENIED", "لا تملك الصلاحية المطلوبة لهذا الإجراء.");
    public static SyncUiActionResult InvalidState() => new(false, "LOCAL_STATE_CONFLICT", "لا تسمح حالة العملية الحالية بهذا الإجراء.");
}
