using System;
using System.Windows.Forms;
namespace TransportERP.EmptyForms;

/// <summary>Navigation host for the independently designed UcAccountProjectLinking.</summary>
public partial class FrmOnyxSCREEN0094 : Form
{
    public FrmOnyxSCREEN0094()
    {
        InitializeComponent();
        content.CloseRequested += Content_CloseRequested;
        CancelButton = content.CloseButton;
    }
    private void Content_CloseRequested(object? sender, EventArgs e) => Close();
}
