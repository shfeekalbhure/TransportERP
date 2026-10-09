using TransportERP.Contracts.Accounting;

namespace TransportERP.EmptyForms;

public partial class UcScreen_04_04_01
{
    private bool receiptWorkflowBusy;
    private LedgerWorkflowPolicy EffectiveReceiptPolicy =>
        (receiptDocument == null ? receiptBootstrap?.Workflow?.Policy : receiptDocument.Workflow?.Policy) ?? LedgerWorkflowPolicy.Legacy;

    private string? NextReceiptStage()
    {
        var policy = EffectiveReceiptPolicy;
        if (receiptDocument?.State == "DRAFT" && policy.RequireReview) return "Review";
        if (policy.RequireApproval && receiptDocument?.State == (policy.RequireReview ? "REVIEWED" : "DRAFT")) return "Approve";
        return null;
    }

    private void RefreshReceiptWorkflowState()
    {
        var policy = EffectiveReceiptPolicy;
        field_postingMethod.ReadOnly = true;
        field_postingMethod.Text = policy.PostingMode == "AUTOMATIC" ? "تلقائي بعد المراحل المطلوبة" : "يدوي بعد المراحل المطلوبة";
        field_postingMethod.AccessibleDescription = policy.Description;
        receiptHints.SetToolTip(field_postingMethod, policy.Description +
            (receiptDocument == null ? " — سياسة الأستاذ العام للسند الجديد." : " — السياسة المحفوظة لهذا السند."));
        var stage = NextReceiptStage();
        btnReceiptApprove.Text = stage == "Review" ? "مراجعة" : "اعتماد";
        btnReceiptApprove.Enabled = stage != null && !receiptWorkflowBusy && !receiptTabsBusy && !Binding.IsBusy && !HasUnsavedChanges &&
            receiptBootstrap?.Actions.Contains(stage == "Review" ? "accounting.receipts.review" : "accounting.receipts.approve") == true;
    }
}
