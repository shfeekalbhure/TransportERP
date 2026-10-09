using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Accounting;
using TransportERP.Contracts.Core;

namespace TransportERP.Infrastructure.Persistence;
public sealed partial class ReceiptWorkspaceService
{
    private async Task<ReceiptDocument> WithWorkflow(OperationContext scope, ReceiptDocument document, CancellationToken ct) =>
        document with { Workflow = await new GeneralLedgerPolicyService(db).ForDocumentAsync(scope, "RECEIPT_VOUCHER", document.Draft.Id, ct) };

    public async Task<ReceiptDocument> ReviewAsync(OperationContext scope, Guid id, string version, CancellationToken ct = default)
    {
        await using var transaction = db.Database.IsRelational() ? await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, ct) : null;
        var entity = await Find(scope, id, ct);
        var workflow = await new GeneralLedgerPolicyService(db).ForDocumentAsync(scope, "RECEIPT_VOUCHER", id, ct);
        if (!workflow.Policy.RequireReview) throw new InvalidOperationException("سياسة هذا السند لا تتطلب مراجعة.");
        if (entity.Status is "REVIEWED" or "APPROVED" or "POSTED") return await WithWorkflow(scope, Document(entity), ct);
        Version(entity, version);
        if (entity.Status != "DRAFT") throw new InvalidOperationException("المراجعة متاحة للمسودة المحفوظة فقط.");
        await Validate(scope, Document(entity).Draft, await SettingsAsync(scope.CompanyId, ct), ct);
        entity.Status = "REVIEWED"; Stamp(entity); await Audit(scope, entity, "ReviewReceipt", ct);
        await db.SaveChangesAsync(ct); if (transaction != null) await transaction.CommitAsync(ct);
        return await WithWorkflow(scope, Document(entity), ct);
    }
    // Persist each human stage first. Automatic posting cannot undo a successful save/review/approval.
    // The API supplies a fresh server-side permission query, never a client checkbox or token claim.
    public async Task<ReceiptDocument> CompleteWorkflowAsync(OperationContext scope, ReceiptDocument document,
        Func<Task<bool>> canPost, CancellationToken ct = default)
    {
        document = await GetAsync(scope, document.Draft.Id, ct);
        var policy = document.Workflow!.Policy;
        if (policy.PostingMode != "AUTOMATIC" || !policy.IsReady(document.State)) return document;
        try
        {
            if (!await canPost()) return document with { WorkflowMessage = "حُفظت المرحلة؛ لم يُرحّل السند لأن صلاحية الترحيل الحالية غير متاحة. يستطيع المخوّل ترحيله لاحقًا." };
            return await PostAsync(scope, document.Draft.Id, document.Version, ct);
        }
        catch (Exception ex) when (ex is InvalidOperationException or DbUpdateException or OverflowException or Npgsql.NpgsqlException)
        {
            db.ChangeTracker.Clear();
            var saved = await GetAsync(scope, document.Draft.Id, ct);
            return saved with { WorkflowMessage = saved.State == "POSTED" ? "تم الترحيل." :
                "حُفظت المرحلة ولم يكتمل الترحيل: " + ex.Message + " يمكن إعادة المحاولة بزر الترحيل بعد معالجة السبب." };
        }
    }
}
