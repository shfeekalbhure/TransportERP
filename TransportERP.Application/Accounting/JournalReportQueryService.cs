using TransportERP.Contracts.Accounting;

namespace TransportERP.Application.Accounting;

public interface IJournalReportQueryService
{
    Task<JournalReportResponse> QueryAsync(JournalReportQuery query, CancellationToken cancellationToken = default);
}

/// <summary>لا يولد بيانات بديلة؛ يعيد مانع التخزين التعاقدي إلى أن يعتمد مستودع القراءة.</summary>
public sealed class JournalReportQueryService : IJournalReportQueryService
{
    public Task<JournalReportResponse> QueryAsync(JournalReportQuery query, CancellationToken cancellationToken = default) =>
        Task.FromResult(new JournalReportResponse(Array.Empty<JournalReportRow>(), 0, false, "STORAGE_NOT_CONFIGURED", "لا يوجد مصدر تخزين معتمد لتقرير دفتر اليومية."));
}
