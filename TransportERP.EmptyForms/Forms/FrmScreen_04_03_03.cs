using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
/// <summary>الحسابات البنكية — ACC-039. Design surface; services remain unconnected.</summary>
public partial class UcScreen_04_03_03 : UserControl, IFoundationScreen
{
    public UcScreen_04_03_03()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        UpdateContentExtent();
        Foundation = new FoundationUiSession(this);
        Foundation.ValidateAction = action => action == "btnSave" && !string.IsNullOrWhiteSpace(fieldwithdrawalTransferLimit.Text)
            && (!decimal.TryParse(fieldwithdrawalTransferLimit.Text, System.Globalization.NumberStyles.AllowLeadingSign | System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.CurrentCulture, out var amount) || amount < 0)
            ? "أدخل حدًا غير سالب أو اتركه فارغًا." : null;
        Foundation.RequireFields(new[] { "btnSave" }, "fieldbankAccountCode", "fieldbankName", "fieldaccountName", "fieldaccountNumber", "fieldcompanyRef", "fieldcurrencyRef", "fieldglAccountRef");
        ApplyKnownRequirementColors();
        Load += (_, _) => ApplyKnownRequirementColors();
        Foundation.DocumentLoaded += (_, _) => ApplyKnownRequirementColors();
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    /// <summary>Adapter-supplied context; no synthetic records or storage actions.</summary>
    public void SetAttachmentsContext(string text) => state4.Text = text ?? string.Empty;
    public void SetAuditContext(string text) => state5.Text = text ?? string.Empty;
    /// <summary>Populate within Foundation.LoadView after validating the full payload.</summary>
    public void PopulateRecords(IEnumerable<object?[]> rows)
    {
        var data = rows.ToArray();
        if (data.Any(row => row.Length != dgvRecords.Columns.Count))
            throw new ArgumentException("Row width does not match the issued UI schema.", nameof(rows));
        dgvRecords.Rows.Clear();
        foreach (var row in data) dgvRecords.Rows.Add(row.Select(value => value!).ToArray());
    }
    public FoundationUiSession Foundation { get; }
    public bool HasUnsavedChanges => Foundation.HasUnsavedChanges;
    public bool IsBusy => Foundation.IsBusy;
    public bool ConfirmLeave() => Foundation.ConfirmLeave();

    private bool sizing;
    private void ApplyKnownRequirementColors()
    {
        foreach (string key in new[] { "fieldbankAccountCode", "fieldbankName", "fieldaccountName", "fieldaccountNumber", "fieldcompanyRef", "fieldcurrencyRef", "fieldglAccountRef" })
            global::TransportERP.RequiredFieldAppearance.Apply(Foundation.Controls[key], true);
        global::TransportERP.RequiredFieldAppearance.Apply(fieldwithdrawalTransferLimit, false);
    }
    public event EventHandler? CloseRequested;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);
    private void ContentSizeChanged(object? sender, EventArgs e) => UpdateContentExtent();
    private void UpdateContentExtent()
    {
        if (sizing || IsDisposed || contentLayout == null) return;
        sizing = true;
        try
        {
            int width = Math.Max(contentLayout.MinimumSize.Width, pnlContent.ClientSize.Width - SystemInformation.VerticalScrollBarWidth);
            int details = 0;
            foreach (TabPage page in detailsTabs.TabPages)
                foreach (Control child in page.Controls)
                    details = Math.Max(details, child.GetPreferredSize(new Size(width - 16, 0)).Height);
            details += Math.Max(32, detailsTabs.Height - detailsTabs.DisplayRectangle.Height);
            contentLayout.RowStyles[0].Height = Math.Max(detailsTabs.MinimumSize.Height, details);
            int detailHeight = (int)contentLayout.RowStyles[0].Height;
            int height = detailHeight + searchLayout.GetPreferredSize(new Size(width, 0)).Height
                + dgvRecords.MinimumSize.Height + paging.GetPreferredSize(new Size(width, 0)).Height
                + statusLayout.GetPreferredSize(new Size(width, 0)).Height + tlpAuditInfo.GetPreferredSize(new Size(width, 0)).Height + 40;
            contentLayout.Size = new Size(width, Math.Max(height, pnlContent.ClientSize.Height));
        }
        finally { sizing = false; }
    }
    private void MonetaryTextChanged(object? sender, EventArgs e)
    {
        var text = fieldwithdrawalTransferLimit.Text;
        bool valid = string.IsNullOrWhiteSpace(text) || (decimal.TryParse(text, System.Globalization.NumberStyles.Number,
            System.Globalization.CultureInfo.CurrentCulture, out decimal amount) && amount >= 0M);
        lblState.Text = valid ? "الحد اختياري؛ لا يوجد حفظ أو تنفيذ للحدود." : "أدخل مبلغاً غير سالب أو اترك الحد فارغاً.";
        UpdateContentExtent();
    }
}
