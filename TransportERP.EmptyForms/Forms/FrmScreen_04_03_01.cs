using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;

/// <summary>الصناديق — ACC-038. Designer-only; no persistence or data providers connected.</summary>
public partial class UcScreen_04_03_01 : UserControl, TransportERP.Desktop.CoreUI.IFoundationScreen
{
    public TransportERP.Desktop.CoreUI.FoundationUiSession Foundation { get; }
    public bool HasUnsavedChanges => Foundation.HasUnsavedChanges;
    public bool IsBusy => Foundation.IsBusy;
    public bool ConfirmLeave() => Foundation.ConfirmLeave();

    public UcScreen_04_03_01()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        UpdateContentExtent();
        Foundation = new TransportERP.Desktop.CoreUI.FoundationUiSession(this);
        Foundation.ValidateAction = action => action == "btnSave"
            && (!ValidOptionalAmount(txtmaximumBalance.Text) || !ValidOptionalAmount(txtsingleTransactionLimit.Text))
            ? "أدخل حدودًا غير سالبة أو اتركها فارغة." : null;
        Foundation.RequireFields(new[] { "btnSave", "btnValidate", "btnPublish" }, txtcashboxCode.Name, txtcashboxName.Name, cmbcompanyRef.Name, cmbbranchRef.Name, cmbcurrencyRef.Name, cmbglAccountRef.Name);
        ApplyKnownRequirementColors();
        Load += (_, _) => ApplyKnownRequirementColors();
        Foundation.DocumentLoaded += (_, _) => ApplyKnownRequirementColors();
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
    private bool sizing;
    private void ApplyKnownRequirementColors()
    {
        foreach (Control editor in new Control[] { txtcashboxCode, txtcashboxName, cmbcompanyRef, cmbbranchRef, cmbcurrencyRef, cmbglAccountRef })
            global::TransportERP.RequiredFieldAppearance.Apply(editor, true);
        global::TransportERP.RequiredFieldAppearance.Apply(txtmaximumBalance, false);
        global::TransportERP.RequiredFieldAppearance.Apply(txtsingleTransactionLimit, false);
    }
    private void ContentSizeChanged(object? sender, EventArgs e) => UpdateContentExtent();
    private void UpdateContentExtent()
    {
        if (sizing || IsDisposed || contentLayout == null || detailsTabs == null) return;
        sizing = true;
        try
        {
            int width = Math.Max(contentLayout.MinimumSize.Width, pnlContent.ClientSize.Width - SystemInformation.VerticalScrollBarWidth);
            int detailHeight = 0;
            foreach (TabPage page in detailsTabs.TabPages)
                foreach (Control child in page.Controls)
                    detailHeight = Math.Max(detailHeight, child.GetPreferredSize(new Size(width - 16, 0)).Height);
            detailHeight += Math.Max(32, detailsTabs.Height - detailsTabs.DisplayRectangle.Height);
            contentLayout.RowStyles[0].Height = Math.Max(detailsTabs.MinimumSize.Height, detailHeight);
            int minimumHeight = (int)contentLayout.RowStyles[0].Height + searchLayout.GetPreferredSize(new Size(width, 0)).Height
                + dgvRecords.MinimumSize.Height + paging.GetPreferredSize(new Size(width, 0)).Height
                + tlpAuditInfo.GetPreferredSize(new Size(width, 0)).Height + 40;
            contentLayout.Size = new Size(width, Math.Max(minimumHeight, pnlContent.ClientSize.Height));
        }
        finally { sizing = false; }
    }
    private void MonetaryTextChanged(object? sender, EventArgs e)
    {
        bool valid = ValidOptionalAmount(txtmaximumBalance.Text) && ValidOptionalAmount(txtsingleTransactionLimit.Text);
        lblMoneyValidation.Text = valid
            ? "المبالغ اختيارية؛ الفراغ يعني عدم التحديد. لا يوجد حفظ أو تنفيذ للحدود."
            : "أدخل مبلغاً غير سالب، أو اترك الحقل فارغاً لعدم تحديد حد.";
        UpdateContentExtent();
    }
    private static bool ValidOptionalAmount(string text) => string.IsNullOrWhiteSpace(text)
        || (decimal.TryParse(text, System.Globalization.NumberStyles.Number,
            System.Globalization.CultureInfo.CurrentCulture, out decimal value) && value >= 0M);
    public event EventHandler? CloseRequested;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);
}
