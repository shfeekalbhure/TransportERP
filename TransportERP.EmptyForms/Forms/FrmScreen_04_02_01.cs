using System;
using System.Drawing;
using System.Windows.Forms;
using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcScreen_04_02_01 : UserControl, IFoundationScreen
{
    public UcScreen_04_02_01()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        UpdateContentExtent();
        Foundation = new FoundationUiSession(this);
        Foundation.RequireFields(new[] { "btnSave" }, "cmbCompanyId", "txtCode", "txtArabicName");
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
    /// <summary>Populate within Foundation.LoadView after validating the full payload.</summary>
    public void PopulateRecords(IEnumerable<object?[]> rows)
    {
        var data = rows.ToArray();
        if (data.Any(row => row.Length != dgvRecords.Columns.Count))
            throw new ArgumentException("Row width does not match the issued UI schema.", nameof(rows));
        dgvRecords.Rows.Clear();
        foreach (var row in data) dgvRecords.Rows.Add(row.Select(value => value!).ToArray());
    }
    public void PopulateTree(IEnumerable<TreeNode> nodes)
    {
        var data = nodes.Select(node => (TreeNode)node.Clone()).ToArray();
        tvCostCenters.BeginUpdate();
        try { tvCostCenters.Nodes.Clear(); tvCostCenters.Nodes.AddRange(data); }
        finally { tvCostCenters.EndUpdate(); }
    }
    public void SetEditContext(bool isCreate, bool canEdit)
    {
        cmbCompanyId.Enabled = canEdit && isCreate;
        cmbParentCostCenterId.Enabled = canEdit && isCreate;
        cmbBranchId.Enabled = canEdit;
        txtCode.ReadOnly = !canEdit;
        txtArabicName.ReadOnly = !canEdit;
        txtEnglishName.ReadOnly = !canEdit;
    }
    public FoundationUiSession Foundation { get; }
    public bool HasUnsavedChanges => Foundation.HasUnsavedChanges;
    public bool IsBusy => Foundation.IsBusy;
    public bool ConfirmLeave() => Foundation.ConfirmLeave();

    private bool sizing;
    private void ContentSizeChanged(object? sender, EventArgs e) => UpdateContentExtent();
    private void UpdateContentExtent()
    {
        if (sizing || IsDisposed || contentLayout == null) return;
        sizing = true;
        try
        {
            int width = Math.Max(contentLayout.MinimumSize.Width,
                pnlContent.ClientSize.Width - SystemInformation.VerticalScrollBarWidth);
            int height = fieldsLayout.GetPreferredSize(new Size(width, 0)).Height
                + searchLayout.GetPreferredSize(new Size(width, 0)).Height
                + treeLayout.MinimumSize.Height
                + paging.GetPreferredSize(new Size(width, 0)).Height
                + tlpAuditInfo.GetPreferredSize(new Size(width, 0)).Height + 40;
            contentLayout.Size = new Size(width, Math.Max(height, pnlContent.ClientSize.Height));
        }
        finally { sizing = false; }
    }
    public event EventHandler? CloseRequested;
    internal Button CloseButton => btnClose;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);
}
