using System;
using System.Windows.Forms;

namespace TransportERP.EmptyForms;

/// <summary>دليل الحسابات. Designer-editable preview; database actions are intentionally disabled.</summary>
public partial class UcChartOfAccounts : UserControl
{
    public UcChartOfAccounts()
    {
        InitializeComponent();
    }

    public event EventHandler? CloseRequested;
    internal Button CloseButton => btnClose;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);

}
