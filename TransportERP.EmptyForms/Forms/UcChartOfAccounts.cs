using System;
using System.Windows.Forms;

namespace TransportERP.EmptyForms;

/// <summary>دليل الحسابات. Designer-editable preview; database actions are intentionally disabled.</summary>
public partial class UcChartOfAccounts : UserControl, TransportERP.Desktop.CoreUI.IFoundationScreen
{
    public TransportERP.Desktop.CoreUI.FoundationUiSession Foundation { get; }
    public bool HasUnsavedChanges => Foundation.HasUnsavedChanges;
    public bool IsBusy => Foundation.IsBusy;
    public bool ConfirmLeave() => Foundation.ConfirmLeave();

    public UcChartOfAccounts()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.ScreenProperties.Apply(this);
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
        // Let the verified header host grow when its shared action bar wraps.
        mainLayout.RowStyles[0].SizeType = SizeType.AutoSize;
        mainLayout.RowStyles[0].Height = 0F;
        pnlHeader.AutoSize = true;
        pnlHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlActions.Dock = DockStyle.Top;
        TransportERP.Desktop.CoreUI.SharedScreenProperties.ApplyContent(this);
        contentViewport.AutoScrollMinSize = chartLayout.MinimumSize;
        DpiChangedAfterParent += (_, _) => contentViewport.AutoScrollMinSize = chartLayout.MinimumSize;
        Foundation = new TransportERP.Desktop.CoreUI.FoundationUiSession(this);
        Foundation.RequireFields(new[] { "btnSave", "btnValidate", "btnPublish" }, cmbCompany.Name, cmbAccountGroup.Name, cmbAccountType.Name, txtAccountCode.Name, txtAccountNameAr.Name);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    public event EventHandler? CloseRequested;
    internal Button CloseButton => btnClose;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);

}
