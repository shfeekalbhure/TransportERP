using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Accounting;
using TransportERP.Contracts.Core;

namespace TransportERP.Infrastructure.Persistence;

public sealed partial class ReceiptWorkspaceService
{
    private async Task ValidateReceiptReferences(OperationContext scope, ReceiptDraft draft, ReceiptConfiguration settings, CancellationToken ct)
    {
        foreach (var (key, code) in new[] { ("costCenter", settings.CostCenterDimensionCode), ("project", settings.ProjectDimensionCode), ("activity", settings.ActivityDimensionCode) })
        {
            foreach (var value in draft.Lines.Select(l => l.Additional?.GetValueOrDefault(key)).Append(draft.Additional.GetValueOrDefault(key)).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct())
            {
                if (code == null || !Guid.TryParse(value, out var id) || !await db.FinancialDimensions.AnyAsync(d => d.Id == id && d.CompanyId == scope.CompanyId &&
                    d.DimensionCode == code && d.Status == "ACTIVE" && d.ValidFrom <= draft.Date && (d.ValidTo == null || d.ValidTo >= draft.Date), ct))
                    throw new InvalidOperationException("مرجع " + key + " غير متاح أو خارج مدة صلاحيته.");
            }
        }
        string? kind = draft.Additional.GetValueOrDefault("linkedDocumentKind");
        string? valueId = draft.Additional.GetValueOrDefault("linkedDocumentId");
        if (!string.IsNullOrWhiteSpace(kind) || !string.IsNullOrWhiteSpace(valueId))
        {
            if (!Guid.TryParse(valueId, out var id)) throw new InvalidOperationException("اختر مستندًا مرتبطًا صالحًا.");
            bool found = kind switch
            {
                "WAYBILL" => await db.Set<WaybillEntity>().AnyAsync(w => w.Id == id && w.CompanyId == scope.CompanyId && w.BranchId == scope.BranchId, ct),
                "RECEIPT" => id != draft.Id && await db.ReceiptVouchers.AnyAsync(w => w.Id == id && w.CompanyId == scope.CompanyId && w.BranchId == scope.BranchId, ct),
                "JOURNAL" => await db.JournalEntries.AnyAsync(w => w.Id == id && w.CompanyId == scope.CompanyId && w.BranchId == scope.BranchId, ct),
                _ => false
            };
            if (!found) throw new InvalidOperationException("المستند المرتبط غير موجود في نطاق السند.");
        }
    }
}
