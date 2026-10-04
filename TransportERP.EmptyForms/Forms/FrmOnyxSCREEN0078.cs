using System;
using System.Windows.Forms;
namespace TransportERP.EmptyForms;

/// <summary>Navigation host for the independently designed UcGeneralLedgerSettings.</summary>
public partial class FrmOnyxSCREEN0078 : Form
{
    public FrmOnyxSCREEN0078()
    {
        InitializeComponent();
        content.CloseRequested += Content_CloseRequested;
        CancelButton = content.CloseButton;
    }
    private void Content_CloseRequested(object? sender, EventArgs e) => Close();
}
