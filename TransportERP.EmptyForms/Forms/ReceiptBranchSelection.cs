using TransportERP.Contracts.Accounting;
using TransportERP.Desktop.CoreUI;

namespace TransportERP.EmptyForms;

public partial class UcScreen_04_04_01 : IWorkspaceCloseGuard
{
    private Guid receiptBranchId;
    private bool receiptBranchBusy;
    private bool receiptBranchUpdating;
    private bool receiptBranchWired;
    private Func<Guid, Task<bool>>? switchReceiptBranch;
    public bool IsBusy => receiptBranchBusy || receiptTabsBusy || receiptWorkflowBusy || Binding?.IsBusy == true;
    public Guid SelectedReceiptBranchId => receiptBranchId;
    public bool CanSelectReceiptBranch => switchReceiptBranch != null && receiptDocument == null && Binding.ExpectedVersion == null &&
        !HasUnsavedChanges && !Binding.IsBusy && !receiptTabsBusy && !receiptWorkflowBusy && !receiptBranchBusy;

    public void ConnectReceiptBranches(Guid currentBranchId, IEnumerable<ReceiptChoice> choices, Func<Guid, Task<bool>> switchBranch)
    {
        var list = choices.Where(c => c.Id != Guid.Empty).GroupBy(c => c.Id).Select(g => g.First()).ToList();
        if (!list.Any(c => c.Id == currentBranchId)) throw new InvalidOperationException("الفرع الحالي غير متاح ضمن قائمة الفروع المسموحة.");
        receiptBranchId = currentBranchId; switchReceiptBranch = switchBranch;
        receiptBranchUpdating = true;
        try
        {
            textBox1.DisplayMember = nameof(ReceiptChoice.Label); textBox1.ValueMember = nameof(ReceiptChoice.Id);
            textBox1.DataSource = list; textBox1.SelectedValue = currentBranchId;
        }
        finally { receiptBranchUpdating = false; }
        if (!receiptBranchWired)
        {
            receiptBranchWired = true;
            textBox1.SelectedIndexChanged += async (_, _) =>
            {
                if (!receiptBranchUpdating && textBox1.SelectedValue is Guid id && id != receiptBranchId)
                    await TrySelectReceiptBranchAsync(id);
            };
            Binding.StateChanged += RefreshReceiptBranchState;
            Binding.DocumentLoaded += RefreshReceiptBranchState;
            Binding.DraftReset += RefreshReceiptBranchState;
        }
        RefreshReceiptBranchState();
    }

    public async Task<bool> TrySelectReceiptBranchAsync(Guid branchId)
    {
        if (branchId == receiptBranchId) return true;
        if (!CanSelectReceiptBranch || !textBox1.Items.Cast<ReceiptChoice>().Any(c => c.Id == branchId))
        { RestoreReceiptBranchSelection(); return false; }
        receiptBranchBusy = true; mainLayout.Enabled = false; RefreshReceiptBranchState();
        try
        {
            bool changed = await switchReceiptBranch!(branchId);
            if (IsDisposed) return changed;
            if (changed) receiptBranchId = branchId;
            else lblStatus.Text = "لم يتغير الفرع؛ بقيت المسودة في نطاقها الحالي.";
            RestoreReceiptBranchSelection();
            return changed;
        }
        catch (Exception ex)
        {
            if (!IsDisposed) { RestoreReceiptBranchSelection(); lblStatus.Text = "تعذر تغيير الفرع: " + ex.Message; }
            return false;
        }
        finally
        {
            receiptBranchBusy = false;
            if (!IsDisposed) { mainLayout.Enabled = true; RefreshReceiptBranchState(); }
        }
    }

    private void RestoreReceiptBranchSelection()
    {
        receiptBranchUpdating = true;
        try { if (receiptBranchId != Guid.Empty) textBox1.SelectedValue = receiptBranchId; }
        finally { receiptBranchUpdating = false; }
    }
    private void SetReceiptBranchName(string name)
    {
        if (receiptBranchId != Guid.Empty) { RestoreReceiptBranchSelection(); return; }
        receiptBranchUpdating = true;
        try
        {
            textBox1.DisplayMember = nameof(ReceiptChoice.Label); textBox1.ValueMember = nameof(ReceiptChoice.Id);
            textBox1.DataSource = new[] { new ReceiptChoice(Guid.Empty, name) }; textBox1.SelectedIndex = 0; textBox1.Enabled = false;
        }
        finally { receiptBranchUpdating = false; }
    }
    private void RefreshReceiptBranchState()
    { if (!IsDisposed && Binding != null) textBox1.Enabled = CanSelectReceiptBranch && textBox1.Items.Count > 1; }
    public void ShowReceiptBranchStatus(string message) => lblStatus.Text = message;
    // FrmMain invokes the batch binding discard guard separately, so do not prompt twice.
    public bool ConfirmLeave() => !IsBusy;
}
