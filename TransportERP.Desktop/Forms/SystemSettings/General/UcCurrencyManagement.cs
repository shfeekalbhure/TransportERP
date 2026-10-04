using System;
using System.Windows.Forms;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

/// <summary>إدارة العملات. Designer-editable preview; database actions are intentionally disabled.</summary>
public partial class UcCurrencyManagement : UserControl
{
    public UcCurrencyManagement()
    {
        InitializeComponent();
    }

    public event EventHandler? CloseRequested;
    internal Button CloseButton => btnClose;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);

}
