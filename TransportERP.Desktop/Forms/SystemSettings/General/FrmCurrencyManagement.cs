using System;
using System.Windows.Forms;
namespace TransportERP.Desktop.Forms.SystemSettings.General;

/// <summary>Navigation host for the independently designed UcCurrencyManagement.</summary>
public partial class FrmCurrencyManagement : TransportERP.Desktop.CoreUI.FrmBase
{
    public FrmCurrencyManagement()
    {
        InitializeComponent();
        content.CloseRequested += Content_CloseRequested;
        CancelButton = content.CloseButton;
    }
    private void Content_CloseRequested(object? sender, EventArgs e) => Close();
}
