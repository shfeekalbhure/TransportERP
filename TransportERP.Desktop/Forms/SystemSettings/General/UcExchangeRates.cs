using System;
using System.Windows.Forms;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

/// <summary>أسعار الصرف. Designer-editable preview; database actions are intentionally disabled.</summary>
public partial class UcExchangeRates : UserControl
{
    public UcExchangeRates()
    {
        InitializeComponent();
    }

    public event EventHandler? CloseRequested;
    internal Button CloseButton => btnClose;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);

}
