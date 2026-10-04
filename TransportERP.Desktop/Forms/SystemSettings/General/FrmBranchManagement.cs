using System;
using System.Windows.Forms;
using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

/// <summary>Single navigation host for the owner's branch UserControl.</summary>
public partial class FrmBranchManagement : FrmBase
{
    public FrmBranchManagement()
    {
        InitializeComponent();
        branchManagement.CloseRequested += BranchManagement_CloseRequested;
        CancelButton = branchManagement.CloseButton;
    }

    private void BranchManagement_CloseRequested(object? sender, EventArgs e) => Close();
}
