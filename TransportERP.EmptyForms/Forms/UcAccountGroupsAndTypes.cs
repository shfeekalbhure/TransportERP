using System;
using System.Drawing;
using System.Windows.Forms;
using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcAccountGroupsAndTypes : UserControl, IFoundationScreen
{
    public UcAccountGroupsAndTypes()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        UpdateMainDataExtent();
        Foundation = new FoundationUiSession(this);
        Foundation.ValidateAction = action => action == "btnSave" && !string.IsNullOrWhiteSpace(txtdisplayOrder.Text)
            && !int.TryParse(txtdisplayOrder.Text, out _) ? "ترتيب العرض يجب أن يكون عددًا صحيحًا." : null;
        Foundation.RequireFields(new[] { "btnSave" }, "txtclassificationCode", "txtarabicName", "cmbfinancialClassification", "cmbnormalBalanceNature");
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
    public void SetRecordContext(string recordKind, bool allowsPostingEditable)
    {
        if (recordKind != "AccountGroup" && recordKind != "AccountType")
            throw new ArgumentException("Expected issued AccountGroup or AccountType discriminator.", nameof(recordKind));
        lblRecordKind.Text = recordKind == "AccountGroup" ? "نوع السجل: مجموعة حسابات" : "نوع السجل: نوع حساب";
        chkallowsPostingAccounts.Enabled = allowsPostingEditable;
    }
    public void LoadRecordChoices(IEnumerable<BatchFiveChoice> choices)
    {
        var items = choices.ToArray();
        if (items.Any(x => string.IsNullOrWhiteSpace(x.Id)) || items.Select(x => x.Id).Distinct().Count() != items.Length)
            throw new ArgumentException("Record identities must be nonempty and unique.", nameof(choices));
        dgvRecords.DataSource = items;
        dgvRecords.SelectedIndex = -1;
    }
    public string? SelectedRecordId => (dgvRecords.SelectedItem as BatchFiveChoice)?.Id;
    public FoundationUiSession Foundation { get; }
    public bool HasUnsavedChanges => Foundation.HasUnsavedChanges;
    public bool IsBusy => Foundation.IsBusy;
    public bool ConfirmLeave() => Foundation.ConfirmLeave();

    private bool sizing;
    private void MainDataSizeChanged(object? sender, EventArgs e) => UpdateMainDataExtent();
    private void UpdateMainDataExtent()
    {
        if (sizing || IsDisposed || mainDataLayout == null) return;
        sizing = true;
        try
        {
            int width = Math.Max(mainDataLayout.MinimumSize.Width,
                tpMain.ClientSize.Width - SystemInformation.VerticalScrollBarWidth);
            int height = lblRecordKind.GetPreferredSize(new Size(width, 0)).Height
                + fieldsLayout.GetPreferredSize(new Size(width, 0)).Height
                + dgvRecords.MinimumSize.Height + 24;
            mainDataLayout.Size = new Size(width, Math.Max(height, tpMain.ClientSize.Height));
        }
        finally { sizing = false; }
    }
    public event EventHandler? CloseRequested;
    internal Button CloseButton => btnClose;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);
}
