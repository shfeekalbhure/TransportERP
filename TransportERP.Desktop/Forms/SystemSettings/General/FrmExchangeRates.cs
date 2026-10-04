using System;
using System.Windows.Forms;
namespace TransportERP.Desktop.Forms.SystemSettings.General;

/// <summary>Navigation host for the independently designed UcExchangeRates.</summary>
public partial class FrmExchangeRates : TransportERP.Desktop.CoreUI.FrmBase
{
    public FrmExchangeRates()
    {
        InitializeComponent();
        content.CloseRequested += Content_CloseRequested;
        CancelButton = content.CloseButton;
    }
    private void Content_CloseRequested(object? sender, EventArgs e) => Close();
}
