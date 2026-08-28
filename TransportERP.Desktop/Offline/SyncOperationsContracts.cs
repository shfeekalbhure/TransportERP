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
    [property: DisplayName("آخر تحديث")] DateTimeOffset UpdatedAt,
    [property: Browsable(false)] SyncConflictReviewDisplay? ConflictReview)
{
    public static SyncOperationDisplayRow From(OfflineOperation operation) => new(
        operation.LocalOperationId,
        operation.ActionCode,
        operation.EntityType,
        operation.Status,
        SyncOperationStatusText.Arabic(operation.Status),
        operation.ClientTransportRetryCount,
        string.IsNullOrWhiteSpace(operation.ResultCode) ? "—" : operation.ResultCode,
        operation.UpdatedAt,
        SyncConflictReviewDisplay.From(operation.ConflictReview));

    [Browsable(false)]
    public bool HasCompleteConflictReview => ConflictReview?.IsDecisionReady == true;
}

public sealed record SyncConflictReviewDisplay(
    long BaseVersion,
    string ConflictReason,
    string LocalSnapshot,
    string ServerSnapshot,
    string Status,
    string Resolution,
    string Resolver,
    string Result,
    bool IsDecisionReady)
{
    internal static SyncConflictReviewDisplay? From(OfflineConflictReview? review)
    {
        if (review is null) return null;
        var local = review.LocalSnapshot is null
            ? "—"
            : $"{review.LocalSnapshot.ActionCode} / {review.LocalSnapshot.EntityType} / " +
              $"{FormatId(review.LocalSnapshot.EntityId)} / v{review.LocalSnapshot.BaseVersion}";
        var server = review.ServerSnapshot is null
            ? "—"
            : $"{review.ServerSnapshot.EntityType} / {FormatId(review.ServerSnapshot.EntityId)} / " +
              $"{(review.ServerSnapshot.Exists ? "موجود" : "غير موجود")} / " +
              $"{(review.ServerSnapshot.CurrentVersion.HasValue ? $"v{review.ServerSnapshot.CurrentVersion}" : "—")}";
        return new SyncConflictReviewDisplay(
            review.BaseVersion,
            review.ConflictReason,
            local,
            server,
            review.Status,
            review.Resolution ?? "—",
            review.ResolvedByAuthorizedUser ? "مستخدم مخوّل" : "—",
            review.ReplacedByOperationId.HasValue ? "أُنشئت عملية بديلة" : review.Resolution ?? "—",
            review.IsDecisionReady);
    }

    private static string FormatId(Guid? value) => value is { } id && id != Guid.Empty ? id.ToString("D") : "—";
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
    public static SyncUiActionResult ReviewRequired() => new(false, "CONFLICT_REVIEW_REQUIRED",
        "تعذر عرض بيانات مراجعة التعارض كاملة؛ لن يُسمح بالقرار قبل تحديثها.");
}
