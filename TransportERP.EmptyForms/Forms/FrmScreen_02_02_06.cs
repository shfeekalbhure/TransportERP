using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;

/// <summary>المديريات — GEN-005. Designer-only; no persistence or data providers connected.</summary>
public partial class UcScreen_02_02_06 : UserControl, TransportERP.Desktop.CoreUI.IFoundationScreen
{
    public TransportERP.Desktop.CoreUI.FoundationUiSession Foundation { get; }
    public bool HasUnsavedChanges => Foundation.HasUnsavedChanges;
    public bool IsBusy => Foundation.IsBusy;
    public bool ConfirmLeave() => Foundation.ConfirmLeave();

    public UcScreen_02_02_06()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        // Filter index 0 means no constraint (null), never a persisted enum or ID.
        // Future lookup binding must retain that clear option independently of data.
        // On record binding, parent selectors are enabled only in Create, never Edit.
        pnlContent.SizeChanged += ContentViewportChanged;
        contentLayout.Layout += ContentLayoutChanged;
        UpdateContentExtent();
        Foundation = new TransportERP.Desktop.CoreUI.FoundationUiSession(this);
        Foundation.RequireFields(new[] { "btnSave", "btnValidate", "btnPublish" }, cmbGovernorateId.Name, txtCode.Name, txtArabicName.Name);
        ApplyKnownRequirementColors();
        Load += (_, _) => ApplyKnownRequirementColors();
        Foundation.DocumentLoaded += (_, _) => ApplyKnownRequirementColors();
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
    private bool updatingContentExtent;
    private void ApplyKnownRequirementColors()
    {
        foreach (Control editor in new Control[] { cmbGovernorateId, txtCode, txtArabicName })
            global::TransportERP.RequiredFieldAppearance.Apply(editor, true);
    }
    private void ContentViewportChanged(object? sender, EventArgs e) => UpdateContentExtent();
    private void ContentLayoutChanged(object? sender, LayoutEventArgs e) => UpdateContentExtent();

    private void UpdateContentExtent()
    {
        if (updatingContentExtent || IsDisposed) return;
        updatingContentExtent = true;
        try
        {
            // The interior scrolls when needed; the root remains workspace Fill.
            int width = Math.Max(1, contentLayout.ClientSize.Width - contentLayout.Padding.Horizontal);
            int requiredHeight = contentLayout.Padding.Vertical
                + fieldsLayout.GetPreferredSize(new Size(width, 0)).Height + fieldsLayout.Margin.Vertical
                + searchLayout.GetPreferredSize(new Size(width, 0)).Height + searchLayout.Margin.Vertical
                + dgvRecords.MinimumSize.Height + dgvRecords.Margin.Vertical
                + paging.GetPreferredSize(new Size(width, 0)).Height + paging.Margin.Vertical
                + tlpAuditInfo.GetPreferredSize(new Size(width, 0)).Height + tlpAuditInfo.Margin.Vertical;
            var extent = new Size(contentLayout.MinimumSize.Width, requiredHeight);
            if (pnlContent.AutoScrollMinSize != extent) pnlContent.AutoScrollMinSize = extent;
            int viewportHeight = pnlContent.ClientSize.Height
                - (pnlContent.HorizontalScroll.Visible ? SystemInformation.HorizontalScrollBarHeight : 0);
            int height = Math.Max(requiredHeight, viewportHeight);
            if (contentLayout.Height != height) contentLayout.Height = height;
        }
        finally { updatingContentExtent = false; }
    }

    public event EventHandler? CloseRequested;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);
}
