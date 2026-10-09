using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
/// <summary>طرق الدفع — ACC-040. Design surface; services remain unconnected.</summary>
public partial class UcScreen_04_03_04 : UserControl, IFoundationScreen
{
    public UcScreen_04_03_04()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        UpdateContentExtent();
        Foundation = new FoundationUiSession(this);
        Foundation.RequireFields(new[] { "btnSave" }, "fieldmethodCode", "fieldmethodName", "fieldmethodType");
        ApplyKnownRequirementColors();
        Load += (_, _) => ApplyKnownRequirementColors();
        Foundation.DocumentLoaded += (_, _) => ApplyKnownRequirementColors();
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    /// <summary>Adapter-supplied context; no synthetic records or storage actions.</summary>
    public void SetAuditContext(string text) => state3.Text = text ?? string.Empty;
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
        foreach (string key in new[] { "fieldmethodCode", "fieldmethodName", "fieldmethodType" })
            global::TransportERP.RequiredFieldAppearance.Apply(Foundation.Controls[key], true);
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
            int detailHeight = detailsLayout.GetPreferredSize(new Size(width, 0)).Height;
            int height = detailHeight + searchLayout.GetPreferredSize(new Size(width, 0)).Height
                + dgvRecords.MinimumSize.Height + paging.GetPreferredSize(new Size(width, 0)).Height
                + statusLayout.GetPreferredSize(new Size(width, 0)).Height + tlpAuditInfo.GetPreferredSize(new Size(width, 0)).Height + 40;
            contentLayout.Size = new Size(width, Math.Max(height, pnlContent.ClientSize.Height));
        }
        finally { sizing = false; }
    }
}
