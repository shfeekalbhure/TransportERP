using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Accounting;
using TransportERP.Contracts.Core;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Accounting;
public static partial class ReceiptApiModule
{
    private static void MapLedgerSettings(WebApplication app)
    {
        var group = app.MapGroup("/api/v1/accounting/settings").RequireAuthorization("Authenticated");
        group.MapGet("", (HttpContext http, TransportErpDbContext db, CancellationToken ct) =>
            Execute(http, db, "accounting.general-ledger.view", async scope =>
                await new GeneralLedgerPolicyService(db).GetSettingsAsync(scope,
                    await Allowed(db, scope, GeneralLedgerPolicyService.ConfigurePermission, ct, companyWide: true), ct), ct));
        group.MapPut("", (LedgerSettingsUpdate update, HttpContext http, TransportErpDbContext db, CancellationToken ct) =>
            Execute(http, db, GeneralLedgerPolicyService.ConfigurePermission, async scope =>
            {
                var service = new GeneralLedgerPolicyService(db);
                await service.SaveSettingsAsync(scope, update, ct);
                return await service.GetSettingsAsync(scope, true, ct);
            }, ct));
    }
    private static Task<ReceiptDocument> CompleteReceipt(TransportErpDbContext db, OperationContext scope, ReceiptDocument document, CancellationToken ct) =>
        new ReceiptWorkspaceService(db).CompleteWorkflowAsync(scope, document, () => Allowed(db, scope, "ACC043.Post", ct), ct);
}
