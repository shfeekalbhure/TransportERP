using System.Net.Http.Json;
using TransportERP.Contracts.Accounting;

namespace TransportERP.Desktop.Services;

public interface IJournalReportApiClient { Task<JournalReportResponse> QueryAsync(JournalReportQuery query, CancellationToken cancellationToken = default); }

public sealed class JournalReportApiClient(HttpClient httpClient) : IJournalReportApiClient
{
    public async Task<JournalReportResponse> QueryAsync(JournalReportQuery query, CancellationToken cancellationToken = default)
    {
        if (httpClient.BaseAddress is null)
            return new JournalReportResponse(Array.Empty<JournalReportRow>(), 0, false, "API_NOT_CONFIGURED", "خدمة التقارير غير مهيأة.");
        var response = await httpClient.GetFromJsonAsync<JournalReportResponse>("api/accounting/journal-report", cancellationToken);
        return response ?? new JournalReportResponse(Array.Empty<JournalReportRow>(), 0, false, "API_EMPTY_RESPONSE", "تعذر استلام استجابة التقرير.");
    }
}
