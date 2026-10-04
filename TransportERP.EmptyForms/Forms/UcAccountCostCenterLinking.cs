using System;
using System.Windows.Forms;

namespace TransportERP.EmptyForms;

/// <summary>ربط الحسابات بالمراكز. Designer-editable preview; database actions are intentionally disabled.</summary>
public partial class UcAccountCostCenterLinking : UserControl
{
    public UcAccountCostCenterLinking()
    {
        InitializeComponent();
    }

    public event EventHandler? CloseRequested;
    internal Button CloseButton => btnClose;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);

}
